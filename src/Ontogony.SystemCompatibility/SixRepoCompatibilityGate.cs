using System.Security.Cryptography;
using System.Text.Json;

namespace Ontogony.SystemCompatibility;

/// <summary>
/// Validates the six-repo compatibility lock (<c>docs/system/ontogony-six-repo-lock.json</c>),
/// confirming that ontogony-platform, conexus-dotnet, kanon-dotnet, allagma-dotnet,
/// ontogony-ui, and ontogony-frontend are pinned together in the same reproducibility unit.
/// </summary>
public static class SixRepoCompatibilityGate
{
    private const string ExpectedSchema = "ontogony-six-repo-lock-v1";

    public static SystemCompatibilityGateResult Evaluate(SystemCompatibilityGateOptions options)
    {
        var workspace = SystemCompatibilityWorkspace.Resolve(options);
        var checks = new List<SystemCompatibilityCheck>();

        var lockPath = SystemCompatibilityPaths.SixRepoLock(workspace);
        if (!File.Exists(lockPath))
        {
            checks.Add(Fail("six-repo-lock", "Six-repo lock present", $"Missing {lockPath}"));
            return BuildResult(workspace, checks, "unknown");
        }

        using var lockDoc = JsonDocument.Parse(File.ReadAllText(lockPath));
        var root = lockDoc.RootElement;

        checks.Add(CheckSchema(root, lockPath));
        var baseline = root.TryGetProperty("baseline", out var b) ? b.GetString() ?? "unknown" : "unknown";

        checks.Add(CheckAllReposPresent(root));
        checks.Add(CheckBackendAlignmentWithRuntimeLock(root, workspace));
        checks.Add(CheckUiPackageVersion(root, workspace));
        checks.Add(CheckFrontendProvenance(root, workspace));
        checks.Add(CheckUiConsumerManifest(root, workspace));
        checks.Add(CheckOpenApiSnapshotHashes(root, workspace));
        checks.Add(CheckFrontendRouteInventoryHash(root, workspace));

        return BuildResult(workspace, checks, baseline);
    }

    private static SystemCompatibilityGateResult BuildResult(
        SystemCompatibilityWorkspace workspace,
        List<SystemCompatibilityCheck> checks,
        string baseline)
    {
        return new SystemCompatibilityGateResult(
            "ontogony-six-repo-lock-v1",
            baseline,
            DateTimeOffset.UtcNow,
            workspace.DevRoot,
            checks);
    }

    private static SystemCompatibilityCheck CheckSchema(JsonElement root, string lockPath)
    {
        if (!root.TryGetProperty("schema", out var schema) ||
            !string.Equals(schema.GetString(), ExpectedSchema, StringComparison.Ordinal))
        {
            var actual = root.TryGetProperty("schema", out var s) ? s.GetString() : "(missing)";
            return Fail("six-repo-schema", "Six-repo lock schema", $"Expected {ExpectedSchema}, got {actual} in {lockPath}");
        }

        return Pass("six-repo-schema", "Six-repo lock schema", $"Schema {ExpectedSchema} confirmed.");
    }

    private static SystemCompatibilityCheck CheckAllReposPresent(JsonElement root)
    {
        if (!root.TryGetProperty("repos", out var repos))
        {
            return Fail("six-repo-presence", "All six repos in lock", "Lock missing 'repos' section.");
        }

        var required = new[] { "ontogony-platform", "conexus-dotnet", "kanon-dotnet", "allagma-dotnet", "ontogony-ui", "ontogony-frontend" };
        var missing = required.Where(r => !repos.TryGetProperty(r, out _)).ToList();

        if (missing.Count > 0)
        {
            return Fail("six-repo-presence", "All six repos in lock", $"Missing repos: {string.Join(", ", missing)}");
        }

        var issues = new List<string>();
        foreach (var name in required)
        {
            if (repos.TryGetProperty(name, out var repo) &&
                repo.TryGetProperty("commit", out var commit))
            {
                var sha = commit.GetString() ?? "";
                if (sha.Length != 40 || !sha.All(c => char.IsAsciiHexDigitLower(c)))
                {
                    issues.Add($"{name}: commit must be 40-char lowercase hex, got '{sha}'");
                }
            }
        }

        return issues.Count == 0
            ? Pass("six-repo-presence", "All six repos in lock", "All six repos present with valid commit SHAs.")
            : Fail("six-repo-presence", "All six repos in lock", string.Join("; ", issues));
    }

