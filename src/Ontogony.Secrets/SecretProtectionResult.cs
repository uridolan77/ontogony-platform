namespace Ontogony.Secrets;

/// <summary>
/// Result of <see cref="ISecretProtector.Protect"/>.
/// </summary>
/// <param name="ProtectedValue">Opaque protected payload suitable for configuration storage.</param>
/// <param name="ProtectionScheme">Scheme name (e.g. <see cref="DevelopmentBase64SecretProtector.Scheme"/>).</param>
/// <param name="KeyId">Optional key or version identifier used by the protector.</param>
public sealed record SecretProtectionResult(
    string ProtectedValue,
    string ProtectionScheme,
    string? KeyId = null);
