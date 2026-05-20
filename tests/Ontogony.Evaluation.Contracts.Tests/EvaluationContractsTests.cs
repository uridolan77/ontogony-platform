using System.Text.Json;
using Ontogony.Evaluation.Contracts;
using Ontogony.Hashing;
using Xunit;

namespace Ontogony.Evaluation.Contracts.Tests;

public sealed class EvaluationContractsTests
{
    [Fact]
    public void RunRecord_serializes_deterministically()
    {
        var run = SampleRun();

        var first = CanonicalJson.Serialize(run);
        var second = CanonicalJson.Serialize(run);

        Assert.Equal(first, second);
        Assert.Contains("eval-run-1", first);
    }

    [Fact]
    public void RunRecord_round_trips_through_json()
    {
        var original = SampleRun();
        var json = JsonSerializer.Serialize(original);
        var restored = JsonSerializer.Deserialize<EvaluationRunRecord>(json);

        Assert.NotNull(restored);
        Assert.Equal(original.EvaluationRunId, restored.EvaluationRunId);
        Assert.Equal(original.SubjectRunId, restored.SubjectRunId);
        Assert.Equal(original.Verdict?.Verdict, restored.Verdict?.Verdict);
        Assert.Equal(original.Scores?.Count, restored.Scores?.Count);
        Assert.Equal(original.Artifacts?[0].ProtocolId, restored.Artifacts?[0].ProtocolId);
        Assert.Equal(original.Artifacts?[0].AuthorityMode, restored.Artifacts?[0].AuthorityMode);
        Assert.Equal(original.Artifacts?[0].SideEffectLevel, restored.Artifacts?[0].SideEffectLevel);
    }

    [Fact]
    public void Minimal_run_record_allows_null_optional_fields()
    {
        var minimal = new EvaluationRunRecord("eval-run-min");

        Assert.Null(minimal.SubjectRunId);
        Assert.Null(minimal.Verdict);
        Assert.Null(minimal.Scores);

        var json = CanonicalJson.Serialize(minimal);
        Assert.Contains("eval-run-min", json);
    }

    [Fact]
    public void BaselineComparisonRecord_has_stable_canonical_hash()
    {
        var comparison = new BaselineComparisonRecord(
            ComparisonId: "cmp-1",
            SubjectRunId: "run-subject",
            BaselineRunId: "run-baseline",
            ScenarioId: "summarize-player-risk-baseline",
            BaselineMode: "single_workflow",
            QualityDelta: 0.05m,
            CostDeltaUsd: -0.12m,
            LatencyDeltaMs: 120,
            PolicyEquivalent: true,
            SideEffectSafe: true,
            PromotionRecommendation: "keep_single_workflow");

        var hasher = new PayloadHasher(new Sha256ContentHashService());
        var first = hasher.ComputeCanonicalJsonHash(comparison);
        var second = hasher.ComputeCanonicalJsonHash(comparison);

        Assert.Equal(first, second);
        Assert.False(string.IsNullOrWhiteSpace(first));
    }

    [Fact]
    public void ScoreRecord_allows_null_score_and_threshold()
    {
        var score = new EvaluationScoreRecord(
            MetricName: "trace_complete",
            MetricKind: "boolean",
            Passed: true,
            Reason: "trace linked across services");

        Assert.Null(score.Score);
        Assert.Null(score.Threshold);
        Assert.True(score.Passed);
    }

    [Fact]
    public void ArtifactRef_can_carry_runtime_protocol_metadata()
    {
        var artifact = new EvaluationArtifactRef(
            ArtifactId: "artifact-1",
            Role: "run_audit_bundle",
            LocatorUri: "/allagma/v0/runs/run-1/audit",
            ProtocolId: "allagma.run.start.v1",
            AuthorityMode: "authoritative",
            SideEffectLevel: "run_state_transition");

        Assert.Equal("allagma.run.start.v1", artifact.ProtocolId);
        Assert.Equal("authoritative", artifact.AuthorityMode);
        Assert.Equal("run_state_transition", artifact.SideEffectLevel);
    }

    private static EvaluationRunRecord SampleRun() =>
        new(
            EvaluationRunId: "eval-run-1",
            SubjectRunId: "run-1",
            TraceId: "trace-1",
            ScenarioId: "summarize-player-risk-baseline",
            EvaluationProfileId: "profile-v0",
            StartedAtUtc: new DateTimeOffset(2026, 5, 18, 12, 0, 0, TimeSpan.Zero),
            CompletedAtUtc: new DateTimeOffset(2026, 5, 18, 12, 1, 0, TimeSpan.Zero),
            Verdict: new EvaluationVerdictRecord("pass", QualityScore: 0.92m),
            Scores:
            [
                new EvaluationScoreRecord(
                    MetricName: "policy_correct",
                    MetricKind: "boolean",
                    Score: 1m,
                    Passed: true,
                    Threshold: 1m,
                    SourceRef: "decision:kanon-1")
            ],
            Artifacts:
            [
                new EvaluationArtifactRef(
                    "artifact-1",
                    ContentHash: "hash-1",
                    Role: "eval-summary",
                    ProtocolId: "allagma.system.cohesion.evidence.v1",
                    AuthorityMode: "simulation_only",
                    SideEffectLevel: "evidence_record")
            ],
            Metadata: new Dictionary<string, string> { ["harness"] = "agm-eval-v0" });
}
