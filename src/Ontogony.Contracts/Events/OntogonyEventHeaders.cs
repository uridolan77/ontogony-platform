namespace Ontogony.Contracts.Events;

/// <summary>
/// Canonical HTTP header names used with Ontogony correlation and tenancy propagation.
/// </summary>
public static class OntogonyEventHeaders
{
    /// <summary>W3C Trace Context <c>traceparent</c>.</summary>
    public const string TraceParent = "traceparent";

    /// <summary>W3C Trace Context <c>tracestate</c>.</summary>
    public const string TraceState = "tracestate";

    /// <summary>Ontogony trace id header.</summary>
    public const string TraceId = "X-Ontogony-Trace-Id";

    /// <summary>Cross-service operation correlation id (distinct from trace id when callers supply one).</summary>
    public const string CorrelationId = "X-Ontogony-Correlation-Id";

    /// <summary>Legacy correlation header accepted for inbound interop.</summary>
    public const string LegacyCorrelationId = "X-Correlation-ID";

    /// <summary>Legacy Athanor trace header (interop).</summary>
    public const string LegacyAthanorTraceId = "X-Athanor-Trace-Id";

    /// <summary>Legacy Agentor trace header (interop).</summary>
    public const string LegacyAgentorTraceId = "X-Agentor-Trace-Id";

    /// <summary>Conexus request id header (interop).</summary>
    public const string ConexusRequestId = "X-Conexus-Request-Id";

    /// <summary>Actor id header.</summary>
    public const string ActorId = "X-Ontogony-Actor-Id";

    /// <summary>Tenant id header.</summary>
    public const string TenantId = "X-Ontogony-Tenant-Id";

    /// <summary>Project id header.</summary>
    public const string ProjectId = "X-Ontogony-Project-Id";

    /// <summary>Workspace id header.</summary>
    public const string WorkspaceId = "X-Ontogony-Workspace-Id";

    /// <summary>Session id header.</summary>
    public const string SessionId = "X-Ontogony-Session-Id";

    /// <summary>Standard idempotency key header.</summary>
    public const string IdempotencyKey = "Idempotency-Key";
}
