using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Ontogony.Http;

namespace Ontogony.SystemCompatibility;

public static class SystemCompatibilityGate
{
    private static readonly Regex FullShaRegex = new("^[0-9a-f]{40}$", RegexOptions.Compiled);

    public static SystemCompatibilityGateResult Evaluate(SystemCompatibilityGateOptions options)
    {
        var workspace = SystemCompatibilityWorkspace.Resolve(options);
        var checks = new List<SystemCompatibilityCheck>();

        checks.Add(CheckWorkspacePresence(workspace, options));
        if (checks.Any(c => c.Id == "workspace" && c.Status == SystemCompatibilityCheckStatus.Fail))
        {
            return BuildResult(workspace, checks);
        }

        checks.Add(CheckRegistryAndRuntimeLock(workspace));
        checks.Add(CheckPlatformPackageVersion(workspace));
        checks.Add(CheckPropagationHeadersContract(workspace));
        checks.Add(CheckEnvironmentMatrix(workspace));
        checks.Add(CheckKanonCompatibilityManifest(workspace));
        checks.Add(CheckConexusCompatibilityManifest(workspace));
        checks.Add(CheckAllagmaFeatureConnectionMatrix(workspace));
        checks.Add(CheckFrontendRouteClientMatrix(workspace));
        checks.Add(CrossServiceErrorEnvelopeConformance.CheckMatrixArtifacts(workspace));
        checks.Add(CrossServiceErrorEnvelopeConformance.CheckPlatformSamples(workspace));
        checks.Add(CrossServiceErrorEnvelopeConformance.CheckTaxonomyAdapterMappings(workspace));
        checks.Add(CrossServiceErrorEnvelopeConformance.CheckOpenApiEnvelopeSchema(workspace));
        checks.Add(CrossServiceErrorEnvelopeConformance.CheckSiblingIntegrationDocs(workspace));
        checks.Add(CrossServiceErrorEnvelopeConformance.CheckFrontendTaxonomyModule(workspace));

        return BuildResult(workspace, checks);
    }

    private static SystemCompatibilityGateResult BuildResult(
        SystemCompatibilityWorkspace workspace,
        List<SystemCompatibilityCheck> checks)
    {
        var baseline = TryReadBaseline(workspace) ?? "unknown";
        return new SystemCompatibilityGateResult(
            SystemCompatibilityPaths.Schema,
            baseline,
            DateTimeOffset.UtcNow,
            workspace.DevRoot,
            checks);
    }

    private static string? TryReadBaseline(SystemCompatibilityWorkspace workspace)
    {
        var registryPath = SystemCompatibilityPaths.Registry(workspace);
        if (!File.Exists(registryPath))
        {
            return null;
        }

        using var registry = JsonDocument.Parse(File.ReadAllText(registryPath));
        return registry.RootElement.TryGetProperty("baseline", out var baseline)
            ? baseline.GetString()
            : null;
    }

    private static SystemCompatibilityCheck CheckWorkspacePresence(
        SystemCompatibilityWorkspace workspace,
        SystemCompatibilityGateOptions options)
    {
        var missing = new List<string>();
        if (workspace.AllagmaRoot is null)
        {
            missing.Add("allagma-dotnet");
        }

        if (workspace.KanonRoot is null)
        {
            missing.Add("kanon-dotnet");
        }

        if (workspace.ConexusRoot is null)
        {
            missing.Add("conexus-dotnet");
        }

        if (!Directory.Exists(workspace.PlatformRoot) || !File.Exists(SystemCompatibilityPaths.Registry(workspace)))
        {
            missing.Add("ontogony-platform");
        }

        if (options.RequireFrontendRepos)
        {
            if (workspace.FrontendRoot is null)
            {
                missing.Add("ontogony-frontend");
            }

            if (workspace.UiRoot is null)
            {
                missing.Add("ontogony-ui");
            }
        }

        if (missing.Count == 0)
        {
            return Pass("workspace", "Sibling workspace layout", "All required repositories are present under DevRoot.");
        }

        if (!options.RequireAllBackendRepos && missing.All(m => m is "ontogony-frontend" or "ontogony-ui"))
        {
            return Skipped("workspace", "Sibling workspace layout", $"Optional repos missing: {string.Join(", ", missing)}.");
        }

        return Fail("workspace", "Sibling workspace layout", $"Missing repositories: {string.Join(", ", missing)}.");
    }

