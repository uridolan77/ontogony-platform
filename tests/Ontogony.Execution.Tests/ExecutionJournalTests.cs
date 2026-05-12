using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ontogony.Execution.Tests;

public sealed class ExecutionJournalTests
{
    private static readonly DateTimeOffset T0 = new(2026, 5, 12, 10, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task Append_and_GetRun_round_trips()
    {
        var journal = new InMemoryExecutionJournal();
        var run = new ExecutionRunRecord
        {
            RunId = "r1",
            RunKind = "job",
            Status = "running",
            CreatedAt = T0,
            TraceId = "t-1"
        };

        await journal.AppendRunAsync(run);

        var got = await journal.GetRunAsync("r1");
        Assert.NotNull(got);
        Assert.Equal("r1", got.RunId);
        Assert.Equal("running", got.Status);
        Assert.Equal("t-1", got.TraceId);
    }

    [Fact]
    public async Task ListSteps_returns_append_order_for_run()
    {
        var journal = new InMemoryExecutionJournal();
        await journal.AppendRunAsync(MinRun("r1"));

        var s1 = MinStep("r1", "s1", ordinal: 1);
        var s2 = MinStep("r1", "s2", ordinal: 0);
        await journal.AppendStepAsync(s1);
        await journal.AppendStepAsync(s2);

        var list = await journal.ListStepsAsync("r1");
        Assert.Equal(2, list.Count);
        Assert.Equal("s1", list[0].StepId);
        Assert.Equal("s2", list[1].StepId);
    }

    [Fact]
    public async Task ListAttempts_returns_append_order_for_step()
    {
        var journal = new InMemoryExecutionJournal();
        await journal.AppendRunAsync(MinRun("r1"));
        await journal.AppendStepAsync(MinStep("r1", "st1", ordinal: 0));

        await journal.AppendAttemptAsync(MinAttempt("st1", "a1", attemptNumber: 1));
        await journal.AppendAttemptAsync(MinAttempt("st1", "a2", attemptNumber: 2));

        var list = await journal.ListAttemptsAsync("st1");
        Assert.Equal(2, list.Count);
        Assert.Equal("a1", list[0].AttemptId);
        Assert.Equal(1, list[0].AttemptNumber);
        Assert.Equal("a2", list[1].AttemptId);
    }

    [Fact]
    public async Task ListTransitions_returns_append_order_for_run()
    {
        var journal = new InMemoryExecutionJournal();
        await journal.AppendRunAsync(MinRun("r1"));

        await journal.AppendTransitionAsync(MinTransition("r1", "tr1", "pending", "running"));
        await journal.AppendTransitionAsync(MinTransition("r1", "tr2", "running", "succeeded"));

        var list = await journal.ListTransitionsAsync("r1");
        Assert.Equal(2, list.Count);
        Assert.Equal("tr1", list[0].TransitionId);
        Assert.Equal("tr2", list[1].TransitionId);
    }

    [Fact]
    public async Task ListCheckpoints_returns_append_order_even_if_Sequence_out_of_band()
    {
        var journal = new InMemoryExecutionJournal();
        await journal.AppendRunAsync(MinRun("r1"));

        await journal.AppendCheckpointAsync(MinCheckpoint("r1", "c1", sequence: 100));
        await journal.AppendCheckpointAsync(MinCheckpoint("r1", "c2", sequence: 5));

        var list = await journal.ListCheckpointsAsync("r1");
        Assert.Equal(2, list.Count);
        Assert.Equal("c1", list[0].CheckpointId);
        Assert.Equal(100, list[0].Sequence);
        Assert.Equal("c2", list[1].CheckpointId);
        Assert.Equal(5, list[1].Sequence);
    }

    [Fact]
    public async Task Duplicate_RunId_throws_InvalidOperationException()
    {
        var journal = new InMemoryExecutionJournal();
        await journal.AppendRunAsync(MinRun("r1"));

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => journal.AppendRunAsync(MinRun("r1")));
        Assert.Contains("r1", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Duplicate_StepId_throws_even_across_runs()
    {
        var journal = new InMemoryExecutionJournal();
        await journal.AppendRunAsync(MinRun("r1"));
        await journal.AppendRunAsync(MinRun("r2"));
        await journal.AppendStepAsync(MinStep("r1", "same-step", ordinal: 0));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            journal.AppendStepAsync(MinStep("r2", "same-step", ordinal: 0)));
    }

    [Fact]
    public async Task Duplicate_AttemptId_throws_even_across_steps()
    {
        var journal = new InMemoryExecutionJournal();
        await journal.AppendRunAsync(MinRun("r1"));
        await journal.AppendStepAsync(MinStep("r1", "st1", ordinal: 0));
        await journal.AppendStepAsync(MinStep("r1", "st2", ordinal: 1));
        await journal.AppendAttemptAsync(MinAttempt("st1", "dup", attemptNumber: 1));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            journal.AppendAttemptAsync(MinAttempt("st2", "dup", attemptNumber: 1)));
    }

