namespace Ontogony.Contracts.Events;

public static class OntogonyEventHeaders
{
    public const string TraceParent = "traceparent";
    public const string TraceState = "tracestate";
    public const string TraceId = "X-Ontogony-Trace-Id";
    public const string LegacyAthanorTraceId = "X-Athanor-Trace-Id";
    public const string LegacyAgentorTraceId = "X-Agentor-Trace-Id";
    public const string ConexusRequestId = "X-Conexus-Request-Id";
    public const string ActorId = "X-Ontogony-Actor-Id";
    public const string TenantId = "X-Ontogony-Tenant-Id";
    public const string ProjectId = "X-Ontogony-Project-Id";
    public const string WorkspaceId = "X-Ontogony-Workspace-Id";
    public const string SessionId = "X-Ontogony-Session-Id";
    public const string IdempotencyKey = "Idempotency-Key";
}
