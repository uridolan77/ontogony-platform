using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ontogony.Errors;

public static class OntogonyErrorsExtensions
{
    public static IServiceCollection AddOntogonyErrors(
        this IServiceCollection services,
        Action<OntogonyExceptionMappingOptions>? configure = null)
    {
        services.Configure<OntogonyExceptionMappingOptions>(options => configure?.Invoke(options));
        return services;
    }

    public static IApplicationBuilder UseOntogonyExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<OntogonyExceptionHandlingMiddleware>();
    }
}
