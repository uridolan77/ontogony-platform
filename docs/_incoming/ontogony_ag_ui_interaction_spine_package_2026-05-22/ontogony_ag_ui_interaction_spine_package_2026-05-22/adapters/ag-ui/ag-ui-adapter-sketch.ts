import type { OntogonyAgentEvent } from "../../contracts/typescript/ontogony-agent-interaction-types";

type AgUiEvent = Record<string, unknown>;

export function toAgUiEvent(event: OntogonyAgentEvent): AgUiEvent {
  switch (event.type) {
    case "RUN_STARTED":
      return {
        type: "RUN_STARTED",
        threadId: event.ids.threadId,
        runId: event.ids.runId,
        timestamp: event.timestamp,
        rawEvent: event,
      };
    case "RUN_FINISHED":
      return {
        type: "RUN_FINISHED",
        threadId: event.ids.threadId,
        runId: event.ids.runId,
        timestamp: event.timestamp,
        rawEvent: event,
      };
    case "RUN_ERROR":
      return {
        type: "RUN_ERROR",
        message: (event.payload as any)?.message ?? "Run failed",
        code: (event.payload as any)?.code,
        timestamp: event.timestamp,
        rawEvent: event,
      };
    case "MESSAGE_STARTED":
      return {
        type: "TEXT_MESSAGE_START",
        messageId: (event.payload as any)?.messageId,
        role: (event.payload as any)?.role ?? "assistant",
        rawEvent: event,
      };
    case "MESSAGE_DELTA":
      return {
        type: "TEXT_MESSAGE_CONTENT",
        messageId: (event.payload as any)?.messageId,
        delta: (event.payload as any)?.delta ?? "",
        rawEvent: event,
      };
    case "MESSAGE_FINISHED":
      return {
        type: "TEXT_MESSAGE_END",
        messageId: (event.payload as any)?.messageId,
        rawEvent: event,
      };
    case "TOOL_CALL_STARTED":
      return {
        type: "TOOL_CALL_START",
        toolCallId: (event.payload as any)?.toolCallId,
        toolCallName: (event.payload as any)?.toolCallName,
        rawEvent: event,
      };
    case "TOOL_CALL_ARGS_DELTA":
      return {
        type: "TOOL_CALL_ARGS",
        toolCallId: (event.payload as any)?.toolCallId,
        delta: (event.payload as any)?.delta ?? "",
        rawEvent: event,
      };
    case "TOOL_CALL_READY":
      return {
        type: "TOOL_CALL_END",
        toolCallId: (event.payload as any)?.toolCallId,
        rawEvent: event,
      };
    case "STATE_SNAPSHOT":
      return {
        type: "STATE_SNAPSHOT",
        snapshot: (event.payload as any)?.state ?? event.payload,
        rawEvent: event,
      };
    case "STATE_DELTA":
      return {
        type: "STATE_DELTA",
        delta: (event.payload as any)?.patch ?? event.payload,
        rawEvent: event,
      };
    default:
      return {
        type: "CUSTOM",
        name: `ontogony.${event.type.toLowerCase()}`,
        value: event.payload,
        rawEvent: event,
      };
  }
}

export function toAgUiEvents(events: OntogonyAgentEvent[]): AgUiEvent[] {
  return events.map(toAgUiEvent);
}
