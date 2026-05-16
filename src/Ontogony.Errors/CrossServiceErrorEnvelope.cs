namespace Ontogony.Errors;

/// <summary>
/// Neutral cross-service error envelope for HTTP clients and downstream failure mapping.
/// Product repos use namespaced <see cref="Code"/> values; platform owns only the shape.
/// </summary>
/// <param name="Code">Machine-readable error code (namespaced by product repo).</param>
/// <param name="Message">Human-readable summary safe for clients.</param>
/// <param name="System">Originating system identifier (for example <c>allagma</c>, <c>kanon</c>).</param>
/// <param name="Stage">Optional processing stage label.</param>
/// <param name="DownstreamSystem">Optional downstream system when this error wraps another service.</param>
/// <param name="TraceId">Optional distributed trace identifier.</param>
/// <param name="CorrelationId">Optional correlation identifier.</param>
/// <param name="Retryable">Whether the client may retry the operation.</param>
/// <param name="Detail">Optional structured detail payload.</param>
public sealed record CrossServiceErrorEnvelope(
    string Code,
    string Message,
    string System,
    string? Stage = null,
    string? DownstreamSystem = null,
    string? TraceId = null,
    string? CorrelationId = null,
    bool? Retryable = null,
    object? Detail = null);
