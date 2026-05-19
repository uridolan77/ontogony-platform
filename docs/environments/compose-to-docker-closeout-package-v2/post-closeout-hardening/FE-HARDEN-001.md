# FE-HARDEN-001 — Frontend hardening beyond walkthrough (Docker-local)

## Purpose

Harden `ontogony-frontend` operator surfaces **after** `ENV-DOCKER-FE-001` manual walkthrough and `TRACE-CONTRACT-001` backend trace proof:

- Automated fixture-route Playwright coverage for Docker-local pages
- HTTP SPA shell + secret-pattern scan against compose frontend (**5175**)
- Run-detail operator visibility for **trace** and **correlation** identifiers (aligned with trace contract)

## Timing

Post-`ENV-DOCKER-CLOSEOUT-001` and `TRACE-CONTRACT-001`. Does not block the closed Docker-local milestone.

## Boundary

- Operator visibility, E2E, and validation scripts **first**
- No production readiness claim
- No real provider keys, no real external execution
- Fixture/live audit (`FE-AUDIT-FIXTURES-001`) and config hygiene remain separate PRs

## Repos

| Repo | Deliverables |
| --- | --- |
| `ontogony-frontend` | `CrossServiceLinksCard` + run detail correlation display; `e2e/docker-local-operator-walkthrough.spec.ts`; `e2e/allagma-run-detail-correlation.spec.ts`; `scripts/docker/inspect-docker-local-operator-frontend.ps1`; `scripts/docker/validate-docker-local-operator-frontend-report.ps1`; walkthrough + operator contract docs; evidence |
| `ontogony-platform` | README link; spec; status board; evidence pointer |

## Acceptance criteria

| # | Topic | Expected |
| --- | --- | --- |
| 1 | Fixture E2E | Playwright covers ENV-DOCKER-FE fixture routes (root, CI-suite dashboard, fixture eval/baseline detail) |
| 2 | Run correlation E2E | Run detail shows trace + correlation on cross-service card (mocked APIs) |
| 3 | HTTP evidence | `inspect-docker-local-operator-frontend.ps1` returns **PASS** with SPA shell **200** and no dev secret patterns in HTML |
| 4 | Report validator | `validate-docker-local-operator-frontend-report.ps1` accepts report schema + checks |
| 5 | Operator walkthrough | `DOCKER_LOCAL_OPERATOR_WALKTHROUGH.md` documents trace/correlation on run detail + automated gates |
| 6 | Report safety | No raw service tokens or API keys in JSON artifacts |
| 7 | No unjustified backend changes | Frontend + docs/scripts only unless evidence gap proven |

## Operator verification

Prerequisites: Docker stack healthy; **ENV-DOCKER-RUN-001** `PASS` for live-ID routes.

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\wait-local-working-system.ps1

cd C:\dev\ontogony-frontend
.\scripts\docker\inspect-docker-local-operator-frontend.ps1
.\scripts\docker\validate-docker-local-operator-frontend-report.ps1

# When compose frontend occupies 5175, set E2E_PORT=5176 for mocked Playwright
$env:E2E_PORT = "5176"
npx playwright test e2e/docker-local-operator-walkthrough.spec.ts e2e/allagma-run-detail-correlation.spec.ts
```

Fixture-only HTTP smoke (no guided report):

```powershell
.\scripts\docker\inspect-docker-local-operator-frontend.ps1 -FixtureOnly
```

## Evidence

| Repo | Path |
| --- | --- |
| `ontogony-frontend` | `docs/evidence/FE_HARDEN_001_FRONTEND_HARDENING_EVIDENCE.md` |
| `ontogony-platform` | `docs/evidence/FE_HARDEN_001_EVIDENCE.md` |

## Follow-up

| PR | Focus |
| --- | --- |
| `FE-AUDIT-FIXTURES-001` | Fixture/live boundary audit |
| `FE-TEST-REPLAY-001` | Replay/test improvements |
