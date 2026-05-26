namespace Ontogony.Topology.Contracts;

/// <summary>Well-known topology mode string constants (names only; Ontogony does not interpret behavior).</summary>
public static class StandardTopologyModes
{
    /// <summary>Mode value: <c>single_workflow</c>.</summary>
    public const string SingleWorkflow = "single_workflow";

    /// <summary>Mode value: <c>centralized_orchestrator</c>.</summary>
    public const string CentralizedOrchestrator = "centralized_orchestrator";

    /// <summary>Mode value: <c>parallel_review</c>.</summary>
    public const string ParallelReview = "parallel_review";

    /// <summary>Mode value: <c>decentralized_research</c>.</summary>
    public const string DecentralizedResearch = "decentralized_research";

    /// <summary>Mode value: <c>hybrid_validation</c>.</summary>
    public const string HybridValidation = "hybrid_validation";

    /// <summary>Mode value: <c>deny_or_human_gate_first</c>.</summary>
    public const string DenyOrHumanGateFirst = "deny_or_human_gate_first";
}
