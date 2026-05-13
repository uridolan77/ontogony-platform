using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ontogony.Primitives;
using Ontogony.Runtime;

namespace Ontogony.Quotas;

/// <summary>
/// DI registration for the in-memory quota ledger.
/// </summary>
public static class QuotaServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="InMemoryQuotaLedger"/> as <see cref="IQuotaLedger"/>.
    /// When the host environment is not <see cref="Microsoft.Extensions.Hosting.Environments.Development"/>, registers a startup warning that this ledger is not durable for production multi-instance use.
    /// </summary>
    public static IServiceCollection AddOntogonyInMemoryQuotaLedger(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddSingleton<IClock, SystemClock>();
        services.TryAddSingleton<InMemoryQuotaLedger>();
        services.TryAddSingleton<IQuotaLedger>(sp => sp.GetRequiredService<InMemoryQuotaLedger>());

        services.AddOntogonyInMemoryNonDurableStartupWarning("Ontogony.Quotas: InMemoryQuotaLedger");

        return services;
    }
}