    private static SystemCompatibilityCheck CheckBackendAlignmentWithRuntimeLock(
        JsonElement sixRepoRoot,
        SystemCompatibilityWorkspace workspace)
    {
        var runtimeLockPath = SystemCompatibilityPaths.RuntimeLock(workspace);
        if (!File.Exists(runtimeLockPath))
        {
            return Skipped("six-repo-backend-align", "Six-repo vs runtime lock alignment", $"Runtime lock not found at {runtimeLockPath}.");
        }

        if (!sixRepoRoot.TryGetProperty("repos", out var sixRepos))
        {
            return Fail("six-repo-backend-align", "Six-repo vs runtime lock alignment", "Six-repo lock missing repos section.");
        }

        using var runtimeLock = JsonDocument.Parse(File.ReadAllText(runtimeLockPath));
        if (!runtimeLock.RootElement.TryGetProperty("lockedCommits", out var lockedCommits))
        {
            return Fail("six-repo-backend-align", "Six-repo vs runtime lock alignment", "Runtime lock missing lockedCommits.");
        }

        var issues = new List<string>();
        foreach (var backendRepo in new[] { "ontogony-platform", "conexus-dotnet", "kanon-dotnet", "allagma-dotnet" })
        {
            if (!lockedCommits.TryGetProperty(backendRepo, out var runtimeCommit))
            {
                continue;
            }

            if (!sixRepos.TryGetProperty(backendRepo, out var sixEntry) ||
                !sixEntry.TryGetProperty("commit", out var sixCommit))
            {
                issues.Add($"{backendRepo}: missing in six-repo lock");
                continue;
            }

            var rc = runtimeCommit.GetString() ?? "";
            var sc = sixCommit.GetString() ?? "";

            if (!string.Equals(rc, sc, StringComparison.OrdinalIgnoreCase))
            {
                issues.Add($"{backendRepo}: runtime-lock={rc[..8]} six-repo-lock={sc[..8]} (post-lock delta if intentional)");
            }
        }

        var apiIssues = CheckApiPrefixAlignment(sixRepos, runtimeLock.RootElement);
        issues.AddRange(apiIssues);

        return issues.Count == 0
            ? Pass("six-repo-backend-align", "Six-repo vs runtime lock alignment",
                "Backend repo commits and API prefixes align with runtime lock.")
            : Warn("six-repo-backend-align", "Six-repo vs runtime lock alignment",
                $"Post-lock delta or drift: {string.Join("; ", issues)}");
    }

    private static IEnumerable<string> CheckApiPrefixAlignment(JsonElement sixRepos, JsonElement runtimeRoot)
    {
        if (!runtimeRoot.TryGetProperty("apiPrefixes", out var runtimePrefixes))
        {
            yield break;
        }

        if (sixRepos.TryGetProperty("allagma-dotnet", out var allagma) &&
            allagma.TryGetProperty("apiPrefix", out var sixPrefix) &&
            runtimePrefixes.TryGetProperty("allagma", out var runtimePrefix))
        {
            if (!string.Equals(sixPrefix.GetString(), runtimePrefix.GetString(), StringComparison.Ordinal))
            {
                yield return $"allagma apiPrefix: six-repo={sixPrefix.GetString()} runtime={runtimePrefix.GetString()}";
            }
        }
    }

