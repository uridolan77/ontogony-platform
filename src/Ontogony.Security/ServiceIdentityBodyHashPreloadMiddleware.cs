using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Ontogony.Security;

/// <summary>
/// When <see cref="ServiceIdentityOptions.RequireHmacSignature"/> is enabled, asynchronously reads and hashes the request body
/// up to <see cref="ServiceIdentityOptions.MaxSignedBodyBytes"/>, stores the result on <see cref="HttpContext.Items"/>,
/// and replaces <see cref="HttpRequest.Body"/> with a bounded in-memory stream so downstream components can re-read it.
/// </summary>
public sealed class ServiceIdentityBodyHashPreloadMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IOptions<ServiceIdentityOptions> _options;
    private readonly ILogger<ServiceIdentityBodyHashPreloadMiddleware> _logger;

    public ServiceIdentityBodyHashPreloadMiddleware(
        RequestDelegate next,
        IOptions<ServiceIdentityOptions> options,
        ILogger<ServiceIdentityBodyHashPreloadMiddleware>? logger = null)
    {
        _next = next;
        _options = options;
        _logger = logger ?? NullLogger<ServiceIdentityBodyHashPreloadMiddleware>.Instance;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var opt = _options.Value;
        if (!opt.RequireHmacSignature)
        {
            await _next(context).ConfigureAwait(false);
            return;
        }

        if (!HttpRequestBodyAnalysis.MayHavePayloadBody(context.Request))
        {
            await _next(context).ConfigureAwait(false);
            return;
        }

        if (string.IsNullOrWhiteSpace(context.Request.Headers[opt.ServiceIdHeaderName].ToString()))
        {
            await _next(context).ConfigureAwait(false);
            return;
        }

        if (opt.EnableBodyHashPreloadOrderDiagnostics && context.GetEndpoint() is not null)
        {
            const string message =
                "ServiceIdentityBodyHashPreloadMiddleware is running after endpoint selection. " +
                "Call UseOntogonyServiceIdentityBodyHashPreload() before UseRouting() and before components that consume request bodies.";
            if (opt.ThrowOnBodyHashPreloadOrderViolation)
            {
                throw new InvalidOperationException(message);
            }

            _logger.LogWarning(message);
        }

        var max = opt.MaxSignedBodyBytes;

        if (opt.AllowUnsignedEmptyBody && HttpRequestBodyAnalysis.IsDefinitelyEmptyBody(context.Request))
        {
            var emptyHex = ServiceIdentityHmacSignatureHelper.ComputeBodyHashHexLower(ReadOnlySpan<byte>.Empty);
            context.Items[ServiceIdentityBodyHashContext.HttpContextItemKey] =
                new ServiceIdentityBodyHashContext.Precomputed(TooLarge: false, HexLower: emptyHex);
            await _next(context).ConfigureAwait(false);
            return;
        }

        if (context.Request.ContentLength is > 0 and var knownLen && knownLen > max)
        {
            context.Items[ServiceIdentityBodyHashContext.HttpContextItemKey] =
                new ServiceIdentityBodyHashContext.Precomputed(TooLarge: true, HexLower: null);
            await _next(context).ConfigureAwait(false);
            return;
        }

        context.Request.EnableBuffering();
        var original = context.Request.Body;
        if (original.CanSeek)
            original.Position = 0;

        var (captured, tooLarge) = await ReadBodyWithLimitAsync(original, max, context.RequestAborted).ConfigureAwait(false);

        if (tooLarge)
        {
            if (original.CanSeek)
                original.Position = 0;

            context.Items[ServiceIdentityBodyHashContext.HttpContextItemKey] =
                new ServiceIdentityBodyHashContext.Precomputed(TooLarge: true, HexLower: null);
            await _next(context).ConfigureAwait(false);
            return;
        }

        var hex = Convert.ToHexString(SHA256.HashData(captured)).ToLowerInvariant();
        context.Items[ServiceIdentityBodyHashContext.HttpContextItemKey] =
            new ServiceIdentityBodyHashContext.Precomputed(TooLarge: false, HexLower: hex);

        var replacement = new MemoryStream(captured, writable: false);
        context.Request.Body = replacement;
        context.Request.ContentLength = captured.Length;

        try
        {
            await _next(context).ConfigureAwait(false);
        }
        finally
        {
            context.Request.Body = original;
            if (original.CanSeek)
                original.Position = 0;
        }
    }

    private static async Task<(byte[] Captured, bool TooLarge)> ReadBodyWithLimitAsync(Stream body, int maxBytes, CancellationToken cancellationToken)
    {
        if (maxBytes < 0)
            throw new ArgumentOutOfRangeException(nameof(maxBytes));

        using var ms = new MemoryStream(Math.Min(maxBytes, 65536));
        var buffer = new byte[8192];
        long total = 0;
        while (true)
        {
            var remainingBudget = maxBytes - total + 1;
            if (remainingBudget <= 0)
                return (Array.Empty<byte>(), TooLarge: true);

            var toRead = (int)Math.Min(buffer.Length, remainingBudget);
            var read = await body.ReadAsync(buffer.AsMemory(0, toRead), cancellationToken).ConfigureAwait(false);
            if (read == 0)
            {
                return (ms.ToArray(), TooLarge: false);
            }

            var newTotal = total + read;
            if (newTotal <= maxBytes)
            {
                await ms.WriteAsync(buffer.AsMemory(0, read), cancellationToken).ConfigureAwait(false);
                total = newTotal;
                continue;
            }

            return (Array.Empty<byte>(), TooLarge: true);
        }
    }
}
