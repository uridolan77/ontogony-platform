namespace Ontogony.Replay.Contracts;

/// <summary>Field-level replay comparison entry.</summary>
public sealed record ReplayComparison(
    string Field,
    string Status,
    string? Original = null,
    string? Replay = null);
