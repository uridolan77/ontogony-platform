using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Ontogony.Persistence;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class InMemoryOutboxStoreTests
{
    private static OutboxMessage CreateMessage(
        string messageId,
        DateTimeOffset occurredAt,
        DateTimeOffset availableAt,
        int attemptCount = 0) =>
        new(
            messageId,
            EventId: "evt-" + messageId,
            EventType: "agentor.run.started",
            Source: "test://source",
            TraceId: "trace-1",
            occurredAt,
            availableAt,
            attemptCount,
            LastError: null,
            PayloadJson: "{}",
            PayloadHash: "hash",
            MetadataJson: "{}");

    [Fact]
    public async Task WriteAsync_DuplicateMessageId_Throws()
    {
        var store = new InMemoryOutboxStore();
        var m = CreateMessage("a", DateTimeOffset.Parse("2026-05-11T10:00:00Z"), DateTimeOffset.Parse("2026-05-11T10:00:00Z"));
        await store.WriteAsync(m);
        await Assert.ThrowsAsync<InvalidOperationException>(() => store.WriteAsync(m));
    }

    [Fact]
    public async Task ReadAvailableAsync_RespectsAvailableAt()
    {
        var store = new InMemoryOutboxStore();
        var asOf = DateTimeOffset.Parse("2026-05-11T12:00:00Z");
        await store.WriteAsync(CreateMessage("early", asOf.AddHours(-1), asOf.AddHours(-1)));
        await store.WriteAsync(CreateMessage("late", asOf.AddHours(-1), asOf.AddHours(1)));

        var batch = await store.ReadAvailableAsync(asOf, maxBatchSize: 10);

        Assert.Single(batch);
        Assert.Equal("early", batch[0].MessageId);
    }

    [Fact]
    public async Task ReadAvailableAsync_OrdersByAvailableAtThenOccurredAt()
    {
        var store = new InMemoryOutboxStore();
        var sameAvailable = DateTimeOffset.Parse("2026-05-11T10:00:00Z");
        await store.WriteAsync(CreateMessage("b", occurredAt: sameAvailable.AddMinutes(2), availableAt: sameAvailable));
        await store.WriteAsync(CreateMessage("a", occurredAt: sameAvailable.AddMinutes(1), availableAt: sameAvailable));

        var batch = await store.ReadAvailableAsync(sameAvailable.AddMinutes(10), maxBatchSize: 10);

        Assert.Equal(new[] { "a", "b" }, batch.Select(m => m.MessageId).ToArray());
    }

    [Fact]
    public async Task MarkDispatchedAsync_IsIdempotent_AndExcludesFromRead()
    {
        var store = new InMemoryOutboxStore();
        var m = CreateMessage("x", DateTimeOffset.Parse("2026-05-11T10:00:00Z"), DateTimeOffset.Parse("2026-05-11T10:00:00Z"));
        await store.WriteAsync(m);

        var t1 = DateTimeOffset.Parse("2026-05-11T10:05:00Z");
        await store.MarkDispatchedAsync("x", t1);
        await store.MarkDispatchedAsync("x", DateTimeOffset.Parse("2026-05-11T10:06:00Z"));

        var batch = await store.ReadAvailableAsync(DateTimeOffset.Parse("2026-05-11T11:00:00Z"), 10);
        Assert.Empty(batch);
    }

    [Fact]
    public async Task MarkFailedAsync_IncrementsAttempt_AndSetsNextAvailable()
    {
        var store = new InMemoryOutboxStore();
        var m = CreateMessage("f", DateTimeOffset.Parse("2026-05-11T10:00:00Z"), DateTimeOffset.Parse("2026-05-11T10:00:00Z"));
        await store.WriteAsync(m);

        var next = DateTimeOffset.Parse("2026-05-11T10:30:00Z");
        await store.MarkFailedAsync("f", "boom", next);

        var batch = await store.ReadAvailableAsync(DateTimeOffset.Parse("2026-05-11T10:20:00Z"), 10);
        Assert.Empty(batch);

        var later = await store.ReadAvailableAsync(DateTimeOffset.Parse("2026-05-11T10:35:00Z"), 10);
        Assert.Single(later);
        Assert.Equal(1, later[0].AttemptCount);
        Assert.Equal("boom", later[0].LastError);
        Assert.Equal(next, later[0].AvailableAt);
    }

    [Fact]
    public async Task MarkFailedAsync_DeadLetters_WhenThresholdReached()
    {
        var dlq = new InMemoryDeadLetterWriter();
        var options = new InMemoryOutboxStoreOptions { MoveToDeadLetterAfterAttempts = 2 };
        var store = new InMemoryOutboxStore(options, dlq);
        var m = CreateMessage("d", DateTimeOffset.Parse("2026-05-11T10:00:00Z"), DateTimeOffset.Parse("2026-05-11T10:00:00Z"));
        await store.WriteAsync(m);

        await store.MarkFailedAsync("d", "e1", DateTimeOffset.Parse("2026-05-11T10:05:00Z"));
        Assert.Equal(0, dlq.Count);

        await store.MarkFailedAsync("d", "e2", DateTimeOffset.Parse("2026-05-11T10:10:00Z"));
        Assert.Equal(1, dlq.Count);
        Assert.Equal("d", dlq.ReadAll()[0].MessageId);
        Assert.Equal(2, dlq.ReadAll()[0].FinalAttemptCount);

        var batch = await store.ReadAvailableAsync(DateTimeOffset.Parse("2026-05-11T12:00:00Z"), 10);
        Assert.Empty(batch);
    }

    private sealed class FixedClock : Ontogony.Primitives.IClock
    {
        public DateTimeOffset UtcNow { get; } = DateTimeOffset.Parse("2026-06-01T12:00:00Z", CultureInfo.InvariantCulture);
    }

    [Fact]
    public async Task MarkFailedAsync_DeadLetter_UsesInjectedClock()
    {
        var dlq = new InMemoryDeadLetterWriter();
        var options = new InMemoryOutboxStoreOptions { MoveToDeadLetterAfterAttempts = 1 };
        var clock = new FixedClock();
        var store = new InMemoryOutboxStore(options, dlq, clock);
        var m = CreateMessage("dl-clock", DateTimeOffset.Parse("2026-05-11T10:00:00Z"), DateTimeOffset.Parse("2026-05-11T10:00:00Z"));
        await store.WriteAsync(m);

        await store.MarkFailedAsync("dl-clock", "e1", DateTimeOffset.Parse("2026-05-11T10:05:00Z"));

        Assert.Equal(1, dlq.Count);
        Assert.Equal(clock.UtcNow, dlq.ReadAll()[0].DeadLetteredAtUtc);
    }

    [Fact]
    public async Task ProcessedMessageStore_UsesConsumerNameAndMessageId()
    {
        var store = new InMemoryOutboxStore();
        Assert.False(await store.HasProcessedAsync("c1", "m1"));
        await store.MarkProcessedAsync(new ProcessedMessage("c1", "m1", DateTimeOffset.Parse("2026-05-11T10:00:00Z")));
        Assert.True(await store.HasProcessedAsync("c1", "m1"));
        Assert.False(await store.HasProcessedAsync("c2", "m1"));
    }

    [Fact]
    public void AddOntogonyInMemoryOutboxStore_RegistersSameInstanceForAllContracts()
    {
        var services = new ServiceCollection();
        services.AddOntogonyInMemoryOutboxStore();
        using var sp = services.BuildServiceProvider();

        var w = sp.GetRequiredService<IOutboxWriter>();
        var r = sp.GetRequiredService<IOutboxReader>();
        var d = sp.GetRequiredService<IOutboxDispatcher>();
        var p = sp.GetRequiredService<IProcessedMessageStore>();

        Assert.Same(w, r);
        Assert.Same(r, d);
        Assert.Same(d, p);
    }
}
