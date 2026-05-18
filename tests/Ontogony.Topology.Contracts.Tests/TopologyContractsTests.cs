using System.Text.Json;
using Ontogony.Hashing;
using Ontogony.Topology.Contracts;
using Xunit;

namespace Ontogony.Topology.Contracts.Tests;

public sealed class TopologyContractsTests
{
    [Fact]
    public void Standard_topology_mode_constants_are_stable()
    {
        Assert.Equal("single_workflow", StandardTopologyModes.SingleWorkflow);
        Assert.Equal("centralized_orchestrator", StandardTopologyModes.CentralizedOrchestrator);
        Assert.Equal("parallel_review", StandardTopologyModes.ParallelReview);
        Assert.Equal("decentralized_research", StandardTopologyModes.DecentralizedResearch);
        Assert.Equal("hybrid_validation", StandardTopologyModes.HybridValidation);
        Assert.Equal("deny_or_human_gate_first", StandardTopologyModes.DenyOrHumanGateFirst);
    }

    [Fact]
    public void Standard_task_classification_constants_are_stable()
    {
        Assert.Equal("sequential", StandardTaskClassifications.Sequential);
        Assert.Equal("parallelizable", StandardTaskClassifications.Parallelizable);
        Assert.Equal("exploratory", StandardTaskClassifications.Exploratory);
        Assert.Equal("tool_heavy", StandardTaskClassifications.ToolHeavy);
        Assert.Equal("high_risk", StandardTaskClassifications.HighRisk);
        Assert.Equal("human_gate_likely", StandardTaskClassifications.HumanGateLikely);
        Assert.Equal("model_only", StandardTaskClassifications.ModelOnly);
        Assert.Equal("policy_only", StandardTaskClassifications.PolicyOnly);
    }

    [Fact]
    public void Classification_record_serializes_deterministically()
    {
        var record = SampleClassification();

        var first = CanonicalJson.Serialize(record);
        var second = CanonicalJson.Serialize(record);

        Assert.Equal(first, second);
        Assert.Contains("taskcls-1", first);
    }

    [Fact]
    public void Classification_record_round_trips_through_json()
    {
        var original = SampleClassification();
        var json = JsonSerializer.Serialize(original);
        var restored = JsonSerializer.Deserialize<TaskClassificationRecord>(json);

        Assert.NotNull(restored);
        Assert.Equal(original.TaskClassificationId, restored.TaskClassificationId);
        Assert.Equal(original.Classification, restored.Classification);
        Assert.Equal(original.Signals?.Count, restored.Signals?.Count);
    }

    [Fact]
    public void Topology_selection_record_round_trips_through_json()
    {
        var original = SampleSelection();
        var json = JsonSerializer.Serialize(original);
        var restored = JsonSerializer.Deserialize<TopologySelectionRecord>(json);

        Assert.NotNull(restored);
        Assert.Equal(original.TopologySelectionId, restored.TopologySelectionId);
        Assert.Equal(original.SelectedTopology, restored.SelectedTopology);
        Assert.Equal(original.RequiresKanonAuthorization, restored.RequiresKanonAuthorization);
    }

    [Fact]
    public void Minimal_selection_record_allows_null_optional_fields()
    {
        var minimal = new TopologySelectionRecord(
            TopologySelectionId: "toposel-min",
            RunId: "run-min",
            SelectedTopology: StandardTopologyModes.SingleWorkflow,
            SelectorVersion: "topology-selector-v0");

        Assert.Null(minimal.Reason);
        Assert.Null(minimal.RequiresKanonAuthorization);

        var json = CanonicalJson.Serialize(minimal);
        Assert.Contains("toposel-min", json);
    }

    [Fact]
    public void Topology_constraint_record_has_stable_canonical_hash()
    {
        var constraint = new TopologyConstraintRecord(
            ConstraintId: "topocon-1",
            TopologyMode: StandardTopologyModes.CentralizedOrchestrator,
            ValidationMode: "centralized_final_validation",
            RequiresHumanGate: false,
            RequiresBaselineComparison: true,
            BaselineMode: "single_workflow",
            AllowedTopologyModes: [StandardTopologyModes.CentralizedOrchestrator],
            Summary: "Centralized validation required for consequential workflow.");

        var hasher = new PayloadHasher(new Sha256ContentHashService());
        var first = hasher.ComputeCanonicalJsonHash(constraint);
        var second = hasher.ComputeCanonicalJsonHash(constraint);

        Assert.Equal(first, second);
        Assert.False(string.IsNullOrWhiteSpace(first));
    }

    private static TaskClassificationRecord SampleClassification() =>
        new(
            TaskClassificationId: "taskcls-1",
            RunId: "run-1",
            Classification: StandardTaskClassifications.Sequential,
            RiskLevel: "low",
            Confidence: 0.82m,
            Signals: ["no_consequential_marker", "single_objective"],
            ClassifierVersion: "task-topology-v0",
            Metadata: new Dictionary<string, string> { ["source"] = "agm-topo-v0" });

    private static TopologySelectionRecord SampleSelection() =>
        new(
            TopologySelectionId: "toposel-1",
            RunId: "run-1",
            SelectedTopology: StandardTopologyModes.SingleWorkflow,
            SelectorVersion: "topology-selector-v0",
            Reason: "Default topology for sequential low-risk task",
            RequiresKanonAuthorization: false,
            TaskClassificationId: "taskcls-1",
            Classification: StandardTaskClassifications.Sequential,
            RiskLevel: "low",
            Confidence: 0.82m);
}
