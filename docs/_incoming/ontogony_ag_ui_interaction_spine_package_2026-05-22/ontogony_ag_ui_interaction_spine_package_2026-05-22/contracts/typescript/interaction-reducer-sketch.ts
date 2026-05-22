import type { OntogonyAgentEvent, OntogonyAgentSession } from "./ontogony-agent-interaction-types";

export function reduceInteractionEvent(
  session: OntogonyAgentSession,
  event: OntogonyAgentEvent,
): OntogonyAgentSession {
  if (session.events.some((existing) => existing.eventId === event.eventId)) {
    return session;
  }

  const next: OntogonyAgentSession = {
    ...session,
    ids: { ...session.ids, ...event.ids },
    events: [...session.events, event],
  };

  switch (event.type) {
    case "RUN_STARTED":
      next.status = "running";
      next.activeRunId = event.ids.runId ?? next.activeRunId;
      break;
    case "RUN_FINISHED":
      next.status = "completed";
      break;
    case "RUN_ERROR":
      next.status = "failed";
      break;
    case "RUN_CANCELLED":
      next.status = "cancelled";
      break;
    case "INTERRUPT_CREATED": {
      const interrupt = event.payload as any;
      next.status = "interrupted";
      if (interrupt?.id && !next.openInterrupts.some((item) => item.id === interrupt.id)) {
        next.openInterrupts = [...next.openInterrupts, interrupt];
      }
      break;
    }
    case "INTERRUPT_RESOLVED": {
      const interruptId = (event.payload as any)?.interruptId;
      next.openInterrupts = next.openInterrupts.filter((item) => item.id !== interruptId);
      if (next.openInterrupts.length === 0 && next.status === "interrupted") {
        next.status = "running";
      }
      break;
    }
    case "STATE_SNAPSHOT":
      next.state = (event.payload as any)?.state ?? {};
      break;
    case "STATE_DELTA":
      next.state = { ...(next.state ?? {}), ...((event.payload as any)?.patch ?? {}) };
      break;
  }

  return next;
}
