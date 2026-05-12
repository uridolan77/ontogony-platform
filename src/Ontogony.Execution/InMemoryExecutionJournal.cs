using System.Collections.Generic;

namespace Ontogony.Execution;

/// <summary>
/// Thread-safe in-memory <see cref="IExecutionJournal"/> reference implementation for tests, examples, and single-process hosts.
/// Not durable and not suitable as a multi-node source of truth.
/// </summary>
public sealed class InMemoryExecutionJournal : IExecutionJournal
{
    private readonly object _gate = new();
    private readonly Dictionary<string, ExecutionRunRecord> _runs = new(StringComparer.Ordinal);
    private readonly Dictionary<string, List<ExecutionStepRecord>> _stepsByRun = new(StringComparer.Ordinal);
    private readonly Dictionary<string, List<ExecutionAttemptRecord>> _attemptsByStep = new(StringComparer.Ordinal);
    private readonly Dictionary<string, List<ExecutionStateTransitionRecord>> _transitionsByRun = new(StringComparer.Ordinal);
    private readonly Dictionary<string, List<ExecutionCheckpointRecord>> _checkpointsByRun = new(StringComparer.Ordinal);

    /// <inheritdoc />
    public Task AppendRunAsync(ExecutionRunRecord run, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(run);
        ThrowIfBlank(run.RunId, nameof(run.RunId));
        cancellationToken.ThrowIfCancellationRequested();

        lock (_gate)
        {
            if (_runs.ContainsKey(run.RunId))
            {
                throw new InvalidOperationException($"A run with RunId '{run.RunId}' is already recorded.");
            }

            _runs.Add(run.RunId, run);
            _stepsByRun.TryAdd(run.RunId, new List<ExecutionStepRecord>());
            _transitionsByRun.TryAdd(run.RunId, new List<ExecutionStateTransitionRecord>());
            _checkpointsByRun.TryAdd(run.RunId, new List<ExecutionCheckpointRecord>());
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task AppendStepAsync(ExecutionStepRecord step, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(step);
        ThrowIfBlank(step.StepId, nameof(step.StepId));
        ThrowIfBlank(step.RunId, nameof(step.RunId));
        cancellationToken.ThrowIfCancellationRequested();

        lock (_gate)
        {
            foreach (var list in _stepsByRun.Values)
            {
                foreach (var existing in list)
                {
                    if (string.Equals(existing.StepId, step.StepId, StringComparison.Ordinal))
                    {
                        throw new InvalidOperationException($"A step with StepId '{step.StepId}' is already recorded.");
                    }
                }
            }

            if (!_stepsByRun.TryGetValue(step.RunId, out var steps))
            {
                steps = new List<ExecutionStepRecord>();
                _stepsByRun[step.RunId] = steps;
            }

            steps.Add(step);
            _attemptsByStep.TryAdd(step.StepId, new List<ExecutionAttemptRecord>());
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task AppendAttemptAsync(ExecutionAttemptRecord attempt, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(attempt);
        ThrowIfBlank(attempt.AttemptId, nameof(attempt.AttemptId));
        ThrowIfBlank(attempt.StepId, nameof(attempt.StepId));
        cancellationToken.ThrowIfCancellationRequested();

        lock (_gate)
        {
            foreach (var list in _attemptsByStep.Values)
            {
                foreach (var existing in list)
                {
                    if (string.Equals(existing.AttemptId, attempt.AttemptId, StringComparison.Ordinal))
                    {
                        throw new InvalidOperationException($"An attempt with AttemptId '{attempt.AttemptId}' is already recorded.");
                    }
                }
            }

            if (!_attemptsByStep.TryGetValue(attempt.StepId, out var attempts))
            {
                attempts = new List<ExecutionAttemptRecord>();
                _attemptsByStep[attempt.StepId] = attempts;
            }

            attempts.Add(attempt);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task AppendTransitionAsync(ExecutionStateTransitionRecord transition, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(transition);
        ThrowIfBlank(transition.TransitionId, nameof(transition.TransitionId));
        ThrowIfBlank(transition.RunId, nameof(transition.RunId));
        cancellationToken.ThrowIfCancellationRequested();

        lock (_gate)
        {
            foreach (var list in _transitionsByRun.Values)
            {
                foreach (var existing in list)
                {
                    if (string.Equals(existing.TransitionId, transition.TransitionId, StringComparison.Ordinal))
                    {
                        throw new InvalidOperationException(
                            $"A transition with TransitionId '{transition.TransitionId}' is already recorded.");
                    }
                }
            }

            if (!_transitionsByRun.TryGetValue(transition.RunId, out var transitions))
            {
                transitions = new List<ExecutionStateTransitionRecord>();
                _transitionsByRun[transition.RunId] = transitions;
            }

            transitions.Add(transition);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task AppendCheckpointAsync(ExecutionCheckpointRecord checkpoint, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(checkpoint);
        ThrowIfBlank(checkpoint.CheckpointId, nameof(checkpoint.CheckpointId));
        ThrowIfBlank(checkpoint.RunId, nameof(checkpoint.RunId));
        cancellationToken.ThrowIfCancellationRequested();

        lock (_gate)
        {
            foreach (var list in _checkpointsByRun.Values)
            {
                foreach (var existing in list)
                {
                    if (string.Equals(existing.CheckpointId, checkpoint.CheckpointId, StringComparison.Ordinal))
                    {
                        throw new InvalidOperationException(
                            $"A checkpoint with CheckpointId '{checkpoint.CheckpointId}' is already recorded.");
                    }
                }
            }

            if (!_checkpointsByRun.TryGetValue(checkpoint.RunId, out var checkpoints))
            {
                checkpoints = new List<ExecutionCheckpointRecord>();
                _checkpointsByRun[checkpoint.RunId] = checkpoints;
            }

            checkpoints.Add(checkpoint);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<ExecutionRunRecord?> GetRunAsync(string runId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfBlank(runId, nameof(runId));

        lock (_gate)
        {
            return Task.FromResult(_runs.TryGetValue(runId, out var r) ? r : null);
        }
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<ExecutionStepRecord>> ListStepsAsync(string runId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfBlank(runId, nameof(runId));

        lock (_gate)
        {
            if (!_stepsByRun.TryGetValue(runId, out var steps))
            {
                return Task.FromResult<IReadOnlyList<ExecutionStepRecord>>(Array.Empty<ExecutionStepRecord>());
            }

            return Task.FromResult<IReadOnlyList<ExecutionStepRecord>>(steps.ToArray());
        }
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<ExecutionAttemptRecord>> ListAttemptsAsync(string stepId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfBlank(stepId, nameof(stepId));

        lock (_gate)
        {
            if (!_attemptsByStep.TryGetValue(stepId, out var attempts))
            {
                return Task.FromResult<IReadOnlyList<ExecutionAttemptRecord>>(Array.Empty<ExecutionAttemptRecord>());
            }

            return Task.FromResult<IReadOnlyList<ExecutionAttemptRecord>>(attempts.ToArray());
        }
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<ExecutionStateTransitionRecord>> ListTransitionsAsync(
        string runId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfBlank(runId, nameof(runId));

        lock (_gate)
        {
            if (!_transitionsByRun.TryGetValue(runId, out var transitions))
            {
                return Task.FromResult<IReadOnlyList<ExecutionStateTransitionRecord>>(
                    Array.Empty<ExecutionStateTransitionRecord>());
            }

            return Task.FromResult<IReadOnlyList<ExecutionStateTransitionRecord>>(transitions.ToArray());
        }
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<ExecutionCheckpointRecord>> ListCheckpointsAsync(
        string runId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfBlank(runId, nameof(runId));

        lock (_gate)
        {
            if (!_checkpointsByRun.TryGetValue(runId, out var checkpoints))
            {
                return Task.FromResult<IReadOnlyList<ExecutionCheckpointRecord>>(Array.Empty<ExecutionCheckpointRecord>());
            }

            return Task.FromResult<IReadOnlyList<ExecutionCheckpointRecord>>(checkpoints.ToArray());
        }
    }

    private static void ThrowIfBlank(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", paramName);
        }
    }
}
