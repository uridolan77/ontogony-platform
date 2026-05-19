# FE-TEST-REPLAY-001 — Replay / test improvements

## Purpose

Harden Allagma and Kanon **replay evidence** operator surfaces after Docker-local closeout: catalogued mock scenarios, Playwright navigation and lookup-mode coverage, and CI `replay:check`.

## Timing

Post-`ENV-DOCKER-CLOSEOUT-001`, `FE-HARDEN-001`, and `FE-AUDIT-FIXTURES-001`. Does not block the closed Docker-local milestone.

## Boundary

- Replay lookup, correlation source evidence, redaction, and navigation tests **first**
- No production readiness claim
- No backend contract changes unless an evidence gap is proven
- Replay **trigger** mutations remain unavailable when absent from the Allagma OpenAPI snapshot (limitation banners only)

## Repos

| Repo | Deliverables |
| --- | --- |
| `ontogony-frontend` | `scripts/replay-test-catalog.json`; `replay:check`; `docs/operators/FRONTEND_REPLAY_OPERATOR_CONTRACT.md`; E2E navigation + decision lookup; unit tests for replay lookup helpers; evidence |
| `ontogony-platform` | Spec; status board; evidence pointer |

## Acceptance criteria

| # | Topic | Expected |
| --- | --- | --- |
| 1 | Catalog | Replay routes, mock scenarios, navigation flows, and E2E specs documented in `replay-test-catalog.json` |
| 2 | CI gate | `replay:check` validates catalog entries and spec file presence (`npm run check`) |
| 3 | Run → replay E2E | Playwright proves triage **Open replay lookup** navigates to workbench with run id |
| 4 | Lookup modes | Playwright covers run, trace, and decision id replay lookup modes |
| 5 | Kanon provenance | Existing `kanon-provenance` replay bundle / unavailable E2E remain in catalog |
| 6 | Redaction | Replay evidence preview redaction E2E remains catalogued |
| 7 | Operator contract | `FRONTEND_REPLAY_OPERATOR_CONTRACT.md` documents routes, modes, and verification commands |
| 8 | Docker-local link | `FRONTEND_DOCKER_LOCAL_CONTRACT.md` references replay operator contract |
| 9 | No unjustified backend changes | Frontend + docs/scripts only |

## Operator verification

```powershell
cd C:\dev\ontogony-frontend

npm run replay:check

# When compose frontend occupies 5175:
$env:E2E_PORT = "5176"
npx playwright test e2e/allagma-replay-evidence.spec.ts e2e/allagma-run-detail-replay-navigation.spec.ts e2e/kanon-provenance.spec.ts
```

Live Docker stack (optional, after ENV-DOCKER-RUN-001 `PASS`):

```powershell
# Subject run from guided flow report
Start-Process "http://localhost:5175/allagma/replay?runId={subjectRunId}"
```

## Evidence

| Repo | Path |
| --- | --- |
| `ontogony-frontend` | `docs/evidence/FE_TEST_REPLAY_001_REPLAY_TEST_EVIDENCE.md` |
| `ontogony-platform` | `docs/evidence/FE_TEST_REPLAY_001_EVIDENCE.md` |

## Follow-up

| PR | Focus |
| --- | --- |
| `FE-HYGIENE-CONFIG-001` | DONE — `ontogony-frontend/docs/evidence/FE_HYGIENE_CONFIG_001_FRONTEND_CONFIG_EVIDENCE.md` |
