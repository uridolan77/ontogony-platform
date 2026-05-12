namespace Ontogony.Hashing;

public sealed class PayloadHasher
{
    private readonly IContentHashService _hashService;

    public PayloadHasher(IContentHashService hashService)
    {
        _hashService = hashService;
    }

    public string ComputeCanonicalJsonHash<T>(T payload)
    {
        var canonical = CanonicalJson.Serialize(payload);
        return _hashService.ComputeUtf8Sha256(canonical);
    }

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
