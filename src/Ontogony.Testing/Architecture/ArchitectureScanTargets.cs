namespace Ontogony.Testing.Architecture;

/// <summary>
/// Helpers for locating MSBuild and source files used by architecture tests.
/// </summary>
public static class ArchitectureScanTargets
{
    /// <summary>
    /// Collects default MSBuild scan targets under <paramref name="repoRoot"/>.
    /// </summary>
    /// <param name="repoRoot">Repository root directory.</param>
    /// <param name="projectSearchRoots">Relative directories to search for <c>*.csproj</c> files. Defaults to <c>src</c> and <c>tests</c>.</param>
    /// <param name="additionalRelativeFiles">Additional repo-relative files to include when present.</param>
    public static IReadOnlyList<string> CollectMsBuildScanTargets(
        string repoRoot,
        IReadOnlyList<string>? projectSearchRoots = null,
        IReadOnlyList<string>? additionalRelativeFiles = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(repoRoot);

        var targets = new List<string>();
        var roots = projectSearchRoots ?? ["src", "tests"];

        foreach (var root in roots)
        {
            var dir = Path.Combine(repoRoot, root);
            if (!Directory.Exists(dir))
            {
                continue;
            }

            targets.AddRange(Directory.EnumerateFiles(dir, "*.csproj", SearchOption.AllDirectories));
        }

        foreach (var relativeFile in DefaultRootMsBuildFiles)
        {
            var path = Path.Combine(repoRoot, relativeFile);
            if (File.Exists(path))
            {
                targets.Add(path);
            }
        }

        if (additionalRelativeFiles is not null)
        {
            foreach (var relativeFile in additionalRelativeFiles)
            {
                var path = Path.IsPathRooted(relativeFile)
                    ? relativeFile
                    : Path.Combine(repoRoot, relativeFile);

                if (File.Exists(path))
                {
                    targets.Add(path);
                }
            }
        }

        return targets
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(static path => path, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    /// <summary>
    /// Resolves repo-relative source globs such as <c>src/**/*.cs</c>.
    /// </summary>
    public static IReadOnlyList<string> ResolveSourceGlobs(string repoRoot, IReadOnlyList<string> sourceGlobs)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(repoRoot);
        ArgumentNullException.ThrowIfNull(sourceGlobs);

        var files = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var glob in sourceGlobs)
        {
            if (string.IsNullOrWhiteSpace(glob))
            {
                continue;
            }

            var normalized = glob.Replace('\\', '/').Trim();
            const string recursiveMarker = "**/";

            if (normalized.Contains(recursiveMarker, StringComparison.Ordinal))
            {
                var splitIndex = normalized.IndexOf(recursiveMarker, StringComparison.Ordinal);
                var rootSegment = normalized[..splitIndex].TrimEnd('/');
                var filePattern = normalized[(splitIndex + recursiveMarker.Length)..];
                var searchRoot = string.IsNullOrWhiteSpace(rootSegment)
                    ? repoRoot
                    : Path.Combine(repoRoot, rootSegment.Replace('/', Path.DirectorySeparatorChar));

                if (!Directory.Exists(searchRoot))
                {
                    continue;
                }

                foreach (var file in Directory.EnumerateFiles(
                             searchRoot,
                             string.IsNullOrWhiteSpace(filePattern) ? "*.cs" : filePattern,
                             SearchOption.AllDirectories))
                {
                    files.Add(file);
                }

                continue;
            }

            var directPatternRoot = repoRoot;
            var directPattern = normalized;
            var lastSlash = normalized.LastIndexOf('/');
            if (lastSlash >= 0)
            {
                directPatternRoot = Path.Combine(repoRoot, normalized[..lastSlash].Replace('/', Path.DirectorySeparatorChar));
                directPattern = normalized[(lastSlash + 1)..];
            }

            if (!Directory.Exists(directPatternRoot))
            {
                continue;
            }

            foreach (var file in Directory.EnumerateFiles(
                         directPatternRoot,
                         directPattern,
                         SearchOption.AllDirectories))
            {
                files.Add(file);
            }
        }

        return files
            .OrderBy(static path => path, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    /// <summary>
    /// Walks parent directories from <paramref name="startDirectory"/> until <paramref name="markerFileName"/> exists.
    /// </summary>
    public static string FindRepoRoot(string startDirectory, string markerFileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(startDirectory);
        ArgumentException.ThrowIfNullOrWhiteSpace(markerFileName);

        var dir = new DirectoryInfo(startDirectory);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, markerFileName)))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new InvalidOperationException($"Could not locate '{markerFileName}' from '{startDirectory}'.");
    }

    private static readonly string[] DefaultRootMsBuildFiles =
    [
        "Directory.Packages.props",
        "Directory.Build.props",
        "Directory.Build.targets",
    ];
}
