namespace Ontogony.Quotas;

public sealed record QuotaConsumptionRequest(
    string RequestId,
    QuotaLimit Limit,
    decimal Amount,
    DateTimeOffset? OccurredAt = null,
    IReadOnlyDictionary<string, string>? Metadata = null);
