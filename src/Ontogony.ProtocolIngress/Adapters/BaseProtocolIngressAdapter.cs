using System.Text.Json;
using Ontogony.Contracts.Events;
using Ontogony.Hashing;
using Ontogony.Primitives;

namespace Ontogony.ProtocolIngress.Adapters;

/// <summary>
/// Base adapter providing common mechanical normalization utilities.
/// </summary>
public abstract class BaseProtocolIngressAdapter
{
    protected readonly PayloadHasher PayloadHasher;
    protected readonly IIdGenerator IdGenerator;

    protected BaseProtocolIngressAdapter(PayloadHasher payloadHasher, IIdGenerator idGenerator)
    {
        PayloadHasher = payloadHasher ?? throw new ArgumentNullException(nameof(payloadHasher));
        IdGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
    }

    /// <summary>
    /// Generates or validates a trace ID according to the ingress policy.
    /// Returns the trace ID value if successful, or a failure result if validation fails.
    /// </summary>
    protected string? ValidateOrGenerateTraceId(string? providedTraceId, ProtocolIngressContext context, out ProtocolIngressResult? error)
    {
        error = null;
        var traceId = providedTraceId ?? context.TraceId;

        if (string.IsNullOrWhiteSpace(traceId))
        {
            if (context.IdGenerationPolicy == TraceIdGenerationPolicy.RequireProvided)
            {
                error = ProtocolIngressResult.Failure(nameof(OntogonyEnvelope<object>.TraceId),
                    "TraceId must be provided in the protocol event or context.");
                return null;
            }

            traceId = IdGenerator.NewId("trace");
        }

        return traceId;
    }

    /// <summary>
    /// Normalizes a timestamp, applying fallback to current UTC time if needed.
    /// </summary>
    protected DateTimeOffset NormalizeTimestamp(DateTimeOffset? providedTime, ProtocolIngressContext context, IClock? clock = null)
    {
        if (providedTime.HasValue)
            return providedTime.Value;

        if (context.OccurredAt.HasValue)
            return context.OccurredAt.Value;

        return clock?.UtcNow ?? DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Computes a deterministic hash of a JSON payload.
    /// </summary>
    protected string ComputePayloadHash(string rawJson)
    {
        return PayloadHasher.ComputeCanonicalJsonHash(rawJson);
    }

    /// <summary>
    /// Applies context metadata to the envelope if available.
    /// </summary>
    protected void ApplyContextMetadata(OntogonyEnvelope<RawProtocolPayload> envelope, ProtocolIngressContext context)
    {
        if (context.Metadata == null)
            return;

        var props = typeof(OntogonyEnvelope<RawProtocolPayload>)
            .GetProperties()
            .Where(p => p.Name.StartsWith("TenantId") || p.Name.StartsWith("WorkspaceId") ||
                        p.Name.StartsWith("ProjectId") || p.Name.StartsWith("ActorId") ||
                        p.Name.StartsWith("SessionId"));

        // Note: OntogonyEnvelope uses init properties, so we apply metadata during construction in subclasses.
    }
}
