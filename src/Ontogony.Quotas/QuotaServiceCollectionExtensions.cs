using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ontogony.Primitives;

namespace Ontogony.Quotas;

/// <summary>
/// DI registration for the in-memory quota ledger.
/// </summary>
public static class QuotaServiceCollectionExtensions
{
    /// <summary>Registers <see cref="InMemoryQuotaLedger"/> as <see cref="IQuotaLedger"/>.</summary>
    public static IServiceCollection AddOntogonyInMemoryQuotaLedger(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddSingleton<IClock, SystemClock>();
        services.TryAddSingleton<InMemoryQuotaLedger>();
        services.TryAddSingleton<IQuotaLedger>(sp => sp.GetRequiredService<InMemoryQuotaLedger>());

        return services;
    }
}
