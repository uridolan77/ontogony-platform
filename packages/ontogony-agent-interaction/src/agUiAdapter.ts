import type { OntogonyAgentEvent, OntogonyAgentEventType } from "./types";
import type {
  AgUiEvent,
  AgUiInterrupt,
  AgUiOntogonyProvenance,
  AgUiRunOutcome,
  AgUiTextMessageRole,
} from "./agUiTypes";

export type AgUiAdapterOptions = {
  /** Embed Ontogony provenance on CUSTOM events for round-trip (default false for external wire). */
  embedOntogonyProvenance?: boolean;
};

const DIRECT_ONTOGONY_TO_AG_UI: Partial<
  Record<OntogonyAgentEventType, AgUiEvent["type"]>
> = {
  RUN_STARTED: "RUN_STARTED",
  RUN_FINISHED: "RUN_FINISHED",
  RUN_ERROR: "RUN_ERROR",
  RUN_CANCELLED: "RUN_CANCELLED",
  STEP_STARTED: "STEP_STARTED",
  STEP_FINISHED: "STEP_FINISHED",
  MESSAGE_STARTED: "TEXT_MESSAGE_START",
  MESSAGE_DELTA: "TEXT_MESSAGE_CONTENT",
  MESSAGE_FINISHED: "TEXT_MESSAGE_END",
  TOOL_CALL_STARTED: "TOOL_CALL_START",
  TOOL_CALL_ARGS_DELTA: "TOOL_CALL_ARGS",
  TOOL_CALL_READY: "TOOL_CALL_END",
  TOOL_CALL_RESULT: "TOOL_CALL_RESULT",
  STATE_SNAPSHOT: "STATE_SNAPSHOT",
  STATE_DELTA: "STATE_DELTA",
};

const CUSTOM_ONTOGONY_PREFIX = "ontogony.";

export function ontogonyCustomAgUiName(ontogonyType: string): string {
  return `${CUSTOM_ONTOGONY_PREFIX}${ontogonyType.toLowerCase()}`;
}

export function parseOntogonyTypeFromAgUiCustomName(
  name: string | undefined,
): string | null {
  if (!name?.startsWith(CUSTOM_ONTOGONY_PREFIX)) return null;
  const suffix = name.slice(CUSTOM_ONTOGONY_PREFIX.length);
  if (!suffix) return null;
  return suffix.toUpperCase();
}

function mapMessageRole(role: unknown): AgUiTextMessageRole {
  if (role === "user" || role === "operator") return "user";
  return "assistant";
}

function payloadRecord(payload: unknown): Record<string, unknown> {
  if (payload && typeof payload === "object" && !Array.isArray(payload)) {
    return payload as Record<string, unknown>;
  }
  return {};
}

function buildInterruptFromPayload(
  payload: Record<string, unknown>,
): AgUiInterrupt {
  const id =
    (typeof payload.id === "string" && payload.id) ||
    (typeof payload.interruptId === "string" && payload.interruptId) ||
    "unknown-interrupt";
  return {
    id,
    reason: typeof payload.reason === "string" ? payload.reason : undefined,
    message: typeof payload.message === "string" ? payload.message : undefined,
    humanGateId:
      typeof payload.humanGateId === "string" ? payload.humanGateId : undefined,
    kanonDecisionId:
      typeof payload.kanonDecisionId === "string"
        ? payload.kanonDecisionId
        : undefined,
    responseSchema:
      payload.responseSchema &&
      typeof payload.responseSchema === "object" &&
      !Array.isArray(payload.responseSchema)
        ? (payload.responseSchema as Record<string, unknown>)
        : undefined,
    metadata:
      payload.metadata &&
      typeof payload.metadata === "object" &&
      !Array.isArray(payload.metadata)
        ? (payload.metadata as Record<string, unknown>)
        : undefined,
  };
}

function buildAgUiContext(event: OntogonyAgentEvent): Pick<
  AgUiEvent,
  "threadId" | "runId" | "parentRunId" | "stepId" | "timestamp"
> {
  return {
    timestamp: event.timestamp,
    threadId: event.ids.threadId,
    runId: event.ids.runId,
    parentRunId: event.ids.parentRunId,
    stepId: event.ids.stepId,
  };
}

function buildOntogonyProvenance(
  event: OntogonyAgentEvent,
): AgUiOntogonyProvenance {
  return {
    schema: event.schema,
    eventId: event.eventId,
    type: event.type,
    timestamp: event.timestamp,
    source: event.source,
    ids: { ...event.ids },
    severity: event.severity,
    redaction: event.redaction,
    evidence: event.evidence,
  };
}

