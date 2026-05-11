using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Ontogony.Security;

/// <summary>
/// Extracts current actor from JWT claims in ClaimsPrincipal.
/// Requires authenticated principal with configured claim priorities.
/// </summary>
public sealed class ClaimsCurrentActorAccessor : ICurrentActorAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ClaimsCurrentActorAccessorOptions _options;

    public ClaimsCurrentActorAccessor(
        IHttpContextAccessor httpContextAccessor,
        ClaimsCurrentActorAccessorOptions? options = null)
    {
        _httpContextAccessor = httpContextAccessor;
        _options = options ?? new ClaimsCurrentActorAccessorOptions();
    }

    public CurrentActor? Current
    {
        get
        {
            var context = _httpContextAccessor.HttpContext;
            if (context?.User?.Identity?.IsAuthenticated != true)
                return null;

            // Extract actor ID from claim priority
            var actorId = ExtractActorId(context.User);
            if (string.IsNullOrWhiteSpace(actorId))
                return null;

            // Extract roles from configured role claim
            var roles = ExtractRoles(context.User);

            // Extract optional tenant/workspace/project IDs
            var tenantId = context.User.FindFirst(_options.TenantIdClaimType)?.Value;
            var workspaceId = context.User.FindFirst(_options.WorkspaceIdClaimType)?.Value;
            var projectId = context.User.FindFirst(_options.ProjectIdClaimType)?.Value;
            var email = context.User.FindFirst(ClaimTypes.Email)?.Value;

            return new CurrentActor(
                actorId,
                ExtractActorType(context.User),
                roles,
                tenantId,
                workspaceId,
                projectId,
                email);
        }
    }

    private string ExtractActorId(ClaimsPrincipal user)
    {
        // Try configured claim types in priority order
        foreach (var claimType in _options.ActorIdClaimPriority)
        {
            var value = user.FindFirst(claimType)?.Value;
            if (!string.IsNullOrWhiteSpace(value))
                return value;
        }

        return string.Empty;
    }

    private string ExtractActorType(ClaimsPrincipal user)
    {
        var type = user.FindFirst(_options.ActorTypeClaimType)?.Value;
        return !string.IsNullOrWhiteSpace(type) ? type : OntogonyActorTypes.Service;
    }

    private string[] ExtractRoles(ClaimsPrincipal user)
    {
        // Check for role claims (standard and custom)
        var roleClaims = user.FindAll(_options.RoleClaimType).ToList();

        if (roleClaims.Count == 0)
            return Array.Empty<string>();

        // Filter to only generic role names if strict mode is enabled
        if (_options.StrictRoleValidation)
        {
            return roleClaims
                .Select(c => c.Value)
                .Where(role => OntogonyRoleNames.AllRoles.Contains(role, StringComparer.OrdinalIgnoreCase))
                .ToArray();
        }

        return roleClaims.Select(c => c.Value).ToArray();
    }
}

/// <summary>
/// Configuration for ClaimsCurrentActorAccessor.
/// </summary>
public sealed class ClaimsCurrentActorAccessorOptions
{
    /// <summary>
    /// Claim types to check for actor ID, in priority order.
    /// Defaults: sub, oid, unique_name
    /// </summary>
    public string[] ActorIdClaimPriority { get; set; } =
    {
        "sub",                      // OIDC standard
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",  // .NET default
        ClaimTypes.NameIdentifier
    };

    /// <summary>
    /// Claim type for actor type (human, service, agent).
    /// Default: actor_type
    /// </summary>
    public string ActorTypeClaimType { get; set; } = "actor_type";

    /// <summary>
    /// Claim type for role extraction.
    /// Defaults to ClaimTypes.Role.
    /// </summary>
    public string RoleClaimType { get; set; } = ClaimTypes.Role;

    /// <summary>
    /// Tenant ID claim type.
    /// Default: tenant_id
    /// </summary>
    public string TenantIdClaimType { get; set; } = "tenant_id";

    /// <summary>
    /// Workspace ID claim type.
    /// Default: workspace_id
    /// </summary>
    public string WorkspaceIdClaimType { get; set; } = "workspace_id";

    /// <summary>
    /// Project ID claim type.
    /// Default: project_id
    /// </summary>
    public string ProjectIdClaimType { get; set; } = "project_id";

    /// <summary>
    /// If true, only allow roles from OntogonyRoleNames.AllRoles.
    /// If false, pass through any role claim value.
    /// Default: true (strict)
    /// </summary>
    public bool StrictRoleValidation { get; set; } = true;
}
