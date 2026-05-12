namespace Ontogony.AI.Contracts;

/// <summary>
/// Mechanical snapshot of an LLM response (or terminal error) correlated to <see cref="LlmRequestEnvelope"/>.
/// </summary>
/// <remarks>
/// <see cref="Provider"/> and <see cref="Model"/> are <b>opaque strings</b>; see <see cref="LlmRequestEnvelope"/> remarks.
/// </remarks>
public sealed record LlmResponseEnvelope
{
    /// <summary>Unique id for this response record.</summary>
    public required string ResponseId { get; init; }

    /// <summary>Correlates to <see cref="LlmRequestEnvelope.RequestId"/>.</summary>
    public required string RequestId { get; init; }

    /// <summary>Distributed trace id (should match the request).</summary>
    public required string TraceId { get; init; }
    /// <summary>Opaque provider identifier (no platform enum).</summary>
    public required string Provider { get; init; }
    /// <summary>Opaque model identifier (no platform enum).</summary>
    public required string Model { get; init; }

    /// <summary>Optional fingerprint of completion text or structured output.</summary>
    public string? CompletionHash { get; init; }

    /// <summary>Optional fingerprint of raw provider response bytes or canonical JSON.</summary>
    public string? RawProviderResponseHash { get; init; }

    /// <summary>Opaque provider finish reason string.</summary>
    public string? FinishReason { get; init; }

    /// <summary>Optional token usage breakdown.</summary>
    public LlmUsageRecord? Usage { get; init; }

    /// <summary>Optional cost breakdown.</summary>
    public LlmCostRecord? Cost { get; init; }

    /// <summary>Optional tool calls returned by the model.</summary>
    public IReadOnlyList<ToolCallRecord>? ToolCalls { get; init; }

    /// <summary>When present, indicates terminal provider failure for this response.</summary>
    public LlmProviderError? Error { get; init; }

    /// <summary>When the response was completed (UTC).</summary>
    public DateTimeOffset CompletedAt { get; init; }
}
