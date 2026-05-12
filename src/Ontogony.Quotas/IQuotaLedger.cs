namespace Ontogony.Quotas;

/// <summary>
/// Mechanical quota consumption ledger (no product plan or tier semantics).
/// </summary>
public interface IQuotaLedger
{
    /// <summary>Attempts to consume <paramref name="request"/>.Amount against the limit window.</summary>
    Task<QuotaDecision> TryConsumeAsync(QuotaConsumptionRequest request, CancellationToken cancellationToken = default);

    /// <summary>Returns cumulative used amount for the limit window containing <paramref name="at"/> (or now).</summary>
    Task<decimal> GetUsedAsync(QuotaLimit limit, DateTimeOffset? at = null, CancellationToken cancellationToken = default);
}
