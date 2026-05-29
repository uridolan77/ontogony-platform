using System.Text.Json;
using Ontogony.Http;

namespace Ontogony.SystemCompatibility;

/// <summary>
/// PLATFORM-9-003 header propagation contract checks for the system compatibility gate.
/// </summary>
public static class HeaderPropagationConformance
{
    public static SystemCompatibilityCheck CheckMatrixArtifacts(SystemCompatibilityWorkspace workspace)
    {
        var matrixPath = Path.Combine(workspace.PlatformRoot, "docs/system/propagation-header.matrix.json");
        var contractPath = Path.Combine(workspace.PlatformRoot, "docs/contracts/HEADER_PROPAGATION_CONTRACT.md");

        var missing = new List<string>();
        foreach (var path in new[] { matrixPath, contractPath })
        {
            if (!File.Exists(path))
            {
                missing.Add(path);
            }
        }

        if (missing.Count > 0)
        {
            return Fail("propagation-header-matrix", "Propagation header matrix", $"Missing: {string.Join("; ", missing)}");
        }

        using var matrix = JsonDocument.Parse(File.ReadAllText(matrixPath));
        var issues = new List<string>();

        if (!string.Equals(
                matrix.RootElement.GetProperty("schema").GetString(),
                "ontogony-propagation-header-v1",
                StringComparison.Ordinal))
        {
            issues.Add("unexpected matrix schema");
        }

        var frozen = matrix.RootElement.GetProperty("frozenRequiredHeaders")
            .EnumerateArray()
            .Select(e => e.GetString()!)
            .ToHashSet(StringComparer.Ordinal);

        foreach (var header in OntogonyPropagationHeaderContract.FrozenRequired)
        {
            if (!frozen.Contains(header))
            {
                issues.Add($"matrix missing frozen header {header}");
            }
        }

        var contractText = File.ReadAllText(contractPath);
        foreach (var header in OntogonyPropagationHeaderContract.FrozenRequired)
        {
            if (!contractText.Contains(header, StringComparison.Ordinal))
            {
                issues.Add($"contract doc missing {header}");
            }
        }

        return issues.Count == 0
            ? Pass("propagation-header-matrix", "Propagation header matrix", "Matrix and contract document align with frozen headers.")
            : Fail("propagation-header-matrix", "Propagation header matrix", string.Join("; ", issues));
    }

    public static SystemCompatibilityCheck CheckFrozenConstants(SystemCompatibilityWorkspace workspace)
    {
        var issues = new List<string>();

        if (SystemCompatibilityPropagationHeaders.Required.Count
            != OntogonyPropagationHeaderContract.FrozenRequired.Count)
        {
            issues.Add("SystemCompatibilityPropagationHeaders count drift");
        }

        foreach (var header in OntogonyPropagationHeaderContract.FrozenRequired)
        {
            if (!SystemCompatibilityPropagationHeaders.Required.Contains(header, StringComparer.Ordinal))
            {
                issues.Add($"gate header list missing {header}");
            }
        }

        return issues.Count == 0
            ? Pass("propagation-header-constants", "Propagation header constants", "Frozen contract matches gate header list.")
            : Fail("propagation-header-constants", "Propagation header constants", string.Join("; ", issues));
    }

