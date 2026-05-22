using Ontogony.Testing.Architecture;
using Xunit;

namespace Ontogony.Architecture.Tests;

public sealed class ProductSemanticLeakageTests
{
    [Fact]
    public void Scanner_detects_forbidden_product_semantics_in_sample_source()
    {
        var policy = new ProductSemanticBoundaryScanner.Policy(
            Version: 1,
            ScanGlobs: ["**/*.cs"],
            ForbiddenPhrases: ["canonical fact resolution"],
            AllowedPathSuffixes: null);

        var tempDir = Path.Combine(Path.GetTempPath(), "ontogony-product-semantics-" + Guid.NewGuid().ToString("n"));
        var srcDir = Path.Combine(tempDir, "src", "Sample");
        Directory.CreateDirectory(srcDir);

        try
        {
            File.WriteAllText(
                Path.Combine(srcDir, "Bad.cs"),
                """
                namespace Sample;

                /// <summary>Implements canonical fact resolution for operators.</summary>
                public sealed class Bad;
                """);

            var violations = ProductSemanticBoundaryScanner.ScanRepo(tempDir, policy);

            Assert.Single(violations);
            Assert.Equal("canonical fact resolution", violations[0].ForbiddenPhrase);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void Platform_source_has_no_product_semantic_leakage()
    {
        var repoRoot = ArchitectureScanTargets.FindRepoRoot(
            AppContext.BaseDirectory,
            "Ontogony.Platform.sln");

        var policyPath = Path.Combine(repoRoot, "scripts", "product-semantic-boundary-policy.json");
        var policy = ProductSemanticBoundaryScanner.LoadPolicy(policyPath);

        ProductSemanticBoundaryScanner.AssertNoProductSemanticLeakage(repoRoot, policy);
    }
}
