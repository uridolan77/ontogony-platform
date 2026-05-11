using Athanor.Infrastructure.Tooling;
using Microsoft.Extensions.Options;

namespace Athanor.Infrastructure.Options;

public sealed class WebImporterOptionsValidator : IValidateOptions<WebImporterOptions>
{
    public ValidateOptionsResult Validate(string? name, WebImporterOptions options)
    {
        if (!options.Enabled)
        {
            return ValidateOptionsResult.Success;
        }

        if (options.AllowedHosts is null || options.AllowedHosts.Count == 0)
        {
            return ValidateOptionsResult.Fail(
                "Tooling:WebImporter:AllowedHosts must be non-empty when the web importer is enabled.");
        }

        if (options.MaxBytes <= 0)
        {
            return ValidateOptionsResult.Fail("Tooling:WebImporter:MaxBytes must be > 0.");
        }

        if (options.TimeoutSeconds <= 0)
        {
            return ValidateOptionsResult.Fail("Tooling:WebImporter:TimeoutSeconds must be > 0.");
        }

        return ValidateOptionsResult.Success;
    }
}

