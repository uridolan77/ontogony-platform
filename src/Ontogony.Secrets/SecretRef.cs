namespace Ontogony.Secrets;

public sealed record SecretRef(
    string SecretId,
    string? KeyId = null,
    string? Version = null,
    string? Scope = null,
    string? Fingerprint = null,
    string? DisplayName = null);
