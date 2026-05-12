using Npgsql;

namespace Ontogony.Persistence.Postgres;

internal static class PostgresOutboxSchema
{
    public static async Task EnsureCreatedAsync(
        NpgsqlDataSource dataSource,
        PostgresSqlNames names,
        CancellationToken cancellationToken)
    {
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

        var sql = $$"""
        CREATE SCHEMA IF NOT EXISTS {{names.SchemaName}};

        CREATE TABLE IF NOT EXISTS {{names.QualifiedOutbox}} (
            message_id TEXT PRIMARY KEY,
            event_id TEXT NOT NULL,
            event_type TEXT NOT NULL,
            source TEXT NOT NULL,
            trace_id TEXT NOT NULL,
            occurred_at TIMESTAMPTZ NOT NULL,
            available_at TIMESTAMPTZ NOT NULL,
            attempt_count INTEGER NOT NULL,
            last_error TEXT NULL,
            payload_json JSONB NOT NULL,
            payload_hash TEXT NOT NULL,
            metadata_json JSONB NOT NULL,
            claimed_by TEXT NULL,
            claimed_until_utc TIMESTAMPTZ NULL,
            dispatched_at_utc TIMESTAMPTZ NULL,
            dead_lettered_at_utc TIMESTAMPTZ NULL,
            created_at_utc TIMESTAMPTZ NOT NULL DEFAULT NOW(),
            updated_at_utc TIMESTAMPTZ NOT NULL DEFAULT NOW()
        );

        CREATE INDEX IF NOT EXISTS {{names.OutboxAvailableIndex}}
            ON {{names.QualifiedOutbox}} (available_at, occurred_at)
            WHERE dispatched_at_utc IS NULL AND dead_lettered_at_utc IS NULL;

        CREATE INDEX IF NOT EXISTS {{names.OutboxClaimIndex}}
            ON {{names.QualifiedOutbox}} (claimed_until_utc)
            WHERE dispatched_at_utc IS NULL AND dead_lettered_at_utc IS NULL;

        CREATE TABLE IF NOT EXISTS {{names.QualifiedProcessed}} (
            consumer_name TEXT NOT NULL,
            message_id TEXT NOT NULL,
            processed_at TIMESTAMPTZ NOT NULL,
            trace_id TEXT NULL,
            PRIMARY KEY (consumer_name, message_id)
        );

        CREATE TABLE IF NOT EXISTS {{names.QualifiedDeadLetter}} (
            dead_letter_id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
            message_id TEXT NOT NULL,
            event_id TEXT NOT NULL,
            event_type TEXT NOT NULL,
            source TEXT NOT NULL,
            trace_id TEXT NOT NULL,
            occurred_at TIMESTAMPTZ NOT NULL,
            dead_lettered_at_utc TIMESTAMPTZ NOT NULL,
            final_attempt_count INTEGER NOT NULL,
            reason TEXT NOT NULL,
            payload_json JSONB NOT NULL,
            payload_hash TEXT NOT NULL,
            metadata_json JSONB NOT NULL,
            CONSTRAINT uq_ontogony_dead_letter_message UNIQUE (message_id)
        );
        """;

        await using var command = new NpgsqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }
}
