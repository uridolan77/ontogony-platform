namespace Ontogony.Secrets;

/// <summary>
/// Protects and unprotects secret strings at rest for development/tests (not a cloud HSM).
/// </summary>
public interface ISecretProtector
{
    /// <summary>Protects plaintext, returning an opaque stored form and scheme metadata.</summary>
    SecretProtectionResult Protect(string plainText, string? keyId = null);

    /// <summary>Reverses <see cref="Protect"/> for the same scheme.</summary>
    string Unprotect(string protectedValue);
}
