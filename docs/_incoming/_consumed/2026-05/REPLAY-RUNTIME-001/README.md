# REPLAY-RUNTIME-001 — Cursor Development Package

## Purpose

This package defines the next coherent replay/simulation development slice for the Ontogony system across:

- `ontogony-platform`
- `allagma-dotnet`
- `kanon-dotnet`
- `conexus-dotnet`
- `ontogony-frontend`
- `ontogony-ui`

The package is intentionally evidence-first. It does not treat replay as a button. It defines a replay vocabulary, shared contract model, backend orchestration plan, UI workbench plan, Evidence Spine integration, export artifacts, smoke/runtime-lock posture, contract discipline updates, and acceptance criteria.

## Source review basis

GitHub current main refs via ChatGPT GitHub connector; local C:\dev working tree and builds were not directly executed in this environment.

Reviewed refs:

| Repo | Ref reviewed |
|---|---|
| `uridolan77/ontogony-platform` | `c019af3a9d9c0ed8ed99d3b8cdf402279d232bf0` |
| `uridolan77/allagma-dotnet` | `13d6280465fd1f85f10b798552e5c4f7659b68ba` |
| `uridolan77/kanon-dotnet` | `1772f3850488a2bc9ae7395935fa809a81f901c1` |
| `uridolan77/conexus-dotnet` | `0115d1c7409272a24b1827a7800b91e3669ea759` |
| `uridolan77/ontogony-frontend` | `33339020d2f95b1ee697a3435418db1505c3062e` |
| `uridolan77/ontogony-ui` | `7ca0bb4ad569b23b2ed43ad09077253372a04b5a` |

## Executive verdict

Replay is already partially real, but not yet a coherent cross-service operator workflow.

Current state:

- Allagma already has run replay as a terminal-run, idempotent **replay manifest** path: `POST /allagma/v0/runs/{runId}/replay` delegates to `ReplayAgentRunService`, emits `RunReplayRequested`, links Kanon replay bundle IDs, and returns a manifest fingerprint.
- Kanon already has decision provenance and replay bundle infrastructure, including replay bundle persistence, signed/export verification docs, and semantic decision replay acceptance scripts.
- Conexus already has chat completion idempotency/replay storage and model-call/route-decision evidence surfaces. **REPLAY-RUNTIME-002** added `/admin/v0/replay/*`; Allagma orchestration uses model-call dry-run only (route-decision dry-run is direct Conexus admin).
- Platform already owns Evidence Spine contracts, identifier taxonomy, cross-service evidence bundle schemas, runtime-lock governed fake evidence, and contract discipline.
- Frontend already has Evidence Spine and Agent Interaction workbenches, Allagma run/audit pages, Human Gates, and Evidence Spine export. It does not need a new crowded console surface.
- UI already has the canonical layout, signal, disclosure, dialog, and table primitives needed for replay without local one-off wrappers.

REPLAY-RUNTIME-001 should therefore normalize replay modes and build an orchestration spine around what exists, rather than restarting from stale assumptions.

## Package contents

| File | Purpose |
|---|---|
| `00_UNPACK_PROMPT.md` | Cursor execution prompt. |
| `REPLAY-RUNTIME-001.md` | Master package spec. |
| `01_CURRENT_STATE_AUDIT.md` | Repo-by-repo audit. |
| `02_REPLAY_VOCABULARY_AND_MODES.md` | Shared replay semantics. |
| `03_REPLAY_CONTRACT_MODEL.md` | Targets, DTOs, schemas, ownership. |
| `04_CROSS_SERVICE_REPLAY_FLOW.md` | End-to-end operator/system flow. |
| `05_BACKEND_IMPLEMENTATION_PLAN.md` | Staged backend plan. |
| `06_FRONTEND_REPLAY_WORKBENCH_PLAN.md` | Canonical frontend plan. |
| `07_EVIDENCE_SPINE_AND_EXPORT_PLAN.md` | Evidence links and export artifacts. |
| `08_RUNTIME_LOCK_AND_SMOKE_PLAN.md` | Smoke/runtime-lock posture. |
| `09_CONTRACT_DISCIPLINE_PLAN.md` | Required route/OpenAPI/client updates. |
| `10_UI_CANONICALIZATION_REQUIREMENTS.md` | UI constraints. |
| `11_TEST_AND_ACCEPTANCE_PLAN.md` | Verification and acceptance. |
| `12_RISK_REGISTER.md` | Risks and mitigations. |
| `13_REVIEW_PROMPT.md` | Final review prompt. |
| `manifest.json` | Machine-readable package inventory. |

## Implementation stance

Do not implement exact replay until the source data proves exact reproducibility. Start with:

1. shared vocabulary and contracts;
2. replay eligibility classification;
3. Allagma replay records and normalized trigger;
4. Kanon replay bundle integration;
5. Conexus dry-run/model-call replay integration;
6. cross-service replay evidence bundle;
7. governed fake replay smoke;
8. UI workbench using canonical primitives.

## Non-negotiables

- No silent real provider calls.
- No silent external tool execution.
- No second Evidence Spine.
- No semantic authority inside Allagma.
- No model routing authority inside Allagma or Kanon.
- No page-local custom replay UI framework.
- No route change without inventory/OpenAPI/generated client/catalog/usage/parity/discipline updates.
