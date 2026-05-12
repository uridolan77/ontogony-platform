using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Ontogony.Contracts.Events;
using Ontogony.Observability;

namespace Ontogony.Testing;

/// <summary>
/// Conformance helpers that verify a service wires up <see cref="RequestTracingMiddleware"/> correctly.
/// </summary>
public static class TracingConformanceAssertions
{
    /// <summary>
    /// Verifies that <see cref="RequestTracingMiddleware"/> echoes <see cref="OntogonyEventHeaders.TraceId"/>
    /// back in the response and populates <see cref="OntogonyCorrelationContext"/> for downstream code.
    /// </summary>
    public static async Task AssertCanonicalTraceHeaderEchoedAsync(string traceId)
    {
        if (string.IsNullOrWhiteSpace(traceId))
            throw new ArgumentException("traceId cannot be empty.", nameof(traceId));

        string? capturedTraceId = null;

        var options = Options.Create(new OntogonyObservabilityOptions { ServiceName = "conformance-test" });
        var middleware = new RequestTracingMiddleware(
            ctx =>
            {
                capturedTraceId = OntogonyCorrelationContext.TraceId;
                return Task.CompletedTask;
            },
            NullLogger<RequestTracingMiddleware>.Instance,
            options);

        var context = MiddlewareTestHarness.CreateHttpContext("GET", "/ping");
        context.Request.Headers[OntogonyEventHeaders.TraceId] = traceId;

        await middleware.InvokeAsync(context);

        if (!string.Equals(capturedTraceId, traceId, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Expected OntogonyCorrelationContext.TraceId to be '{traceId}' inside the pipeline, but was '{capturedTraceId}'.");
        }

        if (!context.Response.Headers.TryGetValue(OntogonyEventHeaders.TraceId, out var echoed) ||
            !string.Equals(echoed.ToString(), traceId, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Expected response header '{OntogonyEventHeaders.TraceId}' to be '{traceId}', but was '{echoed}'.");
        }
    }

    /// <summary>
    /// Verifies that when no trace header is present the middleware still sets a non-empty trace id
    /// on <see cref="OntogonyCorrelationContext"/> and echoes it back in the response.
    /// </summary>
    public static async Task AssertTraceIdGeneratedWhenAbsentAsync()
    {
        string? capturedTraceId = null;

        var options = Options.Create(new OntogonyObservabilityOptions { ServiceName = "conformance-test" });
        var middleware = new RequestTracingMiddleware(
            ctx =>
            {
                capturedTraceId = OntogonyCorrelationContext.TraceId;
                return Task.CompletedTask;
            },
            NullLogger<RequestTracingMiddleware>.Instance,
            options);

        var context = MiddlewareTestHarness.CreateHttpContext("GET", "/ping");

        await middleware.InvokeAsync(context);

        if (string.IsNullOrWhiteSpace(capturedTraceId))
        {
            throw new InvalidOperationException(
                "Expected RequestTracingMiddleware to generate a trace id when none is present in the request.");
        }

        if (!context.Response.Headers.TryGetValue(OntogonyEventHeaders.TraceId, out var echoed) ||
            string.IsNullOrWhiteSpace(echoed.ToString()))
        {
            throw new InvalidOperationException(
                $"Expected response header '{OntogonyEventHeaders.TraceId}' to be set when no incoming header is present.");
        }

        if (!string.Equals(capturedTraceId, echoed.ToString(), StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Generated trace id '{capturedTraceId}' was not echoed in response (got '{echoed}').");
        }
    }

    /// <summary>
    /// Verifies that <see cref="OntogonyCorrelationContext"/> is populated inside the pipeline for the given <paramref name="context"/>
    /// when processed by the supplied <paramref name="middleware"/>.
    /// </summary>
    public static async Task AssertCorrelationContextPopulatedAsync(
        RequestDelegate middleware,
        HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(middleware);
        ArgumentNullException.ThrowIfNull(context);

        bool populated = false;

        RequestDelegate wrapped = async ctx =>
        {
            populated = OntogonyCorrelationContext.Current is not null &&
                        !string.IsNullOrWhiteSpace(OntogonyCorrelationContext.TraceId);
            await middleware(ctx);
        };

        await wrapped(context);

        if (!populated)
        {
            throw new InvalidOperationException(
                "Expected OntogonyCorrelationContext to be populated (non-null, non-empty TraceId) inside the middleware pipeline.");
        }
    }

    /// <summary>
    /// Verifies that <see cref="CorrelationState.TenantId"/> is propagated from request headers into
    /// <see cref="OntogonyCorrelationContext"/> when using <see cref="RequestTracingMiddleware"/>.
    /// </summary>
    public static async Task AssertTenantIdPropagatedAsync(string tenantId)
    {
        if (string.IsNullOrWhiteSpace(tenantId))
            throw new ArgumentException("tenantId cannot be empty.", nameof(tenantId));

        string? capturedTenantId = null;

        var options = Options.Create(new OntogonyObservabilityOptions { ServiceName = "conformance-test" });
        var middleware = new RequestTracingMiddleware(
            ctx =>
            {
                capturedTenantId = OntogonyCorrelationContext.Current?.TenantId;
                return Task.CompletedTask;
            },
            NullLogger<RequestTracingMiddleware>.Instance,
            options);

        var context = MiddlewareTestHarness.CreateHttpContext("GET", "/resource");
        context.Request.Headers[OntogonyEventHeaders.TraceId] = "trace-ct-001";
        context.Request.Headers[OntogonyEventHeaders.TenantId] = tenantId;

        await middleware.InvokeAsync(context);

        if (!string.Equals(capturedTenantId, tenantId, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Expected TenantId '{tenantId}' in OntogonyCorrelationContext but got '{capturedTenantId}'.");
        }
    }
}
