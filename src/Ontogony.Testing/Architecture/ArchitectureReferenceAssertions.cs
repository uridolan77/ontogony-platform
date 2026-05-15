using System.Text.RegularExpressions;

namespace Ontogony.Testing.Architecture;

/// <summary>
/// Reusable architecture assertions for forbidden package/project references and <c>using</c> directives.
/// </summary>
public static class ArchitectureReferenceAssertions
{
    private static readonly Regex[] ReferencePatterns =
    [
        new(
            @"<(?<kind>PackageReference|ProjectReference)\s+Include=""(?<ref>[^""]+)""",
            RegexOptions.IgnoreCase | RegexOptions.Compiled),
        new(
            @"<(?<kind>PackageVersion)\s+Include=""(?<ref>[^""]+)""",
            RegexOptions.IgnoreCase | RegexOptions.Compiled),
    ];

    private static readonly Regex UsingDirectivePattern = new(
        @"^\s*(?:global\s+)?using\s+(?<ns>[\w.]+)\s*;",
        RegexOptions.Multiline | RegexOptions.Compiled);

    /// <summary>
    /// Finds forbidden MSBuild references in a single file.
    /// </summary>
    public static IReadOnlyList<string> FindForbiddenReferences(
        string fileName,
        string content,
        IReadOnlyList<string> forbiddenFragments)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentNullException.ThrowIfNull(content);
        ArgumentNullException.ThrowIfNull(forbiddenFragments);

        var violations = new List<string>();

        foreach (var pattern in ReferencePatterns)
        {
            foreach (Match match in pattern.Matches(content))
            {
                var kind = match.Groups["kind"].Value;
                var reference = match.Groups["ref"].Value;

                foreach (var forbidden in forbiddenFragments)
                {
                    if (string.IsNullOrWhiteSpace(forbidden))
                    {
                        continue;
                    }

                    if (reference.Contains(forbidden, StringComparison.OrdinalIgnoreCase))
                    {
                        violations.Add($"{fileName}: {kind} -> {reference} (matches {forbidden})");
                    }
                }
            }
        }

        return violations;
    }

    /// <summary>
    /// Asserts that none of the scanned MSBuild files contain forbidden references.
    /// </summary>
    public static void AssertNoForbiddenReferences(
        string repoRoot,
        IReadOnlyList<string> files,
        IReadOnlyList<string> forbiddenFragments)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(repoRoot);
        ArgumentNullException.ThrowIfNull(files);
        ArgumentNullException.ThrowIfNull(forbiddenFragments);

        var violations = new List<string>();

        foreach (var file in files)
        {
            if (!File.Exists(file))
            {
                throw new InvalidOperationException($"Scan target does not exist: {file}");
            }

            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);
            violations.AddRange(FindForbiddenReferences(fileName, content, forbiddenFragments));
        }

        if (violations.Count > 0)
        {
            throw new InvalidOperationException(
                "Forbidden dependency references found:\n" + string.Join('\n', violations));
        }
    }

    /// <summary>
    /// Finds forbidden namespace <c>using</c> directives in a single source file.
    /// </summary>
    public static IReadOnlyList<string> FindForbiddenUsingDirectives(
        string fileName,
        string content,
        IReadOnlyList<string> forbiddenNamespaces)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentNullException.ThrowIfNull(content);
        ArgumentNullException.ThrowIfNull(forbiddenNamespaces);

        var violations = new List<string>();

        foreach (Match match in UsingDirectivePattern.Matches(content))
        {
            var ns = match.Groups["ns"].Value;
            foreach (var forbidden in forbiddenNamespaces)
            {
                if (string.IsNullOrWhiteSpace(forbidden))
                {
                    continue;
                }

                if (MatchesForbiddenNamespace(ns, forbidden))
                {
                    violations.Add($"{fileName}: using {ns} (matches {forbidden})");
                }
            }
        }

        return violations;
    }

    /// <summary>
    /// Asserts that resolved source files do not contain forbidden namespace <c>using</c> directives.
    /// </summary>
    public static void AssertNoForbiddenUsingDirectives(
        string repoRoot,
        IReadOnlyList<string> sourceGlobs,
        IReadOnlyList<string> forbiddenNamespaces)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(repoRoot);
        ArgumentNullException.ThrowIfNull(sourceGlobs);
        ArgumentNullException.ThrowIfNull(forbiddenNamespaces);

        var files = ArchitectureScanTargets.ResolveSourceGlobs(repoRoot, sourceGlobs);
        var violations = new List<string>();

        foreach (var file in files)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);
            violations.AddRange(FindForbiddenUsingDirectives(fileName, content, forbiddenNamespaces));
        }

        if (violations.Count > 0)
        {
            throw new InvalidOperationException(
                "Forbidden using directives found:\n" + string.Join('\n', violations));
        }
    }

    private static bool MatchesForbiddenNamespace(string ns, string forbidden) =>
        ns.Equals(forbidden, StringComparison.Ordinal)
        || ns.StartsWith(forbidden + ".", StringComparison.Ordinal);
}
