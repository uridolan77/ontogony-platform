namespace Ontogony.Replay.Contracts;

/// <summary>Single service attempt recorded during replay.</summary>
public sealed record ReplayServiceAttempt(
    string Service,
    string Operation,
    string Target,
    string Status,
    string Mode,
    string? Route = null,
    int? DurationMs = null,
    IReadOnlyList<ReplayEvidenceReference>? EvidenceRefs = null,
    string? SkippedReason = null);
