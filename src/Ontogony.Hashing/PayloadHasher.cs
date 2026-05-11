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
}
