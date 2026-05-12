namespace Ontogony.Quotas;

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
