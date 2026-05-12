using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ontogony.Redaction;

public static class RedactionServiceCollectionExtensions
{
    public static IServiceCollection AddOntogonyRedaction(
        this IServiceCollection services,
        Action<RedactionOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (configure is not null)
        {
            services.Configure(configure);
        }
        else
        {
            services.AddOptions<RedactionOptions>();
        }

        services.TryAddSingleton<IRedactor, DefaultRedactor>();
        return services;
    }
}
