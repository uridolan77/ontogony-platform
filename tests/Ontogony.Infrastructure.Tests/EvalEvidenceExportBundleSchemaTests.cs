using System.Text.Json;
using Json.Schema;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class EvalEvidenceExportBundleSchemaTests
{
    private const string SchemaRelativePath =
        "docs/product-hardening/eval-alignment-frontend-depth/schemas/allagma-eval-evidence-export-bundle-v1.schema.json";

    private const string FixtureRelativePath =
        "docs/product-hardening/eval-alignment-frontend-depth/schemas/fixtures/valid/eval-evidence-export-bundle-minimal.json";

    [Fact]
    public void Minimal_fixture_passes_eval_evidence_export_bundle_schema()
    {
        var repoRoot = GetProjectRoot();
        var schema = JsonSchema.FromText(File.ReadAllText(Path.Combine(repoRoot, SchemaRelativePath)));
        var json = File.ReadAllText(Path.Combine(repoRoot, FixtureRelativePath));
        using var doc = JsonDocument.Parse(json);

        var result = schema.Evaluate(doc.RootElement);

        Assert.True(result.IsValid, $"Schema validation failed: {result}");
    }

    private static string GetProjectRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "Ontogony.Platform.sln")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new InvalidOperationException("Could not locate ontogony-platform repo root.");
    }
}
