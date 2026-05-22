import type { OntogonyAgentEvent } from "./types";

export function parseAgentInteractionJsonl(text: string): OntogonyAgentEvent[] {
  const lines = text
    .split(/\r?\n/)
    .map((line) => line.trim())
    .filter((line) => line.length > 0);

  return lines.map((line, index) => {
    let parsed: unknown;
    try {
      parsed = JSON.parse(line) as unknown;
    } catch {
      throw new Error(`Invalid JSONL at line ${index + 1}`);
    }
    if (
      !parsed ||
      typeof parsed !== "object" ||
      Array.isArray(parsed) ||
      (parsed as OntogonyAgentEvent).schema !==
        "ontogony-agent-interaction-event-v0"
    ) {
      throw new Error(
        `Line ${index + 1}: schema must be ontogony-agent-interaction-event-v0`,
      );
    }
    return parsed as OntogonyAgentEvent;
  });
}

export function serializeAgentInteractionJsonl(
  events: OntogonyAgentEvent[],
): string {
  return events.map((event) => JSON.stringify(event)).join("\n");
}
