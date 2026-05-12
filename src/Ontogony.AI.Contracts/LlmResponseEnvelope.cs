namespace Ontogony.AI.Contracts;

/// <summary>
/// Mechanical snapshot of an LLM response (or terminal error) correlated to <see cref="LlmRequestEnvelope"/>.
/// </summary>
/// <remarks>
/// <see cref="Provider"/> and <see cref="Model"/> are <b>opaque strings</b>; see <see cref="LlmRequestEnvelope"/> remarks.
/// </remarks>
public sealed record LlmResponseEnvelope
{
    public required string ResponseId { get; init; }
    public required string RequestId { get; init; }
    public required string TraceId { get; init; }
    /// <summary>Opaque provider identifier (no platform enum).</summary>
    public required string Provider { get; init; }
    /// <summary>Opaque model identifier (no platform enum).</summary>
    public required string Model { get; init; }
    public string? CompletionHash { get; init; }
    public string? RawProviderResponseHash { get; init; }
    public string? FinishReason { get; init; }
    public LlmUsageRecord? Usage { get; init; }
    public LlmCostRecord? Cost { get; init; }
    public IReadOnlyList<ToolCallRecord>? ToolCalls { get; init; }
    public LlmProviderError? Error { get; init; }
    public DateTimeOffset CompletedAt { get; init; }
}
