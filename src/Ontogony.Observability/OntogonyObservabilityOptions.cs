using Ontogony.Contracts.Events;

namespace Ontogony.Observability;

public sealed class OntogonyObservabilityOptions
{
    public string ServiceName { get; set; } = "unknown-service";
    public string ServiceVersion { get; set; } = "0.1.0";
    public string TraceHeaderName { get; set; } = OntogonyEventHeaders.TraceId;
    public string TraceParentHeaderName { get; set; } = OntogonyEventHeaders.TraceParent;
    public string TraceStateHeaderName { get; set; } = OntogonyEventHeaders.TraceState;

    public string[] AcceptedIncomingTraceHeaders { get; set; } =
    [
        OntogonyEventHeaders.TraceId,
        OntogonyEventHeaders.LegacyAthanorTraceId,
        OntogonyEventHeaders.LegacyAgentorTraceId,
        OntogonyEventHeaders.ConexusRequestId
    ];

    /// <summary>When true, response echoes legacy service-specific trace aliases (for example <c>X-Athanor-Trace-Id</c>). Default is false; services opt in during migration.</summary>
    public bool EchoLegacyHeaders { get; set; } = false;
    public bool EchoCorrelationHeaders { get; set; } = true;
}
