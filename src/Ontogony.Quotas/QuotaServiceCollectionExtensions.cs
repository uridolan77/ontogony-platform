using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ontogony.Primitives;

namespace Ontogony.Quotas;

public static class QuotaServiceCollectionExtensions
{
    public static IServiceCollection AddOntogonyInMemoryQuotaLedger(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddSingleton<IClock, SystemClock>();
        services.TryAddSingleton<InMemoryQuotaLedger>();
        services.TryAddSingleton<IQuotaLedger>(sp => sp.GetRequiredService<InMemoryQuotaLedger>());

        return services;
    }
}
