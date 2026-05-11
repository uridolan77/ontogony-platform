using Microsoft.Extensions.DependencyInjection;

namespace Ontogony.Persistence;

#pragma warning disable CS0618 // Intentional legacy alias registrations for compatibility.
public static class PersistenceServiceCollectionExtensions
{
    public static IServiceCollection AddOntogonyPersistencePrimitives(this IServiceCollection services)
    {
        services.AddSingleton<Ontogony.Primitives.IClock, Ontogony.Primitives.SystemClock>();
        services.AddSingleton<Ontogony.Primitives.IIdGenerator, Ontogony.Primitives.GuidIdGenerator>();

        // Legacy compatibility aliases for early adopters of Ontogony.Persistence.
        services.AddSingleton<IClock>(sp => new SystemClock());
        services.AddSingleton<IIdGenerator>(sp => new GuidIdGenerator());

        return services;
    }

    /// <summary>
    /// Registers <see cref="InMemoryOutboxStore"/> as the shared instance for outbox and idempotent-consumer contracts (tests and single-process hosts).
    /// </summary>
    public static IServiceCollection AddOntogonyInMemoryOutboxStore(
        this IServiceCollection services,
        Action<InMemoryOutboxStoreOptions>? configure = null)
    {
        var options = new InMemoryOutboxStoreOptions();
        configure?.Invoke(options);
        services.AddSingleton(options);
        services.AddSingleton<InMemoryOutboxStore>(sp =>
            new InMemoryOutboxStore(sp.GetRequiredService<InMemoryOutboxStoreOptions>(), sp.GetService<IDeadLetterWriter>()));
        services.AddSingleton<IOutboxWriter>(sp => sp.GetRequiredService<InMemoryOutboxStore>());
        services.AddSingleton<IOutboxReader>(sp => sp.GetRequiredService<InMemoryOutboxStore>());
        services.AddSingleton<IOutboxDispatcher>(sp => sp.GetRequiredService<InMemoryOutboxStore>());
        services.AddSingleton<IProcessedMessageStore>(sp => sp.GetRequiredService<InMemoryOutboxStore>());
        return services;
    }
}
#pragma warning restore CS0618
