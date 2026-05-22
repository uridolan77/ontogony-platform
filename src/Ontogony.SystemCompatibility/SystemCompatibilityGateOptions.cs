namespace Ontogony.SystemCompatibility;

public sealed class SystemCompatibilityGateOptions
{
    public required string DevRoot { get; init; }

    public string PlatformRoot { get; init; } = "";

    public bool RequireAllBackendRepos { get; init; } = true;

    public bool RequireFrontendRepos { get; init; } = true;
}
