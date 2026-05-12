namespace Ontogony.Quotas;

/// <summary>
/// Immutable record of a quota consumption line (audit-friendly; ledger implementation may or may not persist it).
/// </summary>
/// <param name="RequestId">Idempotent request correlation id.</param>
/// <param name="LimitId">Limit definition id.</param>
/// <param name="Scope">Who/what the limit applies to.</param>
/// <param name="Metric">Metric name (often <see cref="QuotaMetric"/> constants).</param>
/// <param name="Window">Active time window.</param>
/// <param name="Amount">Consumed amount in this record.</param>
/// <param name="UsedAfter">Total used in the window after this consumption.</param>
/// <param name="RecordedAt">When the consumption was recorded.</param>
/// <param name="Metadata">Optional small opaque metadata.</param>
public sealed record QuotaConsumptionRecord(
    string RequestId,
    string LimitId,
    QuotaScope Scope,
    string Metric,
    QuotaWindow Window,
    decimal Amount,
    decimal UsedAfter,
    DateTimeOffset RecordedAt,
    IReadOnlyDictionary<string, string>? Metadata = null);
