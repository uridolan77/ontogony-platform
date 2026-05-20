using System.Text.Json;
using Json.Schema;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class EvidenceSpineExportBundleSchemaTests
{
    private const string SchemaRelativePath =
        "docs/schemas/ontogony-cross-service-evidence-spine-bundle-v1.schema.json";

    private const string FixtureRelativePath =
        "docs/schemas/fixtures/valid/cross-service-evidence-spine-bundle-minimal.json";

    [Fact]
    public void Minimal_fixture_passes_cross_service_evidence_spine_bundle_schema()
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
