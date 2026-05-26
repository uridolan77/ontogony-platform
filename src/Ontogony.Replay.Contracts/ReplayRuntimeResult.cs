namespace Ontogony.Replay.Contracts;

/// <summary>Replay runtime result boundary record.</summary>
public sealed record ReplayRuntimeResult(
    string ReplayId,
    string RootIdentifier,
    ReplayTarget Target,
    string Mode,
    string Status,
    string Verdict,
    ReplaySafetyPosture SafetyPosture,
    DateTimeOffset StartedAt,
    DateTimeOffset? CompletedAt = null,
    string? LegacyMode = null,
    IReadOnlyList<ReplayServiceAttempt>? ServiceAttempts = null,
    IReadOnlyList<ReplayEvidenceReference>? OriginalEvidenceRefs = null,
    IReadOnlyList<ReplayEvidenceReference>? ReplayEvidenceRefs = null,
    string? DeltaRef = null,
    string? BundleRef = null);
