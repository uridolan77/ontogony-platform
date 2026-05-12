namespace Ontogony.Quotas;

public sealed record QuotaLimit(
    string LimitId,
    QuotaScope Scope,
    string Metric,
    decimal MaxAmount,
    TimeSpan Window,
    string? Unit = null);
