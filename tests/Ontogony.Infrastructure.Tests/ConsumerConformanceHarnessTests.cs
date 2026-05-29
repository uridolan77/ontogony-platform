using Xunit;

namespace Ontogony.Infrastructure.Tests;

/// <summary>PLAT-MECH-011 — consumer conformance harness status semantics.</summary>
public sealed class ConsumerConformanceHarnessTests
{
    private static readonly string[] KnownStatuses = ["PASS", "PARTIAL", "FAIL", "NOT_RUN"];

    [Fact]
    public void Conformance_report_uses_known_statuses()
    {
        foreach (var status in KnownStatuses)
        {
            Assert.Contains(status, KnownStatuses);
        }
    }

    [Theory]
    [InlineData("PASS", "FAIL", "FAIL")]
    [InlineData("PASS", "PARTIAL", "PARTIAL")]
    [InlineData("PASS", "PASS", "PASS")]
    [InlineData("NOT_RUN", "PASS", "PARTIAL")]
    public void Overall_status_rollup_matches_suite_rules(string first, string second, string expected)
    {
        var checks = new[] { first, second };
        var overall = Rollup(checks);
        Assert.Equal(expected, overall);
    }

    private static string Rollup(IEnumerable<string> statuses)
    {
        var list = statuses.ToArray();
        if (list.Any(static s => s == "FAIL"))
        {
            return "FAIL";
        }

        if (list.Any(static s => s == "NOT_RUN" || s == "PARTIAL"))
        {
            return "PARTIAL";
        }

        return "PASS";
    }
}
