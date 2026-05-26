using System.Text.Json;
using System.Text.RegularExpressions;

namespace Ontogony.SystemCompatibility;

internal static class RuntimeLockConformance
{
    private static readonly Regex FullShaRegex = new("^[0-9a-f]{40}$", RegexOptions.Compiled);

    public static SystemCompatibilityCheck CheckRuntimeLockShape(SystemCompatibilityWorkspace workspace)
    {
        var lockPath = SystemCompatibilityPaths.RuntimeLock(workspace);
        if (string.IsNullOrEmpty(lockPath) || !File.Exists(lockPath))
        {
            return Fail(
                "runtime-lock-shape",
                "Runtime lock schema",
                $"Missing runtime lock at {lockPath}.");
        }

        using var doc = JsonDocument.Parse(File.ReadAllText(lockPath));
        var root = doc.RootElement;
        var issues = new List<string>();

        RequireString(root, "system", issues);
        RequireString(root, "baseline", issues);
        RequireObject(root, "lockedCommits", issues);
        RequireObject(root, "apiPrefixes", issues);
        RequireArray(root, "requiredScenarios", issues);

        if (root.TryGetProperty("lockedCommits", out var commits))
        {
            foreach (var repo in commits.EnumerateObject())
            {
                var sha = repo.Value.GetString() ?? "";
                if (!FullShaRegex.IsMatch(sha))
                {
                    issues.Add($"lockedCommits.{repo.Name}: invalid sha '{sha}'");
                }
            }
        }

        if (root.TryGetProperty("postLockDeltaRegister", out var register)
            && register.ValueKind == JsonValueKind.String)
        {
            var rel = register.GetString() ?? "";
            if (workspace.AllagmaRoot is not null)
            {
                var full = Path.Combine(workspace.AllagmaRoot, rel.Replace('/', Path.DirectorySeparatorChar));
                if (!File.Exists(full))
                {
                    issues.Add($"postLockDeltaRegister path missing: {rel}");
                }
            }
        }

        return issues.Count == 0
            ? Pass("runtime-lock-shape", "Runtime lock schema", "Required runtime lock fields and SHAs are valid.")
            : Fail("runtime-lock-shape", "Runtime lock schema", string.Join("; ", issues));
    }

    public static SystemCompatibilityCheck CheckPostLockDeltaRegister(SystemCompatibilityWorkspace workspace)
    {
        var deltaPath = SystemCompatibilityPaths.PostLockDeltas(workspace);
        if (string.IsNullOrEmpty(deltaPath))
        {
            return Skipped("post-lock-deltas", "Post-lock delta register", "Allagma root not available.");
        }

        if (!File.Exists(deltaPath))
        {
            return Fail("post-lock-deltas", "Post-lock delta register", $"Missing {deltaPath}");
        }

        using var doc = JsonDocument.Parse(File.ReadAllText(deltaPath));
        var root = doc.RootElement;
        var issues = new List<string>();

        if (!root.TryGetProperty("schema", out var schema)
            || !string.Equals(schema.GetString(), "ontogony-post-lock-deltas-v1", StringComparison.Ordinal))
        {
            issues.Add("schema must be ontogony-post-lock-deltas-v1");
        }

        if (!root.TryGetProperty("repos", out var repos) || repos.ValueKind != JsonValueKind.Array)
        {
            issues.Add("repos array missing");
        }

        return issues.Count == 0
            ? Pass("post-lock-deltas", "Post-lock delta register", "Post-lock delta schema and repos[] are present.")
            : Fail("post-lock-deltas", "Post-lock delta register", string.Join("; ", issues));
    }

    public static SystemCompatibilityCheck CheckRuntimeLockRepos(SystemCompatibilityWorkspace workspace)
    {
        var lockPath = SystemCompatibilityPaths.RuntimeLock(workspace);
        if (string.IsNullOrEmpty(lockPath) || !File.Exists(lockPath))
        {
            return Fail("runtime-lock-repos", "Runtime lock companion repos", $"Missing runtime lock at {lockPath}.");
        }

        using var doc = JsonDocument.Parse(File.ReadAllText(lockPath));
        var root = doc.RootElement;
        var issues = new List<string>();

        if (!root.TryGetProperty("lockedCommits", out var commits) || commits.ValueKind != JsonValueKind.Object)
        {
            issues.Add("lockedCommits object missing");
        }
        else
        {
            foreach (var required in OntogonySystemRepos.RequiredCompanionRepos)
            {
                if (!commits.TryGetProperty(required, out var shaProp)
                    || shaProp.ValueKind != JsonValueKind.String
                    || !FullShaRegex.IsMatch(shaProp.GetString() ?? ""))
                {
                    issues.Add($"lockedCommits.{required}: missing or invalid sha");
                }
            }
        }

        return issues.Count == 0
            ? Pass("runtime-lock-repos", "Runtime lock companion repos", "All required companion repos are locked with valid SHAs.")
            : Fail("runtime-lock-repos", "Runtime lock companion repos", string.Join("; ", issues));
    }

