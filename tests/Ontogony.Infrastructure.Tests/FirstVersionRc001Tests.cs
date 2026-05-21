using System.Text.Json;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

/// <summary>FIRST-VERSION-RC-001 — operator v1 product lock and validator.</summary>
public sealed class FirstVersionRc001Tests
{
    private static readonly string RepoRoot = GetProjectRoot();

    [Fact]
    public void FIRST_VERSION_RC_001_Lock_and_validator_exist()
    {
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/system/ontogony-operator-v1.lock.json")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "scripts/validate-operator-v1-lock.ps1")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "scripts/run-operator-v1-validation.ps1")));
        Assert.True(File.Exists(Path.Combine(RepoRoot, "docs/evidence/FIRST_VERSION_RC_001_EVIDENCE.md")));
    }

    [Fact]
    public void FIRST_VERSION_RC_001_Lock_lists_six_repos_and_eight_flows()
    {
        using var doc = JsonDocument.Parse(
            File.ReadAllText(Path.Combine(RepoRoot, "docs/system/ontogony-operator-v1.lock.json")));

        var root = doc.RootElement;
        Assert.Equal("ontogony-operator-v1-lock-v1", root.GetProperty("schema").GetString());
        Assert.Equal("OPERATOR-V1-001", root.GetProperty("baseline").GetString());
        Assert.Equal("SYSTEM-ALPHA-006", root.GetProperty("runtimeBaseline").GetString());

        var repos = root.GetProperty("lockedCommits").EnumerateObject().Select(p => p.Name).ToHashSet();
        Assert.Contains("allagma-dotnet", repos);
        Assert.Contains("ontogony-frontend", repos);
        Assert.Contains("ontogony-ui", repos);
        Assert.True(root.GetProperty("requiredOperatorFlows").GetArrayLength() >= 8);
    }

    [Fact]
    public void FIRST_VERSION_RC_001_Ci_references_validator()
    {
        var ci = File.ReadAllText(Path.Combine(RepoRoot, ".github/workflows/ci.yml"));
        Assert.Contains("validate-operator-v1-lock.ps1", ci, StringComparison.Ordinal);
        Assert.Contains("FIRST-VERSION-RC-001", ci, StringComparison.Ordinal);
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

        throw new InvalidOperationException("Could not locate Ontogony.Platform.sln from test base directory.");
    }
}
