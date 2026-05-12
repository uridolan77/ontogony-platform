namespace Ontogony.ProtocolIngress;

/// <summary>
/// Represents the raw, unparsed payload from a protocol event.
/// Preserves the original JSON or serialized form for reproducibility and audit purposes.
/// </summary>
public sealed record RawProtocolPayload
{
    /// <summary>
    /// Gets the protocol identifier (e.g., "cloudevents", "mcp", "a2a", "ag-ui", "generic-json").
    /// </summary>
    public required string Protocol { get; init; }

    /// <summary>
    /// Gets the raw payload as a JSON string.
    /// </summary>
    public required string RawJson { get; init; }

    /// <summary>
    /// Gets the original protocol-specific event type before ingress normalization.
    /// This preserves protocol meaning while the envelope EventType remains mechanical.
    /// </summary>
    public string? RawEventType { get; init; }

    /// <summary>
    /// Gets the raw payload parsed as a transient object for in-memory processing.
    /// This field is intentionally non-durable and is excluded from serialization.
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public object? ParsedObject { get; init; }

    /// <summary>
    /// Gets the deterministic SHA-256 hash computed over the exact raw payload bytes.
    /// </summary>
    public string? RawPayloadHash { get; init; }

    /// <summary>
    /// Gets the deterministic SHA-256 hash computed over canonical JSON.
    /// </summary>
    public string? CanonicalPayloadHash { get; init; }

    /// <summary>
    /// Backward-compatible alias for <see cref="CanonicalPayloadHash"/>.
    /// </summary>
    [Obsolete("Use CanonicalPayloadHash. This alias will be removed in a future version.")]
    public string? PayloadHash { get; init; }
}
