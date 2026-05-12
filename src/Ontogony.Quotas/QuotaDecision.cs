namespace Ontogony.Quotas;

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
    public bool IsAllowed => Outcome == QuotaOutcome.Allowed;
}
