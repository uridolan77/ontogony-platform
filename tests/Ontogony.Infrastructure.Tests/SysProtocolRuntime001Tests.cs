using System.Text.Json;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

/// <summary>
/// SYS-PROTOCOL-RUNTIME-001 — protocol identity metadata for evidence-producing protocol surfaces.
/// </summary>
public sealed class SysProtocolRuntime001Tests
{
    private static readonly string RepoRoot = GetProjectRoot();

    [Fact]
    public void SYS_PROTOCOL_RUNTIME_001_Artifacts_exist()
    {
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/system/system-protocol-registry.json")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/system/schemas/system-protocol-registry.schema.json")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "scripts/validate-system-protocol-registry.ps1")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/evidence/SYS_PROTOCOL_RUNTIME_001_EVIDENCE.md")));
    }

    [Fact]
    public void SYS_PROTOCOL_RUNTIME_001_Protocol_metadata_is_present_for_all_protocols()
    {
        using var doc = JsonDocument.Parse(
            File.ReadAllText(Path.Combine(RepoRoot, "docs/system/system-protocol-registry.json")));

        var authorityModes = new HashSet<string>(StringComparer.Ordinal)
        {
            "authoritative",
            "draft_only",
            "simulation_only",
            "blocked",
            "local_only",
            "observational",
            "gateway",
            "unknown"
        };

        var sideEffectLevels = new HashSet<string>(StringComparer.Ordinal)
        {
            "none",
            "read_only",
            "evidence_record",
            "semantic_decision",
            "run_state_transition",
            "model_call",
            "local_sandbox_effect",
            "real_external_blocked",
            "unknown"
        };

        foreach (var protocol in doc.RootElement.GetProperty("protocols").EnumerateArray())
        {
            var id = protocol.GetProperty("id").GetString();

            var protocolId = protocol.GetProperty("protocolId").GetString();
            Assert.False(string.IsNullOrWhiteSpace(protocolId), $"{id} must include protocolId.");

            var authorityMode = protocol.GetProperty("authorityMode").GetString();
            Assert.False(string.IsNullOrWhiteSpace(authorityMode), $"{id} must include authorityMode.");
            Assert.Contains(authorityMode!, authorityModes);

            var sideEffectLevel = protocol.GetProperty("sideEffectLevel").GetString();
            Assert.False(string.IsNullOrWhiteSpace(sideEffectLevel), $"{id} must include sideEffectLevel.");
            Assert.Contains(sideEffectLevel!, sideEffectLevels);
        }
    }

    [Fact]
    public void SYS_PROTOCOL_RUNTIME_001_Docs_index_mentions_runtime_metadata_fields()
    {
        var readme = File.ReadAllText(Path.Combine(RepoRoot, "docs/system/README.md"));
        Assert.Contains("protocolId", readme, StringComparison.Ordinal);
        Assert.Contains("authorityMode", readme, StringComparison.Ordinal);
        Assert.Contains("sideEffectLevel", readme, StringComparison.Ordinal);
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
