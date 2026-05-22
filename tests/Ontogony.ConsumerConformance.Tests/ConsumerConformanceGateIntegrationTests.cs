using Ontogony.SystemCompatibility;
using Xunit;

namespace Ontogony.ConsumerConformance.Tests;

[Trait("Category", "ConsumerConformance")]
public sealed class ConsumerConformanceGateIntegrationTests
{
    [Fact]
    public void Run_consumer_conformance_against_dev_root_when_configured()
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

        var artifactDir = Environment.GetEnvironmentVariable("ONTOGONY_CONSUMER_CONFORMANCE_ARTIFACT_DIR");
        if (string.IsNullOrWhiteSpace(artifactDir))
        {
            return;
        }

        var allagmaSln = Path.Combine(devRoot, "allagma-dotnet", "Allagma.sln");
        if (!File.Exists(allagmaSln))
        {
            return;
        }

        var platformRoot = Environment.GetEnvironmentVariable("ONTOGONY_PLATFORM_ROOT");
        if (string.IsNullOrWhiteSpace(platformRoot))
        {
            platformRoot = Path.Combine(devRoot, "ontogony-platform");
        }

        var strictMode = Environment.GetEnvironmentVariable("ONTOGONY_CONSUMER_CONFORMANCE_STRICT") == "1";

        var options = new SystemCompatibilityGateOptions
        {
            DevRoot = devRoot,
            PlatformRoot = platformRoot,
            StrictMode = strictMode
        };

        var result = ConsumerConformanceGate.Evaluate(options);

        ConsumerConformanceSummaryWriter.WriteArtifacts(result, artifactDir);

        Assert.True(
            result.Passed,
            ConsumerConformanceSummaryWriter.RenderMarkdown(result));
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
