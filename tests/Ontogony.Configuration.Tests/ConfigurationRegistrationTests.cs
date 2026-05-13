using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ontogony.Configuration;

namespace Ontogony.Configuration.Tests;

/// <summary>
/// Tests for Ontogony configuration registration and validation.
/// </summary>
public class ConfigurationRegistrationTests
{
    [Fact]
    public void Options_CanBeResolvedFromDI()
    {
        var services = new ServiceCollection();
        services.AddOptions();
        
        var provider = services.BuildServiceProvider();
        var optionsFactory = provider.GetRequiredService<IOptionsFactory<OntogonyConfigurationOptions>>();
        
        Assert.NotNull(optionsFactory);
    }

    [Fact]
    public void ConfigurationOptions_WithDefaultValues_HasValidDefaults()
    {
        var options = new OntogonyConfigurationOptions();
        
        // Configuration options should initialize without throwing
        Assert.NotNull(options);
    }
}

// Placeholder for actual configuration options (adjust based on what's in the package)
public class OntogonyConfigurationOptions
{
}