    private static SystemCompatibilityCheck CheckUiPackageVersion(
        JsonElement root,
        SystemCompatibilityWorkspace workspace)
    {
        if (workspace.UiRoot is null)
        {
            return Skipped("six-repo-ui-version", "UI package version", "ontogony-ui not present.");
        }

        if (!root.TryGetProperty("repos", out var repos) ||
            !repos.TryGetProperty("ontogony-ui", out var uiEntry) ||
            !uiEntry.TryGetProperty("packageVersion", out var lockedVersion))
        {
            return Fail("six-repo-ui-version", "UI package version", "Six-repo lock missing ontogony-ui.packageVersion.");
        }

        var packageJsonPath = Path.Combine(workspace.UiRoot, "package.json");
        if (!File.Exists(packageJsonPath))
        {
            return Fail("six-repo-ui-version", "UI package version", $"Missing {packageJsonPath}");
        }

        using var pkgDoc = JsonDocument.Parse(File.ReadAllText(packageJsonPath));
        var actualVersion = pkgDoc.RootElement.TryGetProperty("version", out var v) ? v.GetString() : null;

        return string.Equals(lockedVersion.GetString(), actualVersion, StringComparison.Ordinal)
            ? Pass("six-repo-ui-version", "UI package version", $"@ontogony/ui {actualVersion} matches six-repo lock.")
            : Fail("six-repo-ui-version", "UI package version",
                $"lock={lockedVersion.GetString()} package.json={actualVersion}");
    }

    private static SystemCompatibilityCheck CheckFrontendProvenance(
        JsonElement root,
        SystemCompatibilityWorkspace workspace)
    {
        if (workspace.FrontendRoot is null)
        {
            return Skipped("six-repo-fe-provenance", "Frontend build provenance", "ontogony-frontend not present.");
        }

        var provenancePath = SystemCompatibilityPaths.FrontendBuildProvenance(workspace);
        if (!File.Exists(provenancePath))
        {
            return Warn("six-repo-fe-provenance", "Frontend build provenance",
                $"dist/provenance.json not found — run a frontend build to produce it.");
        }

        if (!root.TryGetProperty("repos", out var repos) ||
            !repos.TryGetProperty("ontogony-frontend", out var feEntry) ||
            !feEntry.TryGetProperty("buildProvenanceSha256", out var lockedHash))
        {
            return Pass("six-repo-fe-provenance", "Frontend build provenance",
                "Provenance file present; lock does not pin its hash (optional).");
        }

        var actualHash = ComputeSha256Hex(File.ReadAllBytes(provenancePath));
        return string.Equals(lockedHash.GetString(), actualHash, StringComparison.OrdinalIgnoreCase)
            ? Pass("six-repo-fe-provenance", "Frontend build provenance", "Build provenance hash matches six-repo lock.")
            : Warn("six-repo-fe-provenance", "Frontend build provenance",
                $"Provenance hash changed since lock. locked={lockedHash.GetString()?[..16]}… actual={actualHash[..16]}… (rebuild if needed)");
    }

    private static SystemCompatibilityCheck CheckUiConsumerManifest(
        JsonElement root,
        SystemCompatibilityWorkspace workspace)
    {
        if (workspace.FrontendRoot is null)
        {
            return Skipped("six-repo-ui-manifest", "UI consumer compatibility manifest", "ontogony-frontend not present.");
        }

        var manifestPath = SystemCompatibilityPaths.UiConsumerCompatibilityManifest(workspace);
        if (!File.Exists(manifestPath))
        {
            return Fail("six-repo-ui-manifest", "UI consumer compatibility manifest",
                $"Missing {manifestPath} — run npm run check:ui-consumer-preflight in ontogony-frontend.");
        }

        if (root.TryGetProperty("contracts", out var contracts) &&
            contracts.TryGetProperty("uiPublicSubpaths", out var lockedHash))
        {
            var actualHash = ComputeSha256Hex(File.ReadAllBytes(manifestPath));
            if (!string.Equals(lockedHash.GetString(), actualHash, StringComparison.OrdinalIgnoreCase))
            {
                return Warn("six-repo-ui-manifest", "UI consumer compatibility manifest",
                    $"Manifest hash changed since lock — re-run consumer gate and update six-repo lock if intentional.");
            }
        }

        return Pass("six-repo-ui-manifest", "UI consumer compatibility manifest",
            "UI_PACK_CONSUMER_COMPATIBILITY_MANIFEST.json present and hash matches lock.");
    }

