using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Athanor.Infrastructure.Options;

/// <summary>PR118.1: validates extraction profile registry path before <see cref="FileSystemExtractionProfileRegistry"/> ctor runs.</summary>
public sealed class ExtractionProfileOptionsValidator : IValidateOptions<ExtractionProfileOptions>
{
    private readonly IHostEnvironment _hostEnvironment;

    public ExtractionProfileOptionsValidator(IHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
    }

    public ValidateOptionsResult Validate(string? name, ExtractionProfileOptions options)
    {
        var rel = options.RegistryRelativePath?.Trim();
        if (string.IsNullOrEmpty(rel))
        {
            return ValidateOptionsResult.Fail("ExtractionProfiles:RegistryRelativePath is required.");
        }

        if (Path.IsPathRooted(rel))
        {
            return ValidateOptionsResult.Fail(
                "ExtractionProfiles:RegistryRelativePath must be relative to the content root (no absolute paths).");
        }

        foreach (var segment in rel.Split('/', '\\', StringSplitOptions.RemoveEmptyEntries))
        {
            if (string.Equals(segment, "..", StringComparison.Ordinal))
            {
                return ValidateOptionsResult.Fail(
                    "ExtractionProfiles:RegistryRelativePath must not contain parent-directory segments ('..').");
            }
        }

        if (!options.RequireKnownProfiles)
        {
            return ValidateOptionsResult.Success;
        }

        var pathContent = Path.Combine(_hostEnvironment.ContentRootPath, rel);
        var pathBin = Path.Combine(AppContext.BaseDirectory, rel);
        if (!File.Exists(pathContent) && !File.Exists(pathBin))
        {
            return ValidateOptionsResult.Fail(
                $"ExtractionProfiles: registry file not found (RequireKnownProfiles=true). Tried content root '{pathContent}' and base directory '{pathBin}'.");
        }

        return ValidateOptionsResult.Success;
    }
}