    private static SystemCompatibilityCheck CheckRegistryAndRuntimeLock(SystemCompatibilityWorkspace workspace)
    {
        var registryPath = SystemCompatibilityPaths.Registry(workspace);
        var lockPath = SystemCompatibilityPaths.RuntimeLock(workspace);

        if (!File.Exists(registryPath))
        {
            return Fail("registry-lock", "Protocol registry vs runtime lock", $"Missing registry: {registryPath}");
        }

        if (!File.Exists(lockPath))
        {
            return Fail("registry-lock", "Protocol registry vs runtime lock", $"Missing runtime lock: {lockPath}");
        }

        using var registry = JsonDocument.Parse(File.ReadAllText(registryPath));
        using var runtimeLock = JsonDocument.Parse(File.ReadAllText(lockPath));

        var mismatches = new List<string>();

        if (registry.RootElement.TryGetProperty("baseline", out var registryBaseline)
            && runtimeLock.RootElement.TryGetProperty("baseline", out var lockBaseline)
            && !string.Equals(registryBaseline.GetString(), lockBaseline.GetString(), StringComparison.Ordinal))
        {
            mismatches.Add($"baseline registry={registryBaseline.GetString()} lock={lockBaseline.GetString()}");
        }

        if (!registry.RootElement.TryGetProperty("repos", out var registryRepos)
            || !runtimeLock.RootElement.TryGetProperty("lockedCommits", out var lockedCommits))
        {
            return Fail("registry-lock", "Protocol registry vs runtime lock", "Registry repos or lock lockedCommits missing.");
        }

        foreach (var repo in registryRepos.EnumerateArray())
        {
            var name = repo.GetProperty("name").GetString()!;
            if (name is "ontogony-frontend" or "ontogony-ui")
            {
                continue;
            }

            if (!lockedCommits.TryGetProperty(name, out var lockedCommit))
            {
                mismatches.Add($"lock missing repo {name}");
                continue;
            }

            var registryCommit = repo.GetProperty("commit").GetString()!;
            var lockCommit = lockedCommit.GetString()!;
            if (!string.Equals(registryCommit, lockCommit, StringComparison.OrdinalIgnoreCase))
            {
                mismatches.Add($"{name}: registry={registryCommit} lock={lockCommit}");
            }

            if (!FullShaRegex.IsMatch(registryCommit) || !FullShaRegex.IsMatch(lockCommit))
            {
                mismatches.Add($"{name}: commit must be 40-char lowercase hex");
            }
        }

        if (registry.RootElement.TryGetProperty("apiPrefixes", out var registryPrefixes)
            && runtimeLock.RootElement.TryGetProperty("apiPrefixes", out var lockPrefixes))
        {
            ComparePrefix(mismatches, "allagma", registryPrefixes, lockPrefixes);
            ComparePrefix(mismatches, "kanon", registryPrefixes, lockPrefixes);
            ComparePrefix(mismatches, "conexus", registryPrefixes, lockPrefixes);
        }

        return mismatches.Count == 0
            ? Pass("registry-lock", "Protocol registry vs runtime lock", "Registry baseline, commits, and API prefixes match runtime lock.")
            : Fail("registry-lock", "Protocol registry vs runtime lock", string.Join("; ", mismatches));
    }

    private static void ComparePrefix(
        List<string> mismatches,
        string key,
        JsonElement registryPrefixes,
        JsonElement lockPrefixes)
    {
        if (!registryPrefixes.TryGetProperty(key, out var registryValue)
            || !lockPrefixes.TryGetProperty(key, out var lockValue))
        {
            mismatches.Add($"apiPrefixes.{key} missing");
            return;
        }

        if (!string.Equals(registryValue.GetString(), lockValue.GetString(), StringComparison.Ordinal))
        {
            mismatches.Add($"apiPrefixes.{key}: registry={registryValue.GetString()} lock={lockValue.GetString()}");
        }
    }

