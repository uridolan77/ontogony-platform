/** Aligned with docs/schemas/ontogony-agent-interaction-event-v0.schema.json */

export type OntogonyAgentEventSource =
  | "allagma"
  | "kanon"
  | "conexus"
  | "ontogony-frontend"
  | "ontogony-ui"
  | "ontogony-platform"
  | "adapter"
  | "fixture";

export type OntogonyAgentEventType =
  | "RUN_STARTED"
  | "RUN_FINISHED"
  | "RUN_ERROR"
  | "RUN_CANCELLED"
  | "RUN_RETRIED"
  | "RUN_REPLAYED"
  | "STEP_STARTED"
  | "STEP_PROGRESS"
  | "STEP_FINISHED"
  | "STEP_ERROR"
  | "MESSAGE_STARTED"
  | "MESSAGE_DELTA"
  | "MESSAGE_FINISHED"
  | "MESSAGE_SNAPSHOT"
  | "TOOL_CALL_STARTED"
  | "TOOL_CALL_ARGS_DELTA"
  | "TOOL_CALL_READY"
  | "TOOL_CALL_RESULT"
  | "TOOL_CALL_ERROR"
  | "STATE_SNAPSHOT"
  | "STATE_DELTA"
  | "INTERRUPT_CREATED"
  | "INTERRUPT_EXPIRED"
  | "INTERRUPT_CANCELLED"
  | "INTERRUPT_RESOLVED"
  | "RESUME_SUBMITTED"
  | "RESUME_REJECTED"
  | "EVIDENCE_NODE_ADDED"
  | "EVIDENCE_EDGE_ADDED"
  | "EVIDENCE_SOURCE_ATTEMPTED"
  | "EVIDENCE_LINK_ADDED"
  | "EVIDENCE_GRAPH_SNAPSHOT"
  | "DECISION_RECORDED"
  | "DECISION_PROVENANCE_LINKED"
  | "HUMAN_GATE_OPENED"
  | "HUMAN_GATE_CHECKED"
  | "HUMAN_GATE_RESOLVED"
  | "MODEL_CALL_STARTED"
  | "MODEL_CALL_ROUTE_DECIDED"
  | "MODEL_CALL_PROVIDER_ATTEMPTED"
  | "MODEL_CALL_COMPLETED"
  | "MODEL_CALL_FAILED"
  | "USAGE_COST_RECORDED"
  | "UI_INTENT_PROPOSED"
  | "UI_INTENT_ACCEPTED"
  | "UI_INTENT_REJECTED"
  | "UI_INTENT_RENDERED"
  | "CUSTOM";

export interface OntogonyInteractionIds {
  threadId?: string;
  runId?: string;
  parentRunId?: string;
  stepId?: string;
  operationId?: string;
  traceId?: string;
  correlationId?: string;
  modelCallId?: string;
  routeDecisionId?: string;
  kanonDecisionId?: string;
  planningDecisionId?: string;
  humanGateId?: string;
  replayId?: string;
  evidenceNodeId?: string;
  evidenceEdgeId?: string;
}

export interface OntogonyEvidenceLink {
  service:
    | "allagma"
    | "kanon"
    | "conexus"
    | "ontogony-platform"
    | "ontogony-frontend"
    | "external";
  label: string;
  href?: string;
  identifierKind?: string;
  identifierValue?: string;
  missingReasonCode?: string;
}

export interface OntogonyAgentEvent<TPayload = unknown> {
  schema: "ontogony-agent-interaction-event-v0";
  eventId: string;
  type: OntogonyAgentEventType | string;
  timestamp: string;
  source: OntogonyAgentEventSource;
  severity?: "debug" | "info" | "warning" | "error";
  ids: OntogonyInteractionIds;
  redaction?: {
    containsRedactedFields?: boolean;
    redactionReasonCodes?: string[];
  };
  evidence?: OntogonyEvidenceLink[];
  payload: TPayload;
}

export interface OntogonyInterrupt {
  id: string;
  reason: string;
  message?: string;
  toolCallId?: string;
  humanGateId?: string;
  kanonDecisionId?: string;
  responseSchema?: Record<string, unknown>;
  expiresAt?: string;
  metadata?: Record<string, unknown>;
}

export interface OntogonyInteractionMessage {
  id: string;
  role: "user" | "agent" | "tool" | "operator" | "system";
  content: string;
  status?: "streaming" | "complete" | "error";
  createdAt?: string;
  evidence?: OntogonyEvidenceLink[];
}

export interface OntogonyAgentSession {
  schema: "ontogony-agent-interaction-session-v0";
  threadId: string;
  activeRunId?: string;
  status: "idle" | "running" | "interrupted" | "completed" | "failed" | "cancelled";
  ids: OntogonyInteractionIds;
  messages: OntogonyInteractionMessage[];
  openInterrupts: OntogonyInterrupt[];
  evidenceGraph?: unknown;
  state?: Record<string, unknown>;
  events: OntogonyAgentEvent[];
}
