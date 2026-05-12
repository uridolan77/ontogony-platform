namespace Ontogony.Security;

/// <summary>
/// One service HMAC signing secret version.
/// </summary>
/// <param name="KeyId">Key version identifier.</param>
/// <param name="Secret">Raw secret material.</param>
/// <param name="IsCurrent">True when this is the preferred signing key for outbound calls.</param>
public sealed record ServiceSigningSecret(string KeyId, string Secret, bool IsCurrent);

/// <summary>
/// Result of resolving service signing secrets for verification.
/// </summary>
/// <param name="ServiceId">Service id that was resolved.</param>
/// <param name="RequestedKeyId">Key id from the request, if any.</param>
/// <param name="SelectedSecret">Secret selected for verification, if any.</param>
/// <param name="Secrets">All configured secrets for the service.</param>
public sealed record ServiceSigningSecretSet(
    string ServiceId,
    string? RequestedKeyId,
    ServiceSigningSecret? SelectedSecret,
    IReadOnlyList<ServiceSigningSecret> Secrets)
{
    /// <summary>Empty set when no secrets match.</summary>
    public static ServiceSigningSecretSet Empty(string serviceId, string? requestedKeyId) =>
        new(serviceId, requestedKeyId, SelectedSecret: null, Secrets: Array.Empty<ServiceSigningSecret>());
}
