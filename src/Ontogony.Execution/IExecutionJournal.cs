namespace Ontogony.Execution;

/// <summary>
/// Append-oriented mechanical port for recording execution journal entries (no workflow, scheduler, or lifecycle policy).
/// </summary>
/// <remarks>
/// <para>
/// Implementations reject a second append that reuses an existing primary id (<see cref="ExecutionRunRecord.RunId"/>,
/// <see cref="ExecutionStepRecord.StepId"/>, <see cref="ExecutionAttemptRecord.AttemptId"/>,
/// <see cref="ExecutionStateTransitionRecord.TransitionId"/>, <see cref="ExecutionCheckpointRecord.CheckpointId"/>)
/// with <see cref="InvalidOperationException"/> - journals are not arbitrary upsert stores.
/// </para>
/// <para>
/// List APIs return entries in <b>append order</b> (the order each line was accepted). Callers that need ordering by
/// <see cref="ExecutionCheckpointRecord.Sequence"/> or timestamps should sort explicitly.
/// </para>
/// <para>
/// DTOs do not validate cross-field consistency (timestamps, attempt indices, etc.). Hosts enforce stricter rules when required.
/// </para>
/// </remarks>
public interface IExecutionJournal
{
    Task AppendRunAsync(ExecutionRunRecord run, CancellationToken cancellationToken = default);

    Task AppendStepAsync(ExecutionStepRecord step, CancellationToken cancellationToken = default);

    Task AppendAttemptAsync(ExecutionAttemptRecord attempt, CancellationToken cancellationToken = default);

    Task AppendTransitionAsync(ExecutionStateTransitionRecord transition, CancellationToken cancellationToken = default);

    Task AppendCheckpointAsync(ExecutionCheckpointRecord checkpoint, CancellationToken cancellationToken = default);

    Task<ExecutionRunRecord?> GetRunAsync(string runId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ExecutionStepRecord>> ListStepsAsync(string runId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ExecutionAttemptRecord>> ListAttemptsAsync(string stepId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ExecutionStateTransitionRecord>> ListTransitionsAsync(string runId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ExecutionCheckpointRecord>> ListCheckpointsAsync(string runId, CancellationToken cancellationToken = default);
}
