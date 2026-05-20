using Ontogony.Secrets.AzureKeyVault;
using Xunit;

namespace Ontogony.Secrets.AzureKeyVault.Tests;

public sealed class AzureKeyVaultSecretLocatorTests
{
    [Theory]
    [InlineData("providers/openai/api-key", "providers/openai/api-key", null)]
    [InlineData("my-secret@abc-version", "my-secret", "abc-version")]
    public void TryParse_splits_optional_version(string locator, string expectedName, string? expectedVersion)
    {
        Assert.True(AzureKeyVaultSecretLocator.TryParse(locator, out var name, out var version));
        Assert.Equal(expectedName, name);
        Assert.Equal(expectedVersion, version);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("@only-version")]
    public void TryParse_rejects_invalid_locator(string locator)
    {
        Assert.False(AzureKeyVaultSecretLocator.TryParse(locator, out _, out _));
    }
}
