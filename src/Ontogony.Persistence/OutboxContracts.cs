using System.Text.Json;

namespace Ontogony.Persistence;

public interface IOutboxWriter
{
    Task WriteAsync(OutboxMessage message, CancellationToken cancellationToken = default);
}

public interface IOutboxReader
{
    Task<IReadOnlyList<OutboxMessage>> ReadAvailableAsync(
        DateTimeOffset asOfUtc,
        int maxBatchSize,
        CancellationToken cancellationToken = default);
}

public interface IOutboxDispatcher
{
    Task MarkDispatchedAsync(string messageId, DateTimeOffset dispatchedAtUtc, CancellationToken cancellationToken = default);

    Task MarkFailedAsync(string messageId, string lastError, DateTimeOffset nextAvailableAtUtc, CancellationToken cancellationToken = default);
}

public interface IProcessedMessageStore
{
    Task<bool> HasProcessedAsync(string consumerName, string messageId, CancellationToken cancellationToken = default);

    Task MarkProcessedAsync(ProcessedMessage message, CancellationToken cancellationToken = default);
}

public interface IUnitOfWorkBoundary
{
    Task ExecuteAsync(Func<CancellationToken, Task> operation, CancellationToken cancellationToken = default);

    Task<TResult> ExecuteAsync<TResult>(Func<CancellationToken, Task<TResult>> operation, CancellationToken cancellationToken = default);
}

/// <summary>
/// Mechanical DTO for a single outbox row (SQL-agnostic). Hosts map this to storage; this package does not ship a database implementation.
/// </summary>
public sealed record OutboxMessage(
    string MessageId,
    string EventId,
    string EventType,
    string Source,
    string TraceId,
    DateTimeOffset OccurredAt,
    DateTimeOffset AvailableAt,
    int AttemptCount,
    string? LastError,
    string PayloadJson,
    string PayloadHash,
    string MetadataJson);

public sealed record ProcessedMessage(
    string ConsumerName,
    string MessageId,
    DateTimeOffset ProcessedAt,
    string? TraceId = null);

public sealed record OutboxDispatchResult(
    string MessageId,
    bool Dispatched,
    int AttemptCount,
    DateTimeOffset NextAvailableAt,
    string? LastError = null);

/// <summary>
/// Shared validation and helper routines for outbox contracts. Does not perform I/O.
/// </summary>
public static class OutboxContracts
{
    public static void Validate(OutboxMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        Require(message.MessageId, nameof(message.MessageId));
        Require(message.EventId, nameof(message.EventId));
        Require(message.EventType, nameof(message.EventType));
        Require(message.Source, nameof(message.Source));
        Require(message.TraceId, nameof(message.TraceId));
        Require(message.PayloadJson, nameof(message.PayloadJson));
        Require(message.PayloadHash, nameof(message.PayloadHash));
        Require(message.MetadataJson, nameof(message.MetadataJson));

        if (message.AttemptCount < 0)
        {
            throw new ArgumentException("AttemptCount cannot be negative.", nameof(message));
        }
    }

    public static string BuildProcessedMessageKey(string consumerName, string messageId)
    {
        Require(consumerName, nameof(consumerName));
        Require(messageId, nameof(messageId));
        return $"{consumerName}:{messageId}";
    }

    public static DateTimeOffset CalculateNextAvailableAt(
        DateTimeOffset nowUtc,
        int attemptCount,
        TimeSpan? baseDelay = null,
        TimeSpan? maxDelay = null)
    {
        if (attemptCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(attemptCount), "attemptCount cannot be negative.");
        }

        var initial = baseDelay ?? TimeSpan.FromSeconds(2);
        var cap = maxDelay ?? TimeSpan.FromMinutes(5);

        var exponent = Math.Min(attemptCount, 16);
        var multiplier = Math.Pow(2, exponent);
        var delay = TimeSpan.FromMilliseconds(initial.TotalMilliseconds * multiplier);
        if (delay > cap)
        {
            delay = cap;
        }

        return nowUtc.Add(delay);
    }

    public static string SerializeMetadata(IReadOnlyDictionary<string, string>? metadata)
    {
        if (metadata is null || metadata.Count == 0)
        {
            return "{}";
        }

        var ordered = metadata
            .OrderBy(kvp => kvp.Key, StringComparer.Ordinal)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.Ordinal);
        return JsonSerializer.Serialize(ordered);
    }

    private static void Require(string value, string name)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{name} cannot be null or whitespace.", name);
        }
    }
}