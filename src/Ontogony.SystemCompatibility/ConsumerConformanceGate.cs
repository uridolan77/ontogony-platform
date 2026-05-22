namespace Ontogony.SystemCompatibility;

/// <summary>
/// Groups mechanical compatibility checks by alpha consumer (Conexus, Kanon, Allagma, frontend, UI).
/// Proof is derived from <see cref="SystemCompatibilityGate"/> and <see cref="SixRepoCompatibilityGate"/>.
/// </summary>
public static class ConsumerConformanceGate
{
    private static readonly IReadOnlyDictionary<string, string[]> CheckIdPatternsByConsumer =
        new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["conexus"] =
            [
                "conexus-manifest",
                "error-envelope",
                "propagation-header",
                "six-repo-openapi",
            ],
            ["kanon"] =
            [
                "kanon-manifest",
                "kanon-replay",
                "kanon-domain-pack",
                "error-envelope",
                "propagation-header",
            ],
            ["allagma"] =
            [
                "allagma-feature-matrix",
                "registry-lock",
                "error-envelope",
                "propagation-header",
                "six-repo-backend-align",
            ],
            ["frontend"] =
            [
                "frontend-matrix",
                "frontend-route",
                "route-client",
                "live-artifact-evidence",
                "error-envelope-frontend",
                "six-repo-openapi",
                "six-repo-route-inventory",
                "six-repo-fe-provenance",
            ],
            ["ui"] =
            [
                "six-repo-ui-version",
                "six-repo-ui-manifest",
            ],
        };

    public static ConsumerConformanceResult Evaluate(SystemCompatibilityGateOptions options)
    {
        var workspace = SystemCompatibilityWorkspace.Resolve(options);
        var system = SystemCompatibilityGate.Evaluate(options);
        var sixRepo = SixRepoCompatibilityGate.Evaluate(options);
        var allChecks = system.Checks.Concat(sixRepo.Checks).ToList();

        var consumers = CheckIdPatternsByConsumer.Keys
            .OrderBy(c => c, StringComparer.Ordinal)
            .Select(consumer => BuildConsumerSummary(consumer, allChecks))
            .ToList();

        return new ConsumerConformanceResult(
            "ontogony-consumer-conformance-v1",
            system.Baseline,
            DateTimeOffset.UtcNow,
            workspace.DevRoot,
            consumers,
            options.StrictMode);
    }

    private static ConsumerConformanceSummary BuildConsumerSummary(
        string consumer,
        IReadOnlyList<SystemCompatibilityCheck> allChecks)
    {
        var patterns = CheckIdPatternsByConsumer[consumer];
        var matched = allChecks
            .Where(c => patterns.Any(p => c.Id.Contains(p, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        if (matched.Count == 0)
        {
            return new ConsumerConformanceSummary(
                consumer,
                "gap",
                0,
                0,
                0,
                0,
                ["No mapped compatibility checks matched this consumer."],
                []);
        }

        var pass = matched.Count(c => c.Status == SystemCompatibilityCheckStatus.Pass);
        var warn = matched.Count(c => c.Status == SystemCompatibilityCheckStatus.Warn);
        var fail = matched.Count(c => c.Status == SystemCompatibilityCheckStatus.Fail);
        var skipped = matched.Count(c => c.Status == SystemCompatibilityCheckStatus.Skipped);

        var verdict = fail > 0 ? "fail" : warn > 0 ? "warn" : "pass";
        var proofs = matched
            .Select(c => new ConsumerConformanceProof(c.Id, c.Status.ToString(), c.Title, c.Detail))
            .ToList();

        return new ConsumerConformanceSummary(
            consumer,
            verdict,
            pass,
            warn,
            fail,
            skipped,
            [],
            proofs);
    }
}

public sealed record ConsumerConformanceProof(
    string CheckId,
    string Status,
    string Title,
    string Detail);

public sealed record ConsumerConformanceSummary(
    string Consumer,
    string Verdict,
    int PassCount,
    int WarnCount,
    int FailCount,
    int SkippedCount,
    IReadOnlyList<string> Notes,
    IReadOnlyList<ConsumerConformanceProof> Proofs)
{
    public bool Passed => Verdict is "pass";
}

public sealed record ConsumerConformanceResult(
    string Schema,
    string Baseline,
    DateTimeOffset EvaluatedAtUtc,
    string DevRoot,
    IReadOnlyList<ConsumerConformanceSummary> Consumers,
    bool StrictMode = false)
{
    public bool Passed => StrictMode
        ? Consumers.All(c => c.Verdict == "pass")
        : Consumers.All(c => c.Verdict is "pass" or "warn");

    public string Verdict => Passed
        ? "pass"
        : Consumers.Any(c => c.Verdict == "fail")
            ? "fail"
            : "warn";
}
