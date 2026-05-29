namespace Ontogony.Idempotency;

/// <summary>
/// Mechanical idempotency ledger (no product semantics for keys).
/// </summary>
public interface IIdempotencyLedger
{
    /// <summary>Reserves <paramref name="key"/> for in-progress work; false if already reserved or completed.</summary>
    Task<bool> TryBeginAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>Marks <paramref name="key"/> as succeeded with optional opaque result reference.</summary>
    Task MarkSucceededAsync(string key, string? resultReference = null, CancellationToken cancellationToken = default);

    /// <summary>Marks <paramref name="key"/> as failed with optional reason.</summary>
    Task MarkFailedAsync(string key, string? reason = null, CancellationToken cancellationToken = default);

    /// <summary>Returns the stored record for <paramref name="key"/>, if any.</summary>
    Task<IdempotencyRecord?> GetAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes <paramref name="key"/> so a failed or orphaned reservation can be retried.
    /// Returns false when the key was not present.
    /// </summary>
    Task<bool> TryRemoveKeyAsync(string key, CancellationToken cancellationToken = default);
}

/// <summary>
/// Stored idempotency state for a key.
/// </summary>
/// <param name="Key">Idempotency key.</param>
/// <param name="Status">Lifecycle status.</param>
/// <param name="CreatedAt">When the key was first seen.</param>
/// <param name="UpdatedAt">Last mutation time.</param>
/// <param name="ResultReference">Opaque handle to a prior successful result.</param>
/// <param name="FailureReason">Opaque failure reason.</param>
public sealed record IdempotencyRecord(
    string Key,
    IdempotencyStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt = null,
    string? ResultReference = null,
    string? FailureReason = null);

/// <summary>
/// Idempotency key lifecycle states.
/// </summary>
public enum IdempotencyStatus
{
    /// <summary>First reservation accepted; work may be in flight.</summary>
    InProgress = 0,

    /// <summary>Work completed successfully.</summary>
    Succeeded = 1,

    /// <summary>Work failed or was abandoned.</summary>
    Failed = 2
}
