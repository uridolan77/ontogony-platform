using Microsoft.AspNetCore.Http;
using Ontogony.Http;
using Xunit;

namespace Ontogony.Security.Tests;

public sealed class CurrentActorOutboundPropagatorTests
{
    [Fact]
    public void TryGetActor_ReturnsFalse_WhenHttpContextMissing()
    {
        var accessor = new StubActorAccessor(null);
        var propagator = new CurrentActorOutboundPropagator(accessor);

        Assert.False(propagator.TryGetActor(out _));
    }

    [Fact]
    public void TryGetActor_ReturnsActor_WhenAccessorResolves()
    {
        var actor = new CurrentActor("actor-1", OntogonyActorTypes.Human, ["reviewer"], TenantId: "tenant-1");
        var accessor = new StubActorAccessor(actor);
        var propagator = new CurrentActorOutboundPropagator(accessor);

        Assert.True(propagator.TryGetActor(out var snapshot));
        Assert.Equal("actor-1", snapshot.ActorId);
        Assert.Equal(OntogonyActorTypes.Human, snapshot.ActorType);
        Assert.Equal(["reviewer"], snapshot.Roles);
    }

    private sealed class StubActorAccessor(CurrentActor? actor) : ICurrentActorAccessor
    {
        public CurrentActor? Current => actor;
    }
}
