# REPLAY-RUNTIME-001 — Cross-service replay/simulation runtime

## Goal

Make replay/simulation a first-class, evidence-connected, safety-aware operator workflow across the Ontogony system.

The workflow must let an operator start from a run ID, trace ID, correlation ID, decision ID, model call ID, route decision ID, provider attempt ID, or Evidence Spine bundle, then:

1. resolve related evidence;
2. classify replay eligibility;
3. select an allowed replay mode;
4. preview source data and safety posture;
5. execute replay/simulation/reconstruction only within allowed boundaries;
6. produce replay evidence;
7. compare original vs replay result;
8. link the replay result back into Evidence Spine.

## Design thesis

Replay in Ontogony is not one capability. It is a family of evidence-grounded operations with different truth claims.

The system must never imply exact reproducibility where it only has reconstruction. The UI and contracts must make that visible.

## Primary architectural decision

Allagma should own replay orchestration and replay records because replay is a runtime/operator workflow that crosses Kanon and Conexus. However, Allagma must not become semantic authority or model-routing authority.

Ownership:

| Concern | Owner |
|---|---|
| Shared replay vocabulary/schemas | Ontogony Platform |
| Replay orchestration record | Allagma |
| Run replay trigger normalization | Allagma |
| Run/audit/interaction stream reconstruction | Allagma |
| Semantic decision replay bundle | Kanon |
| Semantic plan/provenance eligibility | Kanon |
| Model-call and route-decision evidence | Conexus |
| Provider attempt safety posture | Conexus |
| Cross-service evidence graph links | Platform contract + frontend resolver |
| Operator workflow | Ontogony Frontend |
| Canonical UI primitives | Ontogony UI |

## What exists now

Current repo state already includes significant primitives:

- Allagma: run lifecycle, audit bundle, events, interaction event export/stream, retry/cancel/resume, replay manifest endpoint, eval replay harness, Kanon replay evidence recorder, governed fake smoke, runtime-lock participation.
- Kanon: decision records, provenance, replay bundle persistence, signed/export verification, semantic decision replay acceptance scripts, Evidence Spine handoff.
- Conexus: idempotency replay store for chat completions, model-call evidence bundles, route-decision stores/endpoints, streaming evidence, provider fallback evidence, governance drill-downs.
- Platform: Evidence Spine contracts, identifier taxonomy, cross-service evidence bundle schemas, runtime-lock governed fake evidence, system compatibility gate, contract discipline scripts.
- Frontend: Evidence Spine workbench, Evidence Spine resolver/export, Agent Interaction workbench, Allagma run detail/audit pages, Human Gates, Conexus governance panels, route-workflow inventory.
- UI: canonical page frame, signal summary, disclosure, dialog, destructive confirmation, data table/list primitives.

## What is missing

- A shared replay vocabulary across repos.
- A shared replay target model.
- A replay eligibility classifier.
- A replay result bundle contract.
- Replay records that can be queried/exported/comparison-linked.
- Conexus dry-run replay routes for route decisions/model calls.
- Kanon replay bundle integration into a cross-service replay result.
- Frontend replay workbench using existing Evidence Spine and Agent Interaction surfaces.
- Smoke evidence proving governed fake replay without real providers/tools.

## Implementation shape

### Stage 1 — Contracts only

Define replay modes, target identifiers, eligibility, request, result, delta, safety posture, service attempt, and evidence reference schemas. Add no behavior beyond contract tests and docs.

### Stage 2 — Allagma replay record + trigger normalization

Keep existing `POST /allagma/v0/runs/{runId}/replay`, but make it create/read a replay record and emit normalized replay events. It can still return manifest/evidence-only output at first.

### Stage 3 — Kanon replay bundle integration

Expose a Kanon replay eligibility/result view that can be called by Allagma. Start with decision/provenance replay bundles and semantic plan replay eligibility.

### Stage 4 — Conexus model-call/route-decision dry-run integration

Expose Conexus dry-run replay endpoints that never call real providers by default. Route-decision replay can be deterministic if the route config snapshot is present. Model-call replay is evidence-only or fake-provider simulation unless explicit safe conditions are met.

### Stage 5 — Cross-service replay bundle

Allagma composes service attempts from Allagma/Kanon/Conexus into one replay evidence bundle and emits Evidence Spine links.

### Stage 6 — Runtime-lock smoke evidence

Add governed fake replay smoke, initially optional/manual. Do not make replay a required PR gate until the flow stabilizes.

## Explicit deferrals

- Production IAM/security design.
- Real external tool execution.
- Real provider exact replay.
- Streaming replay of token-by-token model calls.
- Full distributed trace replay.
- Replay scheduling or automation.
- Moving Evidence Spine ownership.
- New dense console surfaces.

## Acceptance headline

REPLAY-RUNTIME-001 is accepted when a governed fake Allagma run can be resolved through Evidence Spine, classified as replay-eligible, replayed/simulated without real provider/tool execution, exported as a replay evidence bundle, compared to original evidence, and linked back into the operator console using canonical UI patterns.
