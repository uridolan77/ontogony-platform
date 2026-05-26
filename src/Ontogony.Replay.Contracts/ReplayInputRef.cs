namespace Ontogony.Replay.Contracts;

/// <summary>Reference to a replay input artifact or inline hash.</summary>
public sealed record ReplayInputRef(
    string Name,
    string Kind,
    ReplayArtifactRef? Artifact = null,
    string? InlineHash = null,
    IReadOnlyDictionary<string, string>? Metadata = null);
