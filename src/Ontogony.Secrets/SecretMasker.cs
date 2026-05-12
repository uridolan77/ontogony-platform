using Ontogony.Redaction;

namespace Ontogony.Secrets;

/// <summary>
/// Masks raw secret strings and references for safe logging (uses <see cref="IRedactor"/>).
/// </summary>
public sealed class SecretMasker
{
    private readonly IRedactor _redactor;

    /// <summary>Creates a masker using the provided redactor.</summary>
    public SecretMasker(IRedactor redactor)
    {
        _redactor = redactor ?? throw new ArgumentNullException(nameof(redactor));
    }

    /// <summary>Masks a raw secret value, or returns null when input is null.</summary>
    public string? Mask(string? secretValue)
    {
        if (secretValue is null)
        {
            return null;
        }

        return _redactor.RedactString(secretValue, RedactionClassification.Secret).Value;
    }

    /// <summary>Returns a non-sensitive display handle for a <see cref="SecretRef"/>.</summary>
    public string? MaskRef(SecretRef? secretRef)
    {
        if (secretRef is null)
        {
            return null;
        }

        return secretRef.DisplayName ?? secretRef.SecretId;
    }
}
