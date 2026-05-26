namespace Ontogony.Replay.Contracts;

/// <summary>Reference to a replay artifact payload (identity and optional content metadata).</summary>
public sealed record ReplayArtifactRef(
    string ArtifactId,
    string? ContentHash = null,
    string? MediaType = null,
    long? SizeBytes = null);
