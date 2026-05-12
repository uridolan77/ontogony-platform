namespace Ontogony.Execution;

/// <summary>
/// Mechanical record of a single attempt to execute a step (retries, timeouts, or partial work without policy).
/// </summary>
/// <remarks>
/// <see cref="Status"/> and <see cref="ErrorCode"/> are <b>opaque strings</b>. <see cref="AttemptNumber"/> is caller-defined (often 1-based); Ontogony does not enforce positivity.
/// This type does not validate lifecycle consistency across fields; consumers validate when they need stricter journal semantics.
/// </remarks>
public sealed record ExecutionAttemptRecord
{
    public required string AttemptId { get; init; }
    public required string StepId { get; init; }
    /// <summary>Caller-defined attempt index (e.g. retry count).</summary>
    public int AttemptNumber { get; init; }
    /// <summary>Opaque outcome status for the attempt.</summary>
    public required string Status { get; init; }
    public DateTimeOffset? StartedAt { get; init; }
    public DateTimeOffset? CompletedAt { get; init; }
    public string? ErrorCode { get; init; }
    /// <summary>Optional opaque fingerprint over error detail (how computed is a caller concern).</summary>
    public string? ErrorDetailHash { get; init; }
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}
