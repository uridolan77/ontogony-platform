namespace Ontogony.Execution;

/// <summary>
/// Mechanical record of a durable execution run boundary (opaque kind and status; no planner or workflow semantics).
/// </summary>
/// <remarks>
/// <see cref="RunKind"/> and <see cref="Status"/> are <b>opaque strings</b>. Callers define vocabulary; Ontogony does not interpret them.
/// </remarks>
public sealed record ExecutionRunRecord
{
    public required string RunId { get; init; }
    /// <summary>Opaque run classifier (e.g. host-defined job type); not a platform registry value.</summary>
    public string? RunKind { get; init; }
    /// <summary>Opaque lifecycle status for the run.</summary>
    public required string Status { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? StartedAt { get; init; }
    public DateTimeOffset? CompletedAt { get; init; }
    public string? TraceId { get; init; }
    public string? TenantId { get; init; }
    public string? WorkspaceId { get; init; }
    public string? ProjectId { get; init; }
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}
