using Ontogony.Primitives;
using Xunit;

namespace Ontogony.Http.Tests;

public sealed class RetryBudgetTrackerTests
{
    [Fact]
    public void TryConsumeRetryBudget_AllowsUnlimitedRetries_WhenBudgetDisabled()
    {
        var registry = new TransportResilienceRegistry(new TestClock());
        var options = new TransportResilienceOptions { RetryBudgetPerMinute = 0 };

        Assert.True(registry.TryConsumeRetryBudget("tests", options));
        Assert.True(registry.TryConsumeRetryBudget("tests", options));
        Assert.True(registry.TryConsumeRetryBudget("tests", options));
    }

    [Fact]
    public void TryConsumeRetryBudget_StopsAtConfiguredLimit()
    {
        var registry = new TransportResilienceRegistry(new TestClock());
        var options = new TransportResilienceOptions { RetryBudgetPerMinute = 2 };

        Assert.True(registry.TryConsumeRetryBudget("tests", options));
        Assert.True(registry.TryConsumeRetryBudget("tests", options));
        Assert.False(registry.TryConsumeRetryBudget("tests", options));
    }

    private sealed class TestClock : IClock
    {
        public DateTimeOffset UtcNow { get; private set; } = new(2026, 5, 13, 12, 0, 0, TimeSpan.Zero);
    }
}
