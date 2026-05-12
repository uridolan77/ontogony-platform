namespace Ontogony.Contracts.References;

/// <summary>
/// Protocol-neutral actor reference (opaque ids; no auth or RBAC semantics).
/// </summary>
/// <param name="ActorId">Stable actor identifier.</param>
/// <param name="ActorType">Opaque actor classifier.</param>
/// <param name="DisplayName">Optional display label.</param>
/// <param name="TenantId">Optional tenant scope.</param>
public sealed record ActorRef(
    string ActorId,
    string ActorType,
    string? DisplayName = null,
    string? TenantId = null);
