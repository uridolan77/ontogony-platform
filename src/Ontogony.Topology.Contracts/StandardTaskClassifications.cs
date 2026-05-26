namespace Ontogony.Topology.Contracts;

/// <summary>Well-known task classification string constants (names only; Ontogony does not interpret behavior).</summary>
public static class StandardTaskClassifications
{
    /// <summary>Classification value: <c>sequential</c>.</summary>
    public const string Sequential = "sequential";

    /// <summary>Classification value: <c>parallelizable</c>.</summary>
    public const string Parallelizable = "parallelizable";

    /// <summary>Classification value: <c>exploratory</c>.</summary>
    public const string Exploratory = "exploratory";

    /// <summary>Classification value: <c>tool_heavy</c>.</summary>
    public const string ToolHeavy = "tool_heavy";

    /// <summary>Classification value: <c>high_risk</c>.</summary>
    public const string HighRisk = "high_risk";

    /// <summary>Classification value: <c>human_gate_likely</c>.</summary>
    public const string HumanGateLikely = "human_gate_likely";

    /// <summary>Classification value: <c>model_only</c>.</summary>
    public const string ModelOnly = "model_only";

    /// <summary>Classification value: <c>policy_only</c>.</summary>
    public const string PolicyOnly = "policy_only";
}
