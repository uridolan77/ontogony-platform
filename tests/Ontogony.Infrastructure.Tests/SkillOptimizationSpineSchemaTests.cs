using System.Text.Json;
using Json.Schema;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

/// <summary>ONTOGONY-SKILL-OPTIMIZATION-SPINE-001A — contract fixture/schema drift guard.</summary>
public sealed class SkillOptimizationSpineSchemaTests
{
    private const string FixtureRelativePath =
        "docs/schemas/fixtures/skill-optimization/fixture-skill-optimization-run-example.json";

    private const string RunSchemaRelativePath =
        "docs/schemas/skill-optimization/skill-optimization-run.schema.json";

    [Fact]
    public void Fixture_skill_optimization_run_passes_run_schema()
    {
        var repoRoot = GetProjectRoot();
        var schema = JsonSchema.FromText(File.ReadAllText(Path.Combine(repoRoot, RunSchemaRelativePath)));
        var json = File.ReadAllText(Path.Combine(repoRoot, FixtureRelativePath));
        using var doc = JsonDocument.Parse(json);

        var result = schema.Evaluate(doc.RootElement);

        Assert.True(result.IsValid, $"Schema validation failed: {result}");
        Assert.True(doc.RootElement.TryGetProperty("skillOptimizationRunId", out var runId));
        Assert.False(string.IsNullOrWhiteSpace(runId.GetString()));
    }

    [Theory]
    [InlineData("skill-artifact.schema.json")]
    [InlineData("skill-version.schema.json")]
    [InlineData("skill-edit.schema.json")]
    [InlineData("skill-evaluation-gate.schema.json")]
    [InlineData("rejected-skill-edit-buffer.schema.json")]
    [InlineData("skill-deployment-binding.schema.json")]
    public void Skill_optimization_schemas_are_loadable(string schemaFileName)
    {
        var repoRoot = GetProjectRoot();
        var path = Path.Combine(repoRoot, "docs/schemas/skill-optimization", schemaFileName);
        Assert.True(File.Exists(path), $"Missing schema: {path}");
        var schema = JsonSchema.FromText(File.ReadAllText(path));
        Assert.NotNull(schema);
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
