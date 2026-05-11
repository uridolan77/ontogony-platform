using Ontogony.Contracts.Events;

namespace Ontogony.Observability;

public sealed class OntogonyObservabilityOptions
{
    public string ServiceName { get; set; } = "unknown-service";
    public string ServiceVersion { get; set; } = "0.1.0";
    public string TraceHeaderName { get; set; } = OntogonyEventHeaders.TraceId;

    public string[] AcceptedIncomingTraceHeaders { get; set; } =
    [
        OntogonyEventHeaders.TraceId,
        OntogonyEventHeaders.LegacyAthanorTraceId,
        OntogonyEventHeaders.LegacyAgentorTraceId,
        OntogonyEventHeaders.ConexusRequestId
    ];

    public bool EchoLegacyHeaders { get; set; } = true;
}
