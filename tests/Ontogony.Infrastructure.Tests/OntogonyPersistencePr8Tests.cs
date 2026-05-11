using Ontogony.Persistence;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class OntogonyPersistencePr8Tests
{
    [Fact]
    public void Validate_WithValidOutboxMessage_Succeeds()
    {
        var message = new OutboxMessage(
            MessageId: "msg-1",
            EventId: "evt-1",
            EventType: "test.event",
            Source: "test://source",
            TraceId: "trace-1",
            OccurredAt: new DateTimeOffset(2026, 5, 11, 10, 0, 0, TimeSpan.Zero),
            AvailableAt: new DateTimeOffset(2026, 5, 11, 10, 0, 0, TimeSpan.Zero),
            AttemptCount: 0,
            LastError: null,
            PayloadJson: "{\"value\":1}",
            PayloadHash: "abc123",
            MetadataJson: "{}");

        OutboxContracts.Validate(message);
    }

    [Fact]
    public void Validate_WithNegativeAttemptCount_Throws()
    {
        var message = new OutboxMessage(
            MessageId: "msg-1",
            EventId: "evt-1",
            EventType: "test.event",
            Source: "test://source",
            TraceId: "trace-1",
            OccurredAt: new DateTimeOffset(2026, 5, 11, 10, 0, 0, TimeSpan.Zero),
            AvailableAt: new DateTimeOffset(2026, 5, 11, 10, 0, 0, TimeSpan.Zero),
            AttemptCount: -1,
            LastError: null,
            PayloadJson: "{\"value\":1}",
            PayloadHash: "abc123",
            MetadataJson: "{}");

        Assert.Throws<ArgumentException>(() => OutboxContracts.Validate(message));
    }

    [Fact]
    public void BuildProcessedMessageKey_ProducesDeterministicKey()
    {
        var key = OutboxContracts.BuildProcessedMessageKey("consumer-a", "msg-42");

        Assert.Equal("consumer-a:msg-42", key);
    }

    [Fact]
    public void CalculateNextAvailableAt_GrowsWithAttemptCount()
    {
        var now = new DateTimeOffset(2026, 5, 11, 10, 0, 0, TimeSpan.Zero);

        var first = OutboxContracts.CalculateNextAvailableAt(now, attemptCount: 0, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30));
        var third = OutboxContracts.CalculateNextAvailableAt(now, attemptCount: 2, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30));

        Assert.True(third > first);
    }

    [Fact]
    public void CalculateNextAvailableAt_IsCappedByMaxDelay()
    {
        var now = new DateTimeOffset(2026, 5, 11, 10, 0, 0, TimeSpan.Zero);

        var next = OutboxContracts.CalculateNextAvailableAt(now, attemptCount: 20, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(10));

        Assert.Equal(now.AddSeconds(10), next);
    }

    [Fact]
    public void SerializeMetadata_SortsKeysDeterministically()
    {
        var metadata = new Dictionary<string, string>
        {
            ["z"] = "last",
            ["a"] = "first"
        };

        var json = OutboxContracts.SerializeMetadata(metadata);

        Assert.Equal("{\"a\":\"first\",\"z\":\"last\"}", json);
    }

    [Fact]
    public void SerializeMetadata_WithNull_ReturnsEmptyObject()
    {
        var json = OutboxContracts.SerializeMetadata(null);

        Assert.Equal("{}", json);
    }
}
