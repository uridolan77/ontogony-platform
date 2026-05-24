using System.Text.Json;
using Json.Schema;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class ReplayRuntimeSchemaTests
{
    [Theory]
    [InlineData("schemas/fixtures/valid/replay-eligibility-minimal.json", "schemas/contracts/replay-eligibility-v1.schema.json")]
    [InlineData("schemas/fixtures/valid/replay-evidence-bundle-minimal.json", "schemas/contracts/replay-evidence-bundle-v1.schema.json")]
    public void ValidReplayFixtures_PassSchemaValidation(string fixtureRelativePath, string schemaRelativePath)
    {
        var root = GetProjectRoot();
        var schemaFullPath = Path.GetFullPath(Path.Combine(root, schemaRelativePath));
        var schema = JsonSchema.FromFile(schemaFullPath);
        using var doc = JsonDocument.Parse(File.ReadAllText(Path.Combine(root, fixtureRelativePath)));
        var result = schema.Evaluate(doc.RootElement);
        Assert.True(result.IsValid, $"Schema validation failed for {fixtureRelativePath}: {result}");
    }

    [Fact]
    public void ReplayRuntimeContractArtifacts_Exist()
    {
        var root = GetProjectRoot();
        Assert.True(File.Exists(Path.Combine(root, "docs/contracts/REPLAY_RUNTIME_CONTRACT.md")));
        Assert.True(File.Exists(Path.Combine(root, "schemas/contracts/replay-runtime-v1.schema.json")));
        Assert.True(File.Exists(Path.Combine(root, "src/Ontogony.Replay.Contracts/ReplayMode.cs")));
    }

    private static string GetProjectRoot()
    {
        var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ontogony.Platform.sln")))
        {
            dir = dir.Parent;
        }

        return dir?.FullName ?? throw new InvalidOperationException("Could not find project root");
    }
}
