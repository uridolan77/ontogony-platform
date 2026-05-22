namespace Ontogony.SystemCompatibility;

public sealed record SystemCompatibilityGateResult(
    string Schema,
    string Baseline,
    DateTimeOffset EvaluatedAtUtc,
    string DevRoot,
    IReadOnlyList<SystemCompatibilityCheck> Checks,
    bool StrictMode = false)
{
    /// <summary>
    /// True when no checks have status <see cref="SystemCompatibilityCheckStatus.Fail"/>,
    /// and — in strict mode — no checks have status <see cref="SystemCompatibilityCheckStatus.Warn"/> either.
    /// </summary>
    public bool Passed => StrictMode
        ? Checks.All(c => c.Status is SystemCompatibilityCheckStatus.Pass or SystemCompatibilityCheckStatus.Skipped)
        : Checks.All(c => c.Status is SystemCompatibilityCheckStatus.Pass
            or SystemCompatibilityCheckStatus.Warn
            or SystemCompatibilityCheckStatus.Skipped);

    public int PassCount => Checks.Count(c => c.Status == SystemCompatibilityCheckStatus.Pass);

    public int WarnCount => Checks.Count(c => c.Status == SystemCompatibilityCheckStatus.Warn);

    public int FailCount => Checks.Count(c => c.Status == SystemCompatibilityCheckStatus.Fail);

    public int SkippedCount => Checks.Count(c => c.Status == SystemCompatibilityCheckStatus.Skipped);

    public string Verdict => Passed ? "pass" : (FailCount > 0 ? "fail" : "warn");
}
