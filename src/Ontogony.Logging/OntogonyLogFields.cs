namespace Ontogony.Logging;

/// <summary>
/// Stable structured log field names shared by Ontogony services.
/// </summary>
public static class OntogonyLogFields
{
    /// <summary>Distributed trace id.</summary>
    public const string TraceId = "trace_id";

    /// <summary>Span id within the trace.</summary>
    public const string SpanId = "span_id";

    /// <summary>Per-request operation id.</summary>
    public const string OperationId = "operation_id";

    /// <summary>Tenant scope id.</summary>
    public const string TenantId = "tenant_id";

    /// <summary>Workspace scope id.</summary>
    public const string WorkspaceId = "workspace_id";

    /// <summary>Project scope id.</summary>
    public const string ProjectId = "project_id";

    /// <summary>Actor id.</summary>
    public const string ActorId = "actor_id";

    /// <summary>Session id.</summary>
    public const string SessionId = "session_id";

    /// <summary>Logical service name.</summary>
    public const string ServiceName = "service_name";

    /// <summary>Service version string.</summary>
    public const string ServiceVersion = "service_version";

    /// <summary>Deployment environment name.</summary>
    public const string Environment = "environment";

    /// <summary>Operation label (e.g. HTTP method and path).</summary>
    public const string Operation = "operation";

    /// <summary>Component or subsystem name.</summary>
    public const string Component = "component";

    /// <summary>High-level outcome label.</summary>
    public const string Outcome = "outcome";

    /// <summary>Machine-readable error code.</summary>
    public const string ErrorCode = "error_code";

    /// <summary>Duration in milliseconds.</summary>
    public const string DurationMs = "duration_ms";
}
