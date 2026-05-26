# Phased Execution Plan

## Phase 0 — Reconnaissance

- Inspect existing Evidence Spine work.
- List existing DTOs and routes related to runs, decisions, model calls, traces, source bindings, semantic graph, and human gates.
- Identify one realistic golden path already represented in fixtures or demo data.
- Write implementation notes before coding.

## Phase 1 — Kanon classifier core

- Add contracts.
- Implement deterministic classifier.
- Add missing-evidence diagnostics.
- Add unit tests.
- Add docs.

This gives the system a stable semantic center.

## Phase 2 — Backend fragment emitters/projections

- Add/extend Allagma operation and human-gate fragment projections.
- Add/extend Conexus route-decision and model-call fragment projections.
- Link Kanon policy/decision records into `policyBasis`.
- Add endpoint or projection tests.

## Phase 3 — Cross-service golden fixture

- Create one fixture that links Allagma + Conexus + Kanon.
- Generate a reconstructability report from it.
- Prove PASS/WARN/FAIL outcomes with variants.

## Phase 4 — Frontend panel

- Add shared UI components if appropriate.
- Add frontend API client.
- Add `DecisionReconstructionPanel`.
- Integrate into Evidence Spine node details and Allagma run/operation detail.
- Add frontend tests.

## Phase 5 — Documentation and operator guide

- Update root docs and relevant repo docs.
- Add local demo steps.
- Add troubleshooting section for missing evidence.

## Phase 6 — Cleanup and acceptance

- Regenerate route inventories/OpenAPI snapshots.
- Remove temporary fixture labels from live paths.
- Ensure no duplicate or dead docs were created.
- Run modified test suites.
- Produce final implementation notes.
