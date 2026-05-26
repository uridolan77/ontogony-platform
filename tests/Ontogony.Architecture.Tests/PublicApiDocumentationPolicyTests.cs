using Ontogony.Testing.Architecture;
using Xunit;

namespace Ontogony.Architecture.Tests;

/// <summary>Guards PLAT-QUALITY-001 Tier A membership in <c>src/Directory.Build.targets</c>.</summary>
public sealed class PublicApiDocumentationPolicyTests
{
    private static readonly string[] TierAProjects =
    [
        "Ontogony.AI.Contracts",
        "Ontogony.Artifacts",
        "Ontogony.Configuration",
        "Ontogony.Contracts",
        "Ontogony.Errors",
        "Ontogony.Evaluation.Contracts",
        "Ontogony.Execution",
        "Ontogony.Hashing",
        "Ontogony.Hosting",
        "Ontogony.Http",
        "Ontogony.Idempotency",
        "Ontogony.Logging",
        "Ontogony.Messaging",
        "Ontogony.Observability",
        "Ontogony.Persistence",
        "Ontogony.Persistence.Postgres",
        "Ontogony.Primitives",
        "Ontogony.ProtocolIngress",
        "Ontogony.Quotas",
        "Ontogony.Redaction",
        "Ontogony.Replay.Contracts",
        "Ontogony.Secrets",
        "Ontogony.Secrets.AzureKeyVault",
        "Ontogony.Security",
        "Ontogony.Topology.Contracts",
    ];

    private static readonly string[] TierCIntentionallyRelaxed =
    [
        "Ontogony.Testing",
        "Ontogony.SystemCompatibility",
    ];

    [Fact]
    public void Directory_build_targets_enforces_cs1591_on_tier_a_shipping_packages()
    {
        var repoRoot = ArchitectureScanTargets.FindRepoRoot(
            AppContext.BaseDirectory,
            "Ontogony.Platform.sln");

        var targets = File.ReadAllText(Path.Combine(repoRoot, "src", "Directory.Build.targets"));

        foreach (var project in TierAProjects)
        {
            Assert.Contains($"'$(MSBuildProjectName)' == '{project}'", targets);
        }

        foreach (var project in TierCIntentionallyRelaxed)
        {
            Assert.DoesNotContain($"'$(MSBuildProjectName)' == '{project}'", targets);
        }
    }
}
