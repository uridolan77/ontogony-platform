using System.Collections.Immutable;

namespace Athanor.Api.Middleware;

public sealed class TraceIdMiddleware
{
    public const string HeaderName = "X-Athanor-Trace-Id";
    public const string ItemKey = "Athanor.TraceId";
    public const string OperationIdItemKey = "Athanor.OperationId";

    private readonly RequestDelegate _next;
    private readonly ILogger<TraceIdMiddleware> _logger;

    public TraceIdMiddleware(RequestDelegate next, ILogger<TraceIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var traceId = GetOrCreateTraceId(context);
        context.Items[ItemKey] = traceId;

        var operationId = Guid.NewGuid().ToString("n");
        context.Items[OperationIdItemKey] = operationId;

        context.Response.OnStarting(() =>
        {
            if (!context.Response.Headers.ContainsKey(HeaderName))
            {
                context.Response.Headers[HeaderName] = traceId;
            }

            return Task.CompletedTask;
        });

        using var scope = _logger.BeginScope(
            ImmutableDictionary<string, object?>.Empty
                .Add("traceId", traceId)
                .Add("operationId", operationId)
                .Add("method", context.Request.Method)
                .Add("path", context.Request.Path.Value));

        await _next(context);
    }

    private static string GetOrCreateTraceId(HttpContext context)
    {
        var incoming = context.Request.Headers[HeaderName].ToString();
        if (!string.IsNullOrWhiteSpace(incoming))
        {
            return incoming.Trim();
        }

        if (!string.IsNullOrWhiteSpace(context.TraceIdentifier))
        {
            return context.TraceIdentifier;
        }

        return Guid.NewGuid().ToString("n");
    }
}