    public static SystemCompatibilityCheck CheckPostLockDeltaRepos(SystemCompatibilityWorkspace workspace)
    {
        var deltaPath = SystemCompatibilityPaths.PostLockDeltas(workspace);
        if (string.IsNullOrEmpty(deltaPath))
        {
            return Skipped("post-lock-delta-repos", "Post-lock delta repo coverage", "Allagma root not available.");
        }

        if (!File.Exists(deltaPath))
        {
            return Fail("post-lock-delta-repos", "Post-lock delta repo coverage", $"Missing {deltaPath}");
        }

        using var doc = JsonDocument.Parse(File.ReadAllText(deltaPath));
        var root = doc.RootElement;
        var issues = new List<string>();

        if (!root.TryGetProperty("repos", out var repos) || repos.ValueKind != JsonValueKind.Array)
        {
            issues.Add("repos array missing");
            return Fail("post-lock-delta-repos", "Post-lock delta repo coverage", string.Join("; ", issues));
        }

        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var entry in repos.EnumerateArray())
        {
            if (!entry.TryGetProperty("repo", out var repoProp)
                || string.IsNullOrWhiteSpace(repoProp.GetString()))
            {
                issues.Add("repo entry missing repo name");
                continue;
            }

            var repo = repoProp.GetString()!;
            if (!seen.Add(repo))
            {
                issues.Add($"duplicate repo entry: {repo}");
            }

            if (entry.TryGetProperty("lockDisposition", out var dispositionProp))
            {
                var disposition = dispositionProp.GetString() ?? "";
                if (!OntogonySystemRepos.AllowedPostLockDispositions.Contains(disposition))
                {
                    issues.Add($"{repo} lockDisposition invalid: {disposition}");
                }
            }

            if (entry.TryGetProperty("groups", out var groups) && groups.ValueKind == JsonValueKind.Array)
            {
                foreach (var group in groups.EnumerateArray())
                {
                    if (!group.TryGetProperty("classification", out var classificationProp))
                    {
                        continue;
                    }

                    if (classificationProp.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var item in classificationProp.EnumerateArray())
                        {
                            var value = item.GetString() ?? "";
                            if (!OntogonySystemRepos.AllowedPostLockClassifications.Contains(value))
                            {
                                issues.Add($"{repo} group classification invalid: {value}");
                            }
                        }
                    }
                    else
                    {
                        var value = classificationProp.GetString() ?? "";
                        if (!OntogonySystemRepos.AllowedPostLockClassifications.Contains(value))
                        {
                            issues.Add($"{repo} group classification invalid: {value}");
                        }
                    }
                }
            }
        }

        foreach (var required in OntogonySystemRepos.RequiredCompanionRepos)
        {
            if (!seen.Contains(required))
            {
                issues.Add($"missing required repo entry: {required}");
            }
        }

        return issues.Count == 0
            ? Pass("post-lock-delta-repos", "Post-lock delta repo coverage", "All companion repos are listed with allowed classifications.")
            : Fail("post-lock-delta-repos", "Post-lock delta repo coverage", string.Join("; ", issues));
    }

    public static SystemCompatibilityCheck CheckRuntimeLockPackageVersions(SystemCompatibilityWorkspace workspace)
    {
        var lockPath = SystemCompatibilityPaths.RuntimeLock(workspace);
        if (string.IsNullOrEmpty(lockPath) || !File.Exists(lockPath))
        {
            return Skipped("runtime-lock-package-versions", "Runtime lock package versions", "Runtime lock not available.");
        }

        using var doc = JsonDocument.Parse(File.ReadAllText(lockPath));
        var root = doc.RootElement;
        var issues = new List<string>();

        if (!root.TryGetProperty("packageVersions", out var versions) || versions.ValueKind != JsonValueKind.Object)
        {
            issues.Add("packageVersions object missing");
        }
        else
        {
            var requiredKeys = new[] { "Ontogony", "Kanon", "Conexus", "Allagma" };
            foreach (var key in requiredKeys)
            {
                if (!versions.TryGetProperty(key, out var value)
                    || value.ValueKind != JsonValueKind.String
                    || string.IsNullOrWhiteSpace(value.GetString()))
                {
                    issues.Add($"packageVersions.{key} missing or empty");
                }
            }

            foreach (var prop in versions.EnumerateObject())
            {
                if (string.IsNullOrWhiteSpace(prop.Value.GetString()))
                {
                    issues.Add($"packageVersions.{prop.Name} is empty");
                }
            }
        }

        return issues.Count == 0
            ? Pass("runtime-lock-package-versions", "Runtime lock package versions", "packageVersions includes Ontogony, Kanon, Conexus, and Allagma pins.")
            : Fail("runtime-lock-package-versions", "Runtime lock package versions", string.Join("; ", issues));
    }

    private static void RequireString(JsonElement root, string name, List<string> issues)
    {
        if (!root.TryGetProperty(name, out var prop) || prop.ValueKind != JsonValueKind.String
            || string.IsNullOrWhiteSpace(prop.GetString()))
        {
            issues.Add($"missing or empty '{name}'");
        }
    }

    private static void RequireObject(JsonElement root, string name, List<string> issues)
    {
        if (!root.TryGetProperty(name, out var prop) || prop.ValueKind != JsonValueKind.Object)
        {
            issues.Add($"missing object '{name}'");
        }
    }

    private static void RequireArray(JsonElement root, string name, List<string> issues)
    {
        if (!root.TryGetProperty(name, out var prop) || prop.ValueKind != JsonValueKind.Array)
        {
            issues.Add($"missing array '{name}'");
        }
    }

    private static SystemCompatibilityCheck Pass(string id, string name, string detail) =>
        new(id, name, SystemCompatibilityCheckStatus.Pass, detail);

    private static SystemCompatibilityCheck Fail(string id, string name, string detail) =>
        new(id, name, SystemCompatibilityCheckStatus.Fail, detail);

    private static SystemCompatibilityCheck Skipped(string id, string name, string detail) =>
        new(id, name, SystemCompatibilityCheckStatus.Skipped, detail);
}
