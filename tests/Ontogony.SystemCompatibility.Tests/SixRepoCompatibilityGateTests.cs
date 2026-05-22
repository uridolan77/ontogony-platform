using Xunit;

namespace Ontogony.SystemCompatibility.Tests;

[Trait("Category", "SystemCompatGate")]
public sealed class SixRepoCompatibilityGateTests
{
    private static string FixtureRoot =>
        Path.Combine(AppContext.BaseDirectory, "Fixtures", "minimal-workspace");

    [Fact]
    public void Minimal_fixture_workspace_passes_without_frontend_repos()
    {
        var options = new SystemCompatibilityGateOptions
        {
            DevRoot = FixtureRoot,
            PlatformRoot = Path.Combine(FixtureRoot, "ontogony-platform"),
            RequireFrontendRepos = false
        };

        var result = SixRepoCompatibilityGate.Evaluate(options);

        var failures = result.Checks
            .Where(c => c.Status == SystemCompatibilityCheckStatus.Fail)
            .Select(c => $"{c.Id}: {c.Detail}");

        Assert.True(result.Passed, string.Join(Environment.NewLine, failures));
        Assert.Equal(0, result.FailCount);
    }

    [Fact]
    public void Missing_six_repo_lock_fails()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "ontogony-six-repo-missing-" + Guid.NewGuid().ToString("N"));
        try
        {
            CopyFixture(FixtureRoot, tempDir);
            var lockPath = Path.Combine(tempDir, "ontogony-platform", "docs", "system", "ontogony-six-repo-lock.json");
            if (File.Exists(lockPath))
            {
                File.Delete(lockPath);
            }

            var options = new SystemCompatibilityGateOptions
            {
                DevRoot = tempDir,
                PlatformRoot = Path.Combine(tempDir, "ontogony-platform"),
                RequireFrontendRepos = false
            };

            var result = SixRepoCompatibilityGate.Evaluate(options);

            Assert.False(result.Passed);
            Assert.Contains(result.Checks, c => c.Id == "six-repo-lock" && c.Status == SystemCompatibilityCheckStatus.Fail);
        }
        finally
        {
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, recursive: true);
            }
        }
    }

    [Fact]
    public void Wrong_schema_fails()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "ontogony-six-repo-schema-" + Guid.NewGuid().ToString("N"));
        try
        {
            CopyFixture(FixtureRoot, tempDir);
            var lockPath = Path.Combine(tempDir, "ontogony-platform", "docs", "system", "ontogony-six-repo-lock.json");
            var json = File.ReadAllText(lockPath).Replace("ontogony-six-repo-lock-v1", "ontogony-six-repo-lock-v999");
            File.WriteAllText(lockPath, json);

            var options = new SystemCompatibilityGateOptions
            {
                DevRoot = tempDir,
                PlatformRoot = Path.Combine(tempDir, "ontogony-platform"),
                RequireFrontendRepos = false
            };

            var result = SixRepoCompatibilityGate.Evaluate(options);

            Assert.Contains(result.Checks, c => c.Id == "six-repo-schema" && c.Status == SystemCompatibilityCheckStatus.Fail);
        }
        finally
        {
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, recursive: true);
            }
        }
    }

    [Fact]
    public void Missing_repo_entry_fails()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "ontogony-six-repo-missing-repo-" + Guid.NewGuid().ToString("N"));
        try
        {
            CopyFixture(FixtureRoot, tempDir);
            var lockPath = Path.Combine(tempDir, "ontogony-platform", "docs", "system", "ontogony-six-repo-lock.json");
            var json = File.ReadAllText(lockPath);
            var modifiedJson = System.Text.Json.JsonDocument.Parse(json).RootElement.ToString();

            // Remove ontogony-ui from the lock
            var doc = System.Text.Json.JsonDocument.Parse(json);
            using var ms = new System.IO.MemoryStream();
            using var writer = new System.Text.Json.Utf8JsonWriter(ms, new System.Text.Json.JsonWriterOptions { Indented = true });
            writer.WriteStartObject();
            foreach (var prop in doc.RootElement.EnumerateObject())
            {
                if (prop.Name == "repos")
                {
                    writer.WritePropertyName("repos");
                    writer.WriteStartObject();
                    foreach (var repo in prop.Value.EnumerateObject())
                    {
                        if (repo.Name != "ontogony-ui")
                        {
                            repo.WriteTo(writer);
                        }
                    }

                    writer.WriteEndObject();
                }
                else
                {
                    prop.WriteTo(writer);
                }
            }

            writer.WriteEndObject();
            writer.Flush();
            File.WriteAllBytes(lockPath, ms.ToArray());

            var options = new SystemCompatibilityGateOptions
            {
                DevRoot = tempDir,
                PlatformRoot = Path.Combine(tempDir, "ontogony-platform"),
                RequireFrontendRepos = false
            };

            var result = SixRepoCompatibilityGate.Evaluate(options);

            Assert.Contains(result.Checks, c =>
                c.Id == "six-repo-presence" && c.Status == SystemCompatibilityCheckStatus.Fail);
        }
        finally
        {
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, recursive: true);
            }
        }
    }

    [Fact]
    public void Gate_result_exposes_warn_count()
    {
        var result = new SystemCompatibilityGateResult(
            "ontogony-six-repo-lock-v1",
            "SYSTEM-TEST-001",
            DateTimeOffset.UtcNow,
            @"C:\dev",
            [
                new SystemCompatibilityCheck("a", "A", SystemCompatibilityCheckStatus.Pass, "ok"),
                new SystemCompatibilityCheck("b", "B", SystemCompatibilityCheckStatus.Warn, "drift"),
                new SystemCompatibilityCheck("c", "C", SystemCompatibilityCheckStatus.Skipped, "n/a")
            ]);

        Assert.True(result.Passed);
        Assert.Equal(1, result.WarnCount);
        Assert.Equal(0, result.FailCount);
    }

    private static void CopyFixture(string source, string destination)
    {
        foreach (var dir in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(dir.Replace(source, destination));
        }

        foreach (var file in Directory.GetFiles(source, "*", SearchOption.AllDirectories))
        {
            File.Copy(file, file.Replace(source, destination), overwrite: true);
        }
    }
}
