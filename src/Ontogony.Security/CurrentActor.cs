namespace Ontogony.Security;

/// <summary>
/// Resolved actor context for the current request (opaque ids; no authorization decisions).
/// </summary>
/// <param name="ActorId">Stable actor identifier.</param>
/// <param name="ActorType">Opaque classifier (often <see cref="OntogonyActorTypes"/> constants).</param>
/// <param name="Roles">Role names attached to the actor.</param>
/// <param name="TenantId">Optional tenant scope.</param>
/// <param name="WorkspaceId">Optional workspace scope.</param>
/// <param name="ProjectId">Optional project scope.</param>
/// <param name="Email">Optional email claim.</param>
public sealed record CurrentActor(
    string ActorId,
    string ActorType,
    string[] Roles,
    string? TenantId = null,
    string? WorkspaceId = null,
    string? ProjectId = null,
    string? Email = null);

/// <summary>
/// Provides access to the current <see cref="CurrentActor"/> when configured in DI.
/// </summary>
public interface ICurrentActorAccessor
{
    /// <summary>Actor for the active HTTP context, if resolved.</summary>
    CurrentActor? Current { get; }
}
