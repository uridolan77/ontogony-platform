namespace Ontogony.AgentInteraction.Contracts;

public sealed record OntogonyAgentEventDto(
    string Schema,
    string EventId,
    string Type,
    DateTimeOffset Timestamp,
    string Source,
    OntogonyInteractionIdsDto Ids,
    object? Payload,
    string? Severity = null,
    OntogonyRedactionDto? Redaction = null,
    IReadOnlyList<OntogonyEvidenceLinkDto>? Evidence = null);

public sealed record OntogonyInteractionIdsDto(
    string? ThreadId = null,
    string? RunId = null,
    string? ParentRunId = null,
    string? StepId = null,
    string? OperationId = null,
    string? TraceId = null,
    string? CorrelationId = null,
    string? ModelCallId = null,
    string? RouteDecisionId = null,
    string? KanonDecisionId = null,
    string? PlanningDecisionId = null,
    string? HumanGateId = null,
    string? EvidenceNodeId = null,
    string? EvidenceEdgeId = null);

public sealed record OntogonyRedactionDto(
    bool ContainsRedactedFields,
    IReadOnlyList<string> RedactionReasonCodes);

public sealed record OntogonyEvidenceLinkDto(
    string Service,
    string Label,
    string? Href = null,
    string? IdentifierKind = null,
    string? IdentifierValue = null,
    string? MissingReasonCode = null);

public sealed record OntogonyInterruptDto(
    string Id,
    string Reason,
    string? Message = null,
    string? ToolCallId = null,
    string? HumanGateId = null,
    string? KanonDecisionId = null,
    object? ResponseSchema = null,
    DateTimeOffset? ExpiresAt = null,
    IReadOnlyDictionary<string, object?>? Metadata = null);

public static class OntogonyAgentEventSchemas
{
    public const string EventV0 = "ontogony-agent-interaction-event-v0";
    public const string SessionV0 = "ontogony-agent-interaction-session-v0";
}

public static class OntogonyAgentEventTypes
{
    public const string RunStarted = "RUN_STARTED";
    public const string RunFinished = "RUN_FINISHED";
    public const string RunError = "RUN_ERROR";
    public const string InterruptCreated = "INTERRUPT_CREATED";
    public const string InterruptResolved = "INTERRUPT_RESOLVED";
    public const string DecisionRecorded = "DECISION_RECORDED";
    public const string ModelCallStarted = "MODEL_CALL_STARTED";
    public const string ModelCallRouteDecided = "MODEL_CALL_ROUTE_DECIDED";
    public const string ModelCallCompleted = "MODEL_CALL_COMPLETED";
    public const string EvidenceGraphSnapshot = "EVIDENCE_GRAPH_SNAPSHOT";
}
