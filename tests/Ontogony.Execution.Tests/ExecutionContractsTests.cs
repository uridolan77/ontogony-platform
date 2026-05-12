using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Ontogony.Execution;
using Ontogony.Hashing;
using Xunit;

namespace Ontogony.Execution.Tests;

public sealed class ExecutionContractsTests
{
    private static readonly DateTimeOffset FixedInstant = new(2026, 5, 12, 12, 0, 0, TimeSpan.Zero);

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    [Fact]
    public void ExecutionRunRecord_CanonicalJson_is_deterministic()
    {
        var a = SampleRun();
        var b = SampleRun();

        var ja = CanonicalJson.Serialize(a);
        var jb = CanonicalJson.Serialize(b);

        Assert.Equal(ja, jb);
        var roundTrip = JsonSerializer.Deserialize<ExecutionRunRecord>(ja, JsonOptions)!;
        Assert.Equal(ja, CanonicalJson.Serialize(roundTrip));
    }

    [Fact]
    public void ExecutionStepRecord_CanonicalJson_is_deterministic()
    {
        var a = SampleStep();
        var b = SampleStep();

        var ja = CanonicalJson.Serialize(a);
        var jb = CanonicalJson.Serialize(b);

        Assert.Equal(ja, jb);
        var roundTrip = JsonSerializer.Deserialize<ExecutionStepRecord>(ja, JsonOptions)!;
        Assert.Equal(ja, CanonicalJson.Serialize(roundTrip));
    }

    [Fact]
    public void ExecutionAttemptRecord_CanonicalJson_is_deterministic()
    {
        var a = SampleAttempt();
        var b = SampleAttempt();

        var ja = CanonicalJson.Serialize(a);
        var jb = CanonicalJson.Serialize(b);

        Assert.Equal(ja, jb);
        var roundTrip = JsonSerializer.Deserialize<ExecutionAttemptRecord>(ja, JsonOptions)!;
        Assert.Equal(ja, CanonicalJson.Serialize(roundTrip));
    }

    [Fact]
    public void ExecutionStateTransitionRecord_CanonicalJson_is_deterministic()
    {
        var a = SampleTransition();
        var b = SampleTransition();

        var ja = CanonicalJson.Serialize(a);
        var jb = CanonicalJson.Serialize(b);

        Assert.Equal(ja, jb);
        var roundTrip = JsonSerializer.Deserialize<ExecutionStateTransitionRecord>(ja, JsonOptions)!;
        Assert.Equal(ja, CanonicalJson.Serialize(roundTrip));
    }

    [Fact]
    public void ExecutionCheckpointRecord_CanonicalJson_is_deterministic()
    {
        var a = SampleCheckpoint();
        var b = SampleCheckpoint();

        var ja = CanonicalJson.Serialize(a);
        var jb = CanonicalJson.Serialize(b);

        Assert.Equal(ja, jb);
        var roundTrip = JsonSerializer.Deserialize<ExecutionCheckpointRecord>(ja, JsonOptions)!;
        Assert.Equal(ja, CanonicalJson.Serialize(roundTrip));
    }

    [Fact]
    public void Metadata_key_order_does_not_change_canonical_json()
    {
        var r1 = SampleRun() with
        {
            Metadata = new Dictionary<string, string>(StringComparer.Ordinal) { ["z"] = "1", ["a"] = "2" }
        };
        var r2 = SampleRun() with
        {
            Metadata = new Dictionary<string, string>(StringComparer.Ordinal) { ["a"] = "2", ["z"] = "1" }
        };

        Assert.Equal(CanonicalJson.Serialize(r1), CanonicalJson.Serialize(r2));
    }

    [Fact]
    public void ExecutionAttemptRecord_accepts_negative_attempt_number_boundary()
    {
        var a = SampleAttempt() with { AttemptNumber = -1 };
        var json = CanonicalJson.Serialize(a);
        var restored = JsonSerializer.Deserialize<ExecutionAttemptRecord>(json, JsonOptions)!;
        Assert.Equal(-1, restored.AttemptNumber);
        Assert.Equal(json, CanonicalJson.Serialize(restored));
    }

    [Fact]
    public void ExecutionCheckpointRecord_accepts_min_and_max_sequence_boundary()
    {
        var min = SampleCheckpoint() with { Sequence = long.MinValue };
        var max = SampleCheckpoint() with { Sequence = long.MaxValue, CheckpointId = "cp-max" };

        Assert.Equal(long.MinValue,
            JsonSerializer.Deserialize<ExecutionCheckpointRecord>(CanonicalJson.Serialize(min), JsonOptions)!.Sequence);
        Assert.Equal(long.MaxValue,
            JsonSerializer.Deserialize<ExecutionCheckpointRecord>(CanonicalJson.Serialize(max), JsonOptions)!.Sequence);
    }

    [Fact]
    public void Public_contract_surface_has_no_orchestration_tokens_in_names()
    {
        var forbidden = new Regex(
            @"\b(Agentor|Athanor|Conexus|PlanStep|ToolChoice|ReviewPolicy|Canoniz)\b",
            RegexOptions.IgnoreCase);

        foreach (var t in typeof(ExecutionRunRecord).Assembly.GetExportedTypes())
        {
            Assert.False(forbidden.IsMatch(t.Name), t.FullName);
            foreach (var m in t.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                if (m is Type) { continue; }
                Assert.False(forbidden.IsMatch(m.Name), $"{t.Name}.{m.Name}");
            }
        }
    }

    private static ExecutionRunRecord SampleRun() =>
        new()
        {
            RunId = "run-1",
            RunKind = "batch",
            Status = "running",
            CreatedAt = FixedInstant,
            StartedAt = FixedInstant.AddMinutes(1),
            CompletedAt = null,
            TraceId = "trace-abc",
            TenantId = "t1",
            WorkspaceId = "w1",
            ProjectId = "p1",
            Metadata = new Dictionary<string, string> { ["host"] = "test" }
        };

    private static ExecutionStepRecord SampleStep() =>
        new()
        {
            StepId = "step-1",
            RunId = "run-1",
            StepKey = "fetch",
            Status = "pending",
            Ordinal = 0,
            CreatedAt = FixedInstant,
            StartedAt = null,
            CompletedAt = null,
            Metadata = null
        };

    private static ExecutionAttemptRecord SampleAttempt() =>
        new()
        {
            AttemptId = "att-1",
            StepId = "step-1",
            AttemptNumber = 1,
            Status = "succeeded",
            StartedAt = FixedInstant,
            CompletedAt = FixedInstant.AddSeconds(5),
            ErrorCode = null,
            ErrorDetailHash = null,
            Metadata = null
        };

    private static ExecutionStateTransitionRecord SampleTransition() =>
        new()
        {
            TransitionId = "tr-1",
            RunId = "run-1",
            SubjectKind = "run",
            SubjectId = "run-1",
            FromState = "pending",
            ToState = "running",
            OccurredAt = FixedInstant,
            ReasonCode = null,
            ActorId = "actor-1",
            Metadata = null
        };

    private static ExecutionCheckpointRecord SampleCheckpoint() =>
        new()
        {
            CheckpointId = "cp-1",
            RunId = "run-1",
            Sequence = 42,
            RecordedAt = FixedInstant,
            Label = "after-step-a",
            PayloadHash = "sha256:deadbeef",
            ResumeToken = "token-opaque",
            Metadata = null
        };
}
