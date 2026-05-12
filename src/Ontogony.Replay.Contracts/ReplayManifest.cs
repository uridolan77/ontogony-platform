namespace Ontogony.Replay.Contracts;

public sealed record ReplayManifest(
    string ReplayId,
    string SourceRunId,
    string CreatedAt,
    string? TraceId = null,
    ReplayEnvironmentSnapshot? Environment = null,
    ReplayDeterminismHints? Determinism = null,
    IReadOnlyList<ReplayInputRef>? Inputs = null,
    IReadOnlyList<ReplayOutputRef>? ExpectedOutputs = null,
    IReadOnlyList<ReplayStepRecord>? Steps = null,
    IReadOnlyDictionary<string, string>? Metadata = null);
