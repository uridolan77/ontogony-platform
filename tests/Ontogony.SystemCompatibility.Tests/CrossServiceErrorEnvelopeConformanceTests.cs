using Xunit;

namespace Ontogony.SystemCompatibility.Tests;

public sealed class CrossServiceErrorEnvelopeConformanceTests
{
    [Fact]
    public void Minimal_fixture_passes_error_envelope_checks()
    {
        var fixtureRoot = Path.Combine(AppContext.BaseDirectory, "Fixtures", "minimal-workspace");
        var options = new SystemCompatibilityGateOptions
        {
            DevRoot = fixtureRoot,
            PlatformRoot = Path.Combine(fixtureRoot, "ontogony-platform"),
            RequireFrontendRepos = false
        };

        var workspace = SystemCompatibilityWorkspace.Resolve(options);
        var checks = new[]
        {
            CrossServiceErrorEnvelopeConformance.CheckMatrixArtifacts(workspace),
            CrossServiceErrorEnvelopeConformance.CheckPlatformSamples(workspace),
            CrossServiceErrorEnvelopeConformance.CheckTaxonomyAdapterMappings(workspace)
        };

        Assert.All(checks, c => Assert.Equal(SystemCompatibilityCheckStatus.Pass, c.Status));
    }

    [Fact]
    public void Dev_root_passes_error_envelope_checks_when_siblings_present()
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

        var workspace = SystemCompatibilityWorkspace.Resolve(new SystemCompatibilityGateOptions
        {
            DevRoot = devRoot,
            PlatformRoot = Path.Combine(devRoot, "ontogony-platform")
        });

        var checks = new[]
        {
            CrossServiceErrorEnvelopeConformance.CheckMatrixArtifacts(workspace),
            CrossServiceErrorEnvelopeConformance.CheckPlatformSamples(workspace),
            CrossServiceErrorEnvelopeConformance.CheckTaxonomyAdapterMappings(workspace),
            CrossServiceErrorEnvelopeConformance.CheckOpenApiEnvelopeSchema(workspace),
            CrossServiceErrorEnvelopeConformance.CheckSiblingIntegrationDocs(workspace),
            CrossServiceErrorEnvelopeConformance.CheckFrontendTaxonomyModule(workspace)
        };

        var failures = checks.Where(c => c.Status == SystemCompatibilityCheckStatus.Fail).ToList();
        Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures.Select(f => $"{f.Id}: {f.Detail}")));
    }

    private static string? ResolveDefaultDevRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "allagma-dotnet", "Allagma.sln")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        return null;
    }
}
