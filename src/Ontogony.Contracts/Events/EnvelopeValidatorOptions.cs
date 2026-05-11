namespace Ontogony.Contracts.Events;

/// <summary>
/// Options for <see cref="DefaultEnvelopeValidator"/>.
/// </summary>
public sealed class EnvelopeValidatorOptions
{
    /// <summary>
    /// When non-null, <see cref="OntogonyEnvelope{TPayload}.Protocol"/> must be one of these values (ordinal case-insensitive).
    /// When null, any non-empty protocol string is accepted.
    /// </summary>
    public HashSet<string>? AllowedProtocols { get; set; }

    /// <summary>Maximum length of <see cref="OntogonyEnvelope{TPayload}.EventId"/> after trim.</summary>
    public int MaxEventIdLength { get; set; } = 256;

    /// <summary>Maximum length of <see cref="OntogonyEnvelope{TPayload}.TraceId"/> after trim.</summary>
    public int MaxTraceIdLength { get; set; } = 256;
}
