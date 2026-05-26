using Npgsql;
using Ontogony.Persistence.Postgres;
using Ontogony.Testing;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

/// <summary>
/// Runs <see cref="OutboxConformanceHarness"/> against <see cref="PostgresOutboxStore"/> when configured.
/// </summary>
public sealed class PostgresOutboxConformanceTests
{
    private static readonly PostgresOutboxOptions Options = CreateOptions();

    [Fact]
    public async Task PostgresOutbox_MeetsConformanceHarness()
    {
        if (!HasConnectionString())
        {
            return;
        }

        await using var dataSource = NpgsqlDataSource.Create(Options.ConnectionString);
        var store = new PostgresOutboxStore(Options, dataSource);
        await store.EnsureSchemaAsync();

        var suffix = Guid.NewGuid().ToString("n");

        await OutboxConformanceHarness.AssertWriteThenReadAsync(
            store,
            store,
            OutboxConformanceHarness.BuildMessage("pg-conformance-read-" + suffix));
        await OutboxConformanceHarness.AssertMarkDispatchedRemovesFromQueueAsync(
            store,
            store,
            store,
            OutboxConformanceHarness.BuildMessage("pg-conformance-dispatch-" + suffix));
        await OutboxConformanceHarness.AssertDuplicateWriteThrowsAsync(
            store,
            OutboxConformanceHarness.BuildMessage("pg-conformance-dup-" + suffix));
        await OutboxConformanceHarness.AssertMarkFailedIsIdempotentAsync(
            store,
            store,
            OutboxConformanceHarness.BuildMessage("pg-conformance-fail-" + suffix));
        await OutboxConformanceHarness.AssertAvailableAtDeferralAsync(store, store);
    }

    private static PostgresOutboxOptions CreateOptions() =>
        new()
        {
            ConnectionString = Environment.GetEnvironmentVariable("ONTOGONY_POSTGRES_TEST_CONNECTION") ?? string.Empty,
            SchemaName = "public",
            OutboxTableName = "ontogony_outbox_messages",
            ProcessedTableName = "ontogony_processed_messages",
            DeadLetterTableName = "ontogony_dead_letter_messages",
            ClaimLeaseDuration = TimeSpan.FromSeconds(30)
        };

    private static bool HasConnectionString() =>
        !string.IsNullOrWhiteSpace(Options.ConnectionString);
}
