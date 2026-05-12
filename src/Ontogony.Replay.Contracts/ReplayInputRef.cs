namespace Ontogony.Replay.Contracts;

public sealed record ReplayInputRef(
    string Name,
    string Kind,
    ReplayArtifactRef? Artifact = null,
    string? InlineHash = null,
    IReadOnlyDictionary<string, string>? Metadata = null);
