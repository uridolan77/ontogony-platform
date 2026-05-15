using Ontogony.Http;

namespace Ontogony.Security;

/// <summary>
/// Propagates <see cref="CurrentActor"/> from the active HTTP request onto outbound integration calls.
/// </summary>
public sealed class CurrentActorOutboundPropagator : IOutboundActorPropagator
{
    private readonly ICurrentActorAccessor _actorAccessor;

    /// <summary>Creates the propagator.</summary>
    public CurrentActorOutboundPropagator(ICurrentActorAccessor actorAccessor)
    {
        _actorAccessor = actorAccessor;
    }

    /// <inheritdoc />
    public bool TryGetActor(out OutboundActorSnapshot snapshot)
    {
        var actor = _actorAccessor.Current;
        if (actor is null)
        {
            snapshot = default;
            return false;
        }

        snapshot = new OutboundActorSnapshot(actor.ActorId, actor.ActorType, actor.Roles);
        return true;
    }
}
