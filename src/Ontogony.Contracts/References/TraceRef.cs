namespace Ontogony.Contracts.References;

/// <summary>
/// Reference to a distributed trace context. Captured for traceability and debugging.
/// </summary>
public sealed record TraceRef(
    string TraceId,
    string? SpanId = null,
    string? ParentSpanId = null,
    string? Sampled = null);
