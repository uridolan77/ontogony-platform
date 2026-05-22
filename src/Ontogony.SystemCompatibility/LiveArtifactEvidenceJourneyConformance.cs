using System.Text.Json;

namespace Ontogony.SystemCompatibility;

/// <summary>
/// SYSTEM-9B-005 — frontend live-artifact evidence journey gate script and catalog presence.
/// Playwright against Docker-local stack is orchestrated from platform docker scripts.
/// </summary>
internal static class LiveArtifactEvidenceJourneyConformance
{
    private const string GateScriptName = "live-artifact-evidence-journey:check";
    private const string DockerLiveScriptName = "test:e2e:docker-live:evidence-journey";
    private const string CatalogRelativePath = "scripts/live-artifact-evidence-journey-catalog.json";
    private const string DockerLiveSpecRelativePath = "e2e/evidence-journey-docker-live.spec.ts";

    public static SystemCompatibilityCheck CheckFrontendLiveArtifactEvidenceJourneyGate(
        SystemCompatibilityWorkspace workspace)
    {
        if (workspace.FrontendRoot is null)
        {
            return Skipped(
                "live-artifact-evidence-journey-gate",
                "Frontend live-artifact evidence journey gate",
                "ontogony-frontend not present.");
        }

        var packagePath = Path.Combine(workspace.FrontendRoot, "package.json");
        if (!File.Exists(packagePath))
        {
            return Fail(
                "live-artifact-evidence-journey-gate",
                "Frontend live-artifact evidence journey gate",
                $"Missing {packagePath}");
        }

        using var package = JsonDocument.Parse(File.ReadAllText(packagePath));
        if (!package.RootElement.TryGetProperty("scripts", out var scripts))
        {
            return Fail(
                "live-artifact-evidence-journey-gate",
                "Frontend live-artifact evidence journey gate",
                "package.json scripts section missing.");
        }

        if (!scripts.TryGetProperty(GateScriptName, out var gateScript) ||
            string.IsNullOrWhiteSpace(gateScript.GetString()))
        {
            return Fail(
                "live-artifact-evidence-journey-gate",
                "Frontend live-artifact evidence journey gate",
                $"package.json must define scripts.{GateScriptName} (SYSTEM-9B-005).");
        }

        if (!scripts.TryGetProperty(DockerLiveScriptName, out var dockerScript) ||
            string.IsNullOrWhiteSpace(dockerScript.GetString()))
        {
            return Fail(
                "live-artifact-evidence-journey-gate",
                "Frontend live-artifact evidence journey gate",
                $"package.json must define scripts.{DockerLiveScriptName}.");
        }

        var catalogPath = Path.Combine(workspace.FrontendRoot, CatalogRelativePath);
        if (!File.Exists(catalogPath))
        {
            return Fail(
                "live-artifact-evidence-journey-gate",
                "Frontend live-artifact evidence journey gate",
                $"Missing {CatalogRelativePath}");
        }

        var specPath = Path.Combine(workspace.FrontendRoot, DockerLiveSpecRelativePath);
        if (!File.Exists(specPath))
        {
            return Fail(
                "live-artifact-evidence-journey-gate",
                "Frontend live-artifact evidence journey gate",
                $"Missing {DockerLiveSpecRelativePath}");
        }

        var specSource = File.ReadAllText(specPath);
        if (specSource.Contains("mockOntogonyServices", StringComparison.Ordinal))
        {
            return Fail(
                "live-artifact-evidence-journey-gate",
                "Frontend live-artifact evidence journey gate",
                "Docker-live spec must not call mockOntogonyServices.");
        }

        var playwrightConfigPath = Path.Combine(workspace.FrontendRoot, "playwright.docker-local.config.ts");
        if (!File.Exists(playwrightConfigPath))
        {
            return Fail(
                "live-artifact-evidence-journey-gate",
                "Frontend live-artifact evidence journey gate",
                "Missing playwright.docker-local.config.ts");
        }

        var configSource = File.ReadAllText(playwrightConfigPath);
        var specFileName = Path.GetFileName(DockerLiveSpecRelativePath);
        if (!configSource.Contains(specFileName, StringComparison.Ordinal))
        {
            return Fail(
                "live-artifact-evidence-journey-gate",
                "Frontend live-artifact evidence journey gate",
                $"playwright.docker-local.config.ts must include {specFileName}");
        }

        return Pass(
            "live-artifact-evidence-journey-gate",
            "Frontend live-artifact evidence journey gate",
            "live-artifact-evidence-journey:check, docker-live spec, and catalog are present.");
    }

    private static SystemCompatibilityCheck Pass(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Pass, detail);

    private static SystemCompatibilityCheck Fail(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Fail, detail);

    private static SystemCompatibilityCheck Skipped(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Skipped, detail);
}
