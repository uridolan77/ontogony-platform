using Ontogony.Contracts.Events;

namespace Ontogony.Observability;

/// <summary>
/// Options for <see cref="RequestTracingMiddleware"/> and correlation header behavior.
/// </summary>
public sealed class OntogonyObservabilityOptions
{
    /// <summary>Logical service name used in metrics and logs.</summary>
    public string ServiceName { get; set; } = "unknown-service";

    /// <summary>Service version string for telemetry.</summary>
    public string ServiceVersion { get; set; } = "0.1.0";

    /// <summary>Primary trace id response/header name.</summary>
    public string TraceHeaderName { get; set; } = OntogonyEventHeaders.TraceId;

    /// <summary>W3C traceparent header name.</summary>
    public string TraceParentHeaderName { get; set; } = OntogonyEventHeaders.TraceParent;

    /// <summary>W3C tracestate header name.</summary>
    public string TraceStateHeaderName { get; set; } = OntogonyEventHeaders.TraceState;

    /// <summary>Incoming header names accepted as trace id sources, in priority after <see cref="TraceHeaderName"/>.</summary>
    public string[] AcceptedIncomingTraceHeaders { get; set; } =
    [
        OntogonyEventHeaders.TraceId,
        OntogonyEventHeaders.LegacyAthanorTraceId,
        OntogonyEventHeaders.LegacyAgentorTraceId,
        OntogonyEventHeaders.ConexusRequestId
    ];

    /// <summary>When true, response echoes legacy service-specific trace aliases (for example <c>X-Athanor-Trace-Id</c>). Default is false; services opt in during migration.</summary>
    public bool EchoLegacyHeaders { get; set; } = false;

    /// <summary>When true, echoes tenant/workspace/project/session headers when present on the request.</summary>
    public bool EchoCorrelationHeaders { get; set; } = true;
}
