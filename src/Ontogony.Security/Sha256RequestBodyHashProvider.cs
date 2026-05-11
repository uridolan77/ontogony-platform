using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;

namespace Ontogony.Security;

/// <summary>
/// Default <see cref="IRequestBodyHashProvider"/> using SHA-256 over the raw request body bytes.
/// </summary>
public sealed class Sha256RequestBodyHashProvider : IRequestBodyHashProvider
{
    public static Sha256RequestBodyHashProvider Instance { get; } = new();

    public string ComputeSha256HexLower(HttpRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        request.EnableBuffering();
        var body = request.Body;
        if (body.CanSeek)
            body.Position = 0;

        using var ms = new MemoryStream();
        body.CopyTo(ms);
        var bytes = ms.ToArray();

        if (body.CanSeek)
            body.Position = 0;

        return Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
    }
}
