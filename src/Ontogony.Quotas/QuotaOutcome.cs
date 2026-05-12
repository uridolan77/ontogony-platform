namespace Ontogony.Quotas;

/// <summary>
/// Result of a quota consumption attempt.
/// </summary>
public enum QuotaOutcome
{
    /// <summary>Consumption applied within the limit.</summary>
    Allowed = 0,

    /// <summary>Consumption rejected (over limit or invalid request).</summary>
    Rejected = 1
}
