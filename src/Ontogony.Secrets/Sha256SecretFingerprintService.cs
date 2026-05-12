using System.Security.Cryptography;
using System.Text;

namespace Ontogony.Secrets;

/// <summary>
/// <see cref="ISecretFingerprintService"/> implementation using SHA-256 over UTF-8 bytes (lowercase hex).
/// </summary>
public sealed class Sha256SecretFingerprintService : ISecretFingerprintService
{
    /// <inheritdoc />
    public string ComputeFingerprint(string secretValue)
    {
        ArgumentNullException.ThrowIfNull(secretValue);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(secretValue));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