function mapRunFinishedOutcome(
  payload: Record<string, unknown>,
  interruptById: Map<string, AgUiInterrupt>,
): AgUiRunOutcome | undefined {
  const outcome = payload.outcome;
  if (!outcome || typeof outcome !== "object" || Array.isArray(outcome)) {
    return undefined;
  }
  const o = outcome as Record<string, unknown>;
  if (o.type === "success") {
    return { type: "success" };
  }
  if (o.type === "interrupt") {
    const ids = Array.isArray(o.interruptIds)
      ? (o.interruptIds as unknown[]).filter((x): x is string => typeof x === "string")
      : [];
    const interrupts = ids.map(
      (id) => interruptById.get(id) ?? { id },
    );
    if (interrupts.length > 0) {
      return { type: "interrupt", interrupts };
    }
    const embedded = Array.isArray(o.interrupts)
      ? (o.interrupts as unknown[])
          .filter((x): x is Record<string, unknown> => !!x && typeof x === "object")
          .map((row) => buildInterruptFromPayload(row))
      : [];
    if (embedded.length > 0) {
      return { type: "interrupt", interrupts: embedded };
    }
  }
  return undefined;
}

function toAgUiCustomEvent(
  event: OntogonyAgentEvent,
  options: AgUiAdapterOptions,
): AgUiEvent {
  const p = payloadRecord(event.payload);
  const value: Record<string, unknown> = { ...p };
  if (options.embedOntogonyProvenance) {
    value._ontogony = buildOntogonyProvenance(event);
  }
  return {
    type: "CUSTOM",
    name: ontogonyCustomAgUiName(event.type),
    value,
    ...buildAgUiContext(event),
  };
}

export function toAgUiEvent(
  event: OntogonyAgentEvent,
  options: AgUiAdapterOptions = {},
): AgUiEvent {
  const ctx = buildAgUiContext(event);
  const p = payloadRecord(event.payload);
  const direct = DIRECT_ONTOGONY_TO_AG_UI[event.type as OntogonyAgentEventType];

  switch (event.type) {
    case "RUN_STARTED":
      return {
        type: "RUN_STARTED",
        ...ctx,
        input: p.input,
      };
    case "RUN_FINISHED":
      return {
        type: "RUN_FINISHED",
        ...ctx,
        outcome: mapRunFinishedOutcome(p, new Map()),
      };
    case "RUN_ERROR":
      return {
        type: "RUN_ERROR",
        ...ctx,
        message:
          typeof p.message === "string" ? p.message : "Run failed",
        code: typeof p.code === "string" ? p.code : undefined,
      };
    case "RUN_CANCELLED":
      return {
        type: "RUN_CANCELLED",
        ...ctx,
        message: typeof p.message === "string" ? p.message : undefined,
      };
    case "STEP_STARTED":
      return {
        type: "STEP_STARTED",
        ...ctx,
        stepName:
          typeof p.stepName === "string" ? p.stepName : undefined,
      };
    case "STEP_FINISHED":
      return {
        type: "STEP_FINISHED",
        ...ctx,
        stepName:
          typeof p.stepName === "string" ? p.stepName : undefined,
      };
    case "MESSAGE_STARTED":
      return {
        type: "TEXT_MESSAGE_START",
        ...ctx,
        messageId:
          typeof p.messageId === "string" ? p.messageId : event.eventId,
        role: mapMessageRole(p.role),
      };
    case "MESSAGE_DELTA":
      return {
        type: "TEXT_MESSAGE_CONTENT",
        ...ctx,
        messageId:
          typeof p.messageId === "string" ? p.messageId : event.eventId,
        delta: typeof p.delta === "string" ? p.delta : "",
        role: mapMessageRole(p.role),
      };
    case "MESSAGE_FINISHED":
      return {
        type: "TEXT_MESSAGE_END",
        ...ctx,
        messageId:
          typeof p.messageId === "string" ? p.messageId : event.eventId,
      };
    case "TOOL_CALL_STARTED":
      return {
        type: "TOOL_CALL_START",
        ...ctx,
        toolCallId:
          typeof p.toolCallId === "string" ? p.toolCallId : event.eventId,
        toolCallName:
          typeof p.toolCallName === "string" ? p.toolCallName : undefined,
      };
    case "TOOL_CALL_ARGS_DELTA":
      return {
        type: "TOOL_CALL_ARGS",
        ...ctx,
        toolCallId:
          typeof p.toolCallId === "string" ? p.toolCallId : event.eventId,
        delta: typeof p.delta === "string" ? p.delta : "",
      };
    case "TOOL_CALL_READY":
      return {
        type: "TOOL_CALL_END",
        ...ctx,
        toolCallId:
          typeof p.toolCallId === "string" ? p.toolCallId : event.eventId,
      };
    case "TOOL_CALL_RESULT":
      return {
        type: "TOOL_CALL_RESULT",
        ...ctx,
        toolCallId:
          typeof p.toolCallId === "string" ? p.toolCallId : event.eventId,
        value: p.result ?? p.value ?? p,
      };
    case "STATE_SNAPSHOT":
      return {
        type: "STATE_SNAPSHOT",
        ...ctx,
        snapshot: p.state ?? p.snapshot ?? event.payload,
      };
    case "STATE_DELTA":
      return {
        type: "STATE_DELTA",
        ...ctx,
        patch: p.patch ?? p.delta ?? event.payload,
      };
    case "INTERRUPT_CREATED":
      return toAgUiCustomEvent(event, options);
    default:
      if (direct) {
        return { type: direct, ...ctx, value: event.payload };
      }
      return toAgUiCustomEvent(event, options);
  }
}

