namespace Ontogony.Hashing;

/// <summary>
/// Canonical JSON + SHA-256 helpers for stable payload fingerprints.
/// </summary>
public sealed class PayloadHasher
{
    private readonly IContentHashService _hashService;

    /// <summary>Creates a payload hasher using <paramref name="hashService"/>.</summary>
    public PayloadHasher(IContentHashService hashService)
    {
        _hashService = hashService;
    }

    /// <summary>Canonicalizes <paramref name="payload"/> to JSON then returns SHA-256 hex.</summary>
    public string ComputeCanonicalJsonHash<T>(T payload)
    {
        var canonical = CanonicalJson.Serialize(payload);
        return _hashService.ComputeUtf8Sha256(canonical);
    }

    /// <summary>Parses JSON, canonicalizes, then returns SHA-256 hex.</summary>
    public string ComputeCanonicalJsonHashFromJson(string json)
    {
        var canonical = CanonicalJson.Normalize(json);
        return _hashService.ComputeUtf8Sha256(canonical);
    }

    /// <summary>
    /// Computes a SHA-256 hash from the exact input JSON string bytes (UTF-8), without canonicalization.
    /// </summary>
    public string ComputeRawJsonHash(string json)
    {
        return _hashService.ComputeUtf8Sha256(json);
    }
}
