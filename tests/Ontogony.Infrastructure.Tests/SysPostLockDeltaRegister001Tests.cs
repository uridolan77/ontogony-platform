using System.Text.Json;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

/// <summary>
/// SYS-POSTLOCK-DELTA-REGISTER-001 — cross-repo post-lock delta register structure.
/// </summary>
public sealed class SysPostLockDeltaRegister001Tests
{
    private static readonly string RepoRoot = GetProjectRoot();

    [Fact]
    public void SYS_POSTLOCK_DELTA_001_Register_and_validator_exist()
    {
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/system/post-lock-delta-register.json")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "scripts/validate-post-lock-delta-register.ps1")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/evidence/SYS_POSTLOCK_DELTA_REGISTER_001_EVIDENCE.md")));
    }

    [Fact]
    public void SYS_POSTLOCK_DELTA_001_Register_lists_all_companion_repos()
    {
        using var doc = JsonDocument.Parse(
            File.ReadAllText(Path.Combine(RepoRoot, "docs/system/post-lock-delta-register.json")));

        var root = doc.RootElement;
        Assert.Equal("ontogony-post-lock-delta-register-v1", root.GetProperty("schema").GetString());
        Assert.Equal("SYSTEM-ALPHA-006", root.GetProperty("baseline").GetString());

        var repos = root.GetProperty("repos").EnumerateArray().Select(e => e.GetProperty("repo").GetString()).ToHashSet();
        Assert.Contains("allagma-dotnet", repos);
        Assert.Contains("ontogony-platform", repos);
        Assert.Contains("kanon-dotnet", repos);
        Assert.Contains("conexus-dotnet", repos);
        Assert.Contains("ontogony-frontend", repos);
        Assert.Contains("ontogony-ui", repos);
    }

    [Fact]
    public void SYS_POSTLOCK_DELTA_001_Each_repo_has_classified_groups()
    {
        using var doc = JsonDocument.Parse(
            File.ReadAllText(Path.Combine(RepoRoot, "docs/system/post-lock-delta-register.json")));

        foreach (var entry in doc.RootElement.GetProperty("repos").EnumerateArray())
        {
            var repo = entry.GetProperty("repo").GetString();
            Assert.False(string.IsNullOrWhiteSpace(entry.GetProperty("lockDisposition").GetString()));
            var groups = entry.GetProperty("groups").EnumerateArray().ToList();
            Assert.True(groups.Count > 0, $"{repo} must have at least one group");
            foreach (var group in groups)
            {
                Assert.False(string.IsNullOrWhiteSpace(group.GetProperty("id").GetString()));
                Assert.True(group.TryGetProperty("classification", out _), $"{repo} group missing classification");
            }
        }
    }

    [Fact]
    public void SYS_POSTLOCK_DELTA_001_Docs_gates_ci_references_validator()
    {
        var ci = File.ReadAllText(Path.Combine(RepoRoot, ".github/workflows/ci.yml"));
        Assert.Contains("validate-post-lock-delta-register.ps1", ci, StringComparison.Ordinal);
        Assert.Contains("SYS-POSTLOCK-DELTA-REGISTER-001", ci, StringComparison.Ordinal);
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
