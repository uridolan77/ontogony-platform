using System.Collections.Frozen;
using System.Text.RegularExpressions;

namespace Ontogony.Contracts.Events;

/// <summary>
/// Default <see cref="IEnvelopeValidator"/> enforcing mechanical rules aligned with <c>schemas/ontogony-envelope.schema.json</c>.
/// </summary>
public sealed partial class DefaultEnvelopeValidator : IEnvelopeValidator
{
    private readonly EnvelopeValidatorOptions _options;
    private readonly FrozenSet<string>? _allowedProtocols;

    public DefaultEnvelopeValidator(EnvelopeValidatorOptions? options = null)
    {
        _options = options ?? new EnvelopeValidatorOptions();
        _allowedProtocols = _options.AllowedProtocols is { Count: > 0 } set
            ? FrozenSet.ToFrozenSet(set, StringComparer.OrdinalIgnoreCase)
            : null;
    }

    /// <summary>
    /// Suggested protocol identifiers (matches JSON schema <c>enum</c>). Runtime validation uses
    /// <see cref="EnvelopeValidatorOptions.AllowedProtocols"/> only when configured.
    /// </summary>
    public static IReadOnlyList<string> SuggestedProtocolIdentifiers { get; } =
    [
        ProtocolNames.AgUi,
        ProtocolNames.Mcp,
        ProtocolNames.A2A,
        ProtocolNames.OpenTelemetry,
        ProtocolNames.CloudEvents,
        ProtocolNames.Conexus,
        ProtocolNames.Agentor,
        ProtocolNames.Athanor,
        ProtocolNames.VercelAiSdk,
        ProtocolNames.OpenAiAgents,
        ProtocolNames.LangGraph
    ];

    public EnvelopeValidationResult Validate<TPayload>(OntogonyEnvelope<TPayload> envelope)
    {
        ArgumentNullException.ThrowIfNull(envelope);

        var errors = new List<EnvelopeValidationError>();

        if (string.IsNullOrWhiteSpace(envelope.EventId))
            errors.Add(new EnvelopeValidationError(nameof(envelope.EventId), "EventId is required.", "required"));
        else if (!SafeTokenRegex().IsMatch(envelope.EventId.Trim()) || envelope.EventId.Trim().Length > _options.MaxEventIdLength)
            errors.Add(new EnvelopeValidationError(nameof(envelope.EventId), "EventId must be a non-empty safe token (alphanumeric, colon, underscore, dot, slash, hyphen) within max length.", "format"));

        if (string.IsNullOrWhiteSpace(envelope.EventType))
            errors.Add(new EnvelopeValidationError(nameof(envelope.EventType), "EventType is required.", "required"));
        else if (!EventTypeRegex().IsMatch(envelope.EventType.Trim()))
            errors.Add(new EnvelopeValidationError(nameof(envelope.EventType), "EventType must match protocol.entity.verb (three dot-separated segments; lowercase letters, digits, hyphen within each segment).", "format"));

        if (string.IsNullOrWhiteSpace(envelope.Source))
            errors.Add(new EnvelopeValidationError(nameof(envelope.Source), "Source is required.", "required"));
        else if (!Uri.TryCreate(envelope.Source.Trim(), UriKind.Absolute, out _))
            errors.Add(new EnvelopeValidationError(nameof(envelope.Source), "Source must be an absolute URI.", "format"));

        if (envelope.OccurredAt == default)
            errors.Add(new EnvelopeValidationError(nameof(envelope.OccurredAt), "OccurredAt must not be default.", "required"));

        if (string.IsNullOrWhiteSpace(envelope.TraceId))
            errors.Add(new EnvelopeValidationError(nameof(envelope.TraceId), "TraceId is required.", "required"));
        else if (!SafeTokenRegex().IsMatch(envelope.TraceId.Trim()) || envelope.TraceId.Trim().Length > _options.MaxTraceIdLength)
            errors.Add(new EnvelopeValidationError(nameof(envelope.TraceId), "TraceId must be a non-empty safe token within max length.", "format"));

        if (string.IsNullOrWhiteSpace(envelope.Protocol))
            errors.Add(new EnvelopeValidationError(nameof(envelope.Protocol), "Protocol is required.", "required"));
        else if (_allowedProtocols is not null && !_allowedProtocols.Contains(envelope.Protocol.Trim()))
            errors.Add(new EnvelopeValidationError(nameof(envelope.Protocol), "Protocol is not in the configured allowed list.", "allowed_values"));

        if (string.IsNullOrWhiteSpace(envelope.SchemaVersion))
            errors.Add(new EnvelopeValidationError(nameof(envelope.SchemaVersion), "SchemaVersion is required.", "required"));

        if (typeof(TPayload).IsClass && envelope.Payload is null)
            errors.Add(new EnvelopeValidationError(nameof(envelope.Payload), "Payload must not be null for reference types.", "required"));

        if (!string.IsNullOrWhiteSpace(envelope.PayloadHash) && !IsValidPayloadHash(envelope.PayloadHash.Trim()))
            errors.Add(new EnvelopeValidationError(nameof(envelope.PayloadHash), "PayloadHash must be 64 lowercase hex digits (SHA-256) when present.", "format"));

        return errors.Count == 0 ? EnvelopeValidationResult.Ok() : EnvelopeValidationResult.Fail(errors);
    }

    private static bool IsValidPayloadHash(string hash) => PayloadHashRegex().IsMatch(hash);

    [GeneratedRegex("^[a-f0-9]{64}$", RegexOptions.Compiled)]
    private static partial Regex PayloadHashRegex();

    [GeneratedRegex("^[a-z0-9][a-z0-9-]{0,63}\\.[a-z0-9][a-z0-9-]{0,63}\\.[a-z0-9][a-z0-9-]{0,63}$", RegexOptions.Compiled)]
    private static partial Regex EventTypeRegex();

    [GeneratedRegex("^[a-zA-Z0-9:_./-]+$", RegexOptions.Compiled)]
    private static partial Regex SafeTokenRegex();
}
