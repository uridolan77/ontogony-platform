namespace Ontogony.Http;

/// <summary>
/// Supplies outbound actor metadata for integration <see cref="HttpClient"/> calls.
/// </summary>
public interface IOutboundActorPropagator
{
    /// <summary>Attempts to resolve actor metadata for the current execution context.</summary>
    bool TryGetActor(out OutboundActorSnapshot snapshot);
}

/// <summary>Actor metadata propagated on outbound integration calls.</summary>
/// <param name="ActorId">Stable actor identifier.</param>
/// <param name="ActorType">Opaque actor type classifier.</param>
/// <param name="Roles">Role names attached to the actor.</param>
/// <param name="TenantId">Optional tenant scope.</param>
/// <param name="WorkspaceId">Optional workspace scope.</param>
public readonly record struct OutboundActorSnapshot(
    string ActorId,
    string? ActorType,
    IReadOnlyList<string> Roles,
    string? TenantId = null,
    string? WorkspaceId = null);
