using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Ontogony.Contracts.Events;
using Ontogony.Primitives;

namespace Ontogony.Security;

/// <summary>
/// Accessor for service-to-service identity verification using HTTP headers.
/// Supports optional <b>StaticSharedSecret</b> comparison (development / internal only) and
/// production-style <b>HMAC-SHA256</b> over a canonical request string.
/// </summary>
public sealed class ServiceIdentityCurrentActorAccessor : ICurrentActorAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ServiceIdentityOptions _options;
    private readonly IServiceSecretResolver _secretResolver;
    private readonly INonceReplayStore? _nonceReplayStore;
    private readonly IRequestBodyHashProvider _bodyHashProvider;
    private readonly IClock _clock;

    public ServiceIdentityCurrentActorAccessor(
        IHttpContextAccessor httpContextAccessor,
        ServiceIdentityOptions? options = null,
        IServiceSecretResolver? secretResolver = null,
        INonceReplayStore? nonceReplayStore = null,
        IRequestBodyHashProvider? bodyHashProvider = null,
        IClock? clock = null)
    {
        _httpContextAccessor = httpContextAccessor;
        _options = options ?? new ServiceIdentityOptions();
        _secretResolver = secretResolver ?? new DictionaryServiceSecretResolver(_options.ServiceSecrets);
        _nonceReplayStore = nonceReplayStore;
        _bodyHashProvider = bodyHashProvider ?? new Sha256RequestBodyHashProvider(Options.Create(_options));
        _clock = clock ?? new SystemClock();
    }

    public CurrentActor? Current
    {
        get
        {
            var context = _httpContextAccessor.HttpContext;
            if (context is null)
                return null;

            var serviceId = context.Request.Headers[_options.ServiceIdHeaderName].ToString();
            if (string.IsNullOrWhiteSpace(serviceId))
                return null;

            if (_options.RequireHmacSignature)
            {
                if (!VerifyHmacSignature(context, serviceId))
                    return null;
            }
            else if (_options.RequireSignatureVerification)
            {
                if (!VerifyStaticSharedSecretSignature(context, serviceId))
                    return null;
            }

            return new CurrentActor(
                serviceId,
                OntogonyActorTypes.Service,
                new[] { OntogonyRoleNames.Service },
                context.Request.Headers[OntogonyEventHeaders.TenantId].ToString(),
                context.Request.Headers[OntogonyEventHeaders.WorkspaceId].ToString(),
                context.Request.Headers[OntogonyEventHeaders.ProjectId].ToString());
        }
    }

    private bool VerifyStaticSharedSecretSignature(HttpContext context, string serviceId)
    {
        var signature = context.Request.Headers[_options.SignatureHeaderName].ToString();
        if (string.IsNullOrWhiteSpace(signature))
            return false;

        var expectedSignature = _options.GetExpectedSignature(serviceId);
        if (string.IsNullOrWhiteSpace(expectedSignature))
            return false;

        var providedBytes = Encoding.UTF8.GetBytes(signature);
        var expectedBytes = Encoding.UTF8.GetBytes(expectedSignature);
        return CryptographicOperations.FixedTimeEquals(providedBytes, expectedBytes);
    }

    private bool VerifyHmacSignature(HttpContext context, string serviceId)
    {
        var secret = _secretResolver.ResolveSecret(serviceId);
        if (string.IsNullOrEmpty(secret))
            return false;

        var timestampHeader = context.Request.Headers[_options.ServiceTimestampHeaderName].ToString();
        if (string.IsNullOrWhiteSpace(timestampHeader) || !long.TryParse(timestampHeader, out var unixSeconds))
            return false;

        var requestUtc = DateTimeOffset.FromUnixTimeSeconds(unixSeconds);
        var now = _clock.UtcNow;
        if (now - requestUtc > _options.MaxTimestampSkew || requestUtc - now > _options.MaxTimestampSkew)
            return false;

        var nonce = context.Request.Headers[_options.ServiceNonceHeaderName].ToString();
        if (_options.RequireNonce && string.IsNullOrWhiteSpace(nonce))
            return false;

        var declaredHeader = context.Request.Headers[_options.ServiceBodyHashHeaderName].ToString();
        string bodyHashNorm;
        if (string.IsNullOrWhiteSpace(declaredHeader))
        {
            if (!_options.AllowUnsignedEmptyBody || !HttpRequestBodyAnalysis.IsDefinitelyEmptyBody(context.Request))
                return false;

            bodyHashNorm = ServiceIdentityHmacSignatureHelper.ComputeBodyHashHexLower(ReadOnlySpan<byte>.Empty);
        }
        else
        {
            bodyHashNorm = declaredHeader.Trim().ToLowerInvariant();
        }

        if (context.Items.TryGetValue(ServiceIdentityBodyHashContext.HttpContextItemKey, out var preItem)
            && preItem is ServiceIdentityBodyHashContext.Precomputed pre)
        {
            if (pre.TooLarge)
                return false;

            if (pre.HexLower is null || !FixedTimeEqualsAsciiHex(bodyHashNorm, pre.HexLower))
                return false;
        }
        else
        {
            if (_options.RequirePreloadedBodyHashForHmacBodies
                && !HttpRequestBodyAnalysis.IsDefinitelyEmptyBody(context.Request))
            {
                return false;
            }

            var computed = _bodyHashProvider.TryComputeSha256HexLower(context.Request);
            if (computed.TooLarge || !computed.Succeeded)
                return false;

            if (!FixedTimeEqualsAsciiHex(bodyHashNorm, computed.HexLower!))
                return false;
        }

        var signatureHeader = context.Request.Headers[_options.SignatureHeaderName].ToString();
        if (string.IsNullOrWhiteSpace(signatureHeader))
            return false;

        byte[] providedBytes;
        try
        {
            providedBytes = Convert.FromBase64String(signatureHeader);
        }
        catch (FormatException)
        {
            return false;
        }

        var method = context.Request.Method.ToUpperInvariant();
        var path = context.Request.Path.HasValue ? context.Request.Path.Value! : "/";
        var pathAndQuery = path + context.Request.QueryString.Value;
        var nonceForCanonical = _options.RequireNonce ? nonce : string.Empty;
        var canonical = ServiceIdentityHmacSignatureHelper.BuildCanonical(
            method,
            pathAndQuery,
            timestampHeader,
            nonceForCanonical,
            bodyHashNorm);

        var expected = ServiceIdentityHmacSignatureHelper.ComputeHmacUtf8(secret, canonical);
        if (!CryptographicOperations.FixedTimeEquals(expected, providedBytes))
            return false;

        if (_options.RequireNonce)
        {
            if (_nonceReplayStore is null || !_nonceReplayStore.TryReserveNonce(serviceId, nonce, requestUtc))
                return false;
        }

        return true;
    }

    private static bool FixedTimeEqualsAsciiHex(string a, string b)
    {
        var aa = a.Trim().ToLowerInvariant();
        var bb = b.Trim().ToLowerInvariant();
        if (aa.Length != bb.Length)
            return false;

        var ba = Encoding.ASCII.GetBytes(aa);
        var bbBytes = Encoding.ASCII.GetBytes(bb);
        return CryptographicOperations.FixedTimeEquals(ba, bbBytes);
    }
}