export function toAgUiEvents(
  events: OntogonyAgentEvent[],
  options: AgUiAdapterOptions = {},
): AgUiEvent[] {
  const interruptById = new Map<string, AgUiInterrupt>();
  for (const event of events) {
    if (event.type !== "INTERRUPT_CREATED") continue;
    const interrupt = buildInterruptFromPayload(payloadRecord(event.payload));
    interruptById.set(interrupt.id, interrupt);
  }

  return events.map((event) => {
    if (event.type !== "RUN_FINISHED") {
      return toAgUiEvent(event, options);
    }
    const ctx = buildAgUiContext(event);
    const p = payloadRecord(event.payload);
    return {
      type: "RUN_FINISHED",
      ...ctx,
      outcome: mapRunFinishedOutcome(p, interruptById),
    };
  });
}

export function serializeAgUiEventsToJsonl(events: AgUiEvent[]): string {
  return events.map((e) => JSON.stringify(e)).join("\n");
}

function provenanceFromCustomValue(
  value: unknown,
): AgUiOntogonyProvenance | null {
  if (!value || typeof value !== "object" || Array.isArray(value)) return null;
  const row = value as Record<string, unknown>;
  const prov = row._ontogony;
  if (!prov || typeof prov !== "object" || Array.isArray(prov)) return null;
  const p = prov as AgUiOntogonyProvenance;
  if (p.schema !== "ontogony-agent-interaction-event-v0") return null;
  return p;
}

/**
 * Map an AG-UI event back to Ontogony when possible.
 * Unknown external CUSTOM events become Ontogony CUSTOM with preserved payload.
 */
