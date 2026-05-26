using Ontogony.Idempotency;

namespace Ontogony.Testing.Conformance;

/// <summary>
/// Facade for <see cref="IIdempotencyLedger"/> conformance harness runs.
/// </summary>
public static class IdempotencyConformanceKit
{
    /// <summary>Runs exclusive begin + lifecycle transitions against the supplied ledger.</summary>
    public static async Task RunStandardLedgerChecksAsync(IIdempotencyLedger ledger, string? keyPrefix = null)
    {
        ArgumentNullException.ThrowIfNull(ledger);

        var prefix = string.IsNullOrWhiteSpace(keyPrefix)
            ? "ontogony://test/conformance/idempotency"
            : keyPrefix.TrimEnd('/');

        var exclusiveKey = $"{prefix}/{Guid.NewGuid():N}";
        await IdempotencyLedgerConformanceHarness.AssertTryBeginIsExclusiveAsync(ledger, exclusiveKey)
            .ConfigureAwait(false);

        var lifecycleKey = $"{prefix}/lifecycle/{Guid.NewGuid():N}";
        await IdempotencyLedgerConformanceHarness.AssertLifecycleTransitionsAsync(ledger, lifecycleKey)
            .ConfigureAwait(false);
    }
}
