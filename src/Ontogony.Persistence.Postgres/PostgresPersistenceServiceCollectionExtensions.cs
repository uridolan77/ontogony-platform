using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Ontogony.Persistence;

namespace Ontogony.Persistence.Postgres;

/// <summary>DI registration for PostgreSQL outbox storage.</summary>
public static class PostgresPersistenceServiceCollectionExtensions
{
    /// <summary>Registers durable PostgreSQL outbox, processed-message, and claim-store services.</summary>
    public static IServiceCollection AddOntogonyPostgresOutbox(
        this IServiceCollection services,
        Action<PostgresOutboxOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        var options = new PostgresOutboxOptions();
        configure(options);
        options.Validate();

        services.AddSingleton(options);
        services.AddSingleton(sp => NpgsqlDataSource.Create(sp.GetRequiredService<PostgresOutboxOptions>().ConnectionString));
        services.TryAddSingleton<Ontogony.Primitives.IClock, Ontogony.Primitives.SystemClock>();
        services.TryAddSingleton<Ontogony.Primitives.IIdGenerator, Ontogony.Primitives.GuidIdGenerator>();

        services.AddSingleton<PostgresDeadLetterWriter>(sp =>
            new PostgresDeadLetterWriter(
                sp.GetRequiredService<PostgresOutboxOptions>(),
                sp.GetRequiredService<NpgsqlDataSource>()));
        services.AddSingleton<IDeadLetterWriter>(sp => sp.GetRequiredService<PostgresDeadLetterWriter>());

        services.AddSingleton<PostgresOutboxStore>(sp =>
            new PostgresOutboxStore(
                sp.GetRequiredService<PostgresOutboxOptions>(),
                sp.GetRequiredService<NpgsqlDataSource>(),
                sp.GetRequiredService<Ontogony.Primitives.IClock>(),
                sp.GetRequiredService<Ontogony.Primitives.IIdGenerator>(),
                sp.GetRequiredService<IDeadLetterWriter>()));

        services.AddSingleton<IOutboxWriter>(sp => sp.GetRequiredService<PostgresOutboxStore>());
        services.AddSingleton<IOutboxReader>(sp => sp.GetRequiredService<PostgresOutboxStore>());
        services.AddSingleton<IOutboxDispatcher>(sp => sp.GetRequiredService<PostgresOutboxStore>());
        services.AddSingleton<IProcessedMessageStore>(sp => sp.GetRequiredService<PostgresOutboxStore>());
        services.AddSingleton<IPostgresOutboxClaimStore>(sp => sp.GetRequiredService<PostgresOutboxStore>());

        if (options.EnsureSchemaOnStartup)
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService, PostgresOutboxSchemaInitializerHostedService>());
        }

        return services;
    }
}
