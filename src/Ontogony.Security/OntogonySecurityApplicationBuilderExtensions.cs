using Microsoft.AspNetCore.Builder;

namespace Ontogony.Security;

/// <summary>
/// ASP.NET Core pipeline registration for Ontogony.Security.
/// </summary>
public static class OntogonySecurityApplicationBuilderExtensions
{
    /// <summary>
    /// Inserts bounded async request-body hashing for HMAC service identity (see <see cref="ServiceIdentityBodyHashPreloadMiddleware"/>).
    /// Call early in the pipeline (before endpoints read the body). Requires <see cref="ServiceCollectionExtensions.AddOntogonyServiceIdentityActorContext"/> and <c>IOptions&lt;ServiceIdentityOptions&gt;</c>.
    /// </summary>
    public static IApplicationBuilder UseOntogonyServiceIdentityBodyHashPreload(this IApplicationBuilder app) =>
        app.UseMiddleware<ServiceIdentityBodyHashPreloadMiddleware>();
}
