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
    public required string TransitionId { get; init; }
    public required string RunId { get; init; }
    /// <summary>Opaque classifier for what <see cref="SubjectId"/> refers to (e.g. "run", "step").</summary>
    public required string SubjectKind { get; init; }
    public required string SubjectId { get; init; }
    public required string FromState { get; init; }
    public required string ToState { get; init; }
    public required DateTimeOffset OccurredAt { get; init; }
    public string? ReasonCode { get; init; }
    public string? ActorId { get; init; }
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}