    private static SystemCompatibilityCheck CheckPlatformPackageVersion(SystemCompatibilityWorkspace workspace)
    {
        var propsPath = SystemCompatibilityPaths.PlatformVersionProps(workspace);
        var lockPath = SystemCompatibilityPaths.RuntimeLock(workspace);

        if (!File.Exists(propsPath))
        {
            return Fail("platform-version", "Platform package version", $"Missing {propsPath}");
        }

        if (!File.Exists(lockPath))
        {
            return Skipped("platform-version", "Platform package version", "Runtime lock not available.");
        }

        var doc = XDocument.Load(propsPath);
        var versionElement = doc.Descendants("Version").FirstOrDefault();
        var platformVersion = versionElement?.Value.Trim();

        using var runtimeLock = JsonDocument.Parse(File.ReadAllText(lockPath));
        var lockOntogony = runtimeLock.RootElement.GetProperty("packageVersions").GetProperty("Ontogony").GetString();

        if (string.IsNullOrWhiteSpace(platformVersion))
        {
            return Fail("platform-version", "Platform package version", "Directory.Build.props Version element not found.");
        }

        return string.Equals(platformVersion, lockOntogony, StringComparison.Ordinal)
            ? Pass("platform-version", "Platform package version", $"Ontogony {platformVersion} matches runtime lock.")
            : Fail("platform-version", "Platform package version", $"Directory.Build.props Version={platformVersion} lock Ontogony={lockOntogony}");
    }

    private static SystemCompatibilityCheck CheckPropagationHeadersContract(SystemCompatibilityWorkspace workspace)
    {
        var contractPath = Path.Combine(workspace.PlatformRoot, "docs/contracts/SYSTEM_COMPATIBILITY_GATE.md");
        var traceContractPath = Path.Combine(workspace.PlatformRoot, "docs/operators/TRACE_CORRELATION_CONTRACT.md");
        var headerMatrixPath = Path.Combine(workspace.PlatformRoot, "docs/contracts/header-compatibility-matrix.md");

        if (!File.Exists(contractPath) || !File.Exists(traceContractPath) || !File.Exists(headerMatrixPath))
        {
            return Fail("propagation-headers", "Propagation header contract", "Missing SYSTEM_COMPATIBILITY_GATE, TRACE_CORRELATION_CONTRACT, or header-compatibility-matrix.");
        }

        var contractText = File.ReadAllText(contractPath);
        var missingInDocs = SystemCompatibilityPropagationHeaders.Required
            .Where(h => !contractText.Contains(h, StringComparison.Ordinal))
            .ToList();

        if (missingInDocs.Count > 0)
        {
            return Fail(
                "propagation-headers",
                "Propagation header contract",
                $"Gate doc missing headers: {string.Join(", ", missingInDocs)}");
        }

        return Pass(
            "propagation-headers",
            "Propagation header contract",
            $"Frozen header set documented ({SystemCompatibilityPropagationHeaders.Required.Count} headers).");
    }

    private static SystemCompatibilityCheck CheckEnvironmentMatrix(SystemCompatibilityWorkspace workspace)
    {
        var matrixPath = SystemCompatibilityPaths.EnvironmentMatrix(workspace);
        if (!File.Exists(matrixPath))
        {
            return Fail("environment-matrix", "Environment and auth matrix", $"Missing {matrixPath}");
        }

        var text = File.ReadAllText(matrixPath);
        var requiredKeys = new[]
        {
            "Allagma:Api:Auth:ServiceToken",
            "Kanon:BaseUrl",
            "Conexus:BaseUrl",
            "Conexus:ProjectApiKey"
        };

        var missing = requiredKeys.Where(k => !text.Contains(k, StringComparison.Ordinal)).ToList();
        return missing.Count == 0
            ? Pass("environment-matrix", "Environment and auth matrix", "SYSTEM_ENVIRONMENT_MATRIX documents core cross-service configuration keys.")
            : Fail("environment-matrix", "Environment and auth matrix", $"Missing keys in matrix: {string.Join(", ", missing)}");
    }

