namespace Ontogony.Http;

/// <summary>
/// Per-call outbound metadata for integration HTTP clients; propagates via <see cref="OntogonyIntegrationContext"/>.
/// </summary>
/// <param name="IdempotencyKey">Optional idempotency key to emit on the next outbound call.</param>
/// <param name="ActorId">Optional actor identifier when no HTTP actor context exists.</param>
/// <param name="ActorType">Optional actor type classifier.</param>
/// <param name="ActorRoles">Optional actor roles (comma-free role names only).</param>
/// <param name="TenantId">Optional tenant scope.</param>
/// <param name="WorkspaceId">Optional workspace scope.</param>
public sealed record IntegrationClientCallOptions(
    string? IdempotencyKey = null,
    string? ActorId = null,
    string? ActorType = null,
    IReadOnlyList<string>? ActorRoles = null,
    string? TenantId = null,
    string? WorkspaceId = null)
{
    /// <summary>Replaces outbound integration state until the returned scope is disposed.</summary>
    public IDisposable PushScope() =>
        OntogonyIntegrationContext.Push(new IntegrationOutboundState(
            IdempotencyKey,
            ActorId,
            ActorType,
            ActorRoles,
            TenantId,
            WorkspaceId));
}
