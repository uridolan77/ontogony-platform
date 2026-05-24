namespace Ontogony.Replay.Contracts;

public sealed record ReplayRuntimeEvidenceBundle(
    string Schema,
    ReplayRuntimeRequest Request,
    ReplayTarget RootTarget,
    ReplayRuntimeResult Result,
    ReplayDelta? Delta = null,
    IReadOnlyList<ReplayServiceAttempt>? ServiceAttempts = null,
    IReadOnlyDictionary<string, string>? RuntimeMetadata = null,
    IReadOnlyDictionary<string, string>? RedactionMetadata = null);
