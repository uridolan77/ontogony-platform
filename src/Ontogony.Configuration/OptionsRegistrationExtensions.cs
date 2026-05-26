using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ontogony.Configuration;

/// <summary>Options registration helpers with data-annotation validation.</summary>
public static class OptionsRegistrationExtensions
{
    /// <summary>Binds, validates, and registers options for startup validation.</summary>
    public static OptionsBuilder<TOptions> AddValidatedOptions<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName)
        where TOptions : class
    {
        return services
            .AddOptions<TOptions>()
            .Bind(configuration.GetSection(sectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}

/// <summary>Validates that a required connection string is present.</summary>
public sealed class RequiredConnectionStringValidator : IValidateOptions<RequiredConnectionStringOptions>
{
    /// <inheritdoc />
    public ValidateOptionsResult Validate(string? name, RequiredConnectionStringOptions options)
    {
        return string.IsNullOrWhiteSpace(options.ConnectionString)
            ? ValidateOptionsResult.Fail($"Missing required connection string: {options.Name}")
            : ValidateOptionsResult.Success;
    }
}

/// <summary>Named connection string required at startup.</summary>
public sealed class RequiredConnectionStringOptions
{
    /// <summary>Logical connection string name.</summary>
    [Required]
    public string Name { get; set; } = "Default";

    /// <summary>Connection string value.</summary>
    [Required]
    public string? ConnectionString { get; set; }
}
