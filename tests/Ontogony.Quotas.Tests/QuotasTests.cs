using Ontogony.Primitives;
using Ontogony.Quotas;
using Xunit;

namespace Ontogony.Quotas.Tests;

public sealed class QuotasTests
{
    [Fact]
    public async Task Allows_until_limit_then_rejects()
    {
        var clock = new FixedClock(new DateTimeOffset(2026, 5, 12, 12, 0, 0, TimeSpan.Zero));
        var ledger = new InMemoryQuotaLedger(clock);
        var limit = new QuotaLimit(
            "requests-per-minute",
            new QuotaScope("project", "project-1"),
            QuotaMetric.Requests,
            2,
            TimeSpan.FromMinutes(1));

        Assert.True((await ledger.TryConsumeAsync(new QuotaConsumptionRequest("r1", limit, 1))).IsAllowed);
        Assert.True((await ledger.TryConsumeAsync(new QuotaConsumptionRequest("r2", limit, 1))).IsAllowed);

        var rejected = await ledger.TryConsumeAsync(new QuotaConsumptionRequest("r3", limit, 1));

        Assert.False(rejected.IsAllowed);
        Assert.Equal("quota_exceeded", rejected.ReasonCode);
        Assert.Equal(0, rejected.Remaining);
    }

    private sealed class FixedClock : IClock
    {
        public FixedClock(DateTimeOffset utcNow) => UtcNow = utcNow;
        public DateTimeOffset UtcNow { get; }
    }
}
