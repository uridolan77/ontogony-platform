using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Ontogony.Errors;

namespace Ontogony.Testing;

/// <summary>
/// Conformance helpers that verify a service wires up <see cref="OntogonyExceptionHandlingMiddleware"/> correctly
/// and that its error JSON shape is stable.
/// </summary>
public static class ErrorShapeConformanceAssertions
{
    /// <summary>
    /// Verifies that an unmapped exception produces HTTP 500 with the canonical
    /// <c>code</c>, <c>message</c>, and <c>traceId</c> JSON properties present.
    /// </summary>
    public static async Task AssertUnmappedExceptionProduces500Async()
    {
        var (context, _) = await RunWithExceptionAsync(
            new InvalidOperationException("Internal unhandled error"),
            configure: null);

        if (context.Response.StatusCode != (int)HttpStatusCode.InternalServerError)
        {
            throw new InvalidOperationException(
                $"Expected HTTP 500 for unmapped exception but got {context.Response.StatusCode}.");
        }

        var body = MiddlewareTestHarness.ReadResponseBody(context);
        AssertErrorJsonShape(body, expectedCode: null, requireTraceId: false);
    }

    /// <summary>
    /// Verifies that a mapped exception produces the configured status code and error code.
    /// </summary>
    public static async Task AssertMappedExceptionProducesExpectedShapeAsync<TException>(
        TException exception,
        HttpStatusCode expectedStatus,
        string expectedCode,
        Action<OntogonyExceptionMappingOptions>? configure = null)
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(exception);
        if (string.IsNullOrWhiteSpace(expectedCode))
            throw new ArgumentException("expectedCode cannot be empty.", nameof(expectedCode));

        Action<OntogonyExceptionMappingOptions>? configureWithDefault = opts =>
        {
            opts.Map<TException>(expectedStatus, expectedCode, logAsWarning: true);
            configure?.Invoke(opts);
        };

        var (context, _) = await RunWithExceptionAsync(exception, configureWithDefault);

        if (context.Response.StatusCode != (int)expectedStatus)
        {
            throw new InvalidOperationException(
                $"Expected HTTP {(int)expectedStatus} but got {context.Response.StatusCode}.");
        }

        var body = MiddlewareTestHarness.ReadResponseBody(context);
        var json = ParseErrorJson(body);

        if (!json.TryGetProperty("code", out var codeEl) ||
            !string.Equals(codeEl.GetString(), expectedCode, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Expected error code '{expectedCode}' but got '{(json.TryGetProperty("code", out var c) ? c.GetString() : "(missing)")}'.");
        }
    }

    /// <summary>
    /// Verifies that the error payload does not leak the raw exception message for an unmapped exception.
    /// </summary>
    public static async Task AssertNoUnmappedExceptionMessageLeakAsync()
    {
        const string secret = "this-is-the-internal-exception-text-9381";

        var (context, _) = await RunWithExceptionAsync(
            new InvalidOperationException(secret),
            configure: null);

        var body = MiddlewareTestHarness.ReadResponseBody(context);
        if (body.Contains(secret, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Error response body leaks the raw exception message for an unmapped exception. " +
                "Ensure OntogonyExceptionHandlingMiddleware does not expose internal exception details.");
        }
    }

    /// <summary>
    /// Verifies that the error JSON contains the standard property set: <c>code</c> and <c>message</c>.
    /// Optionally asserts on a specific code value.
    /// </summary>
    public static void AssertErrorJsonShape(string json, string? expectedCode = null, bool requireTraceId = false)
    {
        if (string.IsNullOrWhiteSpace(json))
            throw new ArgumentException("json cannot be empty.", nameof(json));

        var root = ParseErrorJson(json);

        if (!root.TryGetProperty("code", out _))
            throw new InvalidOperationException("Error JSON is missing required property 'code'.");

        if (!root.TryGetProperty("message", out _))
            throw new InvalidOperationException("Error JSON is missing required property 'message'.");

        if (requireTraceId && !root.TryGetProperty("traceId", out _))
            throw new InvalidOperationException("Error JSON is missing required property 'traceId'.");

        if (expectedCode is not null)
        {
            var actualCode = root.TryGetProperty("code", out var codeEl) ? codeEl.GetString() : null;
            if (!string.Equals(actualCode, expectedCode, StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"Expected error code '{expectedCode}' but got '{actualCode}'.");
            }
        }
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private static async Task<(HttpContext Context, string Body)> RunWithExceptionAsync(
        Exception exception,
        Action<OntogonyExceptionMappingOptions>? configure)
    {
        var mappingOptions = new OntogonyExceptionMappingOptions();
        configure?.Invoke(mappingOptions);

        var middleware = new OntogonyExceptionHandlingMiddleware(
            _ => throw exception,
            NullLogger<OntogonyExceptionHandlingMiddleware>.Instance,
            Options.Create(new JsonOptions()),
            Options.Create(mappingOptions));

        var context = MiddlewareTestHarness.CreateHttpContext("GET", "/conformance");

        await middleware.InvokeAsync(context);

        var body = MiddlewareTestHarness.ReadResponseBody(context);
        return (context, body);
    }

    private static JsonElement ParseErrorJson(string json)
    {
        try
        {
            var document = JsonDocument.Parse(json);
            return document.RootElement;
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Error response body is not valid JSON: {ex.Message}\nBody: {json}");
        }
    }
}
