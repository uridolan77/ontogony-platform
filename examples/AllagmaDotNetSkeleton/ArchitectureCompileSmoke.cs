using Ontogony.Testing.Architecture;

namespace AllagmaDotNetSkeleton;

/// <summary>
/// Compile-only illustration of forbidden-dependency scanning for a future allagma-dotnet test project.
/// See <c>docs/adoption/architecture-tests-adoption.md</c>.
/// </summary>
internal static class ArchitectureCompileSmoke
{
    internal static readonly string[] ExampleForbiddenFragments =
    [
        "OpenAI",
        "Anthropic",
        "Azure.AI.OpenAI",
        "Google.AI",
        "Microsoft.Agents",
        "Kanon.",
        "Conexus.",
        "Agentor."
    ];

    internal static void ExampleScan(string repoRoot)
    {
        var scanTargets = ArchitectureScanTargets.CollectMsBuildScanTargets(
            repoRoot,
            additionalRelativeFiles:
            [
                "eng/Ontogony.References.props",
                "eng/Directory.Packages.props"
            ]);

        ArchitectureReferenceAssertions.AssertNoForbiddenReferences(
            repoRoot,
            scanTargets,
            ExampleForbiddenFragments);
    }
}
