using System.Text.RegularExpressions;

namespace Ontogony.Contracts.Events;

/// <summary>
/// A single mechanical validation failure on an <see cref="OntogonyEnvelope{TPayload}"/>.
/// </summary>
/// <param name="Field">Envelope field name (or synthetic key).</param>
/// <param name="Message">Human-readable failure detail.</param>
/// <param name="Code">Optional machine-readable code.</param>
public sealed record EnvelopeValidationError(string Field, string Message, string? Code = null);

/// <summary>
/// Result of <see cref="IEnvelopeValidator.Validate{T}"/>.
/// </summary>
/// <param name="IsValid">True when <see cref="Errors"/> is empty.</param>
/// <param name="Errors">Validation failures when invalid.</param>
public sealed record EnvelopeValidationResult(bool IsValid, IReadOnlyList<EnvelopeValidationError> Errors)
{
    /// <summary>Valid result with no errors.</summary>
    public static EnvelopeValidationResult Ok() => new(true, []);

    /// <summary>Invalid result with one or more errors.</summary>
    public static EnvelopeValidationResult Fail(IReadOnlyList<EnvelopeValidationError> errors) =>
        new(false, errors.Count == 0
            ? [new EnvelopeValidationError("(none)", "Validation failed with no error details.", "empty")]
            : errors);
}

/// <summary>
/// Validates mechanical envelope fields without interpreting payload meaning.
/// </summary>
public interface IEnvelopeValidator
{
    /// <summary>Validates required envelope fields and formats.</summary>
    /// <typeparam name="TPayload">Payload type.</typeparam>
    /// <param name="envelope">Envelope to validate.</param>
    EnvelopeValidationResult Validate<TPayload>(OntogonyEnvelope<TPayload> envelope);
}
