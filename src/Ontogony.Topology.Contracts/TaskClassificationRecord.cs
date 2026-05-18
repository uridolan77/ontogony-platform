namespace Ontogony.Topology.Contracts;

/// <summary>Task structure classification for a run (contracts only; opaque classification and risk vocabulary).</summary>
/// <remarks>
/// <see cref="Classification"/> and <see cref="RiskLevel"/> are opaque; callers own classifier semantics.
/// </remarks>
public sealed record TaskClassificationRecord(
    string TaskClassificationId,
    string RunId,
    string Classification,
    string? RiskLevel = null,
    decimal? Confidence = null,
    IReadOnlyList<string>? Signals = null,
    string? ClassifierVersion = null,
    IReadOnlyDictionary<string, string>? Metadata = null);
