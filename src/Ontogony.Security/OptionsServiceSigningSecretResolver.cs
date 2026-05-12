namespace Ontogony.Security;

/// <summary>
/// Default signing secret resolver backed by <see cref="ServiceIdentityOptions.ServiceSigningSecrets"/>
/// with compatibility fallback to <see cref="IServiceSecretResolver"/>.
/// </summary>
public sealed class OptionsServiceSigningSecretResolver : IServiceSigningSecretResolver
{
    /// <summary>Synthetic key id used when synthesizing a secret from legacy flat configuration.</summary>
    public const string LegacyCurrentKeyId = "legacy-current";

    private readonly ServiceIdentityOptions _options;
    private readonly IServiceSecretResolver? _legacySecretResolver;

    /// <summary>Creates the resolver with optional legacy flat-secret fallback.</summary>
    public OptionsServiceSigningSecretResolver(
        ServiceIdentityOptions options,
        IServiceSecretResolver? legacySecretResolver = null)
    {
        _options = options;
        _legacySecretResolver = legacySecretResolver;
    }

    /// <inheritdoc />
    public ServiceSigningSecretSet ResolveSecrets(string serviceId, string? keyId)
    {
        ArgumentNullException.ThrowIfNull(serviceId);

        if (_options.ServiceSigningSecrets.TryGetValue(serviceId, out var configuredSecrets)
            && configuredSecrets is { Count: > 0 })
        {
            var selected = string.IsNullOrWhiteSpace(keyId)
                ? configuredSecrets.FirstOrDefault(static s => s.IsCurrent)
                : configuredSecrets.FirstOrDefault(s => string.Equals(s.KeyId, keyId, StringComparison.Ordinal));

            return new ServiceSigningSecretSet(serviceId, keyId, selected, configuredSecrets);
        }

        var legacySecret = _legacySecretResolver?.ResolveSecret(serviceId);
        if (string.IsNullOrWhiteSpace(legacySecret))
            return ServiceSigningSecretSet.Empty(serviceId, keyId);

        var legacy = new ServiceSigningSecret(LegacyCurrentKeyId, legacySecret, IsCurrent: true);
        if (!string.IsNullOrWhiteSpace(keyId)
            && !string.Equals(keyId, LegacyCurrentKeyId, StringComparison.Ordinal))
        {
            return ServiceSigningSecretSet.Empty(serviceId, keyId);
        }

        return new ServiceSigningSecretSet(serviceId, keyId, legacy, new[] { legacy });
    }
}
