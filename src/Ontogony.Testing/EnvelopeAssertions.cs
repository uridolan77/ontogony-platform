using Ontogony.Contracts.Events;

namespace Ontogony.Testing;

public static class EnvelopeAssertions
{
    public static void AssertHasRequiredFields<TPayload>(OntogonyEnvelope<TPayload> envelope)
    {
        ArgumentNullException.ThrowIfNull(envelope);

        AssertString(envelope.EventId, nameof(envelope.EventId));
        AssertString(envelope.EventType, nameof(envelope.EventType));
        AssertString(envelope.Source, nameof(envelope.Source));
        AssertString(envelope.TraceId, nameof(envelope.TraceId));
        AssertString(envelope.Protocol, nameof(envelope.Protocol));

        if (envelope.OccurredAt == default)
        {
            throw new InvalidOperationException("OccurredAt must be set.");
        }

        if (envelope.Payload is null)
        {
            throw new InvalidOperationException("Payload must not be null.");
        }
    }

    public static void AssertCorrelationMetadataEqual<TPayload>(
        OntogonyEnvelope<TPayload> expected,
        OntogonyEnvelope<TPayload> actual)
    {
        ArgumentNullException.ThrowIfNull(expected);
        ArgumentNullException.ThrowIfNull(actual);

        if (!string.Equals(expected.TraceId, actual.TraceId, StringComparison.Ordinal))
            throw new InvalidOperationException("TraceId mismatch.");
        if (!string.Equals(expected.TenantId, actual.TenantId, StringComparison.Ordinal))
            throw new InvalidOperationException("TenantId mismatch.");
        if (!string.Equals(expected.WorkspaceId, actual.WorkspaceId, StringComparison.Ordinal))
            throw new InvalidOperationException("WorkspaceId mismatch.");
        if (!string.Equals(expected.ProjectId, actual.ProjectId, StringComparison.Ordinal))
            throw new InvalidOperationException("ProjectId mismatch.");
        if (!string.Equals(expected.SessionId, actual.SessionId, StringComparison.Ordinal))
            throw new InvalidOperationException("SessionId mismatch.");
    }

    public static void AssertPayloadHashPresent<TPayload>(OntogonyEnvelope<TPayload> envelope)
    {
        ArgumentNullException.ThrowIfNull(envelope);
        if (string.IsNullOrWhiteSpace(envelope.PayloadHash))
        {
            throw new InvalidOperationException("PayloadHash must be present.");
        }
    }

    private static void AssertString(string? value, string name)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"{name} must be set.");
        }
    }
}