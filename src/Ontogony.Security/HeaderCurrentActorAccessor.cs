using Microsoft.AspNetCore.Http;
using Ontogony.Contracts.Events;

namespace Ontogony.Security;

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
            if (context is null) return null;

            var actorId = context.Request.Headers[OntogonyEventHeaders.ActorId].ToString();
            if (string.IsNullOrWhiteSpace(actorId)) return null;

            var roles = context.Request.Headers["X-Ontogony-Roles"].ToString()
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            return new CurrentActor(
                actorId,
                "human-or-service",
                roles,
                context.Request.Headers[OntogonyEventHeaders.TenantId].ToString(),
                context.Request.Headers[OntogonyEventHeaders.WorkspaceId].ToString(),
                context.Request.Headers[OntogonyEventHeaders.ProjectId].ToString());
        }
    }
}
