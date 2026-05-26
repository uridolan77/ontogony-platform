namespace Ontogony.Replay.Contracts;

/// <summary>Reference to an expected replay output artifact or inline hash.</summary>
public sealed record ReplayOutputRef(
    string Name,
    string Kind,
    ReplayArtifactRef? Artifact = null,
    string? InlineHash = null,
    IReadOnlyDictionary<string, string>? Metadata = null);
