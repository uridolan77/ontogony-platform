namespace Ontogony.Secrets;

public sealed record SecretProtectionResult(
    string ProtectedValue,
    string ProtectionScheme,
    string? KeyId = null);
