namespace Ontogony.SystemCompatibility;

/// <summary>
/// KANON-9-002 / SYSTEM-9B-002 — Kanon semantic decision replay acceptance script and matrix presence.
/// </summary>
internal static class KanonSemanticDecisionReplayConformance
{
    public static SystemCompatibilityCheck CheckKanonReplayAcceptanceArtifacts(SystemCompatibilityWorkspace workspace)
    {
        if (workspace.KanonRoot is null)
        {
            return Skipped(
                "kanon-replay-acceptance-artifacts",
                "Kanon semantic decision replay acceptance",
                "kanon-dotnet not present.");
        }

        var required = new (string Id, string RelativePath)[]
        {
            ("kanon-replay-acceptance-script", "scripts/run-semantic-decision-replay-acceptance.ps1"),
            ("kanon-replay-acceptance-matrix", "docs/system/kanon-semantic-decision-replay-acceptance.matrix.json"),
            ("kanon-replay-acceptance-doc", "docs/e2e/KANON_SEMANTIC_DECISION_REPLAY_ACCEPTANCE.md"),
            ("kanon-replay-acceptance-tests", "tests/Kanon.Tests/KanonSemanticDecisionReplayAcceptanceTests.cs"),
        };

        var missing = required
            .Where(entry => !File.Exists(Path.Combine(workspace.KanonRoot, entry.RelativePath)))
            .Select(entry => entry.RelativePath)
            .ToList();

        if (missing.Count > 0)
        {
            return Fail(
                "kanon-replay-acceptance-artifacts",
                "Kanon semantic decision replay acceptance",
                $"Missing: {string.Join(", ", missing)}");
        }

        return Pass(
            "kanon-replay-acceptance-artifacts",
            "Kanon semantic decision replay acceptance",
            "KANON-9-002 scripts, matrix, docs, and acceptance tests are present.");
    }

    private static SystemCompatibilityCheck Pass(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Pass, detail);

    private static SystemCompatibilityCheck Fail(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Fail, detail);

    private static SystemCompatibilityCheck Skipped(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Skipped, detail);
}
