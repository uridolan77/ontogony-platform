using Microsoft.Extensions.Logging.Abstractions;
using Ontogony.Secrets;
using Ontogony.Secrets.AzureKeyVault;
using Xunit;

namespace Ontogony.Secrets.AzureKeyVault.Tests;

public sealed class AzureKeyVaultSecretValueResolverTests
{
    [Fact]
    public async Task Resolves_vault_scheme_without_logging_secret_material()
    {
        var client = new StubVaultClient(new Dictionary<string, string>
        {
            ["providers/openai/api-key"] = "sk-test-value-not-logged"
        });
        var resolver = CreateResolver(client);

        var result = await resolver.TryResolveAsync(new SecretValueReference("vault", "providers/openai/api-key"));

        Assert.True(result.IsResolved);
        Assert.Equal("sk-test-value-not-logged", result.Value);
        Assert.DoesNotContain("sk-test", result.ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public async Task Akv_scheme_is_alias_for_vault()
    {
        var client = new StubVaultClient(new Dictionary<string, string> { ["k"] = "v" });
        var resolver = CreateResolver(client);

        var result = await resolver.TryResolveAsync(new SecretValueReference("akv", "k"));

        Assert.True(result.IsResolved);
        Assert.Equal("v", result.Value);
    }

    [Fact]
    public async Task Returns_scheme_not_supported_for_env()
    {
        var client = new StubVaultClient(new Dictionary<string, string>());
        var resolver = CreateResolver(client);

        var result = await resolver.TryResolveAsync(new SecretValueReference("env", "ANY"));

        Assert.False(result.IsResolved);
        Assert.Equal("scheme_not_supported", result.UnresolvedReason);
    }

    [Fact]
    public async Task Unconfigured_resolver_returns_vault_not_configured()
    {
        var resolver = new AzureKeyVaultSecretValueResolver(
            Microsoft.Extensions.Options.Options.Create(new AzureKeyVaultSecretResolverOptions()),
            NullLogger<AzureKeyVaultSecretValueResolver>.Instance);

        var result = await resolver.TryResolveAsync(new SecretValueReference("vault", "missing"));

        Assert.False(result.IsResolved);
        Assert.Equal("vault_not_configured", result.UnresolvedReason);
    }

    private static AzureKeyVaultSecretValueResolver CreateResolver(IAzureKeyVaultSecretClient client) =>
        new(
            Microsoft.Extensions.Options.Options.Create(
                new AzureKeyVaultSecretResolverOptions { VaultUri = "https://test.vault.azure.net/" }),
            NullLogger<AzureKeyVaultSecretValueResolver>.Instance,
            client);

    private sealed class StubVaultClient(IReadOnlyDictionary<string, string> secrets) : IAzureKeyVaultSecretClient
    {
        public Task<string?> GetSecretValueAsync(string secretName, string? version, CancellationToken cancellationToken = default)
        {
            _ = version;
            secrets.TryGetValue(secretName, out var value);
            return Task.FromResult(value);
        }
    }

}
