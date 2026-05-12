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
    /// <summary>Appends a run record; rejects duplicate <see cref="ExecutionRunRecord.RunId"/>.</summary>
    /// <param name="run">Run line to append.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AppendRunAsync(ExecutionRunRecord run, CancellationToken cancellationToken = default);

    /// <summary>Appends a step record; rejects duplicate <see cref="ExecutionStepRecord.StepId"/>.</summary>
    /// <param name="step">Step line to append.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AppendStepAsync(ExecutionStepRecord step, CancellationToken cancellationToken = default);

    /// <summary>Appends an attempt record; rejects duplicate <see cref="ExecutionAttemptRecord.AttemptId"/>.</summary>
    /// <param name="attempt">Attempt line to append.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AppendAttemptAsync(ExecutionAttemptRecord attempt, CancellationToken cancellationToken = default);

    /// <summary>Appends a state transition; rejects duplicate <see cref="ExecutionStateTransitionRecord.TransitionId"/>.</summary>
    /// <param name="transition">Transition line to append.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AppendTransitionAsync(ExecutionStateTransitionRecord transition, CancellationToken cancellationToken = default);

    /// <summary>Appends a checkpoint; rejects duplicate <see cref="ExecutionCheckpointRecord.CheckpointId"/>.</summary>
    /// <param name="checkpoint">Checkpoint line to append.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AppendCheckpointAsync(ExecutionCheckpointRecord checkpoint, CancellationToken cancellationToken = default);

    /// <summary>Returns the run with the given id, or <c>null</c> if none.</summary>
    /// <param name="runId">Run id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<ExecutionRunRecord?> GetRunAsync(string runId, CancellationToken cancellationToken = default);

    /// <summary>Lists steps for a run in append order.</summary>
    /// <param name="runId">Run id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IReadOnlyList<ExecutionStepRecord>> ListStepsAsync(string runId, CancellationToken cancellationToken = default);

    /// <summary>Lists attempts for a step in append order.</summary>
    /// <param name="stepId">Step id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IReadOnlyList<ExecutionAttemptRecord>> ListAttemptsAsync(string stepId, CancellationToken cancellationToken = default);

    /// <summary>Lists state transitions for a run in append order.</summary>
    /// <param name="runId">Run id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IReadOnlyList<ExecutionStateTransitionRecord>> ListTransitionsAsync(string runId, CancellationToken cancellationToken = default);

    /// <summary>Lists checkpoints for a run in append order.</summary>
    /// <param name="runId">Run id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IReadOnlyList<ExecutionCheckpointRecord>> ListCheckpointsAsync(string runId, CancellationToken cancellationToken = default);
}
