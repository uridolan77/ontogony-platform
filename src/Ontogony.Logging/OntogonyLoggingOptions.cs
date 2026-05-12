namespace Ontogony.Logging;

/// <summary>
/// Options for Ontogony logging scope enrichment.
/// </summary>
public sealed class OntogonyLoggingOptions
{
    /// <summary>When false, <see cref="OntogonyLoggingScopeMiddleware"/> is a no-op.</summary>
    public bool EnableRequestScope { get; set; } = true;

    /// <summary>Include actor id in logging scope when present on the request.</summary>
    public bool IncludeActorId { get; set; } = true;

    /// <summary>Include tenant id in logging scope when present.</summary>
    public bool IncludeTenantId { get; set; } = true;

    /// <summary>Include workspace id in logging scope when present.</summary>
    public bool IncludeWorkspaceId { get; set; } = true;

    /// <summary>Include project id in logging scope when present.</summary>
    public bool IncludeProjectId { get; set; } = true;

    /// <summary>Include session id in logging scope when present.</summary>
    public bool IncludeSessionId { get; set; } = true;

    /// <summary>Optional service name override for scope fields.</summary>
    public string? ServiceName { get; set; }

    /// <summary>Optional service version for scope fields.</summary>
    public string? ServiceVersion { get; set; }

    /// <summary>Optional environment name for scope fields.</summary>
    public string? Environment { get; set; }
}
