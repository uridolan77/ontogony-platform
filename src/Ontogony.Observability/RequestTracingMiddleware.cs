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
        var traceId = ResolveTraceId(context, _options);
        var operationId = Guid.NewGuid().ToString("n");

        context.Items[TraceIdItemKey] = traceId;
        context.Items[OperationIdItemKey] = operationId;

        context.Response.OnStarting(() =>
        {
            context.Response.Headers[_options.TraceHeaderName] = traceId;
            if (_options.EchoLegacyHeaders)
            {
                context.Response.Headers[OntogonyEventHeaders.LegacyAthanorTraceId] = traceId;
                context.Response.Headers[OntogonyEventHeaders.LegacyAgentorTraceId] = traceId;
            }

            return Task.CompletedTask;
        });

        var state = new CorrelationState(
            traceId,
            operationId,
            context.Request.Headers[OntogonyEventHeaders.TenantId].ToString(),
            context.Request.Headers[OntogonyEventHeaders.WorkspaceId].ToString(),
            context.Request.Headers[OntogonyEventHeaders.ProjectId].ToString(),
            context.Request.Headers[OntogonyEventHeaders.ActorId].ToString());

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
        using var activity = OntogonyDiagnostics.ActivitySource.StartActivity("http.request", ActivityKind.Server);
        activity?.SetTag("ontogony.trace_id", traceId);
        activity?.SetTag("ontogony.operation_id", operationId);
        activity?.SetTag("service.name", _options.ServiceName);
        activity?.SetTag("http.request.method", context.Request.Method);
        activity?.SetTag("url.path", context.Request.Path.Value);

        try
        {
            OntogonyDiagnostics.HttpServerRequestCount.Add(1, new KeyValuePair<string, object?>("service", _options.ServiceName));
            await _next(context);
            activity?.SetTag("http.response.status_code", context.Response.StatusCode);
        }
        catch
        {
            OntogonyDiagnostics.HttpServerErrorCount.Add(1, new KeyValuePair<string, object?>("service", _options.ServiceName));
            activity?.SetStatus(ActivityStatusCode.Error);
            throw;
        }
        finally
        {
            var elapsedMs = Stopwatch.GetElapsedTime(started).TotalMilliseconds;
            OntogonyDiagnostics.HttpServerDurationMs.Record(
                elapsedMs,
                new KeyValuePair<string, object?>("service", _options.ServiceName),
                new KeyValuePair<string, object?>("method", context.Request.Method));
        }
    }

    private static string ResolveTraceId(HttpContext context, OntogonyObservabilityOptions options)
    {
        foreach (var header in options.AcceptedIncomingTraceHeaders)
        {
            var value = context.Request.Headers[header].ToString();
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }
        }

        return !string.IsNullOrWhiteSpace(context.TraceIdentifier)
            ? context.TraceIdentifier
            : Guid.NewGuid().ToString("n");
    }
}
