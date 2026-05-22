using System.Text.Json;

namespace Ontogony.Testing.Architecture;

/// <summary>
/// Scans platform source for product-meaning phrases that belong in product repos, not Ontogony.Platform.
/// </summary>
public static class ProductSemanticBoundaryScanner
{
    public sealed record Policy(
        int Version,
        IReadOnlyList<string> ScanGlobs,
        IReadOnlyList<string> ForbiddenPhrases,
        IReadOnlyList<string>? AllowedPathSuffixes);

    public sealed record Violation(string RelativePath, string ForbiddenPhrase, int LineNumber);

    public static Policy LoadPolicy(string policyJsonPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(policyJsonPath);

        if (!File.Exists(policyJsonPath))
        {
            throw new FileNotFoundException($"Product semantic boundary policy not found: {policyJsonPath}", policyJsonPath);
        }

        var json = File.ReadAllText(policyJsonPath);
        var policy = JsonSerializer.Deserialize<PolicyDto>(json, JsonOptions)
            ?? throw new InvalidOperationException($"Invalid product semantic boundary policy: {policyJsonPath}");

        return new Policy(
            policy.Version,
            policy.ScanGlobs ?? [],
            policy.ForbiddenPhrases ?? [],
            policy.AllowedPathSuffixes);
    }

    public static IReadOnlyList<Violation> ScanRepo(string repoRoot, Policy policy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(repoRoot);
        ArgumentNullException.ThrowIfNull(policy);

        var files = ArchitectureScanTargets.ResolveSourceGlobs(repoRoot, policy.ScanGlobs);
        var violations = new List<Violation>();

        foreach (var file in files)
        {
            var relativePath = Path.GetRelativePath(repoRoot, file).Replace('\\', '/');
            if (IsAllowedPath(relativePath, policy.AllowedPathSuffixes))
            {
                continue;
            }

            var lines = File.ReadAllLines(file);
            for (var lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                var line = lines[lineIndex];
                foreach (var phrase in policy.ForbiddenPhrases)
                {
                    if (string.IsNullOrWhiteSpace(phrase))
                    {
                        continue;
                    }

                    if (line.Contains(phrase, StringComparison.OrdinalIgnoreCase))
                    {
                        violations.Add(new Violation(relativePath, phrase, lineIndex + 1));
                    }
                }
            }
        }

        return violations
            .OrderBy(static v => v.RelativePath, StringComparer.OrdinalIgnoreCase)
            .ThenBy(static v => v.LineNumber)
            .ThenBy(static v => v.ForbiddenPhrase, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    public static void AssertNoProductSemanticLeakage(string repoRoot, Policy policy)
    {
        var violations = ScanRepo(repoRoot, policy);
        if (violations.Count == 0)
        {
            return;
        }

        var details = violations
            .Select(static v => $"{v.RelativePath}:{v.LineNumber}: {v.ForbiddenPhrase}")
            .ToArray();

        throw new InvalidOperationException(
            "Product semantic leakage detected in platform source:\n" + string.Join('\n', details));
    }

    private static bool IsAllowedPath(string relativePath, IReadOnlyList<string>? allowedPathSuffixes)
    {
        if (allowedPathSuffixes is null || allowedPathSuffixes.Count == 0)
        {
            return false;
        }

        foreach (var suffix in allowedPathSuffixes)
        {
            if (string.IsNullOrWhiteSpace(suffix))
            {
                continue;
            }

            if (relativePath.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    private sealed class PolicyDto
    {
        public int Version { get; init; }

        public List<string>? ScanGlobs { get; init; }

        public List<string>? ForbiddenPhrases { get; init; }

        public List<string>? AllowedPathSuffixes { get; init; }
    }
}