    private static SystemCompatibilityCheck CheckKanonCompatibilityManifest(SystemCompatibilityWorkspace workspace)
    {
        if (workspace.KanonRoot is null)
        {
            return Skipped("kanon-manifest", "Kanon compatibility manifest", "kanon-dotnet not present.");
        }

        var manifestPath = SystemCompatibilityPaths.KanonManifest(workspace);
        var lockPath = SystemCompatibilityPaths.RuntimeLock(workspace);

        if (!File.Exists(manifestPath))
        {
            return Fail("kanon-manifest", "Kanon compatibility manifest", $"Missing {manifestPath}");
        }

        using var manifest = JsonDocument.Parse(File.ReadAllText(manifestPath));
        var issues = new List<string>();

        if (!string.Equals(
                manifest.RootElement.GetProperty("schema").GetString(),
                "ontogony-kanon-compatibility-manifest-v1.1",
                StringComparison.Ordinal))
        {
            issues.Add("unexpected schema");
        }

        if (!File.Exists(lockPath))
        {
            return Fail("kanon-manifest", "Kanon compatibility manifest", $"Missing runtime lock for package alignment: {lockPath}");
        }

        using var runtimeLock = JsonDocument.Parse(File.ReadAllText(lockPath));
        var lockPackages = runtimeLock.RootElement.GetProperty("packageVersions");
        var manifestPackages = manifest.RootElement.GetProperty("packages");

        AssertPackage(issues, lockPackages, "Kanon.Client", manifestPackages, "kanonClient");
        AssertPackage(issues, lockPackages, "Kanon.Contracts", manifestPackages, "kanonContracts");
        AssertPackage(issues, lockPackages, "Ontogony", manifestPackages, "ontogony");

        var lockKanonPrefix = runtimeLock.RootElement.GetProperty("apiPrefixes").GetProperty("kanon").GetString();
        var manifestPrefix = manifest.RootElement.GetProperty("api").GetProperty("ontologyPrefix").GetString();
        if (!string.Equals(lockKanonPrefix, manifestPrefix, StringComparison.Ordinal))
        {
            issues.Add($"ontology prefix lock={lockKanonPrefix} manifest={manifestPrefix}");
        }

        foreach (var artifactKey in new[] { "routeInventory", "openApiBaseline" })
        {
            if (!manifest.RootElement.GetProperty("artifacts").TryGetProperty(artifactKey, out var rel))
            {
                issues.Add($"artifacts.{artifactKey} missing");
                continue;
            }

            var artifactPath = Path.Combine(workspace.KanonRoot, rel.GetString()!.Replace('/', Path.DirectorySeparatorChar));
            if (!File.Exists(artifactPath))
            {
                issues.Add($"missing artifact {artifactKey}: {artifactPath}");
            }
        }

        if (manifest.RootElement.GetProperty("artifacts").TryGetProperty("openApiBaselineSha256", out var expectedHash)
            && manifest.RootElement.GetProperty("artifacts").TryGetProperty("openApiBaseline", out var openApiRel))
        {
            var openApiPath = Path.Combine(workspace.KanonRoot, openApiRel.GetString()!.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(openApiPath))
            {
                var actual = ComputeSha256Hex(File.ReadAllBytes(openApiPath));
                var expected = expectedHash.GetString()!;
                if (!string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase))
                {
                    issues.Add($"openApi baseline sha256 expected={expected} actual={actual}");
                }
            }
        }

