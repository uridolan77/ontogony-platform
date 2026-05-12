using Ontogony.Contracts.Events;
using Ontogony.Hashing;

namespace Ontogony.Testing;

/// <summary>
/// Conformance helpers that verify an <see cref="OntogonyEnvelope{TPayload}"/> meets platform standards.
/// Complements the lower-level <see cref="EnvelopeAssertions"/> with source/type pattern checks and hash verification.
/// </summary>
public static class EnvelopeConformanceAssertions
{
    /// <summary>
    /// Asserts that <see cref="OntogonyEnvelope{TPayload}.Source"/> is set and follows the canonical
    /// <c>ontogony://&lt;service&gt;/&lt;domain&gt;</c> scheme.
    /// </summary>
    public static void AssertSourceFollowsScheme<TPayload>(OntogonyEnvelope<TPayload> envelope)
    {
        ArgumentNullException.ThrowIfNull(envelope);

        if (string.IsNullOrWhiteSpace(envelope.Source))
            throw new InvalidOperationException("Envelope Source must be set.");

        if (!envelope.Source.StartsWith("ontogony://", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Envelope Source '{envelope.Source}' does not follow the 'ontogony://<service>/<domain>' scheme.");
        }
    }

    /// <summary>
    /// Asserts that <see cref="OntogonyEnvelope{TPayload}.EventType"/> is namespaced
    /// (contains at least one dot), following the convention <c>ontogony.&lt;domain&gt;.&lt;event&gt;</c>.
    /// </summary>
    public static void AssertEventTypeIsNamespaced<TPayload>(OntogonyEnvelope<TPayload> envelope)
    {
        ArgumentNullException.ThrowIfNull(envelope);

        if (string.IsNullOrWhiteSpace(envelope.EventType))
            throw new InvalidOperationException("Envelope EventType must be set.");

        if (!envelope.EventType.Contains('.'))
        {
            throw new InvalidOperationException(
                $"Envelope EventType '{envelope.EventType}' must be namespaced (e.g. 'ontogony.entity.action'). Missing dot separator.");
        }
    }

    /// <summary>
    /// Asserts that <see cref="OntogonyEnvelope{TPayload}.SchemaVersion"/> is present and non-empty.
    /// </summary>
    public static void AssertSchemaVersionPresent<TPayload>(OntogonyEnvelope<TPayload> envelope)
    {
        ArgumentNullException.ThrowIfNull(envelope);

        if (string.IsNullOrWhiteSpace(envelope.SchemaVersion))
            throw new InvalidOperationException("Envelope SchemaVersion must be set.");
    }

    /// <summary>
    /// Asserts that <see cref="OntogonyEnvelope{TPayload}.PayloadHash"/> is present and matches
    /// the canonical JSON SHA-256 hash of the envelope payload.
    /// </summary>
    public static void AssertPayloadHashMatchesContent<TPayload>(OntogonyEnvelope<TPayload> envelope)
    {
        ArgumentNullException.ThrowIfNull(envelope);

        if (string.IsNullOrWhiteSpace(envelope.PayloadHash))
            throw new InvalidOperationException("Envelope PayloadHash must be present for hash verification.");

        var hasher = new EnvelopePayloadHasher(new PayloadHasher(new Sha256ContentHashService()));
        var expected = hasher.ComputeEnvelopePayloadHash(envelope);

        if (!string.Equals(envelope.PayloadHash, expected, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Envelope PayloadHash mismatch. Declared: '{envelope.PayloadHash}', computed: '{expected}'.");
        }
    }

    /// <summary>
    /// Runs all standard conformance checks against the envelope in a single call:
    /// required fields, source scheme, namespaced event type, schema version, and payload hash (if present).
    /// </summary>
    public static void AssertFullConformance<TPayload>(OntogonyEnvelope<TPayload> envelope, bool requirePayloadHash = false)
    {
        EnvelopeAssertions.AssertHasRequiredFields(envelope);
        AssertSourceFollowsScheme(envelope);
        AssertEventTypeIsNamespaced(envelope);
        AssertSchemaVersionPresent(envelope);

        if (requirePayloadHash || !string.IsNullOrWhiteSpace(envelope.PayloadHash))
        {
            AssertPayloadHashMatchesContent(envelope);
        }
    }
}