    private static SystemCompatibilityCheck CheckOpenApiSnapshotHashes(
        JsonElement root,
        SystemCompatibilityWorkspace workspace)
    {
        if (workspace.FrontendRoot is null)
        {
            return Skipped("six-repo-openapi-hashes", "OpenAPI snapshot hashes", "ontogony-frontend not present.");
        }

        if (!root.TryGetProperty("contracts", out var contracts) ||
            !contracts.TryGetProperty("openapiSnapshots", out var snapshots))
        {
            return Skipped("six-repo-openapi-hashes", "OpenAPI snapshot hashes", "No openapiSnapshots in six-repo lock.");
        }

        var openapiDir = Path.Combine(workspace.FrontendRoot, "openapi");
        var issues = new List<string>();

        foreach (var snap in snapshots.EnumerateObject())
        {
            var filePath = Path.Combine(openapiDir, $"{snap.Name}.v0.json");
            if (!File.Exists(filePath))
            {
                issues.Add($"{snap.Name}: missing {filePath}");
                continue;
            }

            var actualHash = ComputeSha256Hex(File.ReadAllBytes(filePath));
            if (!string.Equals(snap.Value.GetString(), actualHash, StringComparison.OrdinalIgnoreCase))
            {
                issues.Add($"{snap.Name}: hash changed (update six-repo lock after reviewing OpenAPI drift)");
            }
        }

        return issues.Count == 0
            ? Pass("six-repo-openapi-hashes", "OpenAPI snapshot hashes", "All pinned OpenAPI snapshots match.")
            : Warn("six-repo-openapi-hashes", "OpenAPI snapshot hashes", string.Join("; ", issues));
    }

    private static SystemCompatibilityCheck CheckFrontendRouteInventoryHash(
        JsonElement root,
        SystemCompatibilityWorkspace workspace)
    {
        if (workspace.FrontendRoot is null)
        {
            return Skipped("six-repo-route-inventory", "Frontend route inventory hash", "ontogony-frontend not present.");
        }

        if (!root.TryGetProperty("contracts", out var contracts) ||
            !contracts.TryGetProperty("frontendRouteInventory", out var lockedHash))
        {
            return Skipped("six-repo-route-inventory", "Frontend route inventory hash", "No frontendRouteInventory hash in lock.");
        }

        var inventoryPath = SystemCompatibilityPaths.FrontendRouteInventory(workspace);
        if (!File.Exists(inventoryPath))
        {
            return Fail("six-repo-route-inventory", "Frontend route inventory hash", $"Missing {inventoryPath}");
        }

        var actualHash = ComputeSha256Hex(File.ReadAllBytes(inventoryPath));
        return string.Equals(lockedHash.GetString(), actualHash, StringComparison.OrdinalIgnoreCase)
            ? Pass("six-repo-route-inventory", "Frontend route inventory hash", "Route inventory hash matches six-repo lock.")
            : Warn("six-repo-route-inventory", "Frontend route inventory hash",
                "Route inventory changed since lock — review route changes and update lock.");
    }

    private static string ComputeSha256Hex(byte[] bytes) =>
        Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();

    private static SystemCompatibilityCheck Pass(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Pass, detail);

    private static SystemCompatibilityCheck Warn(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Warn, detail);

    private static SystemCompatibilityCheck Fail(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Fail, detail);

    private static SystemCompatibilityCheck Skipped(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Skipped, detail);
}
