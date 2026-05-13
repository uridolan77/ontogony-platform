using Ontogony.Primitives;
using Xunit;

namespace Ontogony.Http.Tests;

public sealed class TransportResilienceRegistryTests
{
    [Fact]
    public void RecordFailure_OpensCircuit_AfterThreshold()
    {
        var clock = new TestClock();
        var registry = new TransportResilienceRegistry(clock);
        var options = new TransportResilienceOptions
        {
            Enabled = true,
            CircuitFailureThreshold = 2,
            CircuitOpenDurationSeconds = 60
        };

        registry.RecordFailure("client-a", options);
        Assert.Null(registry.TryGetCircuitOpenSyntheticResponse("client-a", options));

        registry.RecordFailure("client-a", options);
        var response = registry.TryGetCircuitOpenSyntheticResponse("client-a", options);

        Assert.NotNull(response);
        Assert.Equal("ontogony_circuit_open", response!.ReasonPhrase);
    }

    [Fact]
    public void RetryBudget_Resets_WhenClockAdvancesBeyondWindow()
    {
        var clock = new TestClock();
        var registry = new TransportResilienceRegistry(clock);
        var options = new TransportResilienceOptions
        {
            Enabled = true,
            RetryBudgetPerMinute = 1
        };

        Assert.True(registry.TryConsumeRetryBudget("client-budget", options));
        Assert.False(registry.TryConsumeRetryBudget("client-budget", options));

        clock.Advance(TimeSpan.FromMinutes(1).Add(TimeSpan.FromSeconds(1)));

        Assert.True(registry.TryConsumeRetryBudget("client-budget", options));
    }

    private sealed class TestClock : IClock
    {
        public DateTimeOffset UtcNow { get; private set; } = new(2026, 5, 13, 12, 0, 0, TimeSpan.Zero);

        public void Advance(TimeSpan delta) => UtcNow = UtcNow.Add(delta);
    }
}
