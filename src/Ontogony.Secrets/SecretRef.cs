namespace Ontogony.Secrets;

/// <summary>
/// Reference to a secret stored out-of-line (vault, env, etc.); does not embed the secret value.
/// </summary>
/// <param name="SecretId">Stable secret identifier.</param>
/// <param name="KeyId">Optional key version within a vault.</param>
/// <param name="Version">Optional opaque version string.</param>
/// <param name="Scope">Optional scope discriminator (environment, region, etc.).</param>
/// <param name="Fingerprint">Optional fingerprint of the secret material for drift detection.</param>
/// <param name="DisplayName">Optional non-secret label for logs.</param>
public sealed record SecretRef(
    string SecretId,
    string? KeyId = null,
    string? Version = null,
    string? Scope = null,
    string? Fingerprint = null,
    string? DisplayName = null);
