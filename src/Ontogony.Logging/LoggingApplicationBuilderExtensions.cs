using Microsoft.AspNetCore.Builder;

namespace Ontogony.Logging;

/// <summary>
/// ASP.NET Core middleware registration helpers for Ontogony logging.
/// </summary>
public static class LoggingApplicationBuilderExtensions
{
    public static IApplicationBuilder UseOntogonyLoggingScope(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);
        return app.UseMiddleware<OntogonyLoggingScopeMiddleware>();
    }
}
