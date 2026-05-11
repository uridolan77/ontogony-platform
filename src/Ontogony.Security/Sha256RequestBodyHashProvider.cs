using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Ontogony.Security;

/// <summary>
/// Default <see cref="IRequestBodyHashProvider"/> using SHA-256 over the raw request body bytes with a configurable size cap.
/// </summary>
public sealed class Sha256RequestBodyHashProvider : IRequestBodyHashProvider
{
    private readonly IOptions<ServiceIdentityOptions> _options;

    /// <summary>Creates a provider using the supplied options snapshot (typically from DI).</summary>
    public Sha256RequestBodyHashProvider(IOptions<ServiceIdentityOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _options = options;
    }

    /// <summary>Creates a provider for tests or hosts without DI.</summary>
    public Sha256RequestBodyHashProvider(ServiceIdentityOptions options)
        : this(Options.Create(options))
    {
    }

    /// <inheritdoc />
    public RequestBodyHashResult TryComputeSha256HexLower(HttpRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var opt = _options.Value;
        var max = opt.MaxSignedBodyBytes;

        if (opt.AllowUnsignedEmptyBody && HttpRequestBodyAnalysis.IsDefinitelyEmptyBody(request))
        {
            return new RequestBodyHashResult(
                TooLarge: false,
                HexLower: ServiceIdentityHmacSignatureHelper.ComputeBodyHashHexLower(ReadOnlySpan<byte>.Empty));
        }

        if (request.ContentLength is > 0 and var len && len > max)
            return new RequestBodyHashResult(TooLarge: true, HexLower: null);

        request.EnableBuffering();
        var body = request.Body;
        if (body.CanSeek)
            body.Position = 0;

        using var ms = new MemoryStream(capacity: request.ContentLength is > 0 and var known && known <= max ? (int)known : 4096);
        var buffer = new byte[8192];
        long observed;
        try
        {
            observed = CopyBodyWithLimit(body, ms, buffer, max);
        }
        catch
        {
            if (body.CanSeek)
                body.Position = 0;
            throw;
        }

        if (observed > max)
        {
            if (body.CanSeek)
                body.Position = 0;
            return new RequestBodyHashResult(TooLarge: true, HexLower: null);
        }

        var bytes = ms.ToArray();
        if (body.CanSeek)
            body.Position = 0;

        return new RequestBodyHashResult(
            TooLarge: false,
            HexLower: Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant());
    }

    /// <summary>Returns total bytes observed from the source (may exceed <paramref name="maxBytes"/> by at most one).</summary>
    private static long CopyBodyWithLimit(Stream source, MemoryStream destination, byte[] buffer, int maxBytes)
    {
        long total = 0;
        while (true)
        {
            var remainingBudget = maxBytes - total + 1;
            if (remainingBudget <= 0)
                return total;

            var toRead = (int)Math.Min(buffer.Length, remainingBudget);
            var read = source.Read(buffer.AsSpan(0, toRead));
            if (read == 0)
                return total;

            var newTotal = total + read;
            if (newTotal <= maxBytes)
            {
                destination.Write(buffer.AsSpan(0, read));
                total = newTotal;
                continue;
            }

            return newTotal;
        }
    }
}
