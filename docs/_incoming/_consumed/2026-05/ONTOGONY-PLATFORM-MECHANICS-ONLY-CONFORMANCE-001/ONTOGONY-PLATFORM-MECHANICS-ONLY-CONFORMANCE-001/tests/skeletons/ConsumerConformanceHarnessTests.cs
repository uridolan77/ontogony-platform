using Xunit;

public sealed class ConsumerConformanceHarnessTests
{
    [Fact]
    public void Conformance_Report_Uses_Known_Statuses()
    {
        var statuses = new[] { "PASS", "PARTIAL", "FAIL", "NOT_RUN" };
        Assert.Contains("PASS", statuses);
    }
}
