namespace Ontogony.Replay.Contracts;

/// <summary>Manifest for a deterministic replay or debug bundle (contracts only). Temporal values use <see cref="DateTimeOffset"/>.</summary>
public sealed record ReplayManifest(
    string ReplayId,
    string SourceRunId,
    DateTimeOffset CreatedAt,
    string? TraceId = null,
    ReplayEnvironmentSnapshot? Environment = null,
    ReplayDeterminismHints? Determinism = null,
    IReadOnlyList<ReplayInputRef>? Inputs = null,
    IReadOnlyList<ReplayOutputRef>? ExpectedOutputs = null,
    IReadOnlyList<ReplayStepRecord>? Steps = null,
    IReadOnlyDictionary<string, string>? Metadata = null);
