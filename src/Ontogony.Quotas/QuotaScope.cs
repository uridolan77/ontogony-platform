namespace Ontogony.Quotas;

public sealed record QuotaScope(
    string ScopeType,
    string ScopeId,
    string? TenantId = null,
    string? WorkspaceId = null,
    string? ProjectId = null);
