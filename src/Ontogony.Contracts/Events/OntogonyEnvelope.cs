namespace Ontogony.Contracts.Events;

/// <summary>
/// Protocol-neutral event envelope for cross-service emission and recorder-style pipelines.
/// It is intentionally CloudEvents-compatible in spirit while remaining simple for internal .NET hosts.
/// For strict mechanical validation at ingress, use <see cref="DefaultEnvelopeValidator"/> and the JSON schema under <c>schemas/ontogony-envelope.schema.json</c>.
/// </summary>
public sealed record OntogonyEnvelope<TPayload>
{
    public required string EventId { get; init; }
    public required string EventType { get; init; }
    public required string Source { get; init; }
    public required DateTimeOffset OccurredAt { get; init; }

    public required string TraceId { get; init; }
    public string? SpanId { get; init; }
    public string? ParentSpanId { get; init; }

    public string? TenantId { get; init; }
    public string? WorkspaceId { get; init; }
    public string? ProjectId { get; init; }
    public string? ActorId { get; init; }
    public string? SessionId { get; init; }

    public required string Protocol { get; init; }
    public string SchemaVersion { get; init; } = "1.0";

    public required TPayload Payload { get; init; }
    public string? PayloadHash { get; init; }
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}
