# DEC-RECON-004 acceptance checklist

## Prerequisites

- [ ] `docker compose` local working system up (`kanon-api`, `conexus-api`, `allagma-api`, `ontogony-frontend`)
- [ ] `seed-and-verify-local-working-system.ps1` produced `artifacts/env-seed-001-report.json`
- [ ] Allagma/Kanon service tokens match compose `.env`

## API loop

- [ ] Health: `GET` Kanon/Allagma/Conexus `/health` → 200
- [ ] Export: `GET /allagma/v0/runs/{runId}/decision-events` → schema `ontogony-allagma-run-decision-events-v1`
- [ ] `totalCount === decisionEvents.length`
- [ ] `decisionEvents.length >= 1` for seeded baseline run
- [ ] Classify: `POST /ontology/v0/reconstructability/classify-batch` → 200
- [ ] `reports.length === decisionEvents.length`
- [ ] Order preserved: `reports[i].decisionEventId === decisionEvents[i].decisionEventId`
- [ ] Governance: every report status ∈ `{ PASS, WARN, FAIL }`
- [ ] Worst governance computed for evidence artifact

## Frontend

- [ ] `/allagma/runs/{runId}/audit` loads `run-audit-journey-page`
- [ ] `data-testid="run-decision-reconstruction-panel"` visible
- [ ] Panel shows PASS/WARN/FAIL or reconstructability copy

## Evidence

- [ ] `dec-recon-004-smoke-report.json` written with verdict PASS
- [ ] Evidence markdown updated with runId, traceId, counts

## Verdict

**PASS** only when all API + UI checks succeed on the same `runId` from ENV-SEED-001 (or documented substitute).
