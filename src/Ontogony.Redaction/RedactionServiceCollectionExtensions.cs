using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ontogony.Redaction;

/// <summary>
/// Registers Ontogony redaction services and options.
/// </summary>
public static class RedactionServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IRedactor"/> as <see cref="DefaultRedactor"/> with optional options configuration.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configure">Optional options mutator.</param>
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
