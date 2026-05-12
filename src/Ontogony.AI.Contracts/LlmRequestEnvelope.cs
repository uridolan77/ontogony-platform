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
    public required string RequestId { get; init; }
    public required string TraceId { get; init; }
    public required string OperationName { get; init; }
    /// <summary>Opaque provider identifier (no platform enum).</summary>
    public required string Provider { get; init; }
    /// <summary>Opaque model identifier (no platform enum).</summary>
    public required string Model { get; init; }
    public string? TenantId { get; init; }
    public string? WorkspaceId { get; init; }
    public string? ProjectId { get; init; }
    public string? PromptHash { get; init; }
    public string? RawProviderRequestHash { get; init; }
    public IReadOnlyDictionary<string, string>? Parameters { get; init; }
    public IReadOnlyList<ToolCallRecord>? RequestedTools { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}
