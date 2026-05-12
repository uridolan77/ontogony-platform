using Microsoft.Extensions.Logging;

namespace Ontogony.Logging;

/// <summary>
/// Stable mechanical event IDs for structured logs.
/// </summary>
public static class OntogonyLogEvents
{
    public static readonly EventId RequestStarted = new(1000, nameof(RequestStarted));
    public static readonly EventId RequestCompleted = new(1001, nameof(RequestCompleted));
    public static readonly EventId RequestFailed = new(1002, nameof(RequestFailed));

    public static readonly EventId OutboundCallStarted = new(1100, nameof(OutboundCallStarted));
    public static readonly EventId OutboundCallCompleted = new(1101, nameof(OutboundCallCompleted));
    public static readonly EventId OutboundCallFailed = new(1102, nameof(OutboundCallFailed));

    public static readonly EventId AiRequestStarted = new(2000, nameof(AiRequestStarted));
    public static readonly EventId AiRequestCompleted = new(2001, nameof(AiRequestCompleted));
    public static readonly EventId AiRequestFailed = new(2002, nameof(AiRequestFailed));

    public static readonly EventId ArtifactStored = new(3000, nameof(ArtifactStored));
    public static readonly EventId IdempotencyDuplicate = new(4000, nameof(IdempotencyDuplicate));
    public static readonly EventId QuotaRejected = new(5000, nameof(QuotaRejected));
    public static readonly EventId SecretAccessed = new(6000, nameof(SecretAccessed));
}
