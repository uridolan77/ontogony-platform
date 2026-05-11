namespace Ontogony.Security;

/// <summary>
/// Resolves the shared secret used to verify a calling service identity.
/// </summary>
public interface IServiceSecretResolver
{
    /// <summary>Returns the UTF-8 secret material for HMAC or static modes, or null if unknown.</summary>
    string? ResolveSecret(string serviceId);
}
