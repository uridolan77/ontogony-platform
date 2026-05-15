using Ontogony.Observability;

namespace Ontogony.Http;

/// <summary>
/// Propagates actor id from <see cref="OntogonyCorrelationContext"/> when present.
/// </summary>
public sealed class CorrelationOutboundActorPropagator : IOutboundActorPropagator
{
    /// <inheritdoc />
    public bool TryGetActor(out OutboundActorSnapshot snapshot)
    {
        var actorId = OntogonyCorrelationContext.Current?.ActorId;
        if (string.IsNullOrWhiteSpace(actorId))
        {
            snapshot = default;
            return false;
        }

        snapshot = new OutboundActorSnapshot(actorId, ActorType: null, Roles: Array.Empty<string>());
        return true;
    }
}
