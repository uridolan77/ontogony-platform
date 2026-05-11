using Microsoft.AspNetCore.Http;
using Ontogony.Contracts.Events;

namespace Ontogony.Security;

/// <summary>
/// Projects <see cref="CurrentActor"/> from HTTP headers without performing authentication.
/// Intended only when a <b>trusted upstream</b> (gateway, mesh, or host) has already established identity and you are propagating it mechanically.
/// </summary>
public sealed class HeaderCurrentActorAccessor : ICurrentActorAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HeaderCurrentActorAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CurrentActor? Current
    {
        get
        {
            var context = _httpContextAccessor.HttpContext;
            if (context is null)
                return null;

            var actorId = context.Request.Headers[OntogonySecurityHeaders.ActorId].ToString();
            if (string.IsNullOrWhiteSpace(actorId))
                return null;

            var roles = context.Request.Headers[OntogonySecurityHeaders.Roles].ToString()
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var actorType = context.Request.Headers[OntogonySecurityHeaders.ActorType].ToString();
            if (string.IsNullOrWhiteSpace(actorType))
            {
                actorType = OntogonyActorTypes.Service;
            }

            return new CurrentActor(
                actorId,
                actorType,
                roles,
                context.Request.Headers[OntogonyEventHeaders.TenantId].ToString(),
                context.Request.Headers[OntogonyEventHeaders.WorkspaceId].ToString(),
                context.Request.Headers[OntogonyEventHeaders.ProjectId].ToString());
        }
    }
}