    public static SystemCompatibilityCheck CheckOperatorContracts(SystemCompatibilityWorkspace workspace)
    {
        var contractPath = Path.Combine(workspace.PlatformRoot, "docs/contracts/SYSTEM_COMPATIBILITY_GATE.md");
        var traceContractPath = Path.Combine(workspace.PlatformRoot, "docs/operators/TRACE_CORRELATION_CONTRACT.md");
        var headerMatrixPath = Path.Combine(workspace.PlatformRoot, "docs/contracts/header-compatibility-matrix.md");

        if (!File.Exists(contractPath) || !File.Exists(traceContractPath) || !File.Exists(headerMatrixPath))
        {
            return Fail(
                "propagation-headers",
                "Propagation header contract",
                "Missing SYSTEM_COMPATIBILITY_GATE, TRACE_CORRELATION_CONTRACT, or header-compatibility-matrix.");
        }

        var contractText = File.ReadAllText(contractPath);
        var missingInDocs = OntogonyPropagationHeaderContract.FrozenRequired
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
            $"Frozen header set documented ({OntogonyPropagationHeaderContract.FrozenRequired.Count} headers).");
    }

    public static SystemCompatibilityCheck CheckSiblingIntegrationDocs(SystemCompatibilityWorkspace workspace)
    {
        var matrixPath = Path.Combine(workspace.PlatformRoot, "docs/system/propagation-header.matrix.json");
        if (!File.Exists(matrixPath))
        {
            return Fail("propagation-header-sibling-docs", "Sibling propagation integration docs", $"Missing matrix: {matrixPath}");
        }

        using var matrix = JsonDocument.Parse(File.ReadAllText(matrixPath));
        var contracts = matrix.RootElement.GetProperty("integrationContracts");
        var issues = new List<string>();

        foreach (var entry in contracts.EnumerateObject())
        {
            var rel = entry.Value.GetString()!;
            var parts = rel.Split('/', 2);
            if (parts.Length != 2)
            {
                issues.Add($"invalid integration contract path {rel}");
                continue;
            }

            var repoRoot = parts[0] switch
            {
                "allagma-dotnet" => workspace.AllagmaRoot,
                "kanon-dotnet" => workspace.KanonRoot,
                "conexus-dotnet" => workspace.ConexusRoot,
                "metabole-dotnet" => workspace.MetaboleRoot,
                "aisthesis-dotnet" => workspace.AisthesisRoot,
                _ => null
            };

            if (repoRoot is null)
            {
                issues.Add($"sibling repo {parts[0]} not present for {entry.Name}");
                continue;
            }

            var fullPath = Path.Combine(workspace.DevRoot, rel.Replace('/', Path.DirectorySeparatorChar));
            if (!File.Exists(fullPath))
            {
                issues.Add($"missing {entry.Name} contract: {fullPath}");
                continue;
            }

            var text = File.ReadAllText(fullPath);
            var mentionsFrozen = OntogonyPropagationHeaderContract.FrozenRequired
                .Any(h => text.Contains(h, StringComparison.Ordinal));

            if (!mentionsFrozen)
            {
                issues.Add($"{entry.Name} contract does not reference frozen propagation headers");
            }
        }

        return issues.Count == 0
            ? Pass("propagation-header-sibling-docs", "Sibling propagation integration docs", "Product repo propagation contracts are present.")
            : Fail("propagation-header-sibling-docs", "Sibling propagation integration docs", string.Join("; ", issues));
    }

    public static SystemCompatibilityCheck CheckTestingHelpers(SystemCompatibilityWorkspace workspace)
    {
        var assertionsPath = Path.Combine(workspace.PlatformRoot, "src/Ontogony.Testing/HeaderPropagationConformanceAssertions.cs");
        var scenarioPath = Path.Combine(workspace.PlatformRoot, "src/Ontogony.Testing/PropagationHeaderScenario.cs");
        var contractPath = Path.Combine(workspace.PlatformRoot, "src/Ontogony.Http/OntogonyPropagationHeaderContract.cs");

        var missing = new List<string>();
        foreach (var path in new[] { assertionsPath, scenarioPath, contractPath })
        {
            if (!File.Exists(path))
            {
                missing.Add(path);
            }
        }

        if (missing.Count > 0)
        {
            return Skipped(
                "propagation-header-testing",
                "Propagation header test helpers",
                "Platform source tree not present in workspace (docs-only fixture).");
        }

        var text = File.ReadAllText(assertionsPath);
        var issues = new List<string>();

        if (!text.Contains("PropagationHeaderScenario", StringComparison.Ordinal))
        {
            issues.Add("HeaderPropagationConformanceAssertions missing PropagationHeaderScenario");
        }

        if (!text.Contains("AssertIntegrationHandlerPropagatesScenarioAsync", StringComparison.Ordinal))
        {
            issues.Add("HeaderPropagationConformanceAssertions missing outbound handler assertion");
        }

        return issues.Count == 0
            ? Pass("propagation-header-testing", "Propagation header test helpers", "Ontogony.Testing propagation conformance helpers are present.")
            : Fail("propagation-header-testing", "Propagation header test helpers", string.Join("; ", issues));
    }

    private static SystemCompatibilityCheck Pass(string id, string name, string detail) =>
        new(id, name, SystemCompatibilityCheckStatus.Pass, detail);

    private static SystemCompatibilityCheck Fail(string id, string name, string detail) =>
        new(id, name, SystemCompatibilityCheckStatus.Fail, detail);

    private static SystemCompatibilityCheck Skipped(string id, string name, string detail) =>
        new(id, name, SystemCompatibilityCheckStatus.Skipped, detail);
}
