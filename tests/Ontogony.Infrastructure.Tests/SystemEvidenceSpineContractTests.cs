using System.Text.Json;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

/// <summary>
/// SYS-TIGHT-002 — system evidence spine contract index and resolution matrix gates.
/// </summary>
public sealed class SystemEvidenceSpineContractTests
{
    private static readonly string RepoRoot = GetProjectRoot();

    private static readonly string[] RequiredKinds =
    [
        "allagmaRunId",
        "kanonDecisionId",
        "conexusModelCallId",
        "conexusRouteDecisionId",
        "traceId",
        "correlationId",
        "humanGateId",
        "domainPackId",
    ];

    [Fact]
    public void SYS_TIGHT_002_Contract_artifacts_exist()
    {
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/operators/SYSTEM_EVIDENCE_SPINE_CONTRACT.md")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/system/system-evidence-spine-resolution.matrix.json")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/system/schemas/system-evidence-spine-resolution.matrix.schema.json")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "scripts/validate-system-evidence-spine-contract.ps1")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/evidence/SYS_TIGHT_002_SYSTEM_EVIDENCE_SPINE_CONTRACT_EVIDENCE.md")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/system/EVIDENCE_SPINE_GRAPH_TAXONOMY.md")));
    }

    [Fact]
    public void SYS_TIGHT_002_Resolution_matrix_covers_required_identifier_kinds()
    {
        using var doc = JsonDocument.Parse(
            File.ReadAllText(Path.Combine(RepoRoot, "docs/system/system-evidence-spine-resolution.matrix.json")));

        var root = doc.RootElement;
        Assert.Equal("ontogony-system-evidence-spine-resolution-v1", root.GetProperty("schema").GetString());
        Assert.Equal("SYSTEM-ALPHA-006", root.GetProperty("baseline").GetString());
        Assert.Equal("non_fatal_explicit", root.GetProperty("unresolvedEdgePolicy").GetString());
        Assert.Equal("ontogony.evidence-spine.resolve.v1", root.GetProperty("implementation").GetProperty("protocolId").GetString());

        var required = root.GetProperty("requiredIdentifierKinds").EnumerateArray().Select(e => e.GetString()).ToHashSet(StringComparer.Ordinal);
        foreach (var kind in RequiredKinds)
        {
            Assert.Contains(kind, required);
        }

        var matrixKinds = root.GetProperty("identifiers").EnumerateArray()
            .Select(e => e.GetProperty("kind").GetString())
            .ToHashSet(StringComparer.Ordinal);

        foreach (var kind in RequiredKinds)
        {
            Assert.Contains(kind, matrixKinds);
        }

        var conexusModelCall = root.GetProperty("identifiers").EnumerateArray()
            .Single(e => e.GetProperty("kind").GetString() == "conexusModelCallId");
        var paths = conexusModelCall.GetProperty("routes").EnumerateArray()
            .Select(r => r.GetProperty("path").GetString())
            .ToList();
        Assert.Contains("/admin/v0/model-calls/{modelCallId}", paths);
    }

    [Fact]
    public void EVIDENCE_SPINE_REPLAY_KANON_001_Matrix_includes_replay_roots()
    {
        using var doc = JsonDocument.Parse(
            File.ReadAllText(Path.Combine(RepoRoot, "docs/system/system-evidence-spine-resolution.matrix.json")));

        var root = doc.RootElement;
        var supplemental = root.GetProperty("supplementalRequiredIdentifierKinds").EnumerateArray()
            .Select(e => e.GetString())
            .ToHashSet(StringComparer.Ordinal);
        Assert.Contains("allagmaReplayId", supplemental);

        var matrixKinds = root.GetProperty("identifiers").EnumerateArray()
            .Select(e => e.GetProperty("kind").GetString())
            .ToHashSet(StringComparer.Ordinal);
        Assert.Contains("allagmaReplayId", matrixKinds);
        Assert.Contains("replayBundleId", matrixKinds);
        Assert.Contains("replayDeltaId", matrixKinds);

        Assert.Equal(
            "docs/system/EVIDENCE_SPINE_GRAPH_TAXONOMY.md",
            root.GetProperty("graphTaxonomyDocument").GetString());
    }

    [Fact]
    public void SYS_TIGHT_002_Taxonomy_links_to_system_contract()
    {
        var taxonomy = File.ReadAllText(Path.Combine(RepoRoot, "docs/operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md"));
        Assert.Contains("SYSTEM_EVIDENCE_SPINE_CONTRACT.md", taxonomy, StringComparison.Ordinal);
        Assert.Contains("system-evidence-spine-resolution.matrix.json", taxonomy, StringComparison.Ordinal);
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
