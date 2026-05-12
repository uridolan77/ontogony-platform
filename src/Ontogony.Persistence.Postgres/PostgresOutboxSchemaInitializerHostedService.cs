using Microsoft.Extensions.Hosting;

namespace Ontogony.Persistence.Postgres;

internal sealed class PostgresOutboxSchemaInitializerHostedService : IHostedService
{
    private readonly PostgresOutboxStore _store;

    public PostgresOutboxSchemaInitializerHostedService(PostgresOutboxStore store)
    {
        _store = store;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return _store.EnsureSchemaAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
