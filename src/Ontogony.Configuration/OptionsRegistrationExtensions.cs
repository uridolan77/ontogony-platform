using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ontogony.Configuration;

public static class OptionsRegistrationExtensions
{
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

public sealed class RequiredConnectionStringValidator : IValidateOptions<RequiredConnectionStringOptions>
{
    public ValidateOptionsResult Validate(string? name, RequiredConnectionStringOptions options)
    {
        return string.IsNullOrWhiteSpace(options.ConnectionString)
            ? ValidateOptionsResult.Fail($"Missing required connection string: {options.Name}")
            : ValidateOptionsResult.Success;
    }
}

public sealed class RequiredConnectionStringOptions
{
    [Required]
    public string Name { get; set; } = "Default";

    [Required]
    public string? ConnectionString { get; set; }
}
