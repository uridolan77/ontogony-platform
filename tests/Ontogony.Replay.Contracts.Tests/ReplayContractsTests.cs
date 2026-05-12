using Ontogony.Hashing;
using Ontogony.Replay.Contracts;
using Xunit;

namespace Ontogony.Replay.Contracts.Tests;

public sealed class ReplayContractsTests
{
    [Fact]
    public void Manifest_serializes_deterministically()
    {
        var manifest = new ReplayManifest(
            ReplayId: "replay-1",
            SourceRunId: "run-1",
            CreatedAt: "2026-05-12T12:00:00Z",
            TraceId: "trace-1",
            Environment: new ReplayEnvironmentSnapshot("conexus", "0.1.0"),
            Determinism: new ReplayDeterminismHints(Seed: "seed-1"),
            Inputs: [new ReplayInputRef("request", "llm-request", new ReplayArtifactRef("artifact-1", "hash-1"))]);

        var first = CanonicalJson.Serialize(manifest);
        var second = CanonicalJson.Serialize(manifest);

        Assert.Equal(first, second);
        Assert.Contains("replay-1", first);
    }
}
