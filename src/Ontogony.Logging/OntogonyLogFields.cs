namespace Ontogony.Logging;

/// <summary>
/// Stable structured log field names shared by Ontogony services.
/// </summary>
public static class OntogonyLogFields
{
    public const string TraceId = "trace_id";
    public const string SpanId = "span_id";
    public const string OperationId = "operation_id";
    public const string TenantId = "tenant_id";
    public const string WorkspaceId = "workspace_id";
    public const string ProjectId = "project_id";
    public const string ActorId = "actor_id";
    public const string SessionId = "session_id";

    public const string ServiceName = "service_name";
    public const string ServiceVersion = "service_version";
    public const string Environment = "environment";

    public const string Operation = "operation";
    public const string Component = "component";
    public const string Outcome = "outcome";
    public const string ErrorCode = "error_code";
    public const string DurationMs = "duration_ms";
}
