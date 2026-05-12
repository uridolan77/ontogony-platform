namespace Ontogony.Logging;

/// <summary>
/// Options for Ontogony logging scope enrichment.
/// </summary>
public sealed class OntogonyLoggingOptions
{
    public bool EnableRequestScope { get; set; } = true;
    public bool IncludeActorId { get; set; } = true;
    public bool IncludeTenantId { get; set; } = true;
    public bool IncludeWorkspaceId { get; set; } = true;
    public bool IncludeProjectId { get; set; } = true;
    public bool IncludeSessionId { get; set; } = true;
    public string? ServiceName { get; set; }
    public string? ServiceVersion { get; set; }
    public string? Environment { get; set; }
}
