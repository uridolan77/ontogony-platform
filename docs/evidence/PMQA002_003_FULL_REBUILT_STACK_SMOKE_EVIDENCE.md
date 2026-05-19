# PMQA002-003 — Full rebuilt stack route/UI smoke evidence

- Date: `2026-05-19`
- Scope: Docker-local rebuilt stack coherence smoke only
- Boundary: no feature work, not production readiness, no real provider mode, no cloud deployment, no secrets

## 1) Objective

Prove that a fresh Docker-local rebuild from current `main` is coherent end-to-end before running `PRODUCT-MANUAL-QA-002R1`.

## 2) Repos synced to latest main

Pulled `origin/main` in:

- `allagma-dotnet`
- `ontogony-platform`
- `kanon-dotnet`
- `conexus-dotnet`
- `ontogony-frontend`
- `ontogony-ui`

All six were already up to date.

## 3) Execution log

Commands run:

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\reset-local-working-system.ps1 -Force
.\docker\local-working-system\scripts\start-local-working-system.ps1 -Build
.\docker\local-working-system\scripts\wait-local-working-system.ps1
.\docker\local-working-system\scripts\seed-and-verify-local-working-system.ps1
.\docker\local-working-system\scripts\run-docker-guided-main-flow.ps1
.\docker\local-working-system\scripts\validate-docker-guided-main-flow.ps1
cd C:\dev\ontogony-frontend
.\scripts\docker\inspect-docker-local-operator-frontend.ps1
```

## 4) Build/start result

- `start-local-working-system.ps1 -Build` completed successfully.
- Images built in this execution:
  - `local-working-system-conexus-api`
  - `local-working-system-kanon-api`
  - `local-working-system-allagma-api`
  - `local-working-system-ontogony-frontend`
- Health gate passed for:
  - Postgres
  - Kanon (`http://localhost:5081/health`)
  - Conexus (`http://localhost:5082/health/live`)
  - Allagma (`http://localhost:5083/health`)
  - Frontend (`http://localhost:5175/`)

## 5) Seed/bootstrap and guided flow result

- `seed-and-verify-local-working-system.ps1` verdict: **PASS**
- `run-docker-guided-main-flow.ps1` verdict: **PASS**
- `validate-docker-guided-main-flow.ps1` verdict: **PASS**
- Guided flow restart durability check (`allagma-api` restart): **PASS**

Captured IDs (from current fresh artifacts):

- `subjectRunId`: `run_cfbc7c138fd64b0b898a2147bf6803ac`
- `baselineRunId`: `run_70b528ea4ec643e4a1cd8b19c1641e25`
- `subjectEvaluationRunId`: `eval_486b3b22fa8b4fb29aa20108a090f746`
- `baselineEvaluationRunId`: `eval_616da68694184ccaa861af7e62430891`
- `baselineComparisonId`: `cmp_b3f6474297c946f4b5edd8693a8843de`
- `subjectTopologyAuthorizationDecisionId`: `decision_216f98a3d5aa4630b0dfb73b2b89ec08`
- `subjectRouteDecisionId`: `rd-0HNLLPSDJTU83-00000004`

## 6) Backend route probes

Allagma product routes require Bearer service-token auth; probes were run with:

- `Authorization: Bearer allagma-dev-service-token-change-in-production`

Probe results:

- `GET http://localhost:5083/health` -> `200`
- `GET http://localhost:5083/allagma/v0/evaluations` -> `200`
- `GET http://localhost:5083/allagma/v0/evaluations/eval_486b3b22fa8b4fb29aa20108a090f746/evidence` -> `200`
- `GET http://localhost:5083/allagma/v0/evaluations/baseline-comparisons` -> `200`
- `GET http://localhost:5083/allagma/v0/evaluation-datasets` -> `200`
- `GET http://localhost:5081/health` -> `200`
- `GET http://localhost:5082/health/live` -> `200`

## 7) Frontend route probes

Probe results:

- `GET http://localhost:5175/` -> `200`
- `GET http://localhost:5175/allagma/evaluations` -> `200`
- `GET http://localhost:5175/allagma/evaluations/baseline-comparisons` -> `200`
- `GET http://localhost:5175/allagma/evaluations/datasets` -> `200`
- `GET http://localhost:5175/allagma/replay` -> `200`

Existing frontend inspect script:

- `ontogony-frontend/scripts/docker/inspect-docker-local-operator-frontend.ps1` -> **PASS**
- Output artifact: `docker/local-working-system/artifacts/fe-harden-001-frontend-evidence-report.json`

## 8) Artifacts

- `docker/local-working-system/artifacts/env-seed-001-report.json`
- `docker/local-working-system/artifacts/docker-guided-main-flow-report.json`
- `docker/local-working-system/artifacts/fe-harden-001-frontend-evidence-report.json`

## 9) Notes

- Initial first seed attempt in this session failed before migration state was present in `allagma_local` (`relation "allagma_runs" does not exist`), then succeeded after applying Allagma EF migrations to the local Docker Postgres DB.
- This is a Docker-local operational smoke execution only and does not change the production-readiness boundary.

## 10) PMQA002-003 verdict

- **PASS**
- Fresh rebuilt stack coherence confirmed for route/UI smoke gate.
- Proceed to: `PRODUCT-MANUAL-QA-002R1` full manual QA rerun.
