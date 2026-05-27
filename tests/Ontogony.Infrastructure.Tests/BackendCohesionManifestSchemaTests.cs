using System.Text.Json;
using Json.Schema;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

/// <summary>ONTOGONY-BACKEND-COHESION-PLATFORM-001 — manifest/schema drift guard.</summary>
public sealed class BackendCohesionManifestSchemaTests
{
    private const string SchemaRelativePath =
        "docs/schemas/backend-cohesion/backend-cohesion-manifest.v0.schema.json";

    private const string ManifestRelativePath =
        "docs/system/backend-cohesion/ONTOGONY_BACKEND_COHESION_MANIFEST.v0.json";

    private const string ExampleFixtureRelativePath =
        "docs/schemas/fixtures/backend-cohesion/backend-cohesion-manifest.example.json";

    private static readonly string[] RequiredRuntimeBackendRepos =
    [
        "allagma-dotnet",
        "kanon-dotnet",
        "conexus-dotnet",
        "metabole-dotnet"
    ];

    [Fact]
    public void Backend_cohesion_manifest_schema_is_loadable()
    {
        var repoRoot = GetProjectRoot();
        var path = Path.Combine(repoRoot, SchemaRelativePath);
        Assert.True(File.Exists(path), $"Missing schema: {path}");

        var schema = JsonSchema.FromText(File.ReadAllText(path));
        Assert.NotNull(schema);
    }

    [Fact]
    public void Canonical_manifest_passes_schema()
    {
        AssertDocumentPassesSchema(ManifestRelativePath);
    }

    [Fact]
    public void Example_fixture_passes_schema()
    {
        AssertDocumentPassesSchema(ExampleFixtureRelativePath);
    }

    [Fact]
    public void Manifest_declares_platform_truth_repo_and_runtime_backends()
    {
        using var doc = LoadManifest();

        Assert.Equal("ontogony-platform", doc.RootElement.GetProperty("platformTruthRepo").GetString());

        var runtimeIds = doc.RootElement
            .GetProperty("runtimeBackendRepos")
            .EnumerateArray()
            .Select(r => r.GetProperty("id").GetString())
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .ToHashSet(StringComparer.Ordinal);

        foreach (var required in RequiredRuntimeBackendRepos)
        {
            Assert.Contains(required, runtimeIds);
        }
    }

    [Fact]
    public void Manifest_golden_scenario_ids_are_unique()
    {
        using var doc = LoadManifest();
        var ids = doc.RootElement
            .GetProperty("goldenScenarios")
            .EnumerateArray()
            .Select(s => s.GetProperty("id").GetString())
            .ToList();

        Assert.Equal(ids.Count, ids.Distinct(StringComparer.Ordinal).Count());
    }

    [Fact]
    public void Manifest_deferral_ids_are_unique()
    {
        using var doc = LoadManifest();
        var ids = doc.RootElement
            .GetProperty("deferrals")
            .EnumerateArray()
            .Select(d => d.GetProperty("id").GetString())
            .ToList();

        Assert.Equal(ids.Count, ids.Distinct(StringComparer.Ordinal).Count());
    }

    [Fact]
    public void Manifest_each_feature_spine_has_status()
    {
        using var doc = LoadManifest();

        foreach (var spine in doc.RootElement.GetProperty("featureSpines").EnumerateArray())
        {
            Assert.True(spine.TryGetProperty("status", out var status));
            Assert.False(string.IsNullOrWhiteSpace(status.GetString()));
        }
    }

    [Fact]
    public void Manifest_closed_feature_spines_have_evidence_docs()
    {
        using var doc = LoadManifest();

        foreach (var spine in doc.RootElement.GetProperty("featureSpines").EnumerateArray())
        {
            if (!string.Equals(spine.GetProperty("status").GetString(), "closed", StringComparison.Ordinal))
            {
                continue;
            }

            Assert.True(spine.TryGetProperty("evidenceDocs", out var evidenceDocs));
            Assert.Equal(JsonValueKind.Array, evidenceDocs.ValueKind);
            Assert.NotEmpty(evidenceDocs.EnumerateArray());
        }
    }

    [Fact]
    public void Manifest_production_deployment_is_not_complete()
    {
        using var doc = LoadManifest();
        var status = doc.RootElement.GetProperty("productionDeployment").GetProperty("status").GetString();

        Assert.NotEqual("complete", status);
        Assert.Equal("not_complete", status);
    }

    private static JsonDocument LoadManifest()
    {
        var json = File.ReadAllText(Path.Combine(GetProjectRoot(), ManifestRelativePath));
        return JsonDocument.Parse(json);
    }

    private static void AssertDocumentPassesSchema(string relativePath)
    {
        var repoRoot = GetProjectRoot();
        var schema = JsonSchema.FromText(File.ReadAllText(Path.Combine(repoRoot, SchemaRelativePath)));
        var json = File.ReadAllText(Path.Combine(repoRoot, relativePath));
        using var doc = JsonDocument.Parse(json);

        var result = schema.Evaluate(doc.RootElement);

        Assert.True(result.IsValid, $"Schema validation failed for {relativePath}: {result}");
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
