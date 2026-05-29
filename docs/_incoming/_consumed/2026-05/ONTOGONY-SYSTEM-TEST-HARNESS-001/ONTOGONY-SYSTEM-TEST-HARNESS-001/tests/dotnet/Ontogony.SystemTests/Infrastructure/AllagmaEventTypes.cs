namespace Ontogony.SystemTests.Infrastructure;

/// <summary>First-loop milestone event types from Allagma.Application AgentRunEventTypes (string copies for harness isolation).</summary>
public static class AllagmaEventTypes
{
    public const string RunCreated = "Allagma.RunCreated";
    public const string KanonPlanCompiled = "Allagma.KanonPlanCompiled";
    public const string ConexusModelRequested = "Allagma.ConexusModelRequested";
    public const string ConexusModelCompleted = "Allagma.ConexusModelCompleted";
    public const string RunCompleted = "Allagma.RunCompleted";
    public const string RunHumanGatePaused = "Allagma.RunHumanGatePaused";
    public const string RunHumanGateResumed = "Allagma.RunHumanGateResumed";
    public const string RunHumanGateDenied = "Allagma.RunHumanGateDenied";

    public static readonly string[] GovernedHappyPathOrdered =
    [
        RunCreated,
        KanonPlanCompiled,
        ConexusModelRequested,
        ConexusModelCompleted,
        RunCompleted
    ];
}