export function fromAgUiEvent(
  event: AgUiEvent,
  source: OntogonyAgentEvent["source"] = "adapter",
): OntogonyAgentEvent | null {
  if (event.type === "CUSTOM") {
    const prov = provenanceFromCustomValue(event.value);
    if (prov) {
      const value = event.value;
      const payload =
        value && typeof value === "object" && !Array.isArray(value)
          ? Object.fromEntries(
              Object.entries(value as Record<string, unknown>).filter(
                ([k]) => k !== "_ontogony",
              ),
            )
          : {};
      return {
        schema: prov.schema,
        eventId: prov.eventId,
        type: prov.type,
        timestamp: prov.timestamp,
        source: prov.source as OntogonyAgentEvent["source"],
        severity: prov.severity as OntogonyAgentEvent["severity"],
        ids: prov.ids as OntogonyAgentEvent["ids"],
        redaction: prov.redaction as OntogonyAgentEvent["redaction"],
        evidence: prov.evidence as OntogonyAgentEvent["evidence"],
        payload,
      };
    }

    const ontogonyType = parseOntogonyTypeFromAgUiCustomName(event.name);
    if (ontogonyType) {
      const timestamp =
        event.timestamp ?? new Date(0).toISOString();
      return {
        schema: "ontogony-agent-interaction-event-v0",
        eventId: `agui-custom-${event.name ?? "unknown"}-${timestamp}`,
        type: ontogonyType,
        timestamp,
        source,
        ids: {
          threadId: event.threadId,
          runId: event.runId,
          parentRunId: event.parentRunId,
          stepId: event.stepId,
        },
        payload: event.value ?? {},
      };
    }

    return {
      schema: "ontogony-agent-interaction-event-v0",
      eventId: `agui-external-custom-${event.name ?? "unknown"}`,
      type: "CUSTOM",
      timestamp: event.timestamp ?? new Date(0).toISOString(),
      source,
      ids: {
        threadId: event.threadId,
        runId: event.runId,
      },
      payload: {
        agUiName: event.name,
        agUiValue: event.value,
      },
    };
  }

  const reverseMap: Partial<Record<string, OntogonyAgentEventType>> = {
    RUN_STARTED: "RUN_STARTED",
    RUN_FINISHED: "RUN_FINISHED",
    RUN_ERROR: "RUN_ERROR",
    RUN_CANCELLED: "RUN_CANCELLED",
    STEP_STARTED: "STEP_STARTED",
    STEP_FINISHED: "STEP_FINISHED",
    TEXT_MESSAGE_START: "MESSAGE_STARTED",
    TEXT_MESSAGE_CONTENT: "MESSAGE_DELTA",
    TEXT_MESSAGE_END: "MESSAGE_FINISHED",
    TOOL_CALL_START: "TOOL_CALL_STARTED",
    TOOL_CALL_ARGS: "TOOL_CALL_ARGS_DELTA",
    TOOL_CALL_END: "TOOL_CALL_READY",
    TOOL_CALL_RESULT: "TOOL_CALL_RESULT",
    STATE_SNAPSHOT: "STATE_SNAPSHOT",
    STATE_DELTA: "STATE_DELTA",
  };

  const ontogonyType = reverseMap[event.type];
  if (!ontogonyType) return null;

  const timestamp = event.timestamp ?? new Date(0).toISOString();
  const payload: Record<string, unknown> = {};

  if (ontogonyType.startsWith("MESSAGE_")) {
    if (event.messageId) payload.messageId = event.messageId;
    if (event.role) payload.role = event.role === "assistant" ? "agent" : event.role;
    if (event.delta) payload.delta = event.delta;
  }
  if (ontogonyType.startsWith("TOOL_CALL_")) {
    if (event.toolCallId) payload.toolCallId = event.toolCallId;
    if (event.toolCallName) payload.toolCallName = event.toolCallName;
    if (event.delta) payload.delta = event.delta;
    if ("value" in event) payload.result = event.value;
  }
  if (ontogonyType === "STATE_SNAPSHOT") {
    payload.state = event.snapshot;
  }
  if (ontogonyType === "STATE_DELTA") {
    payload.patch = event.patch;
  }
  if (ontogonyType === "RUN_FINISHED" && event.outcome) {
    if (event.outcome.type === "success") {
      payload.outcome = { type: "success" };
    } else if (event.outcome.type === "interrupt") {
      payload.outcome = {
        type: "interrupt",
        interruptIds: event.outcome.interrupts.map((i) => i.id),
        interrupts: event.outcome.interrupts,
      };
    }
  }
  if (ontogonyType === "RUN_ERROR") {
    if (event.message) payload.message = event.message;
    if (event.code) payload.code = event.code;
  }
  if (ontogonyType.startsWith("STEP_") && "stepName" in event) {
    const sn = (event as { stepName?: string }).stepName;
    if (sn) payload.stepName = sn;
  }

  return {
    schema: "ontogony-agent-interaction-event-v0",
    eventId: `agui-${event.type}-${timestamp}`,
    type: ontogonyType,
    timestamp,
    source,
    ids: {
      threadId: event.threadId,
      runId: event.runId,
      parentRunId: event.parentRunId,
      stepId: event.stepId,
    },
    payload,
  };
}

export function fromAgUiEvents(
  events: AgUiEvent[],
  source?: OntogonyAgentEvent["source"],
): OntogonyAgentEvent[] {
  return events
    .map((e) => fromAgUiEvent(e, source))
    .filter((e): e is OntogonyAgentEvent => e !== null);
}

/** Strip embedded Ontogony provenance for external AG-UI consumers. */
export function stripOntogonyProvenanceFromAgUiEvents(
  events: AgUiEvent[],
): AgUiEvent[] {
  return events.map((event) => {
    if (event.type !== "CUSTOM" || !event.value) return event;
    if (typeof event.value !== "object" || Array.isArray(event.value)) {
      return event;
    }
    const { _ontogony: _prov, ...rest } = event.value as Record<string, unknown>;
    return { ...event, value: rest };
  });
}
