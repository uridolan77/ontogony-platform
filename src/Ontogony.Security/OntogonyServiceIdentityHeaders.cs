namespace Ontogony.Security;

/// <summary>
/// Canonical HTTP header names for production HMAC service-to-service verification.
/// </summary>
public static class OntogonyServiceIdentityHeaders
{
    public const string ServiceId = "X-Ontogony-Service-Id";
    public const string KeyId = "X-Ontogony-Service-Key-Id";
    public const string Timestamp = "X-Ontogony-Service-Timestamp";
    public const string Nonce = "X-Ontogony-Service-Nonce";
    public const string BodyHash = "X-Ontogony-Service-Body-Hash";
    public const string Signature = "X-Ontogony-Service-Signature";
}
