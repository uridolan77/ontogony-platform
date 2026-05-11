namespace Ontogony.Security;

/// <summary>
/// Resolves secrets from an in-memory dictionary (typically populated from configuration).
/// </summary>
public sealed class DictionaryServiceSecretResolver : IServiceSecretResolver
{
    private readonly IReadOnlyDictionary<string, string> _secrets;

    public DictionaryServiceSecretResolver(IReadOnlyDictionary<string, string> secrets)
    {
        ArgumentNullException.ThrowIfNull(secrets);
        _secrets = secrets;
    }

    public string? ResolveSecret(string serviceId)
    {
        return _secrets.TryGetValue(serviceId, out var secret) ? secret : null;
    }
}
