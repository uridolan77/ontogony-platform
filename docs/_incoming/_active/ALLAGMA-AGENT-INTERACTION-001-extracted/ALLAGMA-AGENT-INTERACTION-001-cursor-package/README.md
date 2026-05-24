# ALLAGMA-AGENT-INTERACTION-001 — Cursor implementation package

Generated: 2026-05-23T22:10:57Z

## Mission

Turn the Agent Interaction workbench from a fixture-first/demo replay page into a live, operator-grade Allagma interaction inspector.

The workbench must resolve a real Allagma run and render the actual execution story:

```text
Allagma run
  → run state/events
  → Kanon planning decision(s)
  → tool intent / action-evaluation / gate events
  → Conexus model-call messages and provider attempts
  → evaluation/replay/evidence links
```

This package is the sibling of `EVIDENCE-SPINE-REAL-001`, but its center of gravity is the **operator timeline** rather than the graph resolver. It should use Evidence Spine where useful, but it must produce a readable live interaction session, not merely link to a graph.

## Primary repositories

Expected local monorepo layout:

```text
C:\dev\ontogony-platformC:\dev\ontogony-frontendC:\dev\ontogony-uiC:\devllagma-dotnetC:\dev\kanon-dotnetC:\dev\conexus-dotnet```

Primary implementation likely spans:

- `ontogony-frontend` — Agent Interaction page, client adapters, run list polish, Start Run UX.
- `ontogony-ui` — reusable status/mode/timeline cards if appropriate.
- `allagma-dotnet` — expose missing run interaction/replay fields if not available.
- `conexus-dotnet` — expose linked model-call messages/provider route details if missing.
- `kanon-dotnet` — expose linked decision/action/gate provenance if missing.

## Package contents

```text
00_EXECUTIVE_BRIEF.md
01_MASTER_CURSOR_PROMPT.md
02_IMPLEMENTATION_PLAN.md
03_ACCEPTANCE_CHECKLIST.md
04_REGRESSION_TEST_MATRIX.md
05_UI_UX_SPEC.md
06_BACKEND_CONTRACTS_SCOPE.md
07_E2E_VALIDATION_PLAN.md
08_RISK_AND_NON_GOALS.md
manifest.json

prompts/
  00_REPO_AUDIT.md
  01_FRONTEND_AGENT_INTERACTION_WORKBENCH.md
  02_ALLAGMA_LIVE_REPLAY_API.md
  03_CONEXUS_MODEL_CALL_MESSAGES.md
  04_KANON_DECISION_LINKAGE.md
  05_TOOL_AND_HUMAN_GATE_TIMELINE.md
  06_START_RUN_WORKBENCH.md
  07_E2E_AND_TESTS.md
  08_FINAL_REVIEW.md

contracts/
  AGENT_INTERACTION_SESSION_CONTRACT.md
  AGENT_INTERACTION_MODE_CONTRACT.md
  ALLAGMA_TIMELINE_EVENT_CONTRACT.md
  MESSAGE_RENDERING_CONTRACT.md
  TOOL_GATE_EVENT_CONTRACT.md
  REPLAY_LOOKUP_CONTRACT.md
  START_RUN_OPERATOR_CONTRACT.md
  TIMELINE_RESOLUTION_CONTRACT.md

schemas/
  agent-interaction-session.schema.json
  agent-interaction-event.schema.json
  agent-interaction-source-attempt.schema.json
  agent-interaction-message.schema.json

scripts/
  validate-allagma-agent-interaction-001.ps1
  validate-allagma-agent-interaction-001.sh

examples/
  expected-live-agent-interaction-session.md
  expected-fixture-mode-banner.md
  expected-run-list-polish.md
  expected-start-run-preview.md
```

## Definition of done

A local operator can:

1. Start or pick a real completed Allagma run.
2. Open it in Agent Interaction.
3. See **live mode** by default when Allagma is reachable.
4. See real Allagma events, real model-call linkage, Kanon planning/action decisions when present, tool/gate events, and state progression.
5. See unresolved items explained with structured reasons.
6. Switch deliberately to fixture replay or imported JSONL, both clearly marked as non-live evidence.
7. Export an interaction bundle that does not duplicate sections and does not pretend fixtures prove readiness.

## Hard rules

- Do not enable real external tool execution.
- Do not let fixture sessions contribute to readiness, topology, compatibility, or “system connected” claims.
- Do not render raw fake provider responses in run list cards.
- Do not use unlabeled `unknown` anywhere.
- Do not show duplicate Agent Interaction sections in UI or exported bundle.
- Redact raw prompts, secrets, credentials, and secret-looking fields unless existing project policy explicitly allows them.
