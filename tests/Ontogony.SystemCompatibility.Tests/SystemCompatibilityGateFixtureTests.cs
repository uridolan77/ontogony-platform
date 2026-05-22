using Xunit;

namespace Ontogony.SystemCompatibility.Tests;

public sealed class SystemCompatibilityGateFixtureTests
{
    [Fact]
    public void Minimal_fixture_workspace_passes_gate()
    {
        var fixtureRoot = Path.Combine(AppContext.BaseDirectory, "Fixtures", "minimal-workspace");
        var options = new SystemCompatibilityGateOptions
        {
            DevRoot = fixtureRoot,
            PlatformRoot = Path.Combine(fixtureRoot, "ontogony-platform"),
            RequireFrontendRepos = false
        };

        var result = SystemCompatibilityGate.Evaluate(options);

        Assert.True(result.Passed, string.Join(Environment.NewLine, result.Checks.Where(c => c.Status == SystemCompatibilityCheckStatus.Fail).Select(c => $"{c.Id}: {c.Detail}")));
        Assert.Equal(0, result.FailCount);
    }

    [Fact]
    public void Summary_writer_emits_json_and_markdown()
    {
        var result = new SystemCompatibilityGateResult(
            "ontogony-system-compatibility-summary-v1",
            "SYSTEM-TEST-001",
            DateTimeOffset.Parse("2026-05-22T12:00:00Z"),
            @"C:\dev",
            [new SystemCompatibilityCheck("sample", "Sample", SystemCompatibilityCheckStatus.Pass, "ok")]);

        var dir = Path.Combine(Path.GetTempPath(), "ontogony-system-compat-" + Guid.NewGuid().ToString("N"));
        try
        {
            SystemCompatibilitySummaryWriter.WriteArtifacts(result, dir);
            Assert.True(File.Exists(Path.Combine(dir, "system-compatibility-summary.json")));
            Assert.True(File.Exists(Path.Combine(dir, "system-compatibility-summary.md")));
            var md = File.ReadAllText(Path.Combine(dir, "system-compatibility-summary.md"));
            Assert.Contains("PASS", md);
        }
        finally
        {
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, recursive: true);
            }
        }
    }
}
