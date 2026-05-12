namespace Ontogony.Execution;

/// <summary>
/// Mechanical record of a step within a run (opaque key and status).
/// </summary>
/// <remarks>
/// <see cref="StepKey"/> and <see cref="Status"/> are <b>opaque strings</b>. <see cref="Ordinal"/> is a host-defined ordering hint, not a graph edge.
/// </remarks>
public sealed record ExecutionStepRecord
{
    public required string StepId { get; init; }
    public required string RunId { get; init; }
    /// <summary>Opaque discriminator for the step (e.g. stable name or slug from the caller).</summary>
    public required string StepKey { get; init; }
    /// <summary>Opaque lifecycle status for the step.</summary>
    public required string Status { get; init; }
    /// <summary>Ordering hint within the run; callers define interpretation (not a graph edge).</summary>
    public int Ordinal { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? StartedAt { get; init; }
    public DateTimeOffset? CompletedAt { get; init; }
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}
