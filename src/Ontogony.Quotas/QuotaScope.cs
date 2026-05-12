namespace Ontogony.Quotas;

/// <summary>
/// Identifies the subject of a quota limit (opaque scope type/id; no RBAC).
/// </summary>
/// <param name="ScopeType">Opaque classifier (e.g. tenant, project, api_key).</param>
/// <param name="ScopeId">Opaque id within <paramref name="ScopeType"/>.</param>
/// <param name="TenantId">Optional tenant id for cross-filtering.</param>
/// <param name="WorkspaceId">Optional workspace id.</param>
/// <param name="ProjectId">Optional project id.</param>
public sealed record QuotaScope(
    string ScopeType,
    string ScopeId,
    string? TenantId = null,
    string? WorkspaceId = null,
    string? ProjectId = null);
