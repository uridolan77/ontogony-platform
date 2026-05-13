using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ontogony.Configuration;
using Xunit;

namespace Ontogony.Configuration.Tests;

/// <summary>
/// Tests for Ontogony configuration registration and validation helpers.
/// </summary>
public class ConfigurationRegistrationTests
{
    [Fact]
    public void AddValidatedOptions_BindsConfiguredSection()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Sample:RequiredValue"] = "configured-value"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddValidatedOptions<SampleOptions>(configuration, "Sample");

        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<SampleOptions>>();

        Assert.Equal("configured-value", options.Value.RequiredValue);
    }

    [Fact]
    public void AddValidatedOptions_WithMissingRequiredValue_ThrowsOptionsValidationException()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>())
            .Build();

        var services = new ServiceCollection();
        services.AddValidatedOptions<SampleOptions>(configuration, "Sample");

        var provider = services.BuildServiceProvider();

        Assert.Throws<OptionsValidationException>(() => provider.GetRequiredService<IOptions<SampleOptions>>().Value);
    }

    [Fact]
    public void RequiredConnectionStringValidator_WithMissingConnectionString_Fails()
    {
        var validator = new RequiredConnectionStringValidator();
        var result = validator.Validate(null, new RequiredConnectionStringOptions
        {
            Name = "Primary",
            ConnectionString = null
        });

        Assert.True(result.Failed);
        Assert.Contains("Primary", result.FailureMessage);
    }

    [Fact]
    public void EnvironmentGuard_ThrowsInProductionForDefaultSecret()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            EnvironmentGuard.ThrowIfProductionDefaultSecret("Production", "SigningSecret", "changeme", "changeme", "default"));

        Assert.Contains("SigningSecret", exception.Message);
    }

    [Fact]
    public void EnvironmentGuard_DoesNotThrowInDevelopmentForDangerousSetting()
    {
        var exception = Record.Exception(() =>
            EnvironmentGuard.ThrowIfDangerousOutsideDevelopment("Development", isDangerous: true, "should not throw in development"));

        Assert.Null(exception);
    }
}

public sealed class SampleOptions
{
    [Required]
    public string? RequiredValue { get; set; }
}
