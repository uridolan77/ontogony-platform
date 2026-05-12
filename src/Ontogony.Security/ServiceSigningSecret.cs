namespace Ontogony.Security;

/// <summary>
/// One service HMAC signing secret version.
/// </summary>
public sealed record ServiceSigningSecret(string KeyId, string Secret, bool IsCurrent);

/// <summary>
/// Result of resolving service signing secrets for verification.
/// </summary>
public sealed record ServiceSigningSecretSet(
    string ServiceId,
    string? RequestedKeyId,
    ServiceSigningSecret? SelectedSecret,
    IReadOnlyList<ServiceSigningSecret> Secrets)
{
    public static ServiceSigningSecretSet Empty(string serviceId, string? requestedKeyId) =>
        new(serviceId, requestedKeyId, SelectedSecret: null, Secrets: Array.Empty<ServiceSigningSecret>());
}
