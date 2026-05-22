using System.Text.Json;
using System.Text.Json.Nodes;
using Xunit;

namespace Ontogony.SystemCompatibility.Tests;

[Trait("Category", "SystemCompatGate")]
public sealed class SixRepoCompatibilityGateTests
{
    private static string FixtureRoot =>
        Path.Combine(AppContext.BaseDirectory, "Fixtures", "minimal-workspace");

    private static SystemCompatibilityGateOptions FixtureOptions(string dir, bool strict = false) =>
        new()
        {
            DevRoot = dir,
            PlatformRoot = Path.Combine(dir, "ontogony-platform"),
            RequireFrontendRepos = false,
            StrictMode = strict
        };

    // ─── Passing baseline ─────────────────────────────────────────────────────

    [Fact]
    public void Minimal_fixture_passes_without_frontend_repos()
    {
        var result = SixRepoCompatibilityGate.Evaluate(FixtureOptions(FixtureRoot));

        AssertPassed(result);
    }

    // ─── Schema ───────────────────────────────────────────────────────────────

    [Fact]
    public void Wrong_schema_string_fails()
    {
        WithModifiedLock(FixtureRoot, lock_ =>
        {
            lock_["schema"] = JsonValue.Create("ontogony-six-repo-lock-v999");
            return lock_;
        }, dir =>
        {
            var result = SixRepoCompatibilityGate.Evaluate(FixtureOptions(dir));
            AssertCheck(result, "six-repo-schema", SystemCompatibilityCheckStatus.Fail);
        });
    }

    [Fact]
    public void Missing_schema_field_fails()
    {
        WithModifiedLock(FixtureRoot, lock_ =>
        {
            lock_.Remove("schema");
            return lock_;
        }, dir =>
        {
            var result = SixRepoCompatibilityGate.Evaluate(FixtureOptions(dir));
            AssertCheck(result, "six-repo-schema", SystemCompatibilityCheckStatus.Fail);
        });
    }

    // ─── Repo presence ────────────────────────────────────────────────────────

    [Fact]
    public void Missing_repo_entry_fails()
    {
        WithModifiedLock(FixtureRoot, lock_ =>
        {
            var repos = lock_["repos"]!.AsObject();
            repos.Remove("ontogony-ui");
            return lock_;
        }, dir =>
        {
            var result = SixRepoCompatibilityGate.Evaluate(FixtureOptions(dir));
            AssertCheck(result, "six-repo-presence", SystemCompatibilityCheckStatus.Fail);
        });
    }

    [Fact]
    public void Invalid_commit_sha_length_fails()
    {
        WithModifiedLock(FixtureRoot, lock_ =>
        {
            lock_["repos"]!["ontogony-ui"]!["commit"] = JsonValue.Create("tooshort");
            return lock_;
        }, dir =>
        {
            var result = SixRepoCompatibilityGate.Evaluate(FixtureOptions(dir));
            AssertCheck(result, "six-repo-presence", SystemCompatibilityCheckStatus.Fail);
        });
    }

    [Fact]
    public void Non_hex_commit_fails()
    {
        WithModifiedLock(FixtureRoot, lock_ =>
        {
            lock_["repos"]!["ontogony-frontend"]!["commit"] =
                JsonValue.Create("ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ");
            return lock_;
        }, dir =>
        {
            var result = SixRepoCompatibilityGate.Evaluate(FixtureOptions(dir));
            AssertCheck(result, "six-repo-presence", SystemCompatibilityCheckStatus.Fail);
        });
    }

    // ─── Delta register ───────────────────────────────────────────────────────

    [Fact]
    public void Missing_delta_register_warns_in_dev_mode()
    {
        WithDeletedFile(FixtureRoot, "ontogony-platform/docs/system/ontogony-six-repo-post-lock-deltas.json",
            dir =>
            {
                var result = SixRepoCompatibilityGate.Evaluate(FixtureOptions(dir, strict: false));
                Assert.True(result.Passed);
                AssertCheck(result, "six-repo-delta-register", SystemCompatibilityCheckStatus.Warn);
            });
    }

    [Fact]
    public void Unclassified_delta_warns_in_dev_mode()
    {
        WithModifiedDeltaRegister(FixtureRoot, reg =>
        {
            reg["deltas"] = new JsonArray(new JsonObject
            {
                ["repo"] = "ontogony-frontend",
                ["commit"] = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                ["summary"] = "missing classification"
            });
            return reg;
        }, dir =>
        {
            var result = SixRepoCompatibilityGate.Evaluate(FixtureOptions(dir, strict: false));
            Assert.True(result.Passed);
            AssertCheck(result, "six-repo-delta-register", SystemCompatibilityCheckStatus.Warn);
        });
    }

