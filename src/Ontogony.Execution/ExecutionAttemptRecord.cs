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
    /// <summary>Stable unique id for this attempt line in the journal.</summary>
    public required string AttemptId { get; init; }

    /// <summary>Parent step id this attempt belongs to.</summary>
    public required string StepId { get; init; }
    /// <summary>Caller-defined attempt index (e.g. retry count).</summary>
    public int AttemptNumber { get; init; }
    /// <summary>Opaque outcome status for the attempt.</summary>
    public required string Status { get; init; }
    /// <summary>When the attempt started, if known.</summary>
    public DateTimeOffset? StartedAt { get; init; }

    /// <summary>When the attempt finished, if known.</summary>
    public DateTimeOffset? CompletedAt { get; init; }

    /// <summary>Opaque error classifier when <see cref="Status"/> indicates failure.</summary>
    public string? ErrorCode { get; init; }
    /// <summary>Optional opaque fingerprint over error detail (how computed is a caller concern).</summary>
    public string? ErrorDetailHash { get; init; }
    /// <summary>Optional small opaque key/value metadata (not prompts or large payloads).</summary>
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}
