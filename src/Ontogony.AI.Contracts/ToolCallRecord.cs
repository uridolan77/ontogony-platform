namespace Ontogony.AI.Contracts;

/// <summary>
/// Mechanical record of a tool invocation (names, hashes, timing, status) without orchestration semantics.
/// </summary>
public sealed record ToolCallRecord(
    string ToolCallId,
    string ToolName,
    string? InputHash,
    string? OutputHash,
    string? Status,
    string? ErrorCode,
    DateTimeOffset? StartedAt,
    DateTimeOffset? CompletedAt);
