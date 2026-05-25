using Xunit;

namespace Ontogony.Infrastructure.Tests;

/// <summary>
/// SYSTEM-LEARNING-GUIDE-001 — canonical docs/learn index gates.
/// </summary>
public sealed class SystemLearningGuideDocsTests
{
    private static readonly string RepoRoot = GetProjectRoot();

    [Fact]
    public void Learn_index_and_validator_exist()
    {
        var learnIndex = Path.Combine(RepoRoot, "docs", "learn", "INDEX.md");
        var validator = Path.Combine(RepoRoot, "scripts", "validate-learn-docs.ps1");
        var evidence = Path.Combine(RepoRoot, "docs", "evidence", "PLATFORM_SYSTEM_LEARNING_GUIDE_001.md");

        Assert.True(File.Exists(learnIndex));
        Assert.True(File.Exists(validator));
        Assert.True(File.Exists(evidence));

        var index = File.ReadAllText(learnIndex);
        Assert.Contains("SYSTEM-LEARNING-GUIDE-001", index, StringComparison.Ordinal);
        Assert.Contains("00_START_HERE.md", index, StringComparison.Ordinal);
        Assert.Contains("validate-learn-docs.ps1", index, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("00_START_HERE.md")]
    [InlineData("07_DOMAIN_MODEL_ROUTING_BOUNDARIES.md")]
    [InlineData("15_UI_CANONICALIZATION_AND_CONSOLE_UX.md")]
    [InlineData("GLOSSARY.md")]
    public void Priority_guides_exist(string fileName)
    {
        var path = Path.Combine(RepoRoot, "docs", "learn", fileName);
        Assert.True(File.Exists(path));
        var content = File.ReadAllText(path);
        Assert.Contains("Audience:", content, StringComparison.Ordinal);
        Assert.Contains("Source of truth:", content, StringComparison.Ordinal);
    }

    private static string GetProjectRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "Ontogony.Platform.sln")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new InvalidOperationException("Could not find Ontogony.Platform.sln from test base directory.");
    }
}
