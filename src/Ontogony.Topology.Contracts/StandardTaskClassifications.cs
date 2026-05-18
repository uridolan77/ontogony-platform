namespace Ontogony.Topology.Contracts;

/// <summary>Well-known task classification string constants (names only; Ontogony does not interpret behavior).</summary>
public static class StandardTaskClassifications
{
    public const string Sequential = "sequential";

    public const string Parallelizable = "parallelizable";

    public const string Exploratory = "exploratory";

    public const string ToolHeavy = "tool_heavy";

    public const string HighRisk = "high_risk";

    public const string HumanGateLikely = "human_gate_likely";

    public const string ModelOnly = "model_only";

    public const string PolicyOnly = "policy_only";
}
