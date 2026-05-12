using Microsoft.Extensions.DependencyInjection;
using Ontogony.Contracts.Events;
using Ontogony.Hashing;
using Ontogony.ProtocolIngress;
using Ontogony.ProtocolIngress.Adapters;
using Ontogony.Primitives;
using Xunit;

namespace Ontogony.ProtocolIngress.Tests;

public sealed class ProtocolIngressServiceCollectionExtensionsTests
{
    [Fact]
    public void AddOntogonyProtocolIngress_RegistersCoreDependenciesAndAdapters()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddOntogonyProtocolIngress();
        using var provider = services.BuildServiceProvider();

        // Assert: shared dependencies
        Assert.NotNull(provider.GetService<PayloadHasher>());
        Assert.IsType<GuidIdGenerator>(provider.GetRequiredService<IIdGenerator>());
        Assert.IsType<SystemClock>(provider.GetRequiredService<IClock>());
        Assert.IsType<DefaultEnvelopeValidator>(provider.GetRequiredService<IEnvelopeValidator>());

        // Assert: concrete adapters
        Assert.NotNull(provider.GetService<GenericJsonProtocolAdapter>());
        Assert.NotNull(provider.GetService<CloudEventsProtocolAdapter>());
        Assert.NotNull(provider.GetService<McpProtocolAdapter>());
        Assert.NotNull(provider.GetService<A2aProtocolAdapter>());
        Assert.NotNull(provider.GetService<AgUiProtocolAdapter>());

        // Assert: typed adapter contracts
        Assert.NotNull(provider.GetService<IProtocolIngressAdapter<string>>());
        Assert.NotNull(provider.GetService<IProtocolIngressAdapter<CloudEventEnvelope>>());
        Assert.NotNull(provider.GetService<IProtocolIngressAdapter<McpEvent>>());
        Assert.NotNull(provider.GetService<IProtocolIngressAdapter<A2aEvent>>());
        Assert.NotNull(provider.GetService<IProtocolIngressAdapter<AgUiEvent>>());
    }
}