    [Fact]
    public async Task Duplicate_TransitionId_throws()
    {
        var journal = new InMemoryExecutionJournal();
        await journal.AppendRunAsync(MinRun("r1"));
        await journal.AppendRunAsync(MinRun("r2"));
        await journal.AppendTransitionAsync(MinTransition("r1", "tr-dup", "a", "b"));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            journal.AppendTransitionAsync(MinTransition("r2", "tr-dup", "a", "c")));
    }

    [Fact]
    public async Task Duplicate_CheckpointId_throws()
    {
        var journal = new InMemoryExecutionJournal();
        await journal.AppendRunAsync(MinRun("r1"));
        await journal.AppendRunAsync(MinRun("r2"));
        await journal.AppendCheckpointAsync(MinCheckpoint("r1", "cp-dup", sequence: 1));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            journal.AppendCheckpointAsync(MinCheckpoint("r2", "cp-dup", sequence: 2)));
    }

    [Fact]
    public async Task ListSteps_unknown_run_returns_empty()
    {
        var journal = new InMemoryExecutionJournal();
        var list = await journal.ListStepsAsync("missing");
        Assert.Empty(list);
    }

    [Fact]
    public async Task ListAttempts_unknown_step_returns_empty()
    {
        var journal = new InMemoryExecutionJournal();
        var list = await journal.ListAttemptsAsync("missing");
        Assert.Empty(list);
    }

    [Fact]
    public void AddOntogonyInMemoryExecutionJournal_resolves_IExecutionJournal()
    {
        var services = new ServiceCollection();
        services.AddOntogonyInMemoryExecutionJournal();
        var sp = services.BuildServiceProvider();
        var journal = sp.GetRequiredService<IExecutionJournal>();
        Assert.IsType<InMemoryExecutionJournal>(journal);
    }

    private static ExecutionRunRecord MinRun(string runId) =>
        new()
        {
            RunId = runId,
            Status = "running",
            CreatedAt = T0
        };

    private static ExecutionStepRecord MinStep(string runId, string stepId, int ordinal) =>
        new()
        {
            StepId = stepId,
            RunId = runId,
            StepKey = "k",
            Status = "pending",
            Ordinal = ordinal,
            CreatedAt = T0
        };

    private static ExecutionAttemptRecord MinAttempt(string stepId, string attemptId, int attemptNumber) =>
        new()
        {
            AttemptId = attemptId,
            StepId = stepId,
            AttemptNumber = attemptNumber,
            Status = "ok",
            StartedAt = T0
        };

    private static ExecutionStateTransitionRecord MinTransition(string runId, string transitionId, string from, string to) =>
        new()
        {
            TransitionId = transitionId,
            RunId = runId,
            SubjectKind = "run",
            SubjectId = runId,
            FromState = from,
            ToState = to,
            OccurredAt = T0
        };

    private static ExecutionCheckpointRecord MinCheckpoint(string runId, string checkpointId, long sequence) =>
        new()
        {
            CheckpointId = checkpointId,
            RunId = runId,
            Sequence = sequence,
            RecordedAt = T0
        };
}
