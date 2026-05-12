namespace Ontogony.AI.Contracts;

/// <summary>
/// Mechanical snapshot of an outbound LLM request (no prompt strategy or routing).
/// </summary>
public sealed record LlmRequestEnvelope
{
    public required string RequestId { get; init; }
    public required string TraceId { get; init; }
    public required string OperationName { get; init; }
    public required string Provider { get; init; }
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
