namespace Ontogony.Quotas;

/// <summary>
/// Request to consume quota against a <see cref="QuotaLimit"/> within its rolling/fixed window.
/// </summary>
/// <param name="RequestId">Caller-supplied idempotency key for the consumption attempt.</param>
/// <param name="Limit">Limit definition and scope.</param>
/// <param name="Amount">Non-negative amount to consume.</param>
/// <param name="OccurredAt">Optional logical time for the window; defaults to ledger clock.</param>
/// <param name="Metadata">Optional small opaque metadata.</param>
public sealed record QuotaConsumptionRequest(
    string RequestId,
    QuotaLimit Limit,
    decimal Amount,
    DateTimeOffset? OccurredAt = null,
    IReadOnlyDictionary<string, string>? Metadata = null);
