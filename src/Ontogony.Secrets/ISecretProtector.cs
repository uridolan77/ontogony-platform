namespace Ontogony.Secrets;

public interface ISecretProtector
{
    SecretProtectionResult Protect(string plainText, string? keyId = null);
    string Unprotect(string protectedValue);
}
