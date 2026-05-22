namespace Ontogony.SystemCompatibility;

public sealed class SystemCompatibilityGateOptions
{
    public required string DevRoot { get; init; }

    public string PlatformRoot { get; init; } = "";

    public bool RequireAllBackendRepos { get; init; } = true;

    public bool RequireFrontendRepos { get; init; } = true;

    /// <summary>
    /// When true, <see cref="SystemCompatibilityCheckStatus.Warn"/> checks count as failures.
    /// Use for release branches, tags, and promotion gates.
    /// </summary>
    public bool StrictMode { get; init; } = false;
}
