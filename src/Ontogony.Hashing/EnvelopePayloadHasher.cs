using Ontogony.Contracts.Events;

namespace Ontogony.Hashing;

/// <summary>
/// Computes deterministic hashes for OntogonyEnvelope payloads and operation fingerprints.
/// Used for deduplication, retry safety, and fingerprinting across protocol recorders.
/// </summary>
public sealed class EnvelopePayloadHasher
{
    private readonly PayloadHasher _payloadHasher;

    /// <summary>Creates a hasher that delegates payload canonicalization to <paramref name="payloadHasher"/>.</summary>
    public EnvelopePayloadHasher(PayloadHasher payloadHasher)
    {
        _payloadHasher = payloadHasher;
    }

    /// <summary>
    /// Compute a deterministic hash of an envelope's payload only.
    /// This hash remains stable regardless of envelope metadata (trace ID, actor, etc.).
    /// </summary>
    public string ComputeEnvelopePayloadHash<TPayload>(OntogonyEnvelope<TPayload> envelope)
    {
        ArgumentNullException.ThrowIfNull(envelope);
        return _payloadHasher.ComputeCanonicalJsonHash(envelope.Payload);
    }

    /// <summary>
    /// Compute a deterministic fingerprint for an operation and payload combined.
    /// Useful for deduplicating requests before they produce side effects.
    /// </summary>
    public string ComputeOperationFingerprint(string operation, object payload)
    {
        if (string.IsNullOrWhiteSpace(operation))
            throw new ArgumentException("Operation cannot be empty", nameof(operation));

        ArgumentNullException.ThrowIfNull(payload);

        var combined = new { operation, payload };
        return _payloadHasher.ComputeCanonicalJsonHash(combined);
    }

    /// <summary>
    /// Compute a fingerprint from a JSON operation name and payload.
    /// </summary>
    public string ComputeOperationFingerprintFromJson(string operation, string json)
    {
        if (string.IsNullOrWhiteSpace(operation))
            throw new ArgumentException("Operation cannot be empty", nameof(operation));

        if (string.IsNullOrWhiteSpace(json))
            throw new ArgumentException("JSON cannot be empty", nameof(json));

        // Parse and re-serialize to ensure canonical form
        using var doc = System.Text.Json.JsonDocument.Parse(json);
        var payloadObj = System.Text.Json.JsonSerializer.Deserialize<object>(doc.RootElement.GetRawText());

        return ComputeOperationFingerprint(operation, payloadObj!);
    }
}
