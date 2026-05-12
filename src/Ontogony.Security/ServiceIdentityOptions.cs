namespace Ontogony.Security;

/// <summary>
/// Configuration for <see cref="ServiceIdentityCurrentActorAccessor"/>.
/// </summary>
public sealed class ServiceIdentityOptions
{
    /// <summary>Header name containing service ID. Default: <see cref="OntogonyServiceIdentityHeaders.ServiceId"/>.</summary>
    public string ServiceIdHeaderName { get; set; } = OntogonyServiceIdentityHeaders.ServiceId;

    /// <summary>Header name containing the signature (static or HMAC). Default: <see cref="OntogonyServiceIdentityHeaders.Signature"/>.</summary>
    public string SignatureHeaderName { get; set; } = OntogonyServiceIdentityHeaders.Signature;

    /// <summary>Unix epoch seconds (decimal string). Default: <see cref="OntogonyServiceIdentityHeaders.Timestamp"/>.</summary>
    public string ServiceTimestampHeaderName { get; set; } = OntogonyServiceIdentityHeaders.Timestamp;

    /// <summary>Default: <see cref="OntogonyServiceIdentityHeaders.KeyId"/>.</summary>
    public string ServiceKeyIdHeaderName { get; set; } = OntogonyServiceIdentityHeaders.KeyId;

    /// <summary>Default: <see cref="OntogonyServiceIdentityHeaders.Nonce"/>.</summary>
    public string ServiceNonceHeaderName { get; set; } = OntogonyServiceIdentityHeaders.Nonce;

    /// <summary>Default: <see cref="OntogonyServiceIdentityHeaders.BodyHash"/>.</summary>
    public string ServiceBodyHashHeaderName { get; set; } = OntogonyServiceIdentityHeaders.BodyHash;

    /// <summary>
    /// When true without <see cref="RequireHmacSignature"/>, enables <b>StaticSharedSecret</b> mode:
    /// the signature header must match the configured secret string (constant-time UTF-8 compare).
    /// Intended for internal or development scenarios only; it is not a keyed MAC over the request.
    /// </summary>
    public bool RequireSignatureVerification { get; set; }

    /// <summary>
    /// When true, requires a valid HMAC-SHA256 signature over the canonical request string using the resolved secret.
    /// Takes precedence over <see cref="RequireSignatureVerification"/>.
    /// </summary>
    public bool RequireHmacSignature { get; set; }

    /// <summary>Allowed clock skew when validating <see cref="OntogonyServiceIdentityHeaders.Timestamp"/>.</summary>
    public TimeSpan MaxTimestampSkew { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// When true in HMAC mode, a nonce header is required and must be accepted by <see cref="INonceReplayStore"/>.
    /// </summary>
    public bool RequireNonce { get; set; } = true;

    /// <summary>
    /// Dictionary mapping service IDs to shared secrets (UTF-8 strings) used when no custom <see cref="IServiceSecretResolver"/> is supplied.
    /// </summary>
    public Dictionary<string, string> ServiceSecrets { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// Optional multi-secret signing material keyed by service ID.
    /// When set, HMAC verification can resolve current/previous keys for rotation scenarios.
    /// </summary>
    public Dictionary<string, List<ServiceSigningSecret>> ServiceSigningSecrets { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// When true in HMAC mode, callers must send <see cref="ServiceKeyIdHeaderName"/>.
    /// When false, behavior is explicit: resolve the resolver-selected/current key when key-id is missing.
    /// </summary>
    public bool RequireKeyIdForHmacSignature { get; set; }

    /// <summary>
    /// Maximum number of raw request body bytes considered when recomputing the SHA-256 for HMAC verification.
    /// Bodies larger than this fail verification without hashing the remainder.
    /// </summary>
    public int MaxSignedBodyBytes { get; set; } = 1_000_000;

    /// <summary>
    /// When true and the request is <see cref="HttpRequestBodyAnalysis.IsDefinitelyEmptyBody"/> (for example GET or <c>Content-Length: 0</c>),
    /// a missing <see cref="OntogonyServiceIdentityHeaders.BodyHash"/> header is treated as the hash of the empty byte sequence.
    /// </summary>
    public bool AllowUnsignedEmptyBody { get; set; } = true;

    /// <summary>
    /// When true in HMAC mode, a request body hash must have been precomputed by
    /// <c>ServiceIdentityBodyHashPreloadMiddleware</c> on <see cref="Microsoft.AspNetCore.Http.HttpContext.Items"/>;
    /// synchronous body hashing via <see cref="IRequestBodyHashProvider"/> is not used for non-empty bodies.
    /// Empty bodies may still use <see cref="AllowUnsignedEmptyBody"/> without preload when no body hash header is sent.
    /// </summary>
    public bool RequirePreloadedBodyHashForHmacBodies { get; set; }

    /// <summary>
    /// Enables middleware-order diagnostics in <c>ServiceIdentityBodyHashPreloadMiddleware</c>.
    /// If the middleware observes that endpoint selection already ran, ordering is likely unsafe for body preloading.
    /// </summary>
    public bool EnableBodyHashPreloadOrderDiagnostics { get; set; } = true;

    /// <summary>
    /// When true and an order violation is detected while diagnostics are enabled, request processing throws.
    /// When false, the middleware logs a warning and continues.
    /// </summary>
    public bool ThrowOnBodyHashPreloadOrderViolation { get; set; }

    /// <summary>
    /// Gets the expected static signature for a service ID (StaticSharedSecret mode).
    /// </summary>
    public string? GetExpectedSignature(string serviceId)
    {
        ServiceSecrets.TryGetValue(serviceId, out var secret);
        return secret;
    }
}
