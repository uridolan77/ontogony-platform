namespace Ontogony.SystemCompatibility;

public sealed class SystemCompatibilityWorkspace
{
    public required string DevRoot { get; init; }

    public required string PlatformRoot { get; init; }

    public string? AllagmaRoot { get; init; }

    public string? KanonRoot { get; init; }

    public string? ConexusRoot { get; init; }

    public string? MetaboleRoot { get; init; }

    public string? AisthesisRoot { get; init; }

    public string? FrontendRoot { get; init; }

    public string? UiRoot { get; init; }

    public static SystemCompatibilityWorkspace Resolve(SystemCompatibilityGateOptions options)
    {
        var devRoot = Path.GetFullPath(options.DevRoot);
        var platformRoot = string.IsNullOrWhiteSpace(options.PlatformRoot)
            ? Path.Combine(devRoot, "ontogony-platform")
            : Path.GetFullPath(options.PlatformRoot);

        return new SystemCompatibilityWorkspace
        {
            DevRoot = devRoot,
            PlatformRoot = platformRoot,
            AllagmaRoot = ResolveRepo(devRoot, "allagma-dotnet", "Allagma.sln"),
            KanonRoot = ResolveRepo(devRoot, "kanon-dotnet", "Kanon.sln"),
            ConexusRoot = ResolveRepo(devRoot, "conexus-dotnet", "Conexus.sln"),
            MetaboleRoot = ResolveRepo(devRoot, "metabole-dotnet", "Metabole.sln"),
            AisthesisRoot = ResolveRepo(devRoot, "aisthesis-dotnet", "Aisthesis.sln"),
            FrontendRoot = ResolveRepo(devRoot, "ontogony-frontend", "package.json"),
            UiRoot = ResolveRepo(devRoot, "ontogony-ui", "package.json")
        };
    }

    private static string? ResolveRepo(string devRoot, string folder, string marker)
    {
        var root = Path.Combine(devRoot, folder);
        return File.Exists(Path.Combine(root, marker)) ? root : null;
    }
}
