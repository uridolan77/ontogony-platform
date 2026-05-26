using Npgsql;
using Ontogony.Persistence;

namespace Ontogony.Persistence.Postgres;

/// <summary>
/// PostgreSQL dead-letter writer for <see cref="DeadLetterMessage"/>.
/// </summary>
public sealed class PostgresDeadLetterWriter : IDeadLetterWriter
{
    private readonly NpgsqlDataSource _dataSource;
    private readonly PostgresSqlNames _names;

    /// <summary>Creates a PostgreSQL dead-letter writer.</summary>
    public PostgresDeadLetterWriter(PostgresOutboxOptions options, NpgsqlDataSource dataSource)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(dataSource);
        options.Validate();

        _names = PostgresSqlNames.FromOptions(options);
        _dataSource = dataSource;
    }

    /// <summary>Ensures dead-letter and related outbox schema objects exist.</summary>
    public async Task EnsureSchemaAsync(CancellationToken cancellationToken = default)
    {
        await PostgresOutboxSchema.EnsureCreatedAsync(_dataSource, _names, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task WriteAsync(DeadLetterMessage deadLetter, CancellationToken cancellationToken = default)
    {
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

        await WriteAsync(deadLetter, connection, transaction, cancellationToken).ConfigureAwait(false);
        await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
    }

    internal async Task WriteAsync(
        DeadLetterMessage deadLetter,
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(deadLetter);
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(transaction);

        var sql = $$"""
        INSERT INTO {{_names.QualifiedDeadLetter}} (
            message_id,
            event_id,
            event_type,
            source,
            trace_id,
            occurred_at,
            dead_lettered_at_utc,
            final_attempt_count,
            reason,
            payload_json,
            payload_hash,
            metadata_json)
        VALUES (
            @message_id,
            @event_id,
            @event_type,
            @source,
            @trace_id,
            @occurred_at,
            @dead_lettered_at_utc,
            @final_attempt_count,
            @reason,
            CAST(@payload_json AS JSONB),
            @payload_hash,
            CAST(@metadata_json AS JSONB))
        ON CONFLICT (message_id) DO NOTHING;
        """;

        await using var command = new NpgsqlCommand(sql, connection, transaction)
        {
            Parameters =
            {
                new("message_id", deadLetter.MessageId),
                new("event_id", deadLetter.EventId),
                new("event_type", deadLetter.EventType),
                new("source", deadLetter.Source),
                new("trace_id", deadLetter.TraceId),
                new("occurred_at", deadLetter.OccurredAt),
                new("dead_lettered_at_utc", deadLetter.DeadLetteredAtUtc),
                new("final_attempt_count", deadLetter.FinalAttemptCount),
                new("reason", deadLetter.Reason),
                new("payload_json", deadLetter.PayloadJson),
                new("payload_hash", deadLetter.PayloadHash),
                new("metadata_json", deadLetter.MetadataJson)
            }
        };

        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }
}
