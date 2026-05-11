using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ontogony.Configuration;
using Ontogony.Primitives;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class ConfigurationAndPrimitivesTests
{
    [Fact]
    public void AddValidatedOptions_Binds_Values_From_Configuration()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Sample:RequiredValue"] = "configured"
            })
            .Build();

        services.AddValidatedOptions<SampleOptions>(configuration, "Sample");

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<SampleOptions>>().Value;

        Assert.Equal("configured", options.RequiredValue);
    }

    [Fact]
    public void AddValidatedOptions_Throws_For_Invalid_DataAnnotations()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>())
            .Build();

        services.AddValidatedOptions<SampleOptions>(configuration, "Sample");

        using var provider = services.BuildServiceProvider();
        Assert.Throws<OptionsValidationException>(() => provider.GetRequiredService<IOptions<SampleOptions>>().Value);
    }

    [Fact]
    public void RequiredConnectionStringValidator_Fails_For_Missing_ConnectionString()
    {
        var validator = new RequiredConnectionStringValidator();

        var result = validator.Validate(null, new RequiredConnectionStringOptions
        {
            Name = "Primary",
            ConnectionString = null
        });

        Assert.True(result.Failed);
    }

    [Fact]
    public void SystemClock_Returns_Recent_Utc_Time()
    {
        var clock = new SystemClock();
        var delta = DateTimeOffset.UtcNow - clock.UtcNow;

        Assert.True(delta.Duration() < TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void GuidIdGenerator_Uses_Prefix_And_Underscore()
    {
        var generator = new GuidIdGenerator();

        var id = generator.NewId("evt");

        Assert.StartsWith("evt_", id);
        Assert.Equal(36, id.Length);
    }

    public sealed class SampleOptions
    {
        [Required]
        public string? RequiredValue { get; set; }
    }
}
