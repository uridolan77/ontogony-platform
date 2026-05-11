using Ontogony.Hashing;

namespace Ontogony.Idempotency;

public sealed class IdempotencyKeyBuilder
{
    private readonly PayloadHasher _payloadHasher;

    public IdempotencyKeyBuilder(PayloadHasher payloadHasher)
    {
        _payloadHasher = payloadHasher;
    }

    public string FromParts(string operation, params object?[] parts)
    {
        var payload = new { operation, parts };
        return $"{operation}:{_payloadHasher.ComputeCanonicalJsonHash(payload)}";
    }
}
