using Ontogony.Redaction;

namespace Ontogony.Secrets;

public sealed class SecretMasker
{
    private readonly IRedactor _redactor;

    public SecretMasker(IRedactor redactor)
    {
        _redactor = redactor ?? throw new ArgumentNullException(nameof(redactor));
    }

    public string? Mask(string? secretValue)
    {
        if (secretValue is null)
        {
            return null;
        }

        return _redactor.RedactString(secretValue, RedactionClassification.Secret).Value;
    }

    public string? MaskRef(SecretRef? secretRef)
    {
        if (secretRef is null)
        {
            return null;
        }

        return secretRef.DisplayName ?? secretRef.SecretId;
    }
}
