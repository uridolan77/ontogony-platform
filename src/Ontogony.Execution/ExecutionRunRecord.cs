namespace Ontogony.Execution;

/// <summary>
/// Mechanical record of a durable execution run boundary (opaque kind and status; no planner or workflow semantics).
/// </summary>
/// <remarks>
/// <see cref="RunKind"/> and <see cref="Status"/> are <b>opaque strings</b>. Callers define vocabulary; Ontogony does not interpret them.
/// This type does not validate lifecycle consistency across fields; consumers validate when they need stricter journal semantics.
/// </remarks>
public sealed record ExecutionRunRecord
{
    /// <summary>Stable unique id for this run line in the journal.</summary>
    public required string RunId { get; init; }
    /// <summary>Opaque run classifier (e.g. host-defined job type); not a platform registry value.</summary>
    public string? RunKind { get; init; }
    /// <summary>Opaque lifecycle status for the run.</summary>
    public required string Status { get; init; }
    /// <summary>When the run record was created.</summary>
    public required DateTimeOffset CreatedAt { get; init; }

    /// <summary>When execution started, if known.</summary>
    public DateTimeOffset? StartedAt { get; init; }

    /// <summary>When execution completed, if known.</summary>
    public DateTimeOffset? CompletedAt { get; init; }

    /// <summary>Optional distributed trace id for correlation.</summary>
    public string? TraceId { get; init; }

    /// <summary>Optional tenant scope identifier.</summary>
    public string? TenantId { get; init; }

    /// <summary>Optional workspace scope identifier.</summary>
    public string? WorkspaceId { get; init; }

    /// <summary>Optional project scope identifier.</summary>
    public string? ProjectId { get; init; }

    /// <summary>Optional small opaque key/value metadata.</summary>
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}
