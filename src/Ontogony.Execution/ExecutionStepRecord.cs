namespace Ontogony.Execution;

/// <summary>
/// Mechanical record of a step within a run (opaque key and status).
/// </summary>
/// <remarks>
/// <see cref="StepKey"/> and <see cref="Status"/> are <b>opaque strings</b>. <see cref="Ordinal"/> is a host-defined ordering hint, not a graph edge.
/// This type does not validate lifecycle consistency across fields; consumers validate when they need stricter journal semantics.
/// </remarks>
public sealed record ExecutionStepRecord
{
    /// <summary>Stable unique id for this step line in the journal.</summary>
    public required string StepId { get; init; }

    /// <summary>Parent run id.</summary>
    public required string RunId { get; init; }
    /// <summary>Opaque discriminator for the step (e.g. stable name or slug from the caller).</summary>
    public required string StepKey { get; init; }
    /// <summary>Opaque lifecycle status for the step.</summary>
    public required string Status { get; init; }
    /// <summary>Ordering hint within the run; callers define interpretation (not a graph edge).</summary>
    public int Ordinal { get; init; }
    /// <summary>When the step record was created.</summary>
    public required DateTimeOffset CreatedAt { get; init; }

    /// <summary>When the step started executing, if known.</summary>
    public DateTimeOffset? StartedAt { get; init; }

    /// <summary>When the step finished, if known.</summary>
    public DateTimeOffset? CompletedAt { get; init; }

    /// <summary>Optional small opaque key/value metadata.</summary>
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}
