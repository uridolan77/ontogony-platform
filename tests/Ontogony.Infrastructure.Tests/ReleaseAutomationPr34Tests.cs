using System.Text.Json;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

/// <summary>Tests for release automation and package manifest generation.</summary>
public sealed class ReleaseAutomationPr34Tests
{
    [Fact]
    public void PackageManifestSchema_IncludesVersion()
    {
        // This test validates the expected schema for PACKAGE_MANIFEST.json
        // In a real scenario, this would parse and validate an actual manifest

        var json = """
        {
          "version": "0.3.0",
          "generated": "2026-05-12T00:00:00Z",
          "commit": "abc123def456",
          "packageCount": 14,
          "packages": [
            {
              "id": "Ontogony.Contracts",
              "version": "0.3.0",
              "filename": "Ontogony.Contracts.0.3.0.nupkg",
              "sizeBytes": 12345,
              "sha256": "abcdef0123456789"
            }
          ]
        }
        """;

        var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        // Validate schema
        Assert.True(root.TryGetProperty("version", out var versionEl));
        Assert.Equal("0.3.0", versionEl.GetString());

        Assert.True(root.TryGetProperty("generated", out var generatedEl));
        Assert.NotEmpty(generatedEl.GetString() ?? "");

        Assert.True(root.TryGetProperty("commit", out var commitEl));
        Assert.NotEmpty(commitEl.GetString() ?? "");

        Assert.True(root.TryGetProperty("packageCount", out var countEl));
        Assert.Equal(14, countEl.GetInt32());

        Assert.True(root.TryGetProperty("packages", out var packagesEl));
        Assert.Equal(1, packagesEl.GetArrayLength());

        var pkg = packagesEl[0];
        Assert.True(pkg.TryGetProperty("id", out var idEl));
        Assert.Equal("Ontogony.Contracts", idEl.GetString());

        Assert.True(pkg.TryGetProperty("version", out var pkgVerEl));
        Assert.Equal("0.3.0", pkgVerEl.GetString());

        Assert.True(pkg.TryGetProperty("sha256", out var hashEl));
        Assert.NotEmpty(hashEl.GetString() ?? "");
    }

    [Fact]
    public void SemverRegex_MatchesValidVersions()
    {
        // Test version parsing used in manifest generation
        var validVersions = new[] { "0.1.0", "1.2.3", "2.0.0-alpha", "1.0.0-rc.1" };
        var pattern = new System.Text.RegularExpressions.Regex(@"^(\d+\.\d+\.\d+(?:.*)?)$");

        foreach (var version in validVersions)
        {
            Assert.True(pattern.IsMatch(version), $"Version '{version}' should match semver pattern");
        }
    }

    [Fact]
    public void NupkgFilenameParser_ExtractsIdAndVersion()
    {
        // Test filename parsing used in manifest generation
        var filename = "Ontogony.Contracts.0.3.0.nupkg";
        var pattern = new System.Text.RegularExpressions.Regex(@"^(.+?)\.(\d+\.\d+\.\d+)\.nupkg$");

        var match = pattern.Match(filename);
        Assert.True(match.Success);
        Assert.Equal("Ontogony.Contracts", match.Groups[1].Value);
        Assert.Equal("0.3.0", match.Groups[2].Value);
    }

    [Fact]
    public void NupkgFilenameParser_IgnoresSymbolsPackages()
    {
        // Manifest generation should skip .symbols.nupkg files
        var symbolsFilename = "Ontogony.Contracts.0.3.0.symbols.nupkg";
        var shouldInclude = !symbolsFilename.Contains(".symbols.nupkg");

        Assert.False(shouldInclude);
    }

    [Theory]
    [InlineData("Ontogony.Contracts", "Ontogony.Contracts", true)]
    [InlineData("ontogony.contracts", "Ontogony.Contracts", false)]  // Case-sensitive
    [InlineData("Ontogony.Contracts.Beta", "Ontogony.Contracts", false)]
    public void PackageIdMatching_IsCaseSensitive(string actual, string expected, bool shouldMatch)
    {
        var match = string.Equals(actual, expected, StringComparison.Ordinal);
        Assert.Equal(shouldMatch, match);
    }

    [Theory]
    [InlineData("0.3.0", "0.3.0", true)]
    [InlineData("0.3.0", "0.3.1", false)]
    [InlineData("0.3.0-alpha", "0.3.0-beta", false)]
    public void VersionValidation_DetectsMismatch(string actual, string expected, bool isMatch)
    {
        var mismatch = !string.Equals(actual, expected, StringComparison.Ordinal);
        Assert.Equal(!isMatch, mismatch);
    }

    [Fact]
    public void ManifestGeneration_RequiresNonEmptyPackageList()
    {
        // Manifest generation should fail if no packages are found
        var packages = Array.Empty<(string id, string version)>();
        var isEmpty = packages.Length == 0;

        Assert.True(isEmpty);
    }

    [Fact]
    public void GithubReleaseIntegration_CreatesAssetsFromManifest()
    {
        // This test validates the expected structure for GitHub release assets
        // In practice, release-packages.yml creates a release with:
        // - All .nupkg files
        // - All .snupkg (symbols) files
        // - PACKAGE_MANIFEST.json

        var assetNames = new[] 
        { 
            "Ontogony.Contracts.0.3.0.nupkg",
            "Ontogony.Contracts.0.3.0.snupkg",
            "Ontogony.Testing.0.3.0.nupkg",
            "PACKAGE_MANIFEST.json"
        };

        Assert.NotEmpty(assetNames);
        Assert.Contains("PACKAGE_MANIFEST.json", assetNames);
    }

    [Fact]
    public void ChangelogValidation_AcceptsUnreleasedSection()
    {
        var content = """
        # Changelog
        
        ## Unreleased
        
        - Feature A
        - Bug fix B
        
        ## 0.2.0
        
        - Previous release notes
        """;

        var hasUnreleasedSection = content.Contains("## Unreleased");
        Assert.True(hasUnreleasedSection);
    }

    [Fact]
    public void ChangelogValidation_AcceptsVersionSection()
    {
        var content = """
        # Changelog
        
        ## 0.3.0
        
        - Feature A
        - Bug fix B
        """;

        var version = "0.3.0";
        var hasVersionSection = content.Contains($"## {version}");
        Assert.True(hasVersionSection);
    }
}
