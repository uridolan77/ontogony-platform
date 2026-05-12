using System.Security.Cryptography;
using System.Text;

namespace Ontogony.Secrets;

public sealed class Sha256SecretFingerprintService : ISecretFingerprintService
{
    public string ComputeFingerprint(string secretValue)
    {
        ArgumentNullException.ThrowIfNull(secretValue);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(secretValue));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
