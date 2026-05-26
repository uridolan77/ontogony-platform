# ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-004

**Title:** Cross-repo reconstructability smoke and closeout  
**Status:** active  
**Depends on:** SPINE-001 Phases 2A/2B (Allagma export, Kanon classify, frontend panel, `@ontogony/ui`)

## Goal

Prove the full loop against a **live** local stack (not fixtures only):

```text
Allagma GET …/decision-events
  → Kanon POST …/reconstructability/classify-batch
  → Frontend run-decision-reconstruction-panel
```

## Acceptance

| Step | Check |
| --- | --- |
| 1 | Docker-local stack: Allagma + Kanon + Conexus (+ frontend :5175) |
| 2 | ENV-SEED-001 baseline run (or other completed run with events) |
| 3 | `GET /allagma/v0/runs/{runId}/decision-events` returns `ontogony-allagma-run-decision-events-v1` |
| 4 | `POST /ontology/v0/reconstructability/classify-batch` with `decisionEvents[]` |
| 5 | One report per event; `reports[i].decisionEventId === decisionEvents[i].decisionEventId` |
| 6 | Each report `ontogonyGovernanceStatus` ∈ PASS \| WARN \| FAIL |
| 7 | Audit journey renders `run-decision-reconstruction-panel` with governance text |
| 8 | Evidence artifact: runId, traceId, counts, worst governance, kinds |

## Run (operator)

```powershell
cd C:\dev\ontogony-frontend
npm run docker:smoke:dec-recon
```

API-only (no browser):

```powershell
cd C:\dev\ontogony-frontend
npm run dec-recon:smoke:api
```

## Artifacts

| Path | Role |
| --- | --- |
| `ontogony-platform/docker/local-working-system/artifacts/dec-recon-004-smoke-report.json` | Machine-readable closeout |
| `ontogony-frontend/docs/evidence/ONTOGONY_DECISION_RECONSTRUCTABILITY_SPINE_004_EVIDENCE.md` | Human evidence record |

## Out of scope (later packages)

- DEC-RECON-005 — canonical golden fixtures (five Allagma kinds)
- DEC-RECON-006 — Evidence Spine graph nodes
- DEC-RECON-007 — persisted report artifacts (hash + classifier version)
- CONEXUS-DECISION-EVENTS-001 — Conexus decision-event export
