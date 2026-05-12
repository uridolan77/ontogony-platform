using Microsoft.Extensions.Logging;

namespace Ontogony.Logging;

/// <summary>
/// Stable mechanical event IDs for structured logs.
/// </summary>
public static class OntogonyLogEvents
{
    /// <summary>HTTP request started.</summary>
    public static readonly EventId RequestStarted = new(1000, nameof(RequestStarted));

    /// <summary>HTTP request completed.</summary>
    public static readonly EventId RequestCompleted = new(1001, nameof(RequestCompleted));

    /// <summary>HTTP request failed.</summary>
    public static readonly EventId RequestFailed = new(1002, nameof(RequestFailed));

    /// <summary>Outbound integration call started.</summary>
    public static readonly EventId OutboundCallStarted = new(1100, nameof(OutboundCallStarted));

    /// <summary>Outbound integration call completed.</summary>
    public static readonly EventId OutboundCallCompleted = new(1101, nameof(OutboundCallCompleted));

    /// <summary>Outbound integration call failed.</summary>
    public static readonly EventId OutboundCallFailed = new(1102, nameof(OutboundCallFailed));

    /// <summary>AI request started.</summary>
    public static readonly EventId AiRequestStarted = new(2000, nameof(AiRequestStarted));

    /// <summary>AI request completed.</summary>
    public static readonly EventId AiRequestCompleted = new(2001, nameof(AiRequestCompleted));

    /// <summary>AI request failed.</summary>
    public static readonly EventId AiRequestFailed = new(2002, nameof(AiRequestFailed));

    /// <summary>Artifact stored successfully.</summary>
    public static readonly EventId ArtifactStored = new(3000, nameof(ArtifactStored));

    /// <summary>Duplicate idempotency key detected.</summary>
    public static readonly EventId IdempotencyDuplicate = new(4000, nameof(IdempotencyDuplicate));

    /// <summary>Mechanical quota rejection.</summary>
    public static readonly EventId QuotaRejected = new(5000, nameof(QuotaRejected));

    /// <summary>Secret material accessed (metadata only).</summary>
    public static readonly EventId SecretAccessed = new(6000, nameof(SecretAccessed));
}
