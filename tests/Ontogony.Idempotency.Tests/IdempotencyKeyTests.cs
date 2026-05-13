using Ontogony.Hashing;
using Xunit;

namespace Ontogony.Idempotency.Tests;

/// <summary>
/// Tests for idempotency key generation and the in-memory ledger semantics.
/// </summary>
public class IdempotencyKeyTests
{
    [Fact]
    public void BuildKey_WithSameOperationAndParts_IsDeterministic()
    {
        var builder = CreateBuilder();

        var key1 = builder.BuildKey("orders.create", 42, "tenant-a");
        var key2 = builder.BuildKey("orders.create", 42, "tenant-a");

        Assert.Equal(key1, key2);
        Assert.StartsWith("ontogony:orders.create:v1:", key1);
    }

    [Fact]
    public void BuildKeyFromJson_NormalizesEquivalentJson()
    {
        var builder = CreateBuilder();

        var key1 = builder.BuildKeyFromJson("orders.create", "{\"a\":1,\"b\":2}");
        var key2 = builder.BuildKeyFromJson("orders.create", "{\"b\":2,\"a\":1}");

        Assert.Equal(key1, key2);
    }

    [Fact]
    public void BuildKey_WithUnsafeOperationCharacter_Throws()
    {
        var builder = CreateBuilder();

        var exception = Assert.Throws<ArgumentException>(() => builder.BuildKey("orders/create", 42));

        Assert.Contains("unsafe character '/'", exception.Message);
    }

    [Theory]
    [InlineData(8)]
    [InlineData(1)]
    public async Task InMemoryLedger_TryBeginAsync_AcceptsFirstReservationOnly(int concurrentAttempts)
    {
        var ledger = new InMemoryIdempotencyLedger();
        var tasks = Enumerable.Range(0, concurrentAttempts)
            .Select(_ => ledger.TryBeginAsync("key-1"));

        var results = await Task.WhenAll(tasks);

        Assert.Equal(1, results.Count(static x => x));
        Assert.Equal(concurrentAttempts - 1, results.Count(static x => !x));
    }

    [Fact]
    public async Task InMemoryLedger_MarkSucceededAsync_PersistsStatusAndResultReference()
    {
        var ledger = new InMemoryIdempotencyLedger();

        await ledger.TryBeginAsync("key-2");
        await ledger.MarkSucceededAsync("key-2", "artifact://result/123");
        var record = await ledger.GetAsync("key-2");

        Assert.NotNull(record);
        Assert.Equal(IdempotencyStatus.Succeeded, record!.Status);
        Assert.Equal("artifact://result/123", record.ResultReference);
        Assert.NotNull(record.UpdatedAt);
    }

    [Fact]
    public async Task InMemoryLedger_MarkFailedAsync_PersistsFailureReason()
    {
        var ledger = new InMemoryIdempotencyLedger();

        await ledger.TryBeginAsync("key-3");
        await ledger.MarkFailedAsync("key-3", "transient upstream failure");
        var record = await ledger.GetAsync("key-3");

        Assert.NotNull(record);
        Assert.Equal(IdempotencyStatus.Failed, record!.Status);
        Assert.Equal("transient upstream failure", record.FailureReason);
    }

    private static IdempotencyKeyBuilder CreateBuilder() =>
        new(new PayloadHasher(new Sha256ContentHashService()));
}