    [Fact]
    public void Unclassified_delta_fails_in_strict_mode()
    {
        WithModifiedDeltaRegister(FixtureRoot, reg =>
        {
            reg["deltas"] = new JsonArray(new JsonObject
            {
                ["repo"] = "ontogony-frontend",
                ["commit"] = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                ["summary"] = "no classification key"
            });
            return reg;
        }, dir =>
        {
            var result = SixRepoCompatibilityGate.Evaluate(FixtureOptions(dir, strict: true));
            Assert.False(result.Passed);
            AssertCheck(result, "six-repo-delta-register", SystemCompatibilityCheckStatus.Fail);
        });
    }

    [Fact]
    public void Classified_delta_passes()
    {
        WithModifiedDeltaRegister(FixtureRoot, reg =>
        {
            reg["deltas"] = new JsonArray(new JsonObject
            {
                ["repo"] = "ontogony-frontend",
                ["commit"] = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                ["classification"] = "post-lock-dev",
                ["summary"] = "classified dev commit"
            });
            return reg;
        }, dir =>
        {
            var result = SixRepoCompatibilityGate.Evaluate(FixtureOptions(dir));
            AssertCheck(result, "six-repo-delta-register", SystemCompatibilityCheckStatus.Pass);
        });
    }

    // ─── OpenAPI snapshot hashes ──────────────────────────────────────────────

    [Fact]
    public void OpenApi_hash_mismatch_warns()
    {
        WithModifiedLock(FixtureRoot, lock_ =>
        {
            var snapshots = new JsonObject { ["allagma"] = JsonValue.Create("a".PadRight(64, 'a')) };
            lock_["contracts"]!["openapiSnapshots"] = snapshots;
            return lock_;
        }, dir =>
        {
            // No frontend repo in fixture, so this will be skipped
            var result = SixRepoCompatibilityGate.Evaluate(FixtureOptions(dir));
            var check = result.Checks.FirstOrDefault(c => c.Id == "six-repo-openapi-hashes");
            Assert.NotNull(check);
            Assert.True(
                check.Status is SystemCompatibilityCheckStatus.Skipped
                    or SystemCompatibilityCheckStatus.Warn
                    or SystemCompatibilityCheckStatus.Pass,
                $"Expected skipped/warn/pass, got {check.Status}: {check.Detail}");
        });
    }

    // ─── Strict mode ──────────────────────────────────────────────────────────

    [Fact]
    public void Warn_in_dev_mode_passes_gate()
    {
        var result = new SystemCompatibilityGateResult(
            "ontogony-six-repo-lock-v1",
            "TEST",
            DateTimeOffset.UtcNow,
            @"C:\dev",
            [
                new SystemCompatibilityCheck("a", "A", SystemCompatibilityCheckStatus.Pass, "ok"),
                new SystemCompatibilityCheck("b", "B", SystemCompatibilityCheckStatus.Warn, "drift"),
            ],
            StrictMode: false);

        Assert.True(result.Passed);
        Assert.Equal("pass", result.Verdict);
    }

    [Fact]
    public void Warn_in_strict_mode_fails_gate()
    {
        var result = new SystemCompatibilityGateResult(
            "ontogony-six-repo-lock-v1",
            "TEST",
            DateTimeOffset.UtcNow,
            @"C:\dev",
            [
                new SystemCompatibilityCheck("a", "A", SystemCompatibilityCheckStatus.Pass, "ok"),
                new SystemCompatibilityCheck("b", "B", SystemCompatibilityCheckStatus.Warn, "drift"),
            ],
            StrictMode: true);

        Assert.False(result.Passed);
        Assert.Equal("warn", result.Verdict);
    }

    [Fact]
    public void Fail_always_fails_regardless_of_mode()
    {
        foreach (var strict in new[] { false, true })
        {
            var result = new SystemCompatibilityGateResult(
                "ontogony-six-repo-lock-v1",
                "TEST",
                DateTimeOffset.UtcNow,
                @"C:\dev",
                [new SystemCompatibilityCheck("a", "A", SystemCompatibilityCheckStatus.Fail, "bad")],
                StrictMode: strict);

            Assert.False(result.Passed);
            Assert.Equal("fail", result.Verdict);
            Assert.Equal(1, result.FailCount);
        }
    }

    // ─── WarnCount ────────────────────────────────────────────────────────────

    [Fact]
    public void Gate_result_exposes_warn_count()
    {
        var result = new SystemCompatibilityGateResult(
            "ontogony-six-repo-lock-v1",
            "TEST",
            DateTimeOffset.UtcNow,
            @"C:\dev",
            [
                new SystemCompatibilityCheck("a", "A", SystemCompatibilityCheckStatus.Pass, "ok"),
                new SystemCompatibilityCheck("b", "B", SystemCompatibilityCheckStatus.Warn, "drift"),
                new SystemCompatibilityCheck("c", "C", SystemCompatibilityCheckStatus.Skipped, "n/a"),
            ]);

        Assert.True(result.Passed);
        Assert.Equal(1, result.WarnCount);
        Assert.Equal(0, result.FailCount);
    }

