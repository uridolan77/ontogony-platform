namespace Athanor.Infrastructure.Options;

/// <summary>PR97: extraction profile registry location + optional strict validation.</summary>
public sealed class ExtractionProfileOptions
{
    public const string SectionName = "ExtractionProfiles";

    /// <summary>Path relative to ASP.NET Core content root (default <c>config/extraction-profiles.registry.json</c>).</summary>
    public string RegistryRelativePath { get; init; } = "config/extraction-profiles.registry.json";

    /// <summary>
    /// When <c>true</c>, unknown <c>(promptTemplateName, promptTemplateVersion)</c> pairs fail generated extraction with <c>400 ValidationFailed</c>.
    /// Default <c>false</c> preserves permissive behavior for gradual adoption.
    /// </summary>
    public bool RequireKnownProfiles { get; init; }
}
