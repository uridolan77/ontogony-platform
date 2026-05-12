using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ontogony.Logging;

/// <summary>
/// Service registration helpers for Ontogony structured logging.
/// </summary>
public static class LoggingServiceCollectionExtensions
{
    public static IServiceCollection AddOntogonyLogging(
        this IServiceCollection services,
        Action<OntogonyLoggingOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (configure is not null)
        {
            services.Configure(configure);
        }
        else
        {
            services.AddOptions<OntogonyLoggingOptions>();
        }

        return services;
    }
}
