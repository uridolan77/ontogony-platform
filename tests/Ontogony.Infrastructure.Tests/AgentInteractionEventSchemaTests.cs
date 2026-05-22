using System.Text.Json;
using Json.Schema;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

/// <summary>
/// PLAT-AGUI-000 — JSON Schema validation for agent interaction event fixtures (JSONL).
/// </summary>
public sealed class AgentInteractionEventSchemaTests
{
    private const string EventSchemaRelativePath =
        "docs/schemas/ontogony-agent-interaction-event-v0.schema.json";

    private static readonly string[] JsonlFixtureRelativePaths =
    [
        "docs/schemas/fixtures/agent-interaction/sample-run.jsonl",
        "docs/schemas/fixtures/agent-interaction/sample-human-gate-interrupt.jsonl",
    ];

    [Theory]
    [MemberData(nameof(JsonlFixturePaths))]
    public void Jsonl_fixture_lines_pass_agent_interaction_event_schema(string fixtureRelativePath)
    {
        var repoRoot = GetProjectRoot();
        var schema = JsonSchema.FromText(File.ReadAllText(Path.Combine(repoRoot, EventSchemaRelativePath)));
        var fixturePath = Path.Combine(repoRoot, fixtureRelativePath);
        Assert.True(File.Exists(fixturePath), $"Missing fixture: {fixturePath}");

        var lineNo = 0;
        foreach (var line in File.ReadLines(fixturePath))
        {
            lineNo++;
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            using var doc = JsonDocument.Parse(line);
            var result = schema.Evaluate(doc.RootElement);
            Assert.True(
                result.IsValid,
                $"Schema validation failed for {fixtureRelativePath} line {lineNo}: {result}");
        }
    }

    public static IEnumerable<object[]> JsonlFixturePaths =>
        JsonlFixtureRelativePaths.Select(p => new object[] { p });

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
