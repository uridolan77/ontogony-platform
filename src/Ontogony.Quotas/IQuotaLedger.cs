namespace Ontogony.Quotas;

public interface IQuotaLedger
{
    Task<QuotaDecision> TryConsumeAsync(QuotaConsumptionRequest request, CancellationToken cancellationToken = default);
    Task<decimal> GetUsedAsync(QuotaLimit limit, DateTimeOffset? at = null, CancellationToken cancellationToken = default);
}
