namespace Ontogony.Replay.Contracts;

public sealed record ReplayOutputRef(
    string Name,
    string Kind,
    ReplayArtifactRef? Artifact = null,
    string? InlineHash = null,
    IReadOnlyDictionary<string, string>? Metadata = null);
