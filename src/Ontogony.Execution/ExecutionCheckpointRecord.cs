namespace Ontogony.Execution;

/// <summary>
/// Mechanical durable checkpoint marker for resume or audit (no storage backend or retention policy).
/// </summary>
/// <remarks>
/// <see cref="Label"/> and <see cref="ResumeToken"/> are <b>opaque strings</b>. <see cref="PayloadHash"/> is an opaque fingerprint if the caller snapshots payload bytes or canonical JSON.
/// <see cref="PayloadArtifactId"/> may match <c>ArtifactId</c> from an <c>Ontogony.Artifacts.ArtifactRef</c> when checkpoints are stored out-of-line; this package does not reference artifacts types.
/// This type does not validate lifecycle consistency across fields; consumers validate when they need stricter journal semantics.
/// </remarks>
public sealed record ExecutionCheckpointRecord
{
    public required string CheckpointId { get; init; }
    public required string RunId { get; init; }
    /// <summary>Monotonic sequence within the run as defined by the caller.</summary>
    public long Sequence { get; init; }
    public required DateTimeOffset RecordedAt { get; init; }
    public string? Label { get; init; }
    public string? PayloadHash { get; init; }
    /// <summary>Optional opaque artifact identifier when payload bytes live in an artifact store (e.g. same value as <c>ArtifactRef.ArtifactId</c>).</summary>
    public string? PayloadArtifactId { get; init; }
    public string? ResumeToken { get; init; }
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}
