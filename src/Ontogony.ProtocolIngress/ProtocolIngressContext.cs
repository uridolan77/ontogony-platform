namespace Ontogony.ProtocolIngress;

/// <summary>
/// Context information provided when normalizing a raw protocol event.
/// Carries options for trace ID generation and other mechanical behavior.
/// </summary>
public sealed record ProtocolIngressContext
{
    /// <summary>
    /// Gets or initializes the trace ID that should be assigned to the normalized envelope.
    /// If null, a new trace ID will be generated according to IdGenerationPolicy.
    /// </summary>
    public string? TraceId { get; init; }

    /// <summary>
    /// Gets or initializes the optional span ID for distributed tracing.
    /// </summary>
    public string? SpanId { get; init; }

    /// <summary>
    /// Gets or initializes the optional parent span ID for distributed tracing.
    /// </summary>
    public string? ParentSpanId { get; init; }

    /// <summary>
    /// Gets or initializes the policy for generating trace IDs when none is provided.
    /// </summary>
    public TraceIdGenerationPolicy IdGenerationPolicy { get; init; } = TraceIdGenerationPolicy.RequireProvided;

    /// <summary>
    /// Gets or initializes optional context fields for the envelope (tenant, workspace, actor, etc).
    /// </summary>
    public ProtocolIngressContextMetadata? Metadata { get; init; }

    /// <summary>
    /// Gets or initializes the timestamp to use for the normalized envelope.
    /// If null, the raw event's timestamp will be used or DateTimeOffset.UtcNow as fallback.
    /// </summary>
    public DateTimeOffset? OccurredAt { get; init; }
}

/// <summary>
/// Optional context metadata that enriches the normalized envelope.
/// </summary>
public sealed record ProtocolIngressContextMetadata
{
    /// <summary>Optional tenant identifier.</summary>
    public string? TenantId { get; init; }

    /// <summary>Optional workspace identifier.</summary>
    public string? WorkspaceId { get; init; }

    /// <summary>Optional project identifier.</summary>
    public string? ProjectId { get; init; }

    /// <summary>Optional actor identifier.</summary>
    public string? ActorId { get; init; }

    /// <summary>Optional session identifier.</summary>
    public string? SessionId { get; init; }
}

/// <summary>
/// Policy for assigning trace IDs to normalized envelopes when none is provided.
/// </summary>
public enum TraceIdGenerationPolicy
{
    /// <summary>
    /// Trace ID must be provided in the raw event or context. Normalization will fail if not present.
    /// </summary>
    RequireProvided = 0,

    /// <summary>
    /// Generate a new trace ID if not provided in raw event or context.
    /// </summary>
    GenerateIfMissing = 1
}
