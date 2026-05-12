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
    /// <summary>Stable unique id for this checkpoint line in the journal.</summary>
    public required string CheckpointId { get; init; }

    /// <summary>Run this checkpoint is scoped to.</summary>
    public required string RunId { get; init; }
    /// <summary>Monotonic sequence within the run as defined by the caller.</summary>
    public long Sequence { get; init; }
    /// <summary>When the checkpoint was recorded.</summary>
    public required DateTimeOffset RecordedAt { get; init; }

    /// <summary>Opaque human- or machine-readable label for the checkpoint.</summary>
    public string? Label { get; init; }

    /// <summary>Opaque fingerprint of checkpoint payload bytes or canonical form, if any.</summary>
    public string? PayloadHash { get; init; }
    /// <summary>Optional opaque artifact identifier when payload bytes live in an artifact store (e.g. same value as <c>ArtifactRef.ArtifactId</c>).</summary>
    public string? PayloadArtifactId { get; init; }
    /// <summary>Opaque token a host may use to resume after this checkpoint.</summary>
    public string? ResumeToken { get; init; }

    /// <summary>Optional small opaque key/value metadata.</summary>
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}
