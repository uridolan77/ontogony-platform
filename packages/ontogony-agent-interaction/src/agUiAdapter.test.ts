import { readFileSync } from "node:fs";
import { dirname, join } from "node:path";
import { fileURLToPath } from "node:url";
import { describe, expect, it } from "vitest";
import {
  fromAgUiEvent,
  fromAgUiEvents,
  ontogonyCustomAgUiName,
  parseOntogonyTypeFromAgUiCustomName,
  serializeAgUiEventsToJsonl,
  stripOntogonyProvenanceFromAgUiEvents,
  toAgUiEvent,
  toAgUiEvents,
} from "./agUiAdapter";
import { parseAgentInteractionJsonl } from "./jsonl";
import type { OntogonyAgentEvent } from "./types";

const packageSrc = dirname(fileURLToPath(import.meta.url));
const repoRoot = join(packageSrc, "..", "..", "..");
const fixtureDir = join(
  repoRoot,
  "docs",
  "schemas",
  "fixtures",
  "agent-interaction",
);

function loadPlatformFixture(name: string): OntogonyAgentEvent[] {
  const text = readFileSync(join(fixtureDir, `${name}.jsonl`), "utf8");
  return parseAgentInteractionJsonl(text);
}

describe("agUiAdapter", () => {
  it("maps sample-run lifecycle and domain events", () => {
    const events = loadPlatformFixture("sample-run");
    const agUi = toAgUiEvents(events);

    expect(agUi[0]?.type).toBe("RUN_STARTED");
    expect(agUi[0]?.threadId).toBe("thread-demo-001");
    expect(agUi[0]?.runId).toBe("run-demo-001");

    const modelRoute = agUi.find(
      (e) => e.name === "ontogony.model_call_route_decided",
    );
    expect(modelRoute?.type).toBe("CUSTOM");

    const last = agUi[agUi.length - 1];
    expect(last?.type).toBe("RUN_FINISHED");
    expect(last?.outcome).toEqual({ type: "success" });
  });

  it("maps interrupt fixture to interrupt-aware RUN_FINISHED", () => {
    const events = loadPlatformFixture("sample-human-gate-interrupt");
    const agUi = toAgUiEvents(events);

    const finished = agUi.find((e) => e.type === "RUN_FINISHED");
    expect(finished?.outcome?.type).toBe("interrupt");
    if (finished?.outcome?.type === "interrupt") {
      expect(finished.outcome.interrupts[0]?.id).toBe("interrupt-demo-001");
    }
  });

  it("round-trips Ontogony custom events via embedded provenance", () => {
    const events = loadPlatformFixture("sample-run");
    const agUi = toAgUiEvents(events, { embedOntogonyProvenance: true });
    const custom = agUi.find(
      (e) => e.name === "ontogony.model_call_route_decided",
    );
    const restored = fromAgUiEvent(custom!);
    const original = events.find((e) => e.type === "MODEL_CALL_ROUTE_DECIDED");
    expect(restored?.eventId).toBe(original?.eventId);
  });

  it("preserves unknown external CUSTOM events", () => {
    const restored = fromAgUiEvent({
      type: "CUSTOM",
      name: "copilotkit.demo.widget",
      value: { widgetId: "w-1" },
      timestamp: "2026-05-22T12:00:00Z",
    });
    expect(restored?.type).toBe("CUSTOM");
    expect(restored?.payload).toMatchObject({
      agUiName: "copilotkit.demo.widget",
    });
  });

  it("strips provenance for external wire export", () => {
    const events = loadPlatformFixture("sample-run");
    const stripped = stripOntogonyProvenanceFromAgUiEvents(
      toAgUiEvents(events, { embedOntogonyProvenance: true }),
    );
    const custom = stripped.find(
      (e) => e.name === "ontogony.model_call_route_decided",
    );
    if (custom?.value && typeof custom.value === "object") {
      expect("_ontogony" in (custom.value as object)).toBe(false);
    }
  });

  it("serializes to JSONL", () => {
    const events = loadPlatformFixture("sample-run");
    const lines = serializeAgUiEventsToJsonl(toAgUiEvents(events)).split("\n");
    expect(lines.length).toBe(events.length);
  });

  it("reverse-maps standard AG-UI lifecycle events", () => {
    const restored = fromAgUiEvents([
      {
        type: "RUN_STARTED",
        threadId: "t1",
        runId: "r1",
        timestamp: "2026-05-22T09:00:00Z",
      },
      {
        type: "RUN_FINISHED",
        threadId: "t1",
        runId: "r1",
        timestamp: "2026-05-22T09:00:10Z",
        outcome: { type: "success" },
      },
    ]);
    expect(restored.map((e) => e.type)).toEqual(["RUN_STARTED", "RUN_FINISHED"]);
  });

  it("uses ontogony.* custom naming helpers", () => {
    expect(ontogonyCustomAgUiName("EVIDENCE_GRAPH_SNAPSHOT")).toBe(
      "ontogony.evidence_graph_snapshot",
    );
    expect(
      parseOntogonyTypeFromAgUiCustomName("ontogony.evidence_graph_snapshot"),
    ).toBe("EVIDENCE_GRAPH_SNAPSHOT");
  });
});
