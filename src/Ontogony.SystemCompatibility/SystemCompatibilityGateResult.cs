namespace Ontogony.SystemCompatibility;

public sealed record SystemCompatibilityGateResult(
    string Schema,
    string Baseline,
    DateTimeOffset EvaluatedAtUtc,
    string DevRoot,
    IReadOnlyList<SystemCompatibilityCheck> Checks)
{
    public bool Passed => Checks.All(c =>
        c.Status is SystemCompatibilityCheckStatus.Pass
            or SystemCompatibilityCheckStatus.Warn
            or SystemCompatibilityCheckStatus.Skipped);

    public int PassCount => Checks.Count(c => c.Status == SystemCompatibilityCheckStatus.Pass);

    public int WarnCount => Checks.Count(c => c.Status == SystemCompatibilityCheckStatus.Warn);

    public int FailCount => Checks.Count(c => c.Status == SystemCompatibilityCheckStatus.Fail);

    public int SkippedCount => Checks.Count(c => c.Status == SystemCompatibilityCheckStatus.Skipped);
}
