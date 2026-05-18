namespace Ontogony.Topology.Contracts;

/// <summary>Selected execution topology for a run (contracts only; opaque topology and selector vocabulary).</summary>
/// <remarks>
/// <see cref="SelectedTopology"/> and related strings are opaque; Ontogony does not authorize or execute topologies.
/// </remarks>
public sealed record TopologySelectionRecord(
    string TopologySelectionId,
    string RunId,
    string SelectedTopology,
    string SelectorVersion,
    string? Reason = null,
    bool? RequiresKanonAuthorization = null,
    string? TaskClassificationId = null,
    string? Classification = null,
    string? RiskLevel = null,
    decimal? Confidence = null,
    IReadOnlyDictionary<string, string>? Metadata = null);
