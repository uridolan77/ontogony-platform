using System.Text;

namespace Ontogony.Secrets;

/// <summary>
/// Development-only protector. This is reversible Base64 with a marker and must not be used as production encryption.
/// </summary>
public sealed class DevelopmentBase64SecretProtector : ISecretProtector
{
    public const string Scheme = "dev-base64";
    private const string Prefix = "dev:v1:";

    public SecretProtectionResult Protect(string plainText, string? keyId = null)
    {
        ArgumentNullException.ThrowIfNull(plainText);
        var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        return new SecretProtectionResult(Prefix + encoded, Scheme, keyId);
    }

    public string Unprotect(string protectedValue)
    {
        ArgumentNullException.ThrowIfNull(protectedValue);
        if (!protectedValue.StartsWith(Prefix, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Secret value was not protected by the development protector.");
        }

        var encoded = protectedValue[Prefix.Length..];
        return Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
    }
}
