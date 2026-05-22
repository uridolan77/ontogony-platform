using Xunit;

namespace Ontogony.SystemCompatibility.Tests;

[Trait("Category", "SystemCompatGate")]
public sealed class SixRepoCompatibilityGateIntegrationTests
{
    [Fact]
    public void Run_six_repo_gate_against_dev_root_when_configured()
    {
        var devRoot = Environment.GetEnvironmentVariable("ONTOGONY_DEV_ROOT");
        if (string.IsNullOrWhiteSpace(devRoot))
        {
            devRoot = ResolveDefaultDevRoot();
        }

        if (string.IsNullOrWhiteSpace(devRoot) || !Directory.Exists(devRoot))
        {
            return;
        }

        var platformRoot = Environment.GetEnvironmentVariable("ONTOGONY_PLATFORM_ROOT");
        if (string.IsNullOrWhiteSpace(platformRoot))
        {
            platformRoot = Path.Combine(devRoot, "ontogony-platform");
        }

        var strictMode = Environment.GetEnvironmentVariable("ONTOGONY_SIX_REPO_STRICT_MODE") == "1";

        var options = new SystemCompatibilityGateOptions
        {
            DevRoot = devRoot,
            PlatformRoot = platformRoot,
            StrictMode = strictMode
        };

        var result = SixRepoCompatibilityGate.Evaluate(options);

        var artifactDir = Environment.GetEnvironmentVariable("ONTOGONY_SYSTEM_COMPAT_ARTIFACT_DIR");
        if (string.IsNullOrWhiteSpace(artifactDir))
        {
            artifactDir = Path.Combine(platformRoot, "artifacts", "six-repo-compat");
        }

        SystemCompatibilitySummaryWriter.WriteArtifacts(
            result,
            artifactDir,
            "six-repo-compatibility-summary");

        Assert.True(
            result.Passed,
            SystemCompatibilitySummaryWriter.RenderMarkdown(result));
    }

    private static string? ResolveDefaultDevRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            var candidate = Path.Combine(dir.FullName, "allagma-dotnet", "Allagma.sln");
            if (File.Exists(candidate))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        return null;
    }
}
