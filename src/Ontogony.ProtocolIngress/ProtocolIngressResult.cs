using Ontogony.Contracts.Events;

namespace Ontogony.ProtocolIngress;

/// <summary>
/// Result of normalizing a raw protocol event into an OntogonyEnvelope.
/// Contains either a validated envelope or a set of normalization errors.
/// </summary>
public sealed record ProtocolIngressResult
{
    /// <summary>
    /// Gets a value indicating whether normalization succeeded.
    /// </summary>
    public bool IsSuccess => Envelope != null && (Errors == null || Errors.Count == 0);

    /// <summary>
    /// Gets the normalized OntogonyEnvelope if normalization succeeded.
    /// </summary>
    public OntogonyEnvelope<RawProtocolPayload>? Envelope { get; init; }

    /// <summary>
    /// Gets the list of normalization errors if normalization failed.
    /// </summary>
    public IReadOnlyList<ProtocolIngressError> Errors { get; init; } = new List<ProtocolIngressError>();

    /// <summary>
    /// Creates a successful result with a normalized envelope.
    /// </summary>
    public static ProtocolIngressResult Success(OntogonyEnvelope<RawProtocolPayload> envelope)
    {
        ArgumentNullException.ThrowIfNull(envelope);
        return new ProtocolIngressResult { Envelope = envelope };
    }

    /// <summary>
    /// Creates a failure result with one or more errors.
    /// </summary>
    public static ProtocolIngressResult Failure(params ProtocolIngressError[] errors)
    {
        ArgumentNullException.ThrowIfNull(errors);
        if (errors.Length == 0)
            throw new ArgumentException("At least one error must be provided", nameof(errors));
        return new ProtocolIngressResult { Errors = errors.ToList() };
    }

    /// <summary>
    /// Creates a failure result with a single error.
    /// </summary>
    public static ProtocolIngressResult Failure(string field, string message)
    {
        ArgumentNullException.ThrowIfNull(field);
        ArgumentNullException.ThrowIfNull(message);
        return Failure(new ProtocolIngressError(field, message));
    }
}

/// <summary>
/// Represents a single normalization error.
/// </summary>
public sealed record ProtocolIngressError(string Field, string Message)
{
    /// <summary>
    /// Gets the field name where the error occurred (e.g., "eventType", "source", "traceId").
    /// </summary>
    public string Field { get; } = Field ?? throw new ArgumentNullException(nameof(Field));

    /// <summary>
    /// Gets the human-readable error message.
    /// </summary>
    public string Message { get; } = Message ?? throw new ArgumentNullException(nameof(Message));
}
