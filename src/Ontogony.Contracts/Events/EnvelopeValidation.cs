using System.Text.RegularExpressions;

namespace Ontogony.Contracts.Events;

/// <summary>
/// A single mechanical validation failure on an <see cref="OntogonyEnvelope{TPayload}"/>.
/// </summary>
public sealed record EnvelopeValidationError(string Field, string Message, string? Code = null);

/// <summary>
/// Result of <see cref="IEnvelopeValidator.Validate{T}"/>.
/// </summary>
public sealed record EnvelopeValidationResult(bool IsValid, IReadOnlyList<EnvelopeValidationError> Errors)
{
    public static EnvelopeValidationResult Ok() => new(true, []);

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
    EnvelopeValidationResult Validate<TPayload>(OntogonyEnvelope<TPayload> envelope);
}
