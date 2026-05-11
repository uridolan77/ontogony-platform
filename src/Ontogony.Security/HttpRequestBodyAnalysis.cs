using Microsoft.AspNetCore.Http;

namespace Ontogony.Security;

/// <summary>
/// Mechanical classification of HTTP request bodies for service-identity hashing.
/// </summary>
public static class HttpRequestBodyAnalysis
{
    /// <summary>
    /// True when the request is safe to treat as empty without reading the body stream
    /// (for example GET/HEAD or explicit zero content-length).
    /// </summary>
    public static bool IsDefinitelyEmptyBody(HttpRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.ContentLength == 0)
            return true;

        return HttpMethods.IsGet(request.Method)
            || HttpMethods.IsHead(request.Method)
            || HttpMethods.IsOptions(request.Method)
            || HttpMethods.IsTrace(request.Method);
    }

    /// <summary>HTTP methods that may carry a payload body worth hashing.</summary>
    public static bool MayHavePayloadBody(HttpRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return HttpMethods.IsPost(request.Method)
            || HttpMethods.IsPut(request.Method)
            || HttpMethods.IsPatch(request.Method)
            || HttpMethods.IsDelete(request.Method);
    }
}
