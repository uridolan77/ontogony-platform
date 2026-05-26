using Ontogony.Errors;

namespace Ontogony.Testing.Conformance;

/// <summary>
/// Conformance helpers for <see cref="CrossServiceErrorEnvelope"/> (PLATFORM-9-002).
/// </summary>
public static class CrossServiceErrorEnvelopeConformanceAssertions
{
    /// <summary>Validates required neutral envelope fields.</summary>
    public static void AssertValidShape(CrossServiceErrorEnvelope envelope, string? expectedSystem = null)
    {
        ArgumentNullException.ThrowIfNull(envelope);

        if (string.IsNullOrWhiteSpace(envelope.Code))
            throw new InvalidOperationException("CrossServiceErrorEnvelope.Code must be set.");

        if (!envelope.Code.Contains('.', StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"CrossServiceErrorEnvelope.Code '{envelope.Code}' must be namespaced (contain '.').");
        }

        if (string.IsNullOrWhiteSpace(envelope.Message))
            throw new InvalidOperationException("CrossServiceErrorEnvelope.Message must be set.");

        if (string.IsNullOrWhiteSpace(envelope.System))
            throw new InvalidOperationException("CrossServiceErrorEnvelope.System must be set.");

        if (expectedSystem is not null &&
            !string.Equals(envelope.System, expectedSystem, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Expected system '{expectedSystem}' but was '{envelope.System}'.");
        }
    }
}
