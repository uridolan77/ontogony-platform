namespace Ontogony.Security;

/// <summary>
/// Canonical HTTP header names for production HMAC service-to-service verification.
/// </summary>
public static class OntogonyServiceIdentityHeaders
{
    /// <summary>Calling service id.</summary>
    public const string ServiceId = "X-Ontogony-Service-Id";

    /// <summary>Signing key version id.</summary>
    public const string KeyId = "X-Ontogony-Service-Key-Id";

    /// <summary>Unix seconds timestamp for skew checks.</summary>
    public const string Timestamp = "X-Ontogony-Service-Timestamp";

    /// <summary>Unique nonce for replay protection.</summary>
    public const string Nonce = "X-Ontogony-Service-Nonce";

    /// <summary>SHA-256 hex (lowercase) of the raw request body bytes.</summary>
    public const string BodyHash = "X-Ontogony-Service-Body-Hash";

    /// <summary>Base64 HMAC signature over the canonical signing string.</summary>
    public const string Signature = "X-Ontogony-Service-Signature";
}
