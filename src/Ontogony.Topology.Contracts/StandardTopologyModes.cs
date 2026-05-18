namespace Ontogony.Topology.Contracts;

/// <summary>Well-known topology mode string constants (names only; Ontogony does not interpret behavior).</summary>
public static class StandardTopologyModes
{
    public const string SingleWorkflow = "single_workflow";

    public const string CentralizedOrchestrator = "centralized_orchestrator";

    public const string ParallelReview = "parallel_review";

    public const string DecentralizedResearch = "decentralized_research";

    public const string HybridValidation = "hybrid_validation";

    public const string DenyOrHumanGateFirst = "deny_or_human_gate_first";
}
