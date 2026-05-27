using System.Text.Json;
using Json.Schema;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

/// <summary>ONTOGONY-SANDBOX-CONSUMER-ACTIVATION-001A — contract fixture/schema drift guard.</summary>
public sealed class SandboxConsumerActivationSchemaTests
{
    private static readonly string[] ForbiddenTargetEnvironments =
        ["production", "live", "default-runtime"];

    private static readonly (string Fixture, string Schema)[] FixtureSchemaPairs =
    [
        (
            "docs/schemas/fixtures/sandbox-consumer-activation/binding-resolution.sandbox-active.json",
            "docs/schemas/sandbox-consumer-activation/skill-consumer-binding-resolution.v0.schema.json"),
        (
            "docs/schemas/fixtures/sandbox-consumer-activation/binding-resolution.no-active-binding.json",
            "docs/schemas/sandbox-consumer-activation/skill-consumer-binding-resolution.v0.schema.json"),
        (
            "docs/schemas/fixtures/sandbox-consumer-activation/binding-resolution.paused-binding.json",
            "docs/schemas/sandbox-consumer-activation/skill-consumer-binding-resolution.v0.schema.json"),
        (
            "docs/schemas/fixtures/sandbox-consumer-activation/binding-resolution.rolled-back-binding.json",
            "docs/schemas/sandbox-consumer-activation/skill-consumer-binding-resolution.v0.schema.json"),
        (
            "docs/schemas/fixtures/sandbox-consumer-activation/execution-context.sandbox-resolved.json",
            "docs/schemas/sandbox-consumer-activation/skill-consumer-execution-context.v0.schema.json"),
        (
            "docs/schemas/fixtures/sandbox-consumer-activation/skill-version-applied.sandbox.json",
            "docs/schemas/sandbox-consumer-activation/skill-version-applied-evidence.v0.schema.json")
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
    [InlineData("skill-consumer-binding-resolution.v0.schema.json")]
    [InlineData("skill-consumer-binding-resolution-request.v0.schema.json")]
    [InlineData("skill-consumer-execution-context.v0.schema.json")]
    [InlineData("skill-version-applied-evidence.v0.schema.json")]
    public void Sandbox_consumer_activation_schemas_are_loadable(string schemaFileName)
    {
        var repoRoot = GetProjectRoot();
        var path = Path.Combine(repoRoot, "docs/schemas/sandbox-consumer-activation", schemaFileName);
        Assert.True(File.Exists(path), $"Missing schema: {path}");
        var schema = JsonSchema.FromText(File.ReadAllText(path));
        Assert.NotNull(schema);
    }

    [Fact]
    public void Forbidden_production_request_fails_request_schema_validation()
    {
        var repoRoot = GetProjectRoot();
        var schemaPath = Path.Combine(
            repoRoot,
            "docs/schemas/sandbox-consumer-activation/skill-consumer-binding-resolution-request.v0.schema.json");
        var fixturePath = Path.Combine(
            repoRoot,
            "docs/schemas/fixtures/sandbox-consumer-activation/binding-resolution-request.forbidden-production.json");

        var schema = JsonSchema.FromText(File.ReadAllText(schemaPath));
        using var doc = JsonDocument.Parse(File.ReadAllText(fixturePath));
        var result = schema.Evaluate(doc.RootElement);

        Assert.False(result.IsValid, "production targetEnvironment must be rejected by request schema");
        Assert.Equal("production", doc.RootElement.GetProperty("targetEnvironment").GetString());
    }

    [Fact]
    public void Sandbox_active_fixture_resolves_with_skill_version()
    {
        var repoRoot = GetProjectRoot();
        var json = File.ReadAllText(
            Path.Combine(
                repoRoot,
                "docs/schemas/fixtures/sandbox-consumer-activation/binding-resolution.sandbox-active.json"));
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.True(root.GetProperty("resolved").GetBoolean());
        Assert.False(string.IsNullOrWhiteSpace(root.GetProperty("skillVersionId").GetString()));
        Assert.Equal("sandbox", root.GetProperty("targetEnvironment").GetString());
        Assert.True(root.GetProperty("limitations").GetProperty("sandboxOnly").GetBoolean());
    }

    [Theory]
    [InlineData("binding-resolution.no-active-binding.json")]
    [InlineData("binding-resolution.paused-binding.json")]
    [InlineData("binding-resolution.rolled-back-binding.json")]
    public void Unresolved_fixtures_do_not_resolve(string fixtureFileName)
    {
        var repoRoot = GetProjectRoot();
        var json = File.ReadAllText(
            Path.Combine(repoRoot, "docs/schemas/fixtures/sandbox-consumer-activation", fixtureFileName));
        using var doc = JsonDocument.Parse(json);

        Assert.False(doc.RootElement.GetProperty("resolved").GetBoolean());
        Assert.False(string.IsNullOrWhiteSpace(doc.RootElement.GetProperty("reason").GetString()));
    }

    [Fact]
    public void Request_schema_enum_excludes_forbidden_targets()
    {
        var repoRoot = GetProjectRoot();
        var schemaPath = Path.Combine(
            repoRoot,
            "docs/schemas/sandbox-consumer-activation/skill-consumer-binding-resolution-request.v0.schema.json");
        var schemaText = File.ReadAllText(schemaPath);

        foreach (var forbidden in ForbiddenTargetEnvironments)
        {
            Assert.DoesNotContain($"\"{forbidden}\"", schemaText);
        }
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
