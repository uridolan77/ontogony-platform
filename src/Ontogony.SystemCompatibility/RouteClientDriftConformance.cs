using System.Text.Json;
using System.Text.RegularExpressions;

namespace Ontogony.SystemCompatibility;

/// <summary>
/// SYSTEM-9A-005 — cross-repo route/OpenAPI drift checks (Allagma + frontend gate script presence).
/// Kanon parity is enforced in ontogony-frontend via <c>route-client-drift:check</c> (includes kanon:route-parity).
/// </summary>
internal static partial class RouteClientDriftConformance
{
    private static readonly HashSet<string> HttpMethods = new(StringComparer.OrdinalIgnoreCase)
    {
        "get", "post", "put", "patch", "delete",
    };

    public static SystemCompatibilityCheck CheckFrontendRouteClientDriftScript(SystemCompatibilityWorkspace workspace)
    {
        if (workspace.FrontendRoot is null)
        {
            return Skipped(
                "route-client-drift-script",
                "Frontend route-client drift gate",
                "ontogony-frontend not present.");
        }

        var packagePath = Path.Combine(workspace.FrontendRoot, "package.json");
        if (!File.Exists(packagePath))
        {
            return Fail(
                "route-client-drift-script",
                "Frontend route-client drift gate",
                $"Missing {packagePath}");
        }

        using var package = JsonDocument.Parse(File.ReadAllText(packagePath));
        if (!package.RootElement.TryGetProperty("scripts", out var scripts) ||
            !scripts.TryGetProperty("route-client-drift:check", out var gateScript) ||
            string.IsNullOrWhiteSpace(gateScript.GetString()))
        {
            return Fail(
                "route-client-drift-script",
                "Frontend route-client drift gate",
                "package.json must define scripts.route-client-drift:check (SYSTEM-9A-005).");
        }

        var catalogPath = Path.Combine(workspace.FrontendRoot, "src/app/route-workflow-catalog.json");
        if (!File.Exists(catalogPath))
        {
            return Fail(
                "route-client-drift-script",
                "Frontend route-client drift gate",
                $"Missing {catalogPath}");
        }

        return Pass(
            "route-client-drift-script",
            "Frontend route-client drift gate",
            "route-client-drift:check script and route-workflow-catalog.json are present.");
    }

    public static SystemCompatibilityCheck CheckAllagmaOpenApiCrossRepoParity(SystemCompatibilityWorkspace workspace)
    {
        if (workspace.AllagmaRoot is null || workspace.FrontendRoot is null)
        {
            return Skipped(
                "route-client-drift-allagma-openapi",
                "Allagma OpenAPI cross-repo parity",
                "allagma-dotnet or ontogony-frontend not present.");
        }

        var backendPath = Path.Combine(workspace.AllagmaRoot, "docs/api/allagma-openapi-v1.snapshot.json");
        var frontendPath = Path.Combine(workspace.FrontendRoot, "openapi/allagma.v0.json");

        if (!File.Exists(backendPath) || !File.Exists(frontendPath))
        {
            return Fail(
                "route-client-drift-allagma-openapi",
                "Allagma OpenAPI cross-repo parity",
                "Missing Allagma backend or frontend OpenAPI snapshot.");
        }

        var backend = LoadOpenApiSignatures(backendPath, route => route.StartsWith("/allagma/v0", StringComparison.Ordinal) || route == "/health");
        var frontend = LoadOpenApiSignatures(frontendPath, route => route.StartsWith("/allagma/v0", StringComparison.Ordinal) || route == "/health");

        var issues = DiffSignatures("allagma-openapi-v1.snapshot.json", backend, "openapi/allagma.v0.json", frontend);
        return issues.Count == 0
            ? Pass(
                "route-client-drift-allagma-openapi",
                "Allagma OpenAPI cross-repo parity",
                $"{backend.Count} signatures aligned across backend snapshot and frontend openapi/allagma.v0.json.")
            : Fail(
                "route-client-drift-allagma-openapi",
                "Allagma OpenAPI cross-repo parity",
                string.Join("; ", issues));
    }

    private static HashSet<string> LoadOpenApiSignatures(string filePath, Func<string, bool> routeFilter)
    {
        using var document = JsonDocument.Parse(File.ReadAllText(filePath));
        var signatures = new HashSet<string>(StringComparer.Ordinal);

        if (!document.RootElement.TryGetProperty("paths", out var paths))
        {
            return signatures;
        }

        foreach (var pathProperty in paths.EnumerateObject())
        {
            if (!routeFilter(pathProperty.Name))
            {
                continue;
            }

            var normalizedPath = ParamSegmentRegex().Replace(pathProperty.Name, "{}");
            foreach (var operation in pathProperty.Value.EnumerateObject())
            {
                if (HttpMethods.Contains(operation.Name))
                {
                    signatures.Add($"{operation.Name.ToUpperInvariant()} {normalizedPath}");
                }
            }
        }

        return signatures;
    }

    private static List<string> DiffSignatures(
        string leftLabel,
        HashSet<string> left,
        string rightLabel,
        HashSet<string> right)
    {
        var issues = new List<string>();
        foreach (var signature in left.OrderBy(static value => value, StringComparer.Ordinal))
        {
            if (!right.Contains(signature))
            {
                issues.Add($"{leftLabel} has {signature} but {rightLabel} does not");
            }
        }

        foreach (var signature in right.OrderBy(static value => value, StringComparer.Ordinal))
        {
            if (!left.Contains(signature))
            {
                issues.Add($"{rightLabel} has {signature} but {leftLabel} does not");
            }
        }

        return issues;
    }

    [GeneratedRegex(@"\{[^}]+\}")]
    private static partial Regex ParamSegmentRegex();

    private static SystemCompatibilityCheck Pass(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Pass, detail);

    private static SystemCompatibilityCheck Fail(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Fail, detail);

    private static SystemCompatibilityCheck Skipped(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Skipped, detail);
}
