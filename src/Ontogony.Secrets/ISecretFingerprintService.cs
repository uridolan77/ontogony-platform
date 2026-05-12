namespace Ontogony.Secrets;

public interface ISecretFingerprintService
{
    string ComputeFingerprint(string secretValue);
}
