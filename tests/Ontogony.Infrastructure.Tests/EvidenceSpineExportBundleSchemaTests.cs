using System.Text.Json;
using System.Linq;
using Json.Schema;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class EvidenceSpineExportBundleSchemaTests
{
    private const string SchemaRelativePath =
        "docs/schemas/ontogony-cross-service-evidence-spine-bundle-v1.schema.json";

    private const string MinimalFixtureRelativePath =
        "docs/schemas/fixtures/valid/cross-service-evidence-spine-bundle-minimal.json";

    private const string GeneratedFixtureRelativePath =
        "docs/schemas/fixtures/valid/cross-service-evidence-spine-bundle-generated.json";

    [Fact]
    public void Minimal_fixture_passes_cross_service_evidence_spine_bundle_schema()
    {
        AssertFixturePassesSchema(MinimalFixtureRelativePath);
        AssertFixtureContainsRuntimeProtocolMetadata(MinimalFixtureRelativePath);
    }

    [Fact]
    public void Generated_frontend_fixture_passes_cross_service_evidence_spine_bundle_schema()
    {
        AssertFixturePassesSchema(GeneratedFixtureRelativePath);
        AssertFixtureContainsRuntimeProtocolMetadata(GeneratedFixtureRelativePath);
    }

    private static void AssertFixtureContainsRuntimeProtocolMetadata(string fixtureRelativePath)
    {
        using var doc = JsonDocument.Parse(File.ReadAllText(Path.Combine(GetProjectRoot(), fixtureRelativePath)));

        var nodes = doc.RootElement.GetProperty("graph").GetProperty("nodes").EnumerateArray().ToList();
        Assert.NotEmpty(nodes);
        Assert.All(nodes, n =>
        {
            Assert.True(n.TryGetProperty("protocolId", out var protocolId) && protocolId.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(protocolId.GetString()));
            Assert.True(n.TryGetProperty("authorityMode", out var authorityMode) && authorityMode.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(authorityMode.GetString()));
            Assert.True(n.TryGetProperty("sideEffectLevel", out var sideEffectLevel) && sideEffectLevel.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(sideEffectLevel.GetString()));
        });

        var attempts = doc.RootElement.GetProperty("sourceAttempts").EnumerateArray().ToList();
        Assert.NotEmpty(attempts);
        Assert.All(attempts, a =>
        {
            Assert.True(a.TryGetProperty("protocolId", out var protocolId) && protocolId.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(protocolId.GetString()));
            Assert.True(a.TryGetProperty("authorityMode", out var authorityMode) && authorityMode.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(authorityMode.GetString()));
            Assert.True(a.TryGetProperty("sideEffectLevel", out var sideEffectLevel) && sideEffectLevel.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(sideEffectLevel.GetString()));
        });
    }

    private static void AssertFixturePassesSchema(string fixtureRelativePath)
    {
        var repoRoot = GetProjectRoot();
        var schema = JsonSchema.FromText(File.ReadAllText(Path.Combine(repoRoot, SchemaRelativePath)));
        var json = File.ReadAllText(Path.Combine(repoRoot, fixtureRelativePath));
        using var doc = JsonDocument.Parse(json);

        var result = schema.Evaluate(doc.RootElement);

        Assert.True(result.IsValid, $"Schema validation failed for {fixtureRelativePath}: {result}");
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
