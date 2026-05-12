using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ontogony.Contracts.Events;

namespace Ontogony.Observability;

public sealed class RequestTracingMiddleware
{
    public const string TraceIdItemKey = "Ontogony.TraceId";
    public const string OperationIdItemKey = "Ontogony.OperationId";

    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTracingMiddleware> _logger;
    private readonly OntogonyObservabilityOptions _options;

    public RequestTracingMiddleware(
        RequestDelegate next,
        ILogger<RequestTracingMiddleware> logger,
        IOptions<OntogonyObservabilityOptions> options)
    {
        _next = next;
        _logger = logger;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var incomingHeaders = BuildHeaderSnapshot(context.Request.Headers);
        var incomingState = OntogonyCorrelationContext.FromHeaders(
            incomingHeaders,
            _options.TraceHeaderName,
            _options.AcceptedIncomingTraceHeaders);
        var traceId = incomingState?.TraceId ?? CreateTraceId();
        var operationId = Guid.NewGuid().ToString("n");

        context.Items[TraceIdItemKey] = traceId;
        context.Items[OperationIdItemKey] = operationId;

        using var activity = StartServerActivity(context, incomingState, traceId);

        var state = new CorrelationState(
            traceId,
            operationId,
            context.Request.Headers[OntogonyEventHeaders.TenantId].ToString(),
            context.Request.Headers[OntogonyEventHeaders.WorkspaceId].ToString(),
            context.Request.Headers[OntogonyEventHeaders.ProjectId].ToString(),
            context.Request.Headers[OntogonyEventHeaders.ActorId].ToString(),
            context.Request.Headers[OntogonyEventHeaders.SessionId].ToString(),
            activity?.Id ?? incomingState?.TraceParent,
            activity?.TraceStateString ?? incomingState?.TraceState);

        SetResponseTraceHeaders(context.Response.Headers, state, _options);

        context.Response.OnStarting(() =>
        {
            SetResponseTraceHeaders(context.Response.Headers, state, _options);

            return Task.CompletedTask;
        });

        using var _ = OntogonyCorrelationContext.Push(state);
        using var scope = _logger.BeginScope(new Dictionary<string, object?>
        {
            ["traceId"] = traceId,
            ["operationId"] = operationId,
            ["service"] = _options.ServiceName,
            ["method"] = context.Request.Method,
            ["path"] = context.Request.Path.Value
        });

        var started = Stopwatch.GetTimestamp();
        activity?.SetTag(OntogonySpanAttributes.TraceId, traceId);
        activity?.SetTag(OntogonySpanAttributes.OperationId, operationId);
        activity?.SetTag(OntogonySpanAttributes.ActorId, state.ActorId);
        activity?.SetTag(OntogonySpanAttributes.TenantId, state.TenantId);
        activity?.SetTag(OntogonySpanAttributes.WorkspaceId, state.WorkspaceId);
        activity?.SetTag(OntogonySpanAttributes.ProjectId, state.ProjectId);
        activity?.SetTag(OntogonySpanAttributes.SessionId, state.SessionId);
        activity?.SetTag("service.name", _options.ServiceName);
        activity?.SetTag("http.request.method", context.Request.Method);
        activity?.SetTag("url.path", context.Request.Path.Value);

        var statusCode = StatusCodes.Status200OK;
        var unhandledException = false;

        try
        {
            await _next(context);
            statusCode = context.Response.StatusCode;
            activity?.SetTag("http.response.status_code", context.Response.StatusCode);
        }
        catch
        {
            unhandledException = true;
            statusCode = context.Response.HasStarted
                ? context.Response.StatusCode
                : StatusCodes.Status500InternalServerError;
            activity?.SetStatus(ActivityStatusCode.Error);
            activity?.SetTag("http.response.status_code", statusCode);
            throw;
        }
        finally
        {
            var elapsedMs = Stopwatch.GetElapsedTime(started).TotalMilliseconds;
            var isError = unhandledException || statusCode >= StatusCodes.Status500InternalServerError;
            OntogonyMetrics.RecordHttpRequest(
                _options.ServiceName,
                context.Request.Method,
                statusCode,
                elapsedMs,
                isError);
        }
    }

    private static IReadOnlyDictionary<string, string?> BuildHeaderSnapshot(IHeaderDictionary headers)
    {
        var snapshot = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        foreach (var header in headers)
        {
            snapshot[header.Key] = header.Value.ToString();
        }

        return snapshot;
    }

    private static Activity? StartServerActivity(HttpContext context, CorrelationState? incomingState, string traceId)
    {
        if (!string.IsNullOrWhiteSpace(incomingState?.TraceParent) &&
            ActivityContext.TryParse(incomingState.TraceParent, incomingState.TraceState, out var parentContext))
        {
            return OntogonyDiagnostics.ActivitySource.StartActivity("http.request", ActivityKind.Server, parentContext);
        }

        var activity = OntogonyDiagnostics.ActivitySource.StartActivity("http.request", ActivityKind.Server);
        if (activity is not null && activity.TraceId == default)
        {
            activity.SetIdFormat(ActivityIdFormat.W3C);
        }

        if (activity is not null && string.IsNullOrWhiteSpace(activity.Id))
        {
            activity.TraceStateString = incomingState?.TraceState;
        }

        if (activity is null && !string.IsNullOrWhiteSpace(context.TraceIdentifier))
        {
            context.TraceIdentifier = traceId;
        }

        return activity;
    }

    private static string CreateTraceId()
    {
        return ActivityTraceId.CreateRandom().ToHexString();
    }

    private static void SetResponseTraceHeaders(IHeaderDictionary headers, CorrelationState state, OntogonyObservabilityOptions options)
    {
        headers[options.TraceHeaderName] = state.TraceId;
        if (!string.IsNullOrWhiteSpace(state.TraceParent))
        {
            headers[options.TraceParentHeaderName] = state.TraceParent;
        }

        if (!string.IsNullOrWhiteSpace(state.TraceState))
        {
            headers[options.TraceStateHeaderName] = state.TraceState;
        }

        if (options.EchoCorrelationHeaders)
        {
            EchoOptionalHeader(headers, OntogonyEventHeaders.ActorId, state.ActorId);
            EchoOptionalHeader(headers, OntogonyEventHeaders.TenantId, state.TenantId);
            EchoOptionalHeader(headers, OntogonyEventHeaders.WorkspaceId, state.WorkspaceId);
            EchoOptionalHeader(headers, OntogonyEventHeaders.ProjectId, state.ProjectId);
            EchoOptionalHeader(headers, OntogonyEventHeaders.SessionId, state.SessionId);
        }

        if (!options.EchoLegacyHeaders)
        {
            return;
        }

        headers[OntogonyEventHeaders.LegacyAthanorTraceId] = state.TraceId;
        headers[OntogonyEventHeaders.LegacyAgentorTraceId] = state.TraceId;
        headers[OntogonyEventHeaders.ConexusRequestId] = state.TraceId;
    }

    private static void EchoOptionalHeader(IHeaderDictionary headers, string headerName, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            headers[headerName] = value;
        }
    }
}
