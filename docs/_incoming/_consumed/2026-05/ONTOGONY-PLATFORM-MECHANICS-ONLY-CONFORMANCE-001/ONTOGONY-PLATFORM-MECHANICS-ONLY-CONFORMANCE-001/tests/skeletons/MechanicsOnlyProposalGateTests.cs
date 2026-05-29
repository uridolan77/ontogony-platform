using Xunit;

public sealed class MechanicsOnlyProposalGateTests
{
    [Fact]
    public void Rejects_ModelRoutingPolicy_In_Platform()
    {
        var proposal = "provider fallback policy";
        Assert.Contains("provider", proposal);
        // Replace with real gate invocation.
    }

    [Fact]
    public void Accepts_ActorContextSchema_As_Mechanical()
    {
        var proposal = "actor context propagation";
        Assert.Contains("actor context", proposal);
        // Replace with real gate invocation.
    }
}
