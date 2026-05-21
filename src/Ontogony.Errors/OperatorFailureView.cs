namespace Ontogony.Errors;

/// <summary>Normalized operator-facing failure view derived from cross-service envelopes or adapter inputs.</summary>
/// <param name="Taxonomy">Stable category — see <see cref="OperatorFailureTaxonomyKind"/>.</param>
/// <param name="Title">Short operator banner title.</param>
/// <param name="Message">Human-readable summary (from source envelope when present).</param>
/// <param name="SourceCode">Original machine code from the originating wire shape.</param>
/// <param name="SourceSystem">Originating system identifier (for example <c>allagma</c>, <c>kanon</c>).</param>
/// <param name="Retryable">Whether the operator may retry safely.</param>
/// <param name="Stage">Optional processing stage label.</param>
/// <param name="DownstreamSystem">Optional downstream system when failure wraps another service.</param>
/// <param name="TraceId">Optional distributed trace identifier.</param>
/// <param name="CorrelationId">Optional correlation identifier.</param>
/// <param name="RecommendedActions">Operator guidance lines (non-authoritative).</param>
public sealed record OperatorFailureView(
    string Taxonomy,
    string Title,
    string Message,
    string SourceCode,
    string SourceSystem,
    bool? Retryable = null,
    string? Stage = null,
    string? DownstreamSystem = null,
    string? TraceId = null,
    string? CorrelationId = null,
    IReadOnlyList<string>? RecommendedActions = null);
