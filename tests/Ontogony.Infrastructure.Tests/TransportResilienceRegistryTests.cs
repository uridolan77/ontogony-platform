using Ontogony.Http;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class TransportResilienceRegistryTests
{
    [Fact]
    public void RecordFailure_Opens_Circuit_At_Threshold()
    {
        var registry = new TransportResilienceRegistry();
        var options = new TransportResilienceOptions
        {
            Enabled = true,
            CircuitFailureThreshold = 2,
            CircuitOpenDurationSeconds = 60
        };

        registry.RecordFailure("client-a", options);
        var before = registry.TryGetCircuitOpenSyntheticResponse("client-a", options);

        registry.RecordFailure("client-a", options);
        var after = registry.TryGetCircuitOpenSyntheticResponse("client-a", options);

        Assert.Null(before);
        Assert.NotNull(after);
        Assert.Equal("ontogony_circuit_open", after!.ReasonPhrase);
    }

    [Fact]
    public void RecordSuccess_Closes_Open_Circuit_State()
    {
        var registry = new TransportResilienceRegistry();
        var options = new TransportResilienceOptions
        {
            Enabled = true,
            CircuitFailureThreshold = 1,
            CircuitOpenDurationSeconds = 60
        };

        registry.RecordFailure("client-b", options);
        Assert.NotNull(registry.TryGetCircuitOpenSyntheticResponse("client-b", options));

        registry.RecordSuccess("client-b", options);

        Assert.Null(registry.TryGetCircuitOpenSyntheticResponse("client-b", options));
    }
}
