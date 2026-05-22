using System.Text.Json;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

/// <summary>
/// PLAT-AGUI-000 — agent interaction spine contract index and event matrix gates.
/// </summary>
public sealed class SystemAgentInteractionSpineContractTests
{
    private static readonly string RepoRoot = GetProjectRoot();

    private static readonly string[] RequiredFamilies =
    [
        "RUN",
        "STEP",
        "INTERRUPT",
        "DECISION",
        "MODEL_CALL",
        "EVIDENCE",
    ];

    [Fact]
    public void PLAT_AGUI_000_Contract_artifacts_exist()
    {
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/operators/AGENT_INTERACTION_SPINE_CONTRACT.md")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/system/agent-interaction-event.matrix.json")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/schemas/ontogony-agent-interaction-event-v0.schema.json")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/schemas/ontogony-agent-interaction-session-v0.schema.json")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "scripts/validate-agent-interaction-spine.ps1")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/evidence/PLAT_AGUI_000_EVIDENCE.md")));
        Assert.True(Directory.Exists(Path.Combine(RepoRoot, "docs/schemas/fixtures/agent-interaction")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/operators/AG_UI_COMPATIBILITY_ADAPTER.md")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/operators/AG_UI_EVIDENCE_RESOLVER_CONTRACT.md")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/schemas/ontogony-agent-interaction-evidence-graph-v0.schema.json")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "packages/ontogony-agent-interaction/package.json")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "packages/ontogony-agent-interaction/src/agUiAdapter.ts")));
    }

    [Fact]
    public void PLAT_AGUI_000_Matrix_covers_required_event_families()
    {
        using var doc = JsonDocument.Parse(
            File.ReadAllText(Path.Combine(RepoRoot, "docs/system/agent-interaction-event.matrix.json")));

        var root = doc.RootElement;
        Assert.Equal("ontogony-agent-interaction-event-matrix-v0", root.GetProperty("schema").GetString());
        Assert.Equal("SYSTEM-ALPHA-006", root.GetProperty("baseline").GetString());
        Assert.Equal("ontogony.agent-interaction.v0", root.GetProperty("implementation").GetProperty("protocolId").GetString());

        var required = root.GetProperty("requiredEventFamilies").EnumerateArray()
            .Select(e => e.GetString())
            .ToHashSet(StringComparer.Ordinal);

        foreach (var family in RequiredFamilies)
        {
            Assert.Contains(family, required);
        }

        var matrixFamilies = root.GetProperty("eventFamilies").EnumerateArray()
            .Select(e => e.GetProperty("family").GetString())
            .ToHashSet(StringComparer.Ordinal);

        foreach (var family in RequiredFamilies)
        {
            Assert.Contains(family, matrixFamilies);
        }

        var modelCall = root.GetProperty("eventFamilies").EnumerateArray()
            .Single(e => e.GetProperty("family").GetString() == "MODEL_CALL");
        var sources = modelCall.GetProperty("existingSources").EnumerateArray()
            .Select(s => s.GetString())
            .ToList();
        Assert.Contains("/admin/v0/model-calls/{modelCallId}", sources);
    }

    [Fact]
    public void PLAT_AGUI_000_Contract_cross_links_evidence_spine()
    {
        var contract = File.ReadAllText(Path.Combine(RepoRoot, "docs/operators/AGENT_INTERACTION_SPINE_CONTRACT.md"));
        Assert.Contains("SYSTEM_EVIDENCE_SPINE_CONTRACT.md", contract, StringComparison.Ordinal);
        Assert.Contains("agent-interaction-event.matrix.json", contract, StringComparison.Ordinal);
        Assert.Contains("chain-of-thought", contract, StringComparison.OrdinalIgnoreCase);

        var evidenceSpine = File.ReadAllText(Path.Combine(RepoRoot, "docs/operators/SYSTEM_EVIDENCE_SPINE_CONTRACT.md"));
        Assert.Contains("AGENT_INTERACTION_SPINE_CONTRACT.md", evidenceSpine, StringComparison.Ordinal);
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

        throw new InvalidOperationException("Could not find Ontogony.Platform.sln from test base directory.");
    }
}
