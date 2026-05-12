namespace Ontogony.Replay.Contracts;

public sealed record ReplayArtifactRef(
    string ArtifactId,
    string? ContentHash = null,
    string? MediaType = null,
    long? SizeBytes = null);
