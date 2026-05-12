using System.Globalization;

namespace Ontogony.Security;

/// <summary>
/// Outgoing HTTP delegating handler that stamps Ontogony service identity HMAC headers.
/// </summary>
public sealed class OntogonyServiceIdentitySigningHandler : DelegatingHandler
{
    private readonly string _serviceId;
    private readonly string _secret;
    private readonly string? _keyId;
    private readonly Func<DateTimeOffset> _utcNow;
    private readonly Func<string> _nonceFactory;

    public OntogonyServiceIdentitySigningHandler(
        string serviceId,
        string secret,
        string? keyId = null,
        Func<DateTimeOffset>? utcNow = null,
        Func<string>? nonceFactory = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(serviceId);
        ArgumentException.ThrowIfNullOrWhiteSpace(secret);

        _serviceId = serviceId;
        _secret = secret;
        _keyId = string.IsNullOrWhiteSpace(keyId) ? null : keyId;
        _utcNow = utcNow ?? (() => DateTimeOffset.UtcNow);
        _nonceFactory = nonceFactory ?? (() => Guid.NewGuid().ToString("n"));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var method = request.Method.Method.ToUpperInvariant();
        var pathAndQuery = request.RequestUri is null
            ? "/"
            : string.IsNullOrEmpty(request.RequestUri.PathAndQuery)
                ? "/"
                : request.RequestUri.PathAndQuery;

        var timestamp = _utcNow().ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        var nonce = _nonceFactory();
        var bodyBytes = await ReadBodyBytesAsync(request, cancellationToken).ConfigureAwait(false);
        var bodyHash = ServiceIdentityHmacSignatureHelper.ComputeBodyHashHexLower(bodyBytes);
        var signature = ServiceIdentityHmacSignatureHelper.ComputeSignatureBase64(
            _secret,
            method,
            pathAndQuery,
            timestamp,
            nonce,
            bodyHash);

        SetHeader(request, OntogonyServiceIdentityHeaders.ServiceId, _serviceId);
        if (_keyId is not null)
        {
            SetHeader(request, OntogonyServiceIdentityHeaders.KeyId, _keyId);
        }
        else
        {
            request.Headers.Remove(OntogonyServiceIdentityHeaders.KeyId);
        }

        SetHeader(request, OntogonyServiceIdentityHeaders.Timestamp, timestamp);
        SetHeader(request, OntogonyServiceIdentityHeaders.Nonce, nonce);
        SetHeader(request, OntogonyServiceIdentityHeaders.BodyHash, bodyHash);
        SetHeader(request, OntogonyServiceIdentityHeaders.Signature, signature);

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }

    private static async Task<byte[]> ReadBodyBytesAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content is null)
            return Array.Empty<byte>();

        return await request.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
    }

    private static void SetHeader(HttpRequestMessage request, string headerName, string value)
    {
        request.Headers.Remove(headerName);
        request.Headers.TryAddWithoutValidation(headerName, value);
    }
}
