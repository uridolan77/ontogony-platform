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
    /// Gets the raw payload parsed as dynamic object (if available after parsing).
    /// May be null if parsing failed during initial ingestion.
    /// </summary>
    public object? ParsedObject { get; init; }

    /// <summary>
    /// Gets the deterministic hash of the raw payload for deduplication and integrity verification.
    /// </summary>
    public string? PayloadHash { get; init; }
}
