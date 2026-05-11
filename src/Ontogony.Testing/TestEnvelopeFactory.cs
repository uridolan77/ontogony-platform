using Ontogony.Contracts.Events;

namespace Ontogony.Testing;

public static class TestEnvelopeFactory
{
    public static OntogonyEnvelope<TPayload> Create<TPayload>(
        string eventType,
        string protocol,
        TPayload payload,
        string traceId = "trace_test")
    {
        return new OntogonyEnvelope<TPayload>
        {
            EventId = "evt_test_" + Guid.NewGuid().ToString("N"),
            EventType = eventType,
            Source = "test://ontogony-platform",
            OccurredAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero),
            TraceId = traceId,
            Protocol = protocol,
            Payload = payload
        };
    }
}
