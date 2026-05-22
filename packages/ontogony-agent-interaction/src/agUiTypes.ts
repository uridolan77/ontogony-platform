/**
 * AG-UI protocol event shapes (wire-oriented).
 * Ontogony internal events remain canonical — see types.ts.
 * @see https://docs.ag-ui.com/concepts/events
 */

export const AG_UI_EVENT_TYPES = [
  "RUN_STARTED",
  "RUN_FINISHED",
  "RUN_ERROR",
  "RUN_CANCELLED",
  "STEP_STARTED",
  "STEP_FINISHED",
  "TEXT_MESSAGE_START",
  "TEXT_MESSAGE_CONTENT",
  "TEXT_MESSAGE_END",
  "TOOL_CALL_START",
  "TOOL_CALL_ARGS",
  "TOOL_CALL_END",
  "TOOL_CALL_RESULT",
  "STATE_SNAPSHOT",
  "STATE_DELTA",
  "CUSTOM",
] as const;

export type AgUiEventType = (typeof AG_UI_EVENT_TYPES)[number];

export type AgUiTextMessageRole = "user" | "assistant";

export type AgUiRunOutcome =
  | { type: "success" }
  | {
      type: "interrupt";
      interrupts: AgUiInterrupt[];
    };

export interface AgUiInterrupt {
  id: string;
  reason?: string;
  message?: string;
  humanGateId?: string;
  kanonDecisionId?: string;
  responseSchema?: Record<string, unknown>;
  metadata?: Record<string, unknown>;
}

/** Embedded on CUSTOM events for lossless Ontogony round-trip (not required on external wire). */
export interface AgUiOntogonyProvenance {
  schema: "ontogony-agent-interaction-event-v0";
  eventId: string;
  type: string;
  timestamp: string;
  source: string;
  ids: Record<string, string | undefined>;
  severity?: string;
  redaction?: unknown;
  evidence?: unknown;
}

export type AgUiEvent = {
  type: AgUiEventType | string;
  timestamp?: string;
  threadId?: string;
  runId?: string;
  parentRunId?: string;
  stepId?: string;
  messageId?: string;
  role?: AgUiTextMessageRole;
  delta?: string;
  toolCallId?: string;
  toolCallName?: string;
  snapshot?: unknown;
  patch?: unknown;
  message?: string;
  code?: string;
  outcome?: AgUiRunOutcome;
  name?: string;
  value?: unknown;
  input?: unknown;
  stepName?: string;
};