        return issues.Count == 0
            ? Pass("kanon-manifest", "Kanon compatibility manifest", "Manifest schema, packages, prefix, and artifacts align with runtime lock.")
            : Fail("kanon-manifest", "Kanon compatibility manifest", string.Join("; ", issues));
    }

    private static void AssertPackage(
        List<string> issues,
        JsonElement lockPackages,
        string lockProperty,
        JsonElement manifestPackages,
        string manifestProperty)
    {
        if (!lockPackages.TryGetProperty(lockProperty, out var lockValue)
            || !manifestPackages.TryGetProperty(manifestProperty, out var manifestValue))
        {
            issues.Add($"package {lockProperty}/{manifestProperty} missing");
            return;
        }

        if (!string.Equals(lockValue.GetString(), manifestValue.GetString(), StringComparison.Ordinal))
        {
            issues.Add($"{lockProperty}: lock={lockValue.GetString()} manifest={manifestValue.GetString()}");
        }
    }

    private static SystemCompatibilityCheck CheckConexusCompatibilityManifest(SystemCompatibilityWorkspace workspace)
    {
        if (workspace.ConexusRoot is null)
        {
            return Skipped("conexus-manifest", "Conexus OpenAPI snapshots", "conexus-dotnet not present.");
        }

        var manifestPath = SystemCompatibilityPaths.ConexusManifest(workspace);
        var lockPath = SystemCompatibilityPaths.RuntimeLock(workspace);

        if (!File.Exists(manifestPath))
        {
            return Fail("conexus-manifest", "Conexus OpenAPI snapshots", $"Missing {manifestPath}");
        }

        using var manifest = JsonDocument.Parse(File.ReadAllText(manifestPath));
        var issues = new List<string>();

        if (!manifest.RootElement.TryGetProperty("conexus", out var conexus))
        {
            return Fail("conexus-manifest", "Conexus OpenAPI snapshots", "Root conexus section missing.");
        }

        if (!File.Exists(lockPath))
        {
            return Fail("conexus-manifest", "Conexus OpenAPI snapshots", $"Missing runtime lock: {lockPath}");
        }

        using var runtimeLock = JsonDocument.Parse(File.ReadAllText(lockPath));
        var lockPackages = runtimeLock.RootElement.GetProperty("packageVersions");

        if (!string.Equals(
                lockPackages.GetProperty("Conexus.Client").GetString(),
                conexus.GetProperty("clientVersion").GetString(),
                StringComparison.Ordinal))
        {
            issues.Add("Conexus.Client version drift vs lock");
        }

        if (!string.Equals(
                lockPackages.GetProperty("Conexus.Contracts").GetString(),
                conexus.GetProperty("contractsVersion").GetString(),
                StringComparison.Ordinal))
        {
            issues.Add("Conexus.Contracts version drift vs lock");
        }

        if (conexus.TryGetProperty("openApiSnapshots", out var snapshots))
        {
            foreach (var snapshot in snapshots.EnumerateObject())
            {
                var snapshotPath = Path.Combine(workspace.ConexusRoot, snapshot.Value.GetString()!.Replace('/', Path.DirectorySeparatorChar));
                if (!File.Exists(snapshotPath))
                {
                    issues.Add($"missing OpenAPI snapshot {snapshot.Name}: {snapshotPath}");
                }
            }
        }
        else
        {
            issues.Add("openApiSnapshots missing");
        }

        return issues.Count == 0
            ? Pass("conexus-manifest", "Conexus OpenAPI snapshots", "Manifest package versions and OpenAPI snapshot files are present.")
            : Fail("conexus-manifest", "Conexus OpenAPI snapshots", string.Join("; ", issues));
    }

    private static SystemCompatibilityCheck CheckAllagmaFeatureConnectionMatrix(SystemCompatibilityWorkspace workspace)
    {
        if (workspace.AllagmaRoot is null)
        {
            return Skipped("allagma-feature-matrix", "Allagma feature connection matrix", "allagma-dotnet not present.");
        }

        var matrixPath = SystemCompatibilityPaths.AllagmaFeatureMatrix(workspace);
        if (!File.Exists(matrixPath))
        {
            return Fail("allagma-feature-matrix", "Allagma feature connection matrix", $"Missing {matrixPath}");
        }

        using var matrix = JsonDocument.Parse(File.ReadAllText(matrixPath));
        var issues = new List<string>();

        if (!string.Equals(
                matrix.RootElement.GetProperty("schema").GetString(),
                "ontogony-allagma-feature-connection-matrix-v1",
                StringComparison.Ordinal))
        {
            issues.Add("unexpected matrix schema");
        }

        var lockPath = SystemCompatibilityPaths.RuntimeLock(workspace);
        if (File.Exists(lockPath))
        {
            using var runtimeLock = JsonDocument.Parse(File.ReadAllText(lockPath));
            var prefixes = runtimeLock.RootElement.GetProperty("apiPrefixes");
            ValidateMatrixPaths(matrix.RootElement.GetProperty("allagmaEndpoints"), prefixes.GetProperty("allagma").GetString()!, issues);
            ValidateMatrixPaths(matrix.RootElement.GetProperty("kanonCalls"), prefixes.GetProperty("kanon").GetString()!, issues);
            ValidateMatrixPaths(matrix.RootElement.GetProperty("conexusCalls"), prefixes.GetProperty("conexus").GetString()!, issues, allowChatCompletions: true);
        }

        return issues.Count == 0
            ? Pass("allagma-feature-matrix", "Allagma feature connection matrix", "Feature matrix schema and API prefix paths align with runtime lock.")
            : Fail("allagma-feature-matrix", "Allagma feature connection matrix", string.Join("; ", issues));
    }

    private static void ValidateMatrixPaths(
        JsonElement entries,
        string expectedPrefix,
        List<string> issues,
        bool allowChatCompletions = false)
    {
        foreach (var entry in entries.EnumerateArray())
        {
            if (!entry.TryGetProperty("path", out var pathElement))
            {
                continue;
            }

            var path = pathElement.GetString()!;
            if (allowChatCompletions && path.StartsWith("/v1/", StringComparison.Ordinal))
            {
                continue;
            }

            if (!path.StartsWith(expectedPrefix, StringComparison.Ordinal))
            {
                issues.Add($"path {path} does not start with {expectedPrefix}");
            }
        }
    }

    private static SystemCompatibilityCheck CheckFrontendRouteClientMatrix(SystemCompatibilityWorkspace workspace)
    {
        if (workspace.AllagmaRoot is null || workspace.FrontendRoot is null)
        {
            return Skipped("frontend-matrix", "Frontend route/client matrix", "allagma-dotnet or ontogony-frontend not present.");
        }

        var matrixPath = SystemCompatibilityPaths.AllagmaFeatureMatrix(workspace);
        var coveragePath = SystemCompatibilityPaths.FrontendCoverageMatrix(workspace);
        var inventoryPath = SystemCompatibilityPaths.FrontendRouteInventory(workspace);

        if (!File.Exists(matrixPath))
        {
            return Fail("frontend-matrix", "Frontend route/client matrix", $"Missing Allagma matrix {matrixPath}");
        }

        if (!File.Exists(coveragePath) && !File.Exists(inventoryPath))
        {
            return Fail(
                "frontend-matrix",
                "Frontend route/client matrix",
                "Missing ontogony-frontend ALLAGMA_FRONTEND_COVERAGE_MATRIX.md and ROUTE_WORKFLOW_INVENTORY.md.");
        }

        using var matrix = JsonDocument.Parse(File.ReadAllText(matrixPath));
        var endpointIds = matrix.RootElement.GetProperty("allagmaEndpoints")
            .EnumerateArray()
            .Select(e => e.GetProperty("id").GetString()!)
            .ToHashSet(StringComparer.Ordinal);

        var issues = new List<string>();
        if (!matrix.RootElement.TryGetProperty("frontendLinks", out var frontendLinks))
        {
            return Fail("frontend-matrix", "Frontend route/client matrix", "allagma-feature-connection.matrix.json frontendLinks missing.");
        }

        foreach (var link in frontendLinks.EnumerateArray())
        {
            var linkId = link.TryGetProperty("id", out var idElement) ? idElement.GetString()! : "(no-id)";

            if (!link.TryGetProperty("allagmaEndpointId", out var endpointIdElement))
            {
                continue;
            }

            var endpointId = endpointIdElement.GetString()!;
            if (!endpointIds.Contains(endpointId))
            {
                issues.Add($"frontend link {linkId} references unknown endpoint {endpointId}");
            }

            if (!link.TryGetProperty("route", out var route) || string.IsNullOrWhiteSpace(route.GetString()))
            {
                issues.Add($"frontend link {linkId} missing route");
            }
        }

        var openapiDir = Path.Combine(workspace.FrontendRoot, "openapi");
        foreach (var spec in new[] { "allagma.v0.json", "kanon.v0.json", "conexus.v0.json" })
        {
            var specPath = Path.Combine(openapiDir, spec);
            if (!File.Exists(specPath))
            {
                issues.Add($"missing frontend OpenAPI snapshot {spec}");
            }
        }

        return issues.Count == 0
            ? Pass("frontend-matrix", "Frontend route/client matrix", "Frontend links, coverage docs, and OpenAPI snapshots are present.")
            : Fail("frontend-matrix", "Frontend route/client matrix", string.Join("; ", issues));
    }

    private static string ComputeSha256Hex(byte[] bytes)
    {
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private static SystemCompatibilityCheck Pass(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Pass, detail);

    private static SystemCompatibilityCheck Fail(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Fail, detail);

    private static SystemCompatibilityCheck Skipped(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Skipped, detail);
}
