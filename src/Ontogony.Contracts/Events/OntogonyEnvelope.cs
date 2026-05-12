namespace Ontogony.Contracts.Events;

/// <summary>
/// Protocol-neutral event envelope for cross-service emission and recorder-style pipelines.
/// It is intentionally CloudEvents-compatible in spirit while remaining simple for internal .NET hosts.
/// For strict mechanical validation at ingress, use <see cref="DefaultEnvelopeValidator"/> and the JSON schema under <c>schemas/ontogony-envelope.schema.json</c>.
/// </summary>
public sealed record OntogonyEnvelope<TPayload>
{
    /// <summary>Unique event id for deduplication and tracing.</summary>
    public required string EventId { get; init; }

    /// <summary>Event type (typically <c>protocol.entity.verb</c>).</summary>
    public required string EventType { get; init; }

    /// <summary>Absolute URI identifying the emitting context.</summary>
    public required string Source { get; init; }

    /// <summary>When the event occurred (UTC).</summary>
    public required DateTimeOffset OccurredAt { get; init; }

    /// <summary>Distributed trace id.</summary>
    public required string TraceId { get; init; }

    /// <summary>Optional span id within the trace.</summary>
    public string? SpanId { get; init; }

    /// <summary>Optional parent span id.</summary>
    public string? ParentSpanId { get; init; }

    /// <summary>Optional tenant scope.</summary>
    public string? TenantId { get; init; }

    /// <summary>Optional workspace scope.</summary>
    public string? WorkspaceId { get; init; }

    /// <summary>Optional project scope.</summary>
    public string? ProjectId { get; init; }

    /// <summary>Optional actor id.</summary>
    public string? ActorId { get; init; }

    /// <summary>Optional session id.</summary>
    public string? SessionId { get; init; }

    /// <summary>Opaque protocol discriminator for ingress/recorder routing.</summary>
    public required string Protocol { get; init; }

    /// <summary>Envelope schema version string.</summary>
    public string SchemaVersion { get; init; } = "1.0";

    /// <summary>Typed payload body.</summary>
    public required TPayload Payload { get; init; }

    /// <summary>Optional SHA-256 hex fingerprint of canonical payload bytes.</summary>
    public string? PayloadHash { get; init; }

    /// <summary>Small opaque string metadata map (not large blobs).</summary>
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}
