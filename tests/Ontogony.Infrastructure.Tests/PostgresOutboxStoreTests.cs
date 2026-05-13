using Npgsql;
using Ontogony.Persistence;
using Ontogony.Persistence.Postgres;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class PostgresOutboxStoreTests
{
    private static readonly PostgresOutboxOptions Options = CreateOptions();

    [Fact]
    public async Task EnsureSchemaAsync_IsIdempotent()
    {
        if (!HasConnectionString())
        {
            return;
        }

        await using var dataSource = NpgsqlDataSource.Create(Options.ConnectionString);
        var store = new PostgresOutboxStore(Options, dataSource);
        await store.EnsureSchemaAsync();
        await store.EnsureSchemaAsync();
    }

    [Fact]
    public async Task WriteRead_OrdersByAvailableAtThenOccurredAt()
    {
        if (!HasConnectionString())
        {
            return;
        }

        var suffix = Guid.NewGuid().ToString("N");
        await using var dataSource = NpgsqlDataSource.Create(Options.ConnectionString);
        var store = new PostgresOutboxStore(Options, dataSource);
        await store.EnsureSchemaAsync();

        var sameAvailable = DateTimeOffset.Parse("2026-05-12T10:00:00Z");
        var first = CreateMessage("pg-order-a-" + suffix, sameAvailable.AddMinutes(1), sameAvailable);
        var second = CreateMessage("pg-order-b-" + suffix, sameAvailable.AddMinutes(2), sameAvailable);

        await store.WriteAsync(second);
        await store.WriteAsync(first);

        var batch = await store.ReadAvailableAsync(sameAvailable.AddMinutes(10), maxBatchSize: 10);

        var firstIndex = Array.FindIndex(batch.ToArray(), m => m.MessageId == first.MessageId);
        var secondIndex = Array.FindIndex(batch.ToArray(), m => m.MessageId == second.MessageId);
        Assert.True(firstIndex >= 0);
        Assert.True(secondIndex >= 0);
        Assert.True(firstIndex < secondIndex);
    }

    [Fact]
    public async Task WriteAsync_DuplicateMessageId_Throws()
    {
        if (!HasConnectionString())
        {
            return;
        }

        await using var dataSource = NpgsqlDataSource.Create(Options.ConnectionString);
        var store = new PostgresOutboxStore(Options, dataSource);
        await store.EnsureSchemaAsync();

        var messageId = "pg-dup-" + Guid.NewGuid().ToString("N");
        var message = CreateMessage(messageId, DateTimeOffset.Parse("2026-05-12T10:00:00Z"), DateTimeOffset.Parse("2026-05-12T10:00:00Z"));

        await store.WriteAsync(message);
        await Assert.ThrowsAsync<InvalidOperationException>(() => store.WriteAsync(message));
    }

    [Fact]
    public async Task ConcurrentReaders_DoNotDoubleClaim()
    {
        if (!HasConnectionString())
        {
            return;
        }

        var now = DateTimeOffset.Parse("2026-05-12T10:00:00Z");
        var messageId = "pg-claim-" + Guid.NewGuid().ToString("N");

        await using var writerDataSource = NpgsqlDataSource.Create(Options.ConnectionString);
        var writerStore = new PostgresOutboxStore(Options, writerDataSource);
        await writerStore.EnsureSchemaAsync();
        await writerStore.WriteAsync(CreateMessage(messageId, now, now));

        await using var readerDataSource1 = NpgsqlDataSource.Create(Options.ConnectionString);
        await using var readerDataSource2 = NpgsqlDataSource.Create(Options.ConnectionString);
        var reader1 = new PostgresOutboxStore(Options, readerDataSource1);
        var reader2 = new PostgresOutboxStore(Options, readerDataSource2);

        var reads = await Task.WhenAll(
            reader1.ReadAvailableAsync(now.AddMinutes(1), maxBatchSize: 1),
            reader2.ReadAvailableAsync(now.AddMinutes(1), maxBatchSize: 1));

        var allIds = reads.SelectMany(static r => r).Select(static m => m.MessageId).ToArray();
        Assert.Single(allIds);
        Assert.Equal(messageId, allIds[0]);
    }

    /// <summary>
    /// Regression guard: <see cref="PostgresOutboxStore.ClaimAvailableAsync"/> must dispose the reader before
    /// <c>CommitAsync</c> on the same Npgsql connection (Linux CI surfaced failures when they overlapped).
    /// </summary>
    [Fact]
    public async Task ReadAvailableAsync_CompletesAfterWrite_WithoutReaderCommitConflict()
    {
        if (!HasConnectionString())
        {
            return;
        }

        var suffix = Guid.NewGuid().ToString("N");
        await using var dataSource = NpgsqlDataSource.Create(Options.ConnectionString);
        var store = new PostgresOutboxStore(Options, dataSource);
        await store.EnsureSchemaAsync();

        var messageId = "pg-read-commit-" + suffix;
        var now = DateTimeOffset.Parse("2026-05-12T14:00:00Z");
        await store.WriteAsync(CreateMessage(messageId, now, now));

        var batch = await store.ReadAvailableAsync(now.AddMinutes(1), maxBatchSize: 10);
        var matching = batch.Where(m => m.MessageId == messageId).ToArray();
        var found = Assert.Single(matching);
        Assert.Equal(messageId, found.MessageId);
    }

    [Fact]
    public async Task MarkFailedAsync_UpdatesAttemptAndSchedule()
    {
        if (!HasConnectionString())
        {
            return;
        }

        await using var dataSource = NpgsqlDataSource.Create(Options.ConnectionString);
        var store = new PostgresOutboxStore(Options, dataSource);
        await store.EnsureSchemaAsync();

        var messageId = "pg-fail-" + Guid.NewGuid().ToString("N");
        var now = DateTimeOffset.Parse("2026-05-12T11:00:00Z");
        var nextAvailable = now.AddMinutes(5);

        await store.WriteAsync(CreateMessage(messageId, now, now));
        await store.MarkFailedAsync(messageId, "boom", nextAvailable);

        var before = await store.ReadAvailableAsync(now.AddMinutes(2), maxBatchSize: 10);
        Assert.DoesNotContain(before, m => m.MessageId == messageId);

        var later = await store.ReadAvailableAsync(now.AddMinutes(10), maxBatchSize: 10);
        var matching = later.Where(m => m.MessageId == messageId).ToArray();
        var updated = Assert.Single(matching);
        Assert.Equal(1, updated.AttemptCount);
        Assert.Equal("boom", updated.LastError);
        Assert.Equal(nextAvailable, updated.AvailableAt);
    }

    [Fact]
    public async Task DeadLetterThreshold_MovesMessageAndExcludesFromRead()
    {
        if (!HasConnectionString())
        {
            return;
        }

        var options = CreateOptions();
        options.MoveToDeadLetterAfterAttempts = 2;

        await using var dataSource = NpgsqlDataSource.Create(options.ConnectionString);
        var deadLetterWriter = new PostgresDeadLetterWriter(options, dataSource);
        var store = new PostgresOutboxStore(options, dataSource, deadLetterWriter: deadLetterWriter);
        await store.EnsureSchemaAsync();

        var messageId = "pg-dlq-" + Guid.NewGuid().ToString("N");
        var now = DateTimeOffset.Parse("2026-05-12T12:00:00Z");
        await store.WriteAsync(CreateMessage(messageId, now, now));

        await store.MarkFailedAsync(messageId, "first", now.AddMinutes(1));
        await store.MarkFailedAsync(messageId, "second", now.AddMinutes(2));

        var batch = await store.ReadAvailableAsync(now.AddHours(1), maxBatchSize: 10);
        Assert.DoesNotContain(batch, m => m.MessageId == messageId);

        var deadLetterCount = await CountRowsAsync(options.ConnectionString, options.SchemaName, options.DeadLetterTableName, messageId);
        Assert.Equal(1, deadLetterCount);
    }

    [Fact]
    public async Task ProcessedMessage_MarkIsIdempotent()
    {
        if (!HasConnectionString())
        {
            return;
        }

        var options = CreateOptions();
        await using var dataSource = NpgsqlDataSource.Create(options.ConnectionString);
        var store = new PostgresOutboxStore(options, dataSource);
        await store.EnsureSchemaAsync();

        var consumer = "projection-worker-" + Guid.NewGuid().ToString("N");
        var messageId = "pg-processed-" + Guid.NewGuid().ToString("N");
        var processed = new ProcessedMessage(consumer, messageId, DateTimeOffset.UtcNow, "trace-1");

        await store.MarkProcessedAsync(processed);
        await store.MarkProcessedAsync(processed);

        Assert.True(await store.HasProcessedAsync(consumer, messageId));

        var processedCount = await CountProcessedRowsAsync(options.ConnectionString, options.SchemaName, options.ProcessedTableName, consumer, messageId);
        Assert.Equal(1, processedCount);
    }

    [Fact]
    public async Task ClaimApi_TryClaimRenewRelease_WorksForClaimOwner()
    {
        if (!HasConnectionString())
        {
            return;
        }

        await using var dataSource = NpgsqlDataSource.Create(Options.ConnectionString);
        var store = new PostgresOutboxStore(Options, dataSource);
        await store.EnsureSchemaAsync();

        var messageId = "pg-claim-api-" + Guid.NewGuid().ToString("N");
        var now = DateTimeOffset.UtcNow;
        await store.WriteAsync(CreateMessage(messageId, now, now));

        var claimed = await store.TryClaimAsync(messageId, now.AddMinutes(1), TimeSpan.FromSeconds(15));
        Assert.True(claimed);

        var renewed = await store.RenewClaimAsync(messageId, TimeSpan.FromSeconds(45));
        Assert.True(renewed);

        await store.ReleaseClaimAsync(messageId);

        var claimAgain = await store.TryClaimAsync(messageId, now.AddMinutes(1), TimeSpan.FromSeconds(15));
        Assert.True(claimAgain);
    }

    [Fact]
    public async Task ClaimApi_ClaimAvailableAsync_ClaimsBatchAndPreventsSecondReader()
    {
        if (!HasConnectionString())
        {
            return;
        }

        var now = DateTimeOffset.UtcNow;
        var messageId = "pg-claim-batch-" + Guid.NewGuid().ToString("N");

        await using var writerDataSource = NpgsqlDataSource.Create(Options.ConnectionString);
        var writer = new PostgresOutboxStore(Options, writerDataSource);
        await writer.EnsureSchemaAsync();
        await writer.WriteAsync(CreateMessage(messageId, now, now));

        await using var readerDataSource1 = NpgsqlDataSource.Create(Options.ConnectionString);
        await using var readerDataSource2 = NpgsqlDataSource.Create(Options.ConnectionString);
        var reader1 = new PostgresOutboxStore(Options, readerDataSource1);
        var reader2 = new PostgresOutboxStore(Options, readerDataSource2);

        var claimed = await reader1.ClaimAvailableAsync(now.AddMinutes(1), maxBatchSize: 10, leaseDuration: TimeSpan.FromSeconds(30));
        Assert.Contains(claimed, m => m.MessageId == messageId);

        var secondClaim = await reader2.ClaimAvailableAsync(now.AddMinutes(1), maxBatchSize: 10, leaseDuration: TimeSpan.FromSeconds(30));
        Assert.DoesNotContain(secondClaim, m => m.MessageId == messageId);
    }

    [Fact]
    public async Task OwnedMarking_OnlyClaimOwnerCanDispatch()
    {
        if (!HasConnectionString())
        {
            return;
        }

        var messageId = "pg-owned-dispatch-" + Guid.NewGuid().ToString("N");
        var now = DateTimeOffset.UtcNow;

        await using var writerDataSource = NpgsqlDataSource.Create(Options.ConnectionString);
        var writer = new PostgresOutboxStore(Options, writerDataSource);
        await writer.EnsureSchemaAsync();
        await writer.WriteAsync(CreateMessage(messageId, now, now));

        await using var ownerDataSource = NpgsqlDataSource.Create(Options.ConnectionString);
        await using var otherDataSource = NpgsqlDataSource.Create(Options.ConnectionString);
        var owner = new PostgresOutboxStore(Options, ownerDataSource);
        var other = new PostgresOutboxStore(Options, otherDataSource);

        Assert.True(await owner.TryClaimAsync(messageId, now.AddMinutes(1)));
        Assert.False(await other.MarkDispatchedIfOwnedAsync(messageId, now.AddMinutes(2)));
        Assert.True(await owner.MarkDispatchedIfOwnedAsync(messageId, now.AddMinutes(3)));
    }

    [Fact]
    public async Task ReleasedClaim_CannotBeRenewedByPreviousOwnerAfterReclaim()
    {
        if (!HasConnectionString())
        {
            return;
        }

        var messageId = "pg-renew-after-reclaim-" + Guid.NewGuid().ToString("N");
        var now = DateTimeOffset.UtcNow;

        await using var writerDataSource = NpgsqlDataSource.Create(Options.ConnectionString);
        var writer = new PostgresOutboxStore(Options, writerDataSource);
        await writer.EnsureSchemaAsync();
        await writer.WriteAsync(CreateMessage(messageId, now, now));

        await using var ownerDataSource = NpgsqlDataSource.Create(Options.ConnectionString);
        await using var newOwnerDataSource = NpgsqlDataSource.Create(Options.ConnectionString);
        var owner = new PostgresOutboxStore(Options, ownerDataSource);
        var newOwner = new PostgresOutboxStore(Options, newOwnerDataSource);

        Assert.True(await owner.TryClaimAsync(messageId, now.AddMinutes(1), TimeSpan.FromSeconds(30)));
        await owner.ReleaseClaimAsync(messageId);
        Assert.True(await newOwner.TryClaimAsync(messageId, now.AddMinutes(1), TimeSpan.FromSeconds(30)));
        Assert.False(await owner.RenewClaimAsync(messageId, TimeSpan.FromSeconds(30)));
    }

    [Fact]
    public async Task Schema_WithCustomOutboxTable_UsesScopedIndexNames()
    {
        if (!HasConnectionString())
        {
            return;
        }

        var suffix = Guid.NewGuid().ToString("N");
        var optionsA = CreateOptions();
        optionsA.OutboxTableName = "ontogony_outbox_a_" + suffix;
        var optionsB = CreateOptions();
        optionsB.OutboxTableName = "ontogony_outbox_b_" + suffix;

        await using var dataSourceA = NpgsqlDataSource.Create(optionsA.ConnectionString);
        await using var dataSourceB = NpgsqlDataSource.Create(optionsB.ConnectionString);
        var storeA = new PostgresOutboxStore(optionsA, dataSourceA);
        var storeB = new PostgresOutboxStore(optionsB, dataSourceB);

        await storeA.EnsureSchemaAsync();
        await storeB.EnsureSchemaAsync();

        var indexCount = await CountOutboxIndexesAsync(optionsA.ConnectionString, optionsA.SchemaName, optionsA.OutboxTableName);
        indexCount += await CountOutboxIndexesAsync(optionsB.ConnectionString, optionsB.SchemaName, optionsB.OutboxTableName);

        Assert.Equal(4, indexCount);
    }

    private static PostgresOutboxOptions CreateOptions()
    {
        return new PostgresOutboxOptions
        {
            ConnectionString = Environment.GetEnvironmentVariable("ONTOGONY_POSTGRES_TEST_CONNECTION") ?? string.Empty,
            SchemaName = "public",
            OutboxTableName = "ontogony_outbox_messages",
            ProcessedTableName = "ontogony_processed_messages",
            DeadLetterTableName = "ontogony_dead_letter_messages",
            ClaimLeaseDuration = TimeSpan.FromSeconds(30)
        };
    }

    private static OutboxMessage CreateMessage(string messageId, DateTimeOffset occurredAt, DateTimeOffset availableAt)
    {
        return new OutboxMessage(
            MessageId: messageId,
            EventId: "evt-" + messageId,
            EventType: "integration.event.raised",
            Source: "test://postgres",
            TraceId: "trace-" + messageId,
            OccurredAt: occurredAt,
            AvailableAt: availableAt,
            AttemptCount: 0,
            LastError: null,
            PayloadJson: "{}",
            PayloadHash: "hash",
            MetadataJson: "{}");
    }

    private static async Task<int> CountRowsAsync(string connectionString, string schemaName, string tableName, string messageId)
    {
        var qualifiedTable = QuoteQualified(schemaName, tableName);

        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();

        var sql = $"SELECT COUNT(*) FROM {qualifiedTable} WHERE message_id = @message_id;";
        await using var command = new NpgsqlCommand(sql, conn);
        command.Parameters.AddWithValue("message_id", messageId);

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    private static async Task<int> CountProcessedRowsAsync(string connectionString, string schemaName, string tableName, string consumer, string messageId)
    {
        var qualifiedTable = QuoteQualified(schemaName, tableName);

        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();

        var sql = $"SELECT COUNT(*) FROM {qualifiedTable} WHERE consumer_name = @consumer_name AND message_id = @message_id;";
        await using var command = new NpgsqlCommand(sql, conn);
        command.Parameters.AddWithValue("consumer_name", consumer);
        command.Parameters.AddWithValue("message_id", messageId);

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    private static async Task<int> CountOutboxIndexesAsync(string connectionString, string schemaName, string tableName)
    {
        const string sql = """
            SELECT COUNT(*)
            FROM pg_indexes
            WHERE schemaname = @schema_name
              AND tablename = @table_name
              AND indexname LIKE 'ix_%';
            """;

        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();

        await using var command = new NpgsqlCommand(sql, conn);
        command.Parameters.AddWithValue("schema_name", schemaName);
        command.Parameters.AddWithValue("table_name", tableName);

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    private static string QuoteQualified(string schemaName, string tableName)
    {
        return $"\"{schemaName}\".\"{tableName}\"";
    }

    private static bool HasConnectionString()
    {
        return !string.IsNullOrWhiteSpace(Options.ConnectionString);
    }
}