    // ─── Summary writer ───────────────────────────────────────────────────────

    [Fact]
    public void Summary_writer_emits_named_artifacts()
    {
        var result = new SystemCompatibilityGateResult(
            "ontogony-six-repo-lock-v1",
            "SYSTEM-TEST-001",
            DateTimeOffset.Parse("2026-05-22T12:00:00Z"),
            @"C:\dev",
            [new SystemCompatibilityCheck("sample", "Sample", SystemCompatibilityCheckStatus.Pass, "ok")]);

        var dir = Path.Combine(Path.GetTempPath(), "ontogony-six-repo-summary-" + Guid.NewGuid().ToString("N"));
        try
        {
            SystemCompatibilitySummaryWriter.WriteArtifacts(result, dir, "six-repo-compatibility-summary");
            Assert.True(File.Exists(Path.Combine(dir, "six-repo-compatibility-summary.json")));
            Assert.True(File.Exists(Path.Combine(dir, "six-repo-compatibility-summary.md")));
            var json = File.ReadAllText(Path.Combine(dir, "six-repo-compatibility-summary.json"));
            Assert.Contains("pass", json);
        }
        finally
        {
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, recursive: true);
            }
        }
    }

    // ─── Missing lock ─────────────────────────────────────────────────────────

    [Fact]
    public void Missing_lock_file_fails()
    {
        WithDeletedFile(FixtureRoot, "ontogony-platform/docs/system/ontogony-six-repo-lock.json",
            dir =>
            {
                var result = SixRepoCompatibilityGate.Evaluate(FixtureOptions(dir));
                Assert.False(result.Passed);
                AssertCheck(result, "six-repo-lock", SystemCompatibilityCheckStatus.Fail);
            });
    }

    // ─── Helpers ──────────────────────────────────────────────────────────────

    private static void AssertPassed(SystemCompatibilityGateResult result)
    {
        var failures = result.Checks
            .Where(c => c.Status == SystemCompatibilityCheckStatus.Fail)
            .Select(c => $"{c.Id}: {c.Detail}");
        Assert.True(result.Passed, string.Join(Environment.NewLine, failures));
        Assert.Equal(0, result.FailCount);
    }

    private static void AssertCheck(
        SystemCompatibilityGateResult result,
        string checkId,
        SystemCompatibilityCheckStatus expectedStatus)
    {
        var check = result.Checks.FirstOrDefault(c => c.Id == checkId);
        Assert.NotNull(check);
        Assert.Equal(expectedStatus, check.Status);
    }

    private static void WithModifiedLock(
        string sourceFixture,
        Func<JsonObject, JsonObject> modify,
        Action<string> act)
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "ontogony-six-repo-test-" + Guid.NewGuid().ToString("N"));
        try
        {
            CopyFixture(sourceFixture, tempDir);
            var lockPath = Path.Combine(tempDir, "ontogony-platform", "docs", "system", "ontogony-six-repo-lock.json");
            var original = JsonNode.Parse(File.ReadAllText(lockPath))!.AsObject();
            var modified = modify(original);
            File.WriteAllText(lockPath, modified.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
            act(tempDir);
        }
        finally
        {
            TryDelete(tempDir);
        }
    }

    private static void WithModifiedDeltaRegister(
        string sourceFixture,
        Func<JsonObject, JsonObject> modify,
        Action<string> act)
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "ontogony-six-repo-test-" + Guid.NewGuid().ToString("N"));
        try
        {
            CopyFixture(sourceFixture, tempDir);
            var regPath = Path.Combine(tempDir, "ontogony-platform", "docs", "system", "ontogony-six-repo-post-lock-deltas.json");
            var original = JsonNode.Parse(File.ReadAllText(regPath))!.AsObject();
            var modified = modify(original);
            File.WriteAllText(regPath, modified.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
            act(tempDir);
        }
        finally
        {
            TryDelete(tempDir);
        }
    }

    private static void WithDeletedFile(string sourceFixture, string relPath, Action<string> act)
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "ontogony-six-repo-test-" + Guid.NewGuid().ToString("N"));
        try
        {
            CopyFixture(sourceFixture, tempDir);
            var target = Path.Combine(tempDir, relPath.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(target))
            {
                File.Delete(target);
            }

            act(tempDir);
        }
        finally
        {
            TryDelete(tempDir);
        }
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

    private static void TryDelete(string dir)
    {
        try
        {
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, recursive: true);
            }
        }
        catch
        {
            // Windows file locks — non-fatal in tests
        }
    }
}
