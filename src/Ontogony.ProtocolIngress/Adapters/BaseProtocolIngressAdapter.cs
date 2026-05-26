using Ontogony.Contracts.Events;
using Ontogony.Hashing;
using Ontogony.Primitives;

namespace Ontogony.ProtocolIngress.Adapters;

/// <summary>
/// Base adapter providing common mechanical normalization utilities.
/// Enforces determinism, proper fallback ordering, and envelope validation.
/// </summary>
public abstract class BaseProtocolIngressAdapter
{
    /// <summary>Payload hasher used for raw and canonical hashes.</summary>
    protected readonly PayloadHasher PayloadHasher;

    /// <summary>Identifier generator for trace and event identifiers.</summary>
    protected readonly IIdGenerator IdGenerator;

    /// <summary>Clock used for timestamp fallback.</summary>
    protected readonly IClock Clock;

    /// <summary>Envelope validator applied before returning success.</summary>
    protected readonly IEnvelopeValidator EnvelopeValidator;

    /// <summary>Initializes base adapter dependencies.</summary>
    protected BaseProtocolIngressAdapter(
        PayloadHasher payloadHasher,
        IIdGenerator idGenerator,
        IClock clock,
        IEnvelopeValidator envelopeValidator)
    {
        PayloadHasher = payloadHasher ?? throw new ArgumentNullException(nameof(payloadHasher));
        IdGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
        Clock = clock ?? throw new ArgumentNullException(nameof(clock));
        EnvelopeValidator = envelopeValidator ?? throw new ArgumentNullException(nameof(envelopeValidator));
    }

    /// <summary>
    /// Returns the first non-whitespace value from the provided arguments.
    /// Used for proper fallback precedence in trace ID and other field resolution.
    /// </summary>
    private static string? FirstNonWhiteSpace(params string?[] values)
    {
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value))
                return value;
        }
        return null;
    }

    /// <summary>
    /// Generates or validates a trace ID according to the ingress policy.
    /// Follows correct precedence: raw → context → generate/fail.
    /// </summary>
    protected string? ValidateOrGenerateTraceId(string? providedTraceId, ProtocolIngressContext context, out ProtocolIngressResult? error)
    {
        error = null;

        // Proper fallback: raw trace -> context trace -> generate/fail
        var traceId = FirstNonWhiteSpace(providedTraceId, context.TraceId);

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
    /// Normalizes a timestamp, applying fallback to context or current UTC time if needed.
    /// Uses injected IClock for determinism.
    /// </summary>
    protected DateTimeOffset NormalizeTimestamp(DateTimeOffset? providedTime, ProtocolIngressContext context)
    {
        if (providedTime.HasValue)
            return providedTime.Value;

        if (context.OccurredAt.HasValue)
            return context.OccurredAt.Value;

        return Clock.UtcNow;  // Use injected clock for determinism
    }

    /// <summary>
    /// Normalizes a source into absolute URI format: {protocol}://{source}.
    /// </summary>
    protected string NormalizeSourceUri(string protocol, string source)
    {
        if (Uri.TryCreate(source, UriKind.Absolute, out _))
            return source;

        return $"{protocol}://{source}";
    }

    /// <summary>
    /// Computes a deterministic hash of the exact raw JSON payload bytes.
    /// </summary>
    protected string ComputeRawPayloadHash(string rawJson)
    {
        return PayloadHasher.ComputeRawJsonHash(rawJson);
    }

    /// <summary>
    /// Computes the canonical JSON hash used for deterministic payload identity.
    /// </summary>
    protected string ComputeCanonicalPayloadHash(string rawJson)
    {
        return PayloadHasher.ComputeCanonicalJsonHash(rawJson);
    }

    /// <summary>
    /// Applies the mechanical ingress event type policy for normalized envelopes.
    /// </summary>
    protected string NormalizeEnvelopeEventType(string protocol)
    {
        return $"{protocol}.ingress.normalized";
    }

    /// <summary>
    /// Validates the constructed envelope against platform contracts and returns success or failure.
    /// All adapters must use this to ensure envelopes meet DefaultEnvelopeValidator requirements.
    /// </summary>
    protected ProtocolIngressResult ValidateAndReturnEnvelope(OntogonyEnvelope<RawProtocolPayload> envelope)
    {
        var validationResult = EnvelopeValidator.Validate(envelope);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new ProtocolIngressError(e.Field, e.Message))
                .ToList();
            return ProtocolIngressResult.Failure(errors.ToArray());
        }

        return ProtocolIngressResult.Success(envelope);
    }
}
