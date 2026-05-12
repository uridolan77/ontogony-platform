namespace Ontogony.Quotas;

/// <summary>
/// Outcome of a <see cref="IQuotaLedger.TryConsumeAsync"/> call.
/// </summary>
/// <param name="Outcome">Allowed or rejected.</param>
/// <param name="Limit">Limit that was evaluated.</param>
/// <param name="Window">Resolved evaluation window.</param>
/// <param name="UsedBefore">Used amount before this attempt.</param>
/// <param name="Requested">Amount requested.</param>
/// <param name="UsedAfter">Used amount after an allowed consumption (unchanged when rejected).</param>
/// <param name="Remaining">Remaining capacity after decision.</param>
/// <param name="ReasonCode">Opaque reason when <see cref="Outcome"/> is <see cref="QuotaOutcome.Rejected"/>.</param>
public sealed record QuotaDecision(
    QuotaOutcome Outcome,
    QuotaLimit Limit,
    QuotaWindow Window,
    decimal UsedBefore,
    decimal Requested,
    decimal UsedAfter,
    decimal Remaining,
    string? ReasonCode = null)
{
    /// <summary>True when <see cref="Outcome"/> is <see cref="QuotaOutcome.Allowed"/>.</summary>
    public bool IsAllowed => Outcome == QuotaOutcome.Allowed;
}
