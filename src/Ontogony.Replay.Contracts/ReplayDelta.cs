namespace Ontogony.Replay.Contracts;

/// <summary>Replay comparison delta record (contracts only).</summary>
public sealed record ReplayDelta(
    string ReplayId,
    string OriginalRootIdentifier,
    string Mode,
    string Verdict,
    IReadOnlyList<ReplayComparison> Comparisons,
    IReadOnlyList<string>? Uncompared = null,
    IReadOnlyList<string>? Notes = null);
