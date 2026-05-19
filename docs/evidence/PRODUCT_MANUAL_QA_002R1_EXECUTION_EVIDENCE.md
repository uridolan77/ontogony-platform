# PRODUCT-MANUAL-QA-002R1 — rerun execution evidence

**Recorded at (UTC):** 2026-05-19  
**Verdict:** **PASS**

## Purpose

Execute a full rerun of `PRODUCT-MANUAL-QA-002` from fresh rebuilt Docker-local images after PMQA002-001/002/003 closeout, and confirm all required product routes/checklist surfaces are healthy.

## Inputs used

- `docs/product-hardening/manual-guided-qa/`
- `docs/product-hardening/manual-guided-qa/13_RESULTS_TEMPLATE.md`
- `docs/evidence/PMQA002_001_DOCKER_REBUILD_TLS_FIX_EVIDENCE.md`
- `docs/evidence/PMQA002_002_FRONTEND_DOCKER_BUILD_FIX_EVIDENCE.md`
- `docs/evidence/PMQA002_003_FULL_REBUILT_STACK_SMOKE_EVIDENCE.md`

## Rerun execution summary

1. Fresh rebuilt stack:
   - `reset-local-working-system.ps1 -Force`
   - `start-local-working-system.ps1 -Build`
   - `wait-local-working-system.ps1`
2. Seed/guided run:
   - `seed-and-verify-local-working-system.ps1` -> PASS
   - `run-docker-guided-main-flow.ps1` -> PASS
   - `validate-docker-guided-main-flow.ps1` -> PASS
3. Supporting operator checks:
   - `inspect-trace-correlation-evidence.ps1` + validation -> PASS
   - `inspect-kanon-topology-evidence.ps1` + validation -> PASS
   - `ontogony-frontend/scripts/docker/inspect-docker-local-operator-frontend.ps1` -> PASS
4. Full endpoint probe bundle generated for rerun:
   - `docker/local-working-system/artifacts/manual-qa/2026-05-19/product-manual-qa-002r1-endpoint-probe.json`

## Route outcome summary

- All required backend/health routes returned `200`.
- All required Allagma product routes returned `200` with required Bearer service token.
- All required frontend SPA routes returned `200`.
- Kanon provenance routes and Conexus route-decision route returned `200` with required operator headers.

## Operational note

- On a fresh volume, initial seed attempt still required applying Allagma EF migrations before rerun seed/guided flow passed:
  - `allagma-dotnet/scripts/apply-allagma-postgres-migration.ps1`
- This did not change product route verdicts for the rerun; rerun acceptance checks are based on post-rebuild/post-seed route and checklist outcomes.

## Artifacts produced (rerun)

- `docker/local-working-system/artifacts/env-seed-001-report.json`
- `docker/local-working-system/artifacts/docker-guided-main-flow-report.json`
- `docker/local-working-system/artifacts/trace-contract-001-evidence-report.json`
- `docker/local-working-system/artifacts/kanon-op-001-topology-evidence-report.json`
- `docker/local-working-system/artifacts/fe-harden-001-frontend-evidence-report.json`
- `docker/local-working-system/artifacts/manual-qa/2026-05-19/product-manual-qa-002r1-endpoint-probe.json`

## Boundary

```text
This work is NOT production readiness. It does not authorize real provider mode by default,
cloud deployment, production identity/TLS/secrets, or unscoped runtime refactors.
```
