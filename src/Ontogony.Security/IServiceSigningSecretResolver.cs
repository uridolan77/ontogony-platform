namespace Ontogony.Security;

/// <summary>
/// Resolves one or more HMAC signing secrets for a service identity request.
/// </summary>
public interface IServiceSigningSecretResolver
{
    /// <summary>
    /// Resolves signing secrets for a service and optional key-id.
    /// </summary>
    ServiceSigningSecretSet ResolveSecrets(string serviceId, string? keyId);
}
