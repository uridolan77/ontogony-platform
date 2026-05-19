# FE-AUDIT-FIXTURES-001 — Fixture / live boundary audit

## Purpose

Audit frontend fixture/live boundaries after Docker-local closeout so operators can distinguish labelled demo data from live Allagma/Kanon/Conexus responses.

## Timing

Post-`ENV-DOCKER-CLOSEOUT-001` and `FE-HARDEN-001`. Does not block the closed Docker-local milestone.

## Boundary

- Catalog, automated checks, E2E, and operator docs **first**
- No production readiness claim
- No backend contract changes unless an evidence gap is proven

## Repos

| Repo | Deliverables |
| --- | --- |
| `ontogony-frontend` | `scripts/fixture-live-audit-catalog.json`; `fixtures:audit` / `fixtures:check`; `docs/operators/FRONTEND_FIXTURE_LIVE_BOUNDARY.md`; `docs/generated/FE_FIXTURE_LIVE_BOUNDARY_AUDIT.md`; `e2e/fixture-live-boundary.spec.ts`; evidence |
| `ontogony-platform` | Spec; status board; evidence pointer |

## Acceptance criteria

| # | Topic | Expected |
| --- | --- | --- |
| 1 | Catalog | All fixture query params, surfaces, and fixture modules documented in `fixture-live-audit-catalog.json` |
| 2 | Banner wiring | Each fixture surface has a `data-testid` present in source (validated by `fixtures:check`) |
| 3 | Generated report | `FE_FIXTURE_LIVE_BOUNDARY_AUDIT.md` current in CI (`fixtures:check` in `npm run check`) |
| 4 | Operator matrix | `FRONTEND_FIXTURE_LIVE_BOUNDARY.md` explains fixture vs live per route |
| 5 | E2E boundaries | Playwright proves fixture banners on fixture routes; live routes omit fixture-only banners |
| 6 | Docker-local link | `FRONTEND_DOCKER_LOCAL_CONTRACT.md` references fixture/live matrix |
| 7 | No unjustified backend changes | Frontend + docs/scripts only |

## Operator verification

```powershell
cd C:\dev\ontogony-frontend

npm run fixtures:audit
npm run fixtures:check

npx playwright test e2e/fixture-live-boundary.spec.ts e2e/allagma-eval-dashboards.spec.ts e2e/docker-local-operator-walkthrough.spec.ts
```

## Evidence

| Repo | Path |
| --- | --- |
| `ontogony-frontend` | `docs/evidence/FE_AUDIT_FIXTURES_001_FIXTURE_LIVE_BOUNDARY_EVIDENCE.md` |
| `ontogony-platform` | `docs/evidence/FE_AUDIT_FIXTURES_001_EVIDENCE.md` |

## Follow-up

| PR | Focus |
| --- | --- |
| `FE-TEST-REPLAY-001` | DONE — `ontogony-frontend/docs/evidence/FE_TEST_REPLAY_001_REPLAY_TEST_EVIDENCE.md` |
