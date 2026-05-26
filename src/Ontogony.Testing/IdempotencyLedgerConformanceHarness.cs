using Ontogony.Idempotency;

namespace Ontogony.Testing;

/// <summary>
/// Conformance helpers for <see cref="IIdempotencyLedger"/> implementations (in-memory or EF-backed).
/// </summary>
public static class IdempotencyLedgerConformanceHarness
{
    /// <summary>Verifies first <see cref="IIdempotencyLedger.TryBeginAsync"/> succeeds and second fails.</summary>
    public static async Task AssertTryBeginIsExclusiveAsync(IIdempotencyLedger ledger, string key)
    {
        ArgumentNullException.ThrowIfNull(ledger);
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("key cannot be empty.", nameof(key));

        var first = await ledger.TryBeginAsync(key);
        var second = await ledger.TryBeginAsync(key);

        if (!first)
            throw new InvalidOperationException($"Expected first TryBeginAsync('{key}') to succeed.");
        if (second)
            throw new InvalidOperationException($"Expected second TryBeginAsync('{key}') to fail while in progress.");
    }

    /// <summary>Verifies succeed/fail transitions and <see cref="IIdempotencyLedger.GetAsync"/> reads.</summary>
    public static async Task AssertLifecycleTransitionsAsync(IIdempotencyLedger ledger, string key)
    {
        ArgumentNullException.ThrowIfNull(ledger);
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("key cannot be empty.", nameof(key));

        if (!await ledger.TryBeginAsync(key))
            throw new InvalidOperationException($"TryBeginAsync('{key}') failed.");

        await ledger.MarkSucceededAsync(key, resultReference: "ref-conformance-ok");
        var succeeded = await ledger.GetAsync(key);
        if (succeeded is null || succeeded.Status != IdempotencyStatus.Succeeded)
            throw new InvalidOperationException($"Expected Succeeded status for '{key}'.");

        if (succeeded.ResultReference != "ref-conformance-ok")
            throw new InvalidOperationException("ResultReference was not persisted.");

        var otherKey = key + "-failed";
        if (!await ledger.TryBeginAsync(otherKey))
            throw new InvalidOperationException($"TryBeginAsync('{otherKey}') failed.");

        await ledger.MarkFailedAsync(otherKey, reason: "conformance-failure");
        var failed = await ledger.GetAsync(otherKey);
        if (failed is null || failed.Status != IdempotencyStatus.Failed)
            throw new InvalidOperationException($"Expected Failed status for '{otherKey}'.");
    }
}
