namespace Ontogony.SystemCompatibility;

internal static class SystemCompatibilityPaths
{
    public const string Schema = "ontogony-system-compatibility-summary-v1";

    public static string Registry(SystemCompatibilityWorkspace workspace) =>
        Path.Combine(workspace.PlatformRoot, "docs/system/system-protocol-registry.json");

    public static string RuntimeLock(SystemCompatibilityWorkspace workspace) =>
        workspace.AllagmaRoot is null
            ? ""
            : Path.Combine(workspace.AllagmaRoot, "docs/system/ontogony-runtime.lock.json");

    public static string KanonManifest(SystemCompatibilityWorkspace workspace) =>
        workspace.KanonRoot is null
            ? ""
            : Path.Combine(workspace.KanonRoot, "docs/generated/KANON_COMPATIBILITY_MANIFEST.json");

    public static string ConexusManifest(SystemCompatibilityWorkspace workspace) =>
        workspace.ConexusRoot is null
            ? ""
            : Path.Combine(workspace.ConexusRoot, "docs/generated/CONEXUS_COMPATIBILITY_MANIFEST.json");

    public static string AllagmaFeatureMatrix(SystemCompatibilityWorkspace workspace) =>
        workspace.AllagmaRoot is null
            ? ""
            : Path.Combine(workspace.AllagmaRoot, "docs/system/allagma-feature-connection.matrix.json");

    public static string FrontendCoverageMatrix(SystemCompatibilityWorkspace workspace) =>
        workspace.FrontendRoot is null
            ? ""
            : Path.Combine(workspace.FrontendRoot, "docs/system/ALLAGMA_FRONTEND_COVERAGE_MATRIX.md");

    public static string FrontendRouteInventory(SystemCompatibilityWorkspace workspace) =>
        workspace.FrontendRoot is null
            ? ""
            : Path.Combine(workspace.FrontendRoot, "docs/generated/ROUTE_WORKFLOW_INVENTORY.md");

    public static string EnvironmentMatrix(SystemCompatibilityWorkspace workspace) =>
        workspace.AllagmaRoot is null
            ? ""
            : Path.Combine(workspace.AllagmaRoot, "docs/system/SYSTEM_ENVIRONMENT_MATRIX.md");

    public static string PlatformVersionProps(SystemCompatibilityWorkspace workspace) =>
        Path.Combine(workspace.PlatformRoot, "Directory.Build.props");
}
