using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Ontogony.Testing;

public static class MiddlewareTestHarness
{
    public static DefaultHttpContext CreateHttpContext(string method = "GET", string path = "/")
    {
        var context = new DefaultHttpContext();
        context.Request.Method = method;
        context.Request.Path = path;
        context.Response.Body = new MemoryStream();
        return context;
    }

    public static async Task RunAsync(RequestDelegate middleware, HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(middleware);
        ArgumentNullException.ThrowIfNull(context);
        await middleware(context).ConfigureAwait(false);
    }

    public static string ReadResponseBody(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        if (context.Response.Body.CanSeek)
        {
            context.Response.Body.Position = 0;
        }

        using var reader = new StreamReader(context.Response.Body, Encoding.UTF8, leaveOpen: true);
        return reader.ReadToEnd();
    }

    public static void AssertResponseHeader(HttpContext context, string headerName, string expectedValue)
    {
        ArgumentNullException.ThrowIfNull(context);
        if (!context.Response.Headers.TryGetValue(headerName, out var actual) || !string.Equals(actual.ToString(), expectedValue, StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"Expected header '{headerName}' to be '{expectedValue}'.");
        }
    }

    public static void AssertResponseJsonContains(HttpContext context, string propertyName)
    {
        var body = ReadResponseBody(context);
        using var document = JsonDocument.Parse(body);
        if (!document.RootElement.TryGetProperty(propertyName, out _))
        {
            throw new InvalidOperationException($"Expected response JSON to contain property '{propertyName}'.");
        }
    }
}