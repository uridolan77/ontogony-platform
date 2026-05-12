namespace Ontogony.Observability;

/// <summary>
/// OpenTelemetry span attribute keys used by Ontogony middleware.
/// </summary>
public static class OntogonySpanAttributes
{
    /// <summary>Trace id tag.</summary>
    public const string TraceId = "ontogony.trace_id";

    /// <summary>Operation id tag.</summary>
    public const string OperationId = "ontogony.operation_id";

    /// <summary>Actor id tag.</summary>
    public const string ActorId = "ontogony.actor_id";

    /// <summary>Tenant id tag.</summary>
    public const string TenantId = "ontogony.tenant_id";

    /// <summary>Workspace id tag.</summary>
    public const string WorkspaceId = "ontogony.workspace_id";

    /// <summary>Project id tag.</summary>
    public const string ProjectId = "ontogony.project_id";

    /// <summary>Session id tag.</summary>
    public const string SessionId = "ontogony.session_id";

    /// <summary>Protocol discriminator tag.</summary>
    public const string Protocol = "ontogony.protocol";

    /// <summary>Event type tag.</summary>
    public const string EventType = "ontogony.event_type";
}
