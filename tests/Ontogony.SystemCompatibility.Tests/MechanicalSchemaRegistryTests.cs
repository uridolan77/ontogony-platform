using System.Text.Json;
using Json.Schema;
using Xunit;

namespace Ontogony.SystemCompatibility.Tests;

/// <summary>PLAT-MECH-005/006 — mechanical schema registry validation.</summary>
public sealed class MechanicalSchemaRegistryTests
{
    private static readonly string[] RequiredSchemaFiles =
    [
        "error-envelope.schema.json",
        "correlation-headers.schema.json",
        "evidence-reference.schema.json",
        "idempotency-state.schema.json",
        "replay-contract.schema.json",
        "actor-context.schema.json",
        "observability-meter.schema.json",
        "consumer-conformance-report.schema.json",
        "platform-proposal-gate.schema.json",
    ];

    [Fact]
    public void All_mechanical_schemas_have_id_title_and_type()
    {
        var schemaDir = GetSchemaDirectory();
        Assert.True(Directory.Exists(schemaDir), $"Missing schema directory: {schemaDir}");

        foreach (var fileName in RequiredSchemaFiles)
        {
            var path = Path.Combine(schemaDir, fileName);
            Assert.True(File.Exists(path), $"Missing schema: {path}");

            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            var root = doc.RootElement;
            Assert.True(root.TryGetProperty("$id", out var id) && id.ValueKind == JsonValueKind.String);
            Assert.True(root.TryGetProperty("title", out var title) && title.ValueKind == JsonValueKind.String);
            Assert.True(root.TryGetProperty("type", out var type) && type.GetString() == "object");
        }
    }

    [Theory]
    [InlineData("error-envelope.schema.json", "fixtures/mechanics/v1/valid/error-envelope-minimal.json", true)]
    [InlineData("error-envelope.schema.json", "fixtures/mechanics/v1/invalid/error-envelope-missing-code.json", false)]
    [InlineData("correlation-headers.schema.json", "fixtures/mechanics/v1/valid/correlation-headers-minimal.json", true)]
    [InlineData("idempotency-state.schema.json", "fixtures/mechanics/v1/valid/idempotency-state-reserved.json", true)]
    [InlineData("evidence-reference.schema.json", "fixtures/mechanics/v1/valid/evidence-reference-minimal.json", true)]
    [InlineData("actor-context.schema.json", "fixtures/mechanics/v1/valid/actor-context-minimal.json", true)]
    [InlineData("replay-contract.schema.json", "fixtures/mechanics/v1/valid/replay-contract-minimal.json", true)]
    [InlineData("observability-meter.schema.json", "fixtures/mechanics/v1/valid/observability-meter-minimal.json", true)]
    [InlineData("platform-proposal-gate.schema.json", "fixtures/mechanics/v1/valid/platform-proposal-accepted.json", true)]
    public void Fixture_matches_expected_schema_validation(string schemaFile, string fixtureRelativePath, bool shouldBeValid)
    {
        AssertSchemaValidation(schemaFile, fixtureRelativePath, shouldBeValid);
    }

    [Fact]
    public void Validate_fixture_against_schema()
    {
        var schemaPath = Environment.GetEnvironmentVariable("ONTOGONY_MECH_SCHEMA");
        var fixturePath = Environment.GetEnvironmentVariable("ONTOGONY_MECH_FIXTURE");
        if (string.IsNullOrWhiteSpace(schemaPath) || string.IsNullOrWhiteSpace(fixturePath))
        {
            return;
        }

        var expectInvalid = string.Equals(
            Environment.GetEnvironmentVariable("ONTOGONY_MECH_EXPECT_INVALID"),
            "1",
            StringComparison.Ordinal);

        AssertSchemaValidation(
            Path.GetFileName(schemaPath),
            fixturePath,
            !expectInvalid,
            schemaPath: schemaPath);
    }

    private static void AssertSchemaValidation(
        string schemaFile,
        string fixtureRelativePath,
        bool shouldBeValid,
        string? schemaPath = null)
    {
        var repoRoot = FindRepoRoot();
        schemaPath ??= Path.Combine(GetSchemaDirectory(), schemaFile);
        var fixturePath = Path.IsPathRooted(fixtureRelativePath)
            ? fixtureRelativePath
            : Path.Combine(repoRoot, fixtureRelativePath);

        var schema = JsonSchema.FromText(File.ReadAllText(schemaPath));
        using var doc = JsonDocument.Parse(File.ReadAllText(fixturePath));
        var result = schema.Evaluate(doc.RootElement);

        if (shouldBeValid)
        {
            Assert.True(result.IsValid, $"Expected valid fixture: {fixtureRelativePath}");
        }
        else
        {
            Assert.False(result.IsValid, $"Expected invalid fixture: {fixtureRelativePath}");
        }
    }

    private static string GetSchemaDirectory()
    {
        return Path.Combine(FindRepoRoot(), "schemas", "mechanics", "v1");
    }

    private static string FindRepoRoot()
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

        throw new InvalidOperationException("Could not locate Ontogony.Platform.sln from test output.");
    }
}
