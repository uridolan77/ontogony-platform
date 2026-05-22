namespace Ontogony.SystemCompatibility;

/// <summary>
/// KANON-9-001 / SYSTEM-9A-004 — Kanon domain pack lifecycle manifest and acceptance presence.
/// </summary>
internal static class KanonDomainPackLifecycleConformance
{
    public static SystemCompatibilityCheck CheckKanonDomainPackLifecycleArtifacts(
        SystemCompatibilityWorkspace workspace)
    {
        if (workspace.KanonRoot is null)
        {
            return Skipped(
                "kanon-domain-pack-lifecycle-artifacts",
                "Kanon domain pack lifecycle acceptance",
                "kanon-dotnet not present.");
        }

        var required = new (string Id, string RelativePath)[]
        {
            ("kanon-lifecycle-script", "scripts/run-domain-pack-lifecycle-acceptance.ps1"),
            ("kanon-lifecycle-manifest", "docs/generated/KANON_DOMAIN_PACK_LIFECYCLE_MANIFEST.json"),
            ("kanon-lifecycle-contract", "docs/contracts/KANON_DOMAIN_PACK_LIFECYCLE_CONTRACT.md"),
            ("kanon-lifecycle-matrix", "docs/system/kanon-domain-pack-lifecycle-acceptance.matrix.json"),
            ("kanon-lifecycle-doc", "docs/e2e/KANON_DOMAIN_PACK_LIFECYCLE_ACCEPTANCE.md"),
            ("kanon-lifecycle-governance-tests", "tests/Kanon.Tests/DomainPackLifecycleGovernanceTests.cs"),
        };

        var missing = required
            .Where(entry => !File.Exists(Path.Combine(workspace.KanonRoot, entry.RelativePath)))
            .Select(entry => entry.RelativePath)
            .ToList();

        if (missing.Count > 0)
        {
            return Fail(
                "kanon-domain-pack-lifecycle-artifacts",
                "Kanon domain pack lifecycle acceptance",
                $"Missing: {string.Join(", ", missing)}");
        }

        return Pass(
            "kanon-domain-pack-lifecycle-artifacts",
            "Kanon domain pack lifecycle acceptance",
            "KANON-9-001 manifest, contract, matrix, and governance tests are present.");
    }

    private static SystemCompatibilityCheck Pass(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Pass, detail);

    private static SystemCompatibilityCheck Fail(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Fail, detail);

    private static SystemCompatibilityCheck Skipped(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Skipped, detail);
}
