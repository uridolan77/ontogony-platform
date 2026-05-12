namespace Ontogony.AI.Contracts;

/// <summary>
/// Mechanical snapshot of an outbound LLM request (no prompt strategy or routing).
/// </summary>
/// <remarks>
/// <see cref="Provider"/> and <see cref="Model"/> are <b>opaque strings</b>. Ontogony does not define a provider registry,
/// ranking, or model-selection policy; callers use whatever identifiers their host or product already uses.
/// </remarks>
public sealed record LlmRequestEnvelope
{
    /// <summary>Unique id for this request record.</summary>
    public required string RequestId { get; init; }

    /// <summary>Distributed trace id correlating logs and downstream calls.</summary>
    public required string TraceId { get; init; }

    /// <summary>Opaque operation or handler name from the caller.</summary>
    public required string OperationName { get; init; }
    /// <summary>Opaque provider identifier (no platform enum).</summary>
    public required string Provider { get; init; }
    /// <summary>Opaque model identifier (no platform enum).</summary>
    public required string Model { get; init; }

    /// <summary>Optional tenant scope.</summary>
    public string? TenantId { get; init; }

    /// <summary>Optional workspace scope.</summary>
    public string? WorkspaceId { get; init; }

    /// <summary>Optional project scope.</summary>
    public string? ProjectId { get; init; }

    /// <summary>Optional fingerprint of prompt material (how computed is caller-defined).</summary>
    public string? PromptHash { get; init; }

    /// <summary>Optional fingerprint of raw provider request bytes or canonical JSON.</summary>
    public string? RawProviderRequestHash { get; init; }

    /// <summary>Optional small opaque parameters (temperature, max_tokens, etc. as strings).</summary>
    public IReadOnlyDictionary<string, string>? Parameters { get; init; }

    /// <summary>Optional tool call requests attached to the prompt.</summary>
    public IReadOnlyList<ToolCallRecord>? RequestedTools { get; init; }

    /// <summary>When the envelope was created (UTC).</summary>
    public DateTimeOffset CreatedAt { get; init; }
}
