# Ontogony AG-UI Interaction Spine Package

**Date:** 2026-05-22  
**Target system:** Ontogony / Allagma / Kanon / Conexus / ontogony-ui / ontogony-frontend  
**Package goal:** Define an implementation-ready AG-UI-compatible interaction spine that turns the existing evidence/correlation backbone into a live agent-user co-execution surface.

This package is intentionally **additive**. It does not replace the existing Evidence Spine, trace correlation, Allagma run lifecycle, Kanon semantic authority, or Conexus model-call evidence. It adds a typed interaction-event layer across them.

## Why this package exists

AG-UI’s main architectural lesson is that agentic systems need a standard interaction boundary between user-facing applications and agentic backends: lifecycle events, streaming messages, tool calls, state deltas, interrupts, and generative UI intents. Ontogony already has much of the evidence substrate. The missing layer is a coherent, cross-service **Agent Interaction Spine**.

## Package contents

| Path | Purpose |
| --- | --- |
| `00_EXECUTIVE_SUMMARY.md` | Decision summary and recommended direction |
| `01_CURRENT_STATE_GROUNDING.md` | Repo-grounded baseline and what already exists |
| `02_REFERENCE_AG_UI_MODEL.md` | AG-UI concepts translated into Ontogony terms |
| `03_ONTOGONY_AGENT_INTERACTION_SPINE_CONTRACT.md` | Canonical internal contract proposal |
| `04_EVENT_TAXONOMY_AND_MAPPING.md` | Event taxonomy and service ownership matrix |
| `05_REPO_PR_SEQUENCE.md` | Ordered PR plan across platform, UI, frontend, Allagma, Kanon, Conexus |
| `06_FRONTEND_IMPLEMENTATION_PLAN.md` | ontogony-frontend workbench, client, store, routes, tests |
| `07_UI_PACKAGE_PLAN.md` | @ontogony/ui components and export slices |
| `08_ALLAGMA_ADAPTER_PLAN.md` | Run-event to interaction-event adapter and streaming endpoints |
| `09_KANON_HITL_PLAN.md` | Human gate / policy decision interrupt contract |
| `10_CONEXUS_TRACE_EVENTS_PLAN.md` | Model-call / route-decision event integration |
| `11_PLATFORM_SCHEMAS_AND_DRIFT_GATES.md` | Platform schemas, validators, drift tests |
| `12_TESTING_AND_FIXTURE_HARNESS.md` | Mocking, replay, deterministic JSONL fixtures |
| `13_ACCEPTANCE_CHECKLIST.md` | Concrete acceptance criteria per PR |
| `14_BACKLOG_RISKS_AND_NON_GOALS.md` | Deferrals and risks |
| `schemas/` | JSON Schemas for interaction events and sessions |
| `contracts/typescript/` | TypeScript contract sketches for frontend/UI |
| `contracts/csharp/` | C# record sketches for .NET services |
| `adapters/ag-ui/` | AG-UI compatibility strategy and mapping |
| `fixtures/events/` | JSONL fixture examples |
| `prompts/` | Copy-paste implementation prompts per PR slice |
| `repo-copy-plan/` | Suggested destination paths in each repo |

## Recommended implementation stance

Adopt an **internal Ontogony event contract first**, with an AG-UI adapter. This avoids waiting on a stable .NET AG-UI SDK and lets Ontogony preserve its domain concepts: semantic authority, evidence spine, model-call evidence, run audit, human gates, and governed approvals.

The target shape is:

```text
Allagma run lifecycle/events
  -> OntogonyAgentEvent stream
Kanon human gates / semantic decisions
  -> Interrupt + approval events
Conexus model calls / route decisions
  -> Tool/model/route evidence events
ontogony-frontend
  -> Agent Interaction Workbench + event-store client
@ontogony/ui
  -> reusable agent timeline, approval, state, tool, evidence components
Platform
  -> schemas, drift gates, acceptance contracts
AG-UI adapter
  -> compatibility layer for external AG-UI clients later
```

## Immediate next action

Start with `PLAT-AGUI-000` and `OUI-AGENT-001`, then wire the frontend through `OFE-AGUI-001`. Avoid backend streaming changes until the contract and UI primitives are stable.
