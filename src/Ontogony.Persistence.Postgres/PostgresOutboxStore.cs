using Npgsql;
using Ontogony.Persistence;

namespace Ontogony.Persistence.Postgres;

/// <summary>
/// PostgreSQL implementation of outbox and processed-message contracts.
/// </summary>
public sealed class PostgresOutboxStore : IOutboxWriter, IOutboxReader, IOutboxDispatcher, IProcessedMessageStore, IPostgresOutboxClaimStore
{
    private readonly PostgresOutboxOptions _options;
    private readonly PostgresSqlNames _names;
    private readonly Ontogony.Primitives.IClock _clock;
    private readonly IDeadLetterWriter? _deadLetterWriter;
    private readonly string _claimOwner;
    private readonly NpgsqlDataSource _dataSource;

    /// <summary>Creates a PostgreSQL outbox store.</summary>
    public PostgresOutboxStore(
        PostgresOutboxOptions options,
        NpgsqlDataSource dataSource,
        Ontogony.Primitives.IClock? clock = null,
        Ontogony.Primitives.IIdGenerator? idGenerator = null,
        IDeadLetterWriter? deadLetterWriter = null)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        _options.Validate();

        _names = PostgresSqlNames.FromOptions(options);
        _clock = clock ?? new Ontogony.Primitives.SystemClock();
        _deadLetterWriter = deadLetterWriter;
        _claimOwner = (idGenerator ?? new Ontogony.Primitives.GuidIdGenerator()).NewId("outbox-worker");
    }

    /// <summary>Ensures outbox schema objects exist.</summary>
    public async Task EnsureSchemaAsync(CancellationToken cancellationToken = default)
    {
        await PostgresOutboxSchema.EnsureCreatedAsync(_dataSource, _names, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task WriteAsync(OutboxMessage message, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);
        OutboxContracts.Validate(message);

        var sql = $$"""
        INSERT INTO {{_names.QualifiedOutbox}} (
            message_id,
            event_id,
            event_type,
            source,
            trace_id,
            occurred_at,
            available_at,
            attempt_count,
            last_error,
            payload_json,
            payload_hash,
            metadata_json,
            updated_at_utc)
        VALUES (
            @message_id,
            @event_id,
            @event_type,
            @source,
            @trace_id,
            @occurred_at,
            @available_at,
            @attempt_count,
            @last_error,
            CAST(@payload_json AS JSONB),
            @payload_hash,
            CAST(@metadata_json AS JSONB),
            @updated_at_utc);
        """;

        try
        {
            await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
            await using var command = new NpgsqlCommand(sql, connection)
            {
                Parameters =
                {
                    new("message_id", message.MessageId),
                    new("event_id", message.EventId),
                    new("event_type", message.EventType),
                    new("source", message.Source),
                    new("trace_id", message.TraceId),
                    new("occurred_at", message.OccurredAt),
                    new("available_at", message.AvailableAt),
                    new("attempt_count", message.AttemptCount),
                    new("last_error", (object?)message.LastError ?? DBNull.Value),
                    new("payload_json", message.PayloadJson),
                    new("payload_hash", message.PayloadHash),
                    new("metadata_json", message.MetadataJson),
                    new("updated_at_utc", _clock.UtcNow)
                }
            };

            await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            throw new InvalidOperationException($"An outbox message with MessageId '{message.MessageId}' already exists.", ex);
        }
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<OutboxMessage>> ReadAvailableAsync(
        DateTimeOffset asOfUtc,
        int maxBatchSize,
        CancellationToken cancellationToken = default)
    {
        return await ClaimAvailableAsync(asOfUtc, maxBatchSize, leaseDuration: null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<OutboxMessage>> ClaimAvailableAsync(
        DateTimeOffset asOfUtc,
        int maxBatchSize,
        TimeSpan? leaseDuration = null,
        CancellationToken cancellationToken = default)
    {
        if (maxBatchSize < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxBatchSize));
        }

        if (maxBatchSize == 0)
        {
            return [];
        }

        var now = _clock.UtcNow;
        var effectiveLease = leaseDuration ?? _options.ClaimLeaseDuration;
        if (effectiveLease <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(leaseDuration), "Lease duration must be positive.");
        }

        var leaseUntil = now.Add(effectiveLease);

        var sql = $$"""
        WITH candidates AS (
            SELECT message_id
            FROM {{_names.QualifiedOutbox}}
            WHERE dispatched_at_utc IS NULL
              AND dead_lettered_at_utc IS NULL
              AND available_at <= @as_of_utc
              AND (claimed_until_utc IS NULL OR claimed_until_utc <= @now_utc OR claimed_by = @claim_owner)
            ORDER BY available_at ASC, occurred_at ASC
            LIMIT @max_batch_size
            FOR UPDATE SKIP LOCKED
        )
        UPDATE {{_names.QualifiedOutbox}} outbox
        SET claimed_by = @claim_owner,
            claimed_until_utc = @lease_until_utc,
            updated_at_utc = @now_utc
        FROM candidates
        WHERE outbox.message_id = candidates.message_id
        RETURNING
            outbox.message_id,
            outbox.event_id,
            outbox.event_type,
            outbox.source,
            outbox.trace_id,
            outbox.occurred_at,
            outbox.available_at,
            outbox.attempt_count,
            outbox.last_error,
            outbox.payload_json,
            outbox.payload_hash,
            outbox.metadata_json
        ;
        """;

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        await using var command = new NpgsqlCommand(sql, connection, transaction)
        {
            Parameters =
            {
                new("as_of_utc", asOfUtc),
                new("now_utc", now),
                new("claim_owner", _claimOwner),
                new("lease_until_utc", leaseUntil),
                new("max_batch_size", maxBatchSize)
            }
        };

        var rows = new List<OutboxMessage>(Math.Min(maxBatchSize, 512));
        {
            await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                rows.Add(new OutboxMessage(
                    MessageId: reader.GetString(0),
                    EventId: reader.GetString(1),
                    EventType: reader.GetString(2),
                    Source: reader.GetString(3),
                    TraceId: reader.GetString(4),
                    OccurredAt: reader.GetFieldValue<DateTimeOffset>(5),
                    AvailableAt: reader.GetFieldValue<DateTimeOffset>(6),
                    AttemptCount: reader.GetInt32(7),
                    LastError: reader.IsDBNull(8) ? null : reader.GetString(8),
                    PayloadJson: reader.GetFieldValue<string>(9),
                    PayloadHash: reader.GetString(10),
                    MetadataJson: reader.GetFieldValue<string>(11)));
            }
        }

        await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);

        return rows;
    }

    /// <inheritdoc />
    public async Task<bool> TryClaimAsync(
        string messageId,
        DateTimeOffset asOfUtc,
        TimeSpan? leaseDuration = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(messageId))
        {
            throw new ArgumentException("MessageId is required.", nameof(messageId));
        }

        var now = _clock.UtcNow;
        var effectiveLease = leaseDuration ?? _options.ClaimLeaseDuration;
        if (effectiveLease <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(leaseDuration), "Lease duration must be positive.");
        }

        var sql = $$"""
        UPDATE {{_names.QualifiedOutbox}}
        SET claimed_by = @claim_owner,
            claimed_until_utc = @lease_until_utc,
            updated_at_utc = @now_utc
        WHERE message_id = @message_id
          AND available_at <= @as_of_utc
          AND dispatched_at_utc IS NULL
          AND dead_lettered_at_utc IS NULL
          AND (claimed_until_utc IS NULL OR claimed_until_utc <= @now_utc OR claimed_by = @claim_owner);
        """;

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new("message_id", messageId),
                new("as_of_utc", asOfUtc),
                new("claim_owner", _claimOwner),
                new("lease_until_utc", now.Add(effectiveLease)),
                new("now_utc", now)
            }
        };

        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected == 1;
    }

    /// <inheritdoc />
    public async Task<bool> RenewClaimAsync(
        string messageId,
        TimeSpan? leaseDuration = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(messageId))
        {
            throw new ArgumentException("MessageId is required.", nameof(messageId));
        }

        var now = _clock.UtcNow;
        var effectiveLease = leaseDuration ?? _options.ClaimLeaseDuration;
        if (effectiveLease <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(leaseDuration), "Lease duration must be positive.");
        }

        var sql = $$"""
        UPDATE {{_names.QualifiedOutbox}}
        SET claimed_until_utc = @lease_until_utc,
            updated_at_utc = @now_utc
        WHERE message_id = @message_id
          AND claimed_by = @claim_owner
          AND dispatched_at_utc IS NULL
          AND dead_lettered_at_utc IS NULL;
        """;

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new("message_id", messageId),
                new("claim_owner", _claimOwner),
                new("lease_until_utc", now.Add(effectiveLease)),
                new("now_utc", now)
            }
        };

        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected == 1;
    }

    /// <inheritdoc />
    public async Task ReleaseClaimAsync(string messageId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(messageId))
        {
            throw new ArgumentException("MessageId is required.", nameof(messageId));
        }

        var sql = $$"""
        UPDATE {{_names.QualifiedOutbox}}
        SET claimed_by = NULL,
            claimed_until_utc = NULL,
            updated_at_utc = @now_utc
        WHERE message_id = @message_id
          AND claimed_by = @claim_owner
          AND dispatched_at_utc IS NULL
          AND dead_lettered_at_utc IS NULL;
        """;

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new("message_id", messageId),
                new("claim_owner", _claimOwner),
                new("now_utc", _clock.UtcNow)
            }
        };

        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<bool> MarkDispatchedIfOwnedAsync(
        string messageId,
        DateTimeOffset dispatchedAtUtc,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(messageId))
        {
            throw new ArgumentException("MessageId is required.", nameof(messageId));
        }

        var now = _clock.UtcNow;
        var sql = $$"""
        UPDATE {{_names.QualifiedOutbox}}
        SET dispatched_at_utc = @dispatched_at_utc,
            claimed_by = NULL,
            claimed_until_utc = NULL,
            updated_at_utc = @updated_at_utc
        WHERE message_id = @message_id
          AND claimed_by = @claim_owner
          AND (claimed_until_utc IS NULL OR claimed_until_utc > @now_utc)
          AND dispatched_at_utc IS NULL
          AND dead_lettered_at_utc IS NULL;
        """;

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new("message_id", messageId),
                new("claim_owner", _claimOwner),
                new("now_utc", now),
                new("dispatched_at_utc", dispatchedAtUtc),
                new("updated_at_utc", now)
            }
        };

        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected == 1;
    }

    /// <inheritdoc />
    public Task<bool> MarkFailedIfOwnedAsync(
        string messageId,
        string lastError,
        DateTimeOffset nextAvailableAtUtc,
        CancellationToken cancellationToken = default)
    {
        return MarkFailedCoreAsync(messageId, lastError, nextAvailableAtUtc, requireOwnership: true, cancellationToken);
    }

    /// <inheritdoc />
    public async Task MarkDispatchedAsync(string messageId, DateTimeOffset dispatchedAtUtc, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(messageId))
        {
            throw new ArgumentException("MessageId is required.", nameof(messageId));
        }

        var sql = $$"""
        UPDATE {{_names.QualifiedOutbox}}
        SET dispatched_at_utc = @dispatched_at_utc,
            claimed_by = NULL,
            claimed_until_utc = NULL,
            updated_at_utc = @updated_at_utc
        WHERE message_id = @message_id
          AND dispatched_at_utc IS NULL
          AND dead_lettered_at_utc IS NULL;
        """;

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new("message_id", messageId),
                new("dispatched_at_utc", dispatchedAtUtc),
                new("updated_at_utc", _clock.UtcNow)
            }
        };

        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task MarkFailedAsync(
        string messageId,
        string lastError,
        DateTimeOffset nextAvailableAtUtc,
        CancellationToken cancellationToken = default)
    {
        await MarkFailedCoreAsync(messageId, lastError, nextAvailableAtUtc, requireOwnership: false, cancellationToken).ConfigureAwait(false);
    }

    private async Task<bool> MarkFailedCoreAsync(
        string messageId,
        string lastError,
        DateTimeOffset nextAvailableAtUtc,
        bool requireOwnership,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(messageId))
        {
            throw new ArgumentException("MessageId is required.", nameof(messageId));
        }

        ArgumentNullException.ThrowIfNull(lastError);

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

        var selected = await SelectForUpdateAsync(messageId, connection, transaction, cancellationToken).ConfigureAwait(false);
        if (selected is null || selected.DispatchedAtUtc is not null || selected.DeadLetteredAtUtc is not null)
        {
            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            return false;
        }

        var now = _clock.UtcNow;
        if (requireOwnership &&
            (!string.Equals(selected.ClaimedBy, _claimOwner, StringComparison.Ordinal) ||
             (selected.ClaimedUntilUtc is { } claimUntil && claimUntil <= now)))
        {
            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            return false;
        }

        var incrementedAttemptCount = selected.AttemptCount + 1;
        var deadLetterThreshold = _options.MoveToDeadLetterAfterAttempts;
        var shouldDeadLetter = deadLetterThreshold is { } threshold && incrementedAttemptCount >= threshold && _deadLetterWriter is not null;

        var updateSql = shouldDeadLetter
            ? $$"""
                UPDATE {{_names.QualifiedOutbox}}
                SET attempt_count = @attempt_count,
                    last_error = @last_error,
                    available_at = @available_at,
                    dead_lettered_at_utc = @dead_lettered_at_utc,
                    claimed_by = NULL,
                    claimed_until_utc = NULL,
                    updated_at_utc = @updated_at_utc
                WHERE message_id = @message_id{{(requireOwnership ? " AND claimed_by = @claim_owner" : string.Empty)}};
                """
            : $$"""
                UPDATE {{_names.QualifiedOutbox}}
                SET attempt_count = @attempt_count,
                    last_error = @last_error,
                    available_at = @available_at,
                    claimed_by = NULL,
                    claimed_until_utc = NULL,
                    updated_at_utc = @updated_at_utc
                WHERE message_id = @message_id{{(requireOwnership ? " AND claimed_by = @claim_owner" : string.Empty)}};
                """;

        await using (var update = new NpgsqlCommand(updateSql, connection, transaction)
        {
            Parameters =
            {
                new("message_id", messageId),
                new("attempt_count", incrementedAttemptCount),
                new("last_error", lastError),
                new("available_at", nextAvailableAtUtc),
                new("updated_at_utc", now)
            }
        })
        {
            if (requireOwnership)
            {
                update.Parameters.AddWithValue("claim_owner", _claimOwner);
            }

            if (shouldDeadLetter)
            {
                update.Parameters.AddWithValue("dead_lettered_at_utc", now);
            }

            await update.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }

        if (shouldDeadLetter)
        {
            var deadLetter = new DeadLetterMessage(
                MessageId: selected.MessageId,
                EventId: selected.EventId,
                EventType: selected.EventType,
                Source: selected.Source,
                TraceId: selected.TraceId,
                OccurredAt: selected.OccurredAt,
                DeadLetteredAtUtc: now,
                FinalAttemptCount: incrementedAttemptCount,
                Reason: lastError,
                PayloadJson: selected.PayloadJson,
                PayloadHash: selected.PayloadHash,
                MetadataJson: selected.MetadataJson);

            if (_deadLetterWriter is PostgresDeadLetterWriter postgresWriter)
            {
                await postgresWriter.WriteAsync(deadLetter, connection, transaction, cancellationToken).ConfigureAwait(false);
                await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
                return true;
            }

            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            await _deadLetterWriter!.WriteAsync(deadLetter, cancellationToken).ConfigureAwait(false);
            return true;
        }

        await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <inheritdoc />
    public async Task<bool> HasProcessedAsync(string consumerName, string messageId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(consumerName))
        {
            throw new ArgumentException("ConsumerName is required.", nameof(consumerName));
        }

        if (string.IsNullOrWhiteSpace(messageId))
        {
            throw new ArgumentException("MessageId is required.", nameof(messageId));
        }

        var sql = $$"""
        SELECT EXISTS (
            SELECT 1
            FROM {{_names.QualifiedProcessed}}
            WHERE consumer_name = @consumer_name
              AND message_id = @message_id);
        """;

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new("consumer_name", consumerName),
                new("message_id", messageId)
            }
        };

        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return result is true;
    }

    /// <inheritdoc />
    public async Task MarkProcessedAsync(ProcessedMessage message, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        if (string.IsNullOrWhiteSpace(message.ConsumerName))
        {
            throw new ArgumentException("ConsumerName is required.", nameof(message));
        }

        if (string.IsNullOrWhiteSpace(message.MessageId))
        {
            throw new ArgumentException("MessageId is required.", nameof(message));
        }

        var sql = $$"""
        INSERT INTO {{_names.QualifiedProcessed}} (
            consumer_name,
            message_id,
            processed_at,
            trace_id)
        VALUES (
            @consumer_name,
            @message_id,
            @processed_at,
            @trace_id)
        ON CONFLICT (consumer_name, message_id) DO NOTHING;
        """;

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new("consumer_name", message.ConsumerName),
                new("message_id", message.MessageId),
                new("processed_at", message.ProcessedAt),
                new("trace_id", (object?)message.TraceId ?? DBNull.Value)
            }
        };

        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task<SelectedOutboxRow?> SelectForUpdateAsync(
        string messageId,
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        CancellationToken cancellationToken)
    {
        var sql = $$"""
        SELECT
            message_id,
            event_id,
            event_type,
            source,
            trace_id,
            occurred_at,
            attempt_count,
            payload_json,
            payload_hash,
            metadata_json,
            claimed_by,
            claimed_until_utc,
            dispatched_at_utc,
            dead_lettered_at_utc
        FROM {{_names.QualifiedOutbox}}
        WHERE message_id = @message_id
        FOR UPDATE;
        """;

        await using var command = new NpgsqlCommand(sql, connection, transaction)
        {
            Parameters =
            {
                new("message_id", messageId)
            }
        };

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return null;
        }

        return new SelectedOutboxRow(
            MessageId: reader.GetString(0),
            EventId: reader.GetString(1),
            EventType: reader.GetString(2),
            Source: reader.GetString(3),
            TraceId: reader.GetString(4),
            OccurredAt: reader.GetFieldValue<DateTimeOffset>(5),
            AttemptCount: reader.GetInt32(6),
            PayloadJson: reader.GetFieldValue<string>(7),
            PayloadHash: reader.GetString(8),
            MetadataJson: reader.GetFieldValue<string>(9),
                ClaimedBy: reader.IsDBNull(10) ? null : reader.GetString(10),
                ClaimedUntilUtc: reader.IsDBNull(11) ? null : reader.GetFieldValue<DateTimeOffset>(11),
                DispatchedAtUtc: reader.IsDBNull(12) ? null : reader.GetFieldValue<DateTimeOffset>(12),
                DeadLetteredAtUtc: reader.IsDBNull(13) ? null : reader.GetFieldValue<DateTimeOffset>(13));
    }

    private sealed record SelectedOutboxRow(
        string MessageId,
        string EventId,
        string EventType,
        string Source,
        string TraceId,
        DateTimeOffset OccurredAt,
        int AttemptCount,
        string PayloadJson,
        string PayloadHash,
        string MetadataJson,
        string? ClaimedBy,
        DateTimeOffset? ClaimedUntilUtc,
        DateTimeOffset? DispatchedAtUtc,
        DateTimeOffset? DeadLetteredAtUtc);
}
