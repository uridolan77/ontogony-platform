using System.Text.Json;
using Json.Schema;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

/// <summary>ONTOGONY-SKILL-RELEASE-GOVERNANCE-001A — contract fixture/schema drift guard.</summary>
public sealed class SkillReleaseGovernanceSchemaTests
{
    private static readonly string[] ForbiddenTargetEnvironments =
        ["production", "live", "default-runtime"];

    private static readonly (string Fixture, string Schema)[] FixtureSchemaPairs =
    [
        (
            "docs/schemas/fixtures/skill-release/promotion-request.approved-sandbox.json",
            "docs/schemas/skill-release/skill-promotion-request.v0.schema.json"),
        (
            "docs/schemas/fixtures/skill-release/deployment-binding.sandbox-active.json",
            "docs/schemas/skill-release/skill-deployment-binding.v0.schema.json"),
        (
            "docs/schemas/fixtures/skill-release/rollback.executed.json",
            "docs/schemas/skill-release/skill-rollback.v0.schema.json")
    ];

    [Theory]
    [MemberData(nameof(FixtureSchemaPairsMemberData))]
    public void Fixture_validates_against_schema(string fixtureRelativePath, string schemaRelativePath)
    {
        var repoRoot = GetProjectRoot();
        var schema = JsonSchema.FromText(File.ReadAllText(Path.Combine(repoRoot, schemaRelativePath)));
        var json = File.ReadAllText(Path.Combine(repoRoot, fixtureRelativePath));
        using var doc = JsonDocument.Parse(json);

        var result = schema.Evaluate(doc.RootElement);

        Assert.True(result.IsValid, $"Schema validation failed for {fixtureRelativePath}: {result}");
        AssertNoForbiddenTargetEnvironment(doc);
    }

    [Theory]
    [InlineData("skill-promotion-request.v0.schema.json")]
    [InlineData("skill-release-decision.v0.schema.json")]
    [InlineData("skill-deployment-binding.v0.schema.json")]
    [InlineData("skill-rollback.v0.schema.json")]
    [InlineData("skill-release-evidence-export.v0.schema.json")]
    public void Skill_release_schemas_are_loadable(string schemaFileName)
    {
        var repoRoot = GetProjectRoot();
        var path = Path.Combine(repoRoot, "docs/schemas/skill-release", schemaFileName);
        Assert.True(File.Exists(path), $"Missing schema: {path}");
        var schema = JsonSchema.FromText(File.ReadAllText(path));
        Assert.NotNull(schema);
    }

    [Fact]
    public void Promotion_fixture_has_approved_for_sandbox_status()
    {
        var repoRoot = GetProjectRoot();
        var json = File.ReadAllText(
            Path.Combine(repoRoot, "docs/schemas/fixtures/skill-release/promotion-request.approved-sandbox.json"));
        using var doc = JsonDocument.Parse(json);
        Assert.Equal("approved_for_sandbox", doc.RootElement.GetProperty("status").GetString());
        Assert.Equal("sandbox", doc.RootElement.GetProperty("targetEnvironment").GetString());
    }

    public static IEnumerable<object[]> FixtureSchemaPairsMemberData() =>
        FixtureSchemaPairs.Select(p => new object[] { p.Fixture, p.Schema });

    private static void AssertNoForbiddenTargetEnvironment(JsonDocument doc)
    {
        if (!doc.RootElement.TryGetProperty("targetEnvironment", out var env))
        {
            return;
        }

        var value = env.GetString();
        Assert.DoesNotContain(value, ForbiddenTargetEnvironments);
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
