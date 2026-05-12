namespace Ontogony.Execution;

/// <summary>
/// Mechanical record of a state change on some scoped subject (no state machine or policy engine).
/// </summary>
/// <remarks>
/// <see cref="SubjectKind"/>, <see cref="FromState"/>, <see cref="ToState"/>, and <see cref="ReasonCode"/> are <b>opaque strings</b>.
/// This type does not validate lifecycle consistency across fields; consumers validate when they need stricter journal semantics.
/// </remarks>
public sealed record ExecutionStateTransitionRecord
{
    /// <summary>Stable unique id for this transition line in the journal.</summary>
    public required string TransitionId { get; init; }

    /// <summary>Run this transition is scoped to.</summary>
    public required string RunId { get; init; }
    /// <summary>Opaque classifier for what <see cref="SubjectId"/> refers to (e.g. "run", "step").</summary>
    public required string SubjectKind { get; init; }
    /// <summary>Opaque id of the subject (interpretation depends on <see cref="SubjectKind"/>).</summary>
    public required string SubjectId { get; init; }

    /// <summary>Opaque prior state label.</summary>
    public required string FromState { get; init; }

    /// <summary>Opaque new state label.</summary>
    public required string ToState { get; init; }

    /// <summary>When the transition occurred.</summary>
    public required DateTimeOffset OccurredAt { get; init; }

    /// <summary>Optional opaque reason or error code for the transition.</summary>
    public string? ReasonCode { get; init; }

    /// <summary>Optional actor that initiated the transition.</summary>
    public string? ActorId { get; init; }

    /// <summary>Optional small opaque key/value metadata.</summary>
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}
