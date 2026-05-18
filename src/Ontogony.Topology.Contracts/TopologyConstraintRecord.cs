namespace Ontogony.Topology.Contracts;

/// <summary>Neutral constraint attached to a topology choice (opaque validation and baseline vocabulary).</summary>
/// <remarks>
/// Records policy or pack constraints without Kanon evaluation semantics; callers own constraint meaning.
/// </remarks>
public sealed record TopologyConstraintRecord(
    string ConstraintId,
    string? TopologyMode = null,
    string? ValidationMode = null,
    bool? RequiresHumanGate = null,
    bool? RequiresBaselineComparison = null,
    string? BaselineMode = null,
    IReadOnlyList<string>? AllowedTopologyModes = null,
    string? Summary = null,
    IReadOnlyDictionary<string, string>? Metadata = null);
