namespace Ontogony.Replay.Contracts;

public sealed record ReplayComparison(
    string Field,
    string Status,
    string? Original = null,
    string? Replay = null);
