namespace Ontogony.Secrets;

/// <summary>
/// Computes stable fingerprints of secret material for comparison without storing plaintext.
/// </summary>
public interface ISecretFingerprintService
{
    /// <summary>Returns a deterministic fingerprint string for <paramref name="secretValue"/>.</summary>
    string ComputeFingerprint(string secretValue);
}
