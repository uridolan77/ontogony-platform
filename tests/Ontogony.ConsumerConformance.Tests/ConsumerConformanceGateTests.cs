using Ontogony.SystemCompatibility;
using Xunit;

namespace Ontogony.ConsumerConformance.Tests;

[Trait("Category", "ConsumerConformance")]
public sealed class ConsumerConformanceGateTests
{
    private static string FixtureRoot =>
        Path.Combine(AppContext.BaseDirectory, "Fixtures", "minimal-workspace");

    private static SystemCompatibilityGateOptions FixtureOptions(string dir) =>
        new()
        {
            DevRoot = dir,
            PlatformRoot = Path.Combine(dir, "ontogony-platform"),
            RequireFrontendRepos = false
        };

    [Fact]
    public void Minimal_fixture_reports_all_five_consumers()
    {
        var result = ConsumerConformanceGate.Evaluate(FixtureOptions(FixtureRoot));

        Assert.Equal(5, result.Consumers.Count);
        Assert.Contains(result.Consumers, c => c.Consumer == "conexus");
        Assert.Contains(result.Consumers, c => c.Consumer == "kanon");
        Assert.Contains(result.Consumers, c => c.Consumer == "allagma");
        Assert.Contains(result.Consumers, c => c.Consumer == "frontend");
        Assert.Contains(result.Consumers, c => c.Consumer == "ui");
    }

    [Fact]
    public void Minimal_fixture_passes_consumer_conformance()
    {
        var result = ConsumerConformanceGate.Evaluate(FixtureOptions(FixtureRoot));

        Assert.True(result.Passed, ConsumerConformanceSummaryWriter.RenderMarkdown(result));
        Assert.All(
            result.Consumers.Where(c => c.Consumer is not "ui" and not "frontend"),
            c => Assert.True(c.Passed, $"{c.Consumer}: {c.Verdict}"));
    }
}
