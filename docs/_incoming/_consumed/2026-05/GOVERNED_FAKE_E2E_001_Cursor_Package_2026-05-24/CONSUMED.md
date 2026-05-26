# Consumed package

**Status:** implemented / closed  
**Closed:** 2026-05-24

## Evidence

- [GOVERNED_FAKE_E2E_001_PASS_20260524T102932Z.md](../../evidence/GOVERNED_FAKE_E2E_001_PASS_20260524T102932Z.md)
- [GOVERNED_FAKE_E2E_001_PASS_20260523T232255Z.md](../../evidence/GOVERNED_FAKE_E2E_001_PASS_20260523T232255Z.md)
- Artifacts: [../../evidence/artifacts/governed-fake-e2e/20260524T102932Z/](../../evidence/artifacts/governed-fake-e2e/20260524T102932Z/)

## Smoke / E2E

- `allagma-dotnet/scripts/smoke/run-governed-fake-e2e.ps1`
- `ontogony-frontend/e2e/governed-fake-e2e-docker-live.spec.ts` (Playwright docker-local, 3/3)

## Commits (representative)

- `allagma-dotnet`: trace/correlation, model purpose, Conexus alias on runs
- `ontogony-frontend`: live summary, provider panel, docker-live E2E assertions

## Remaining follow-ups

- `AGENT-INTERACTION-LIVE-001` — deepen live workbench (grouping, missing-data discipline, export bundle truth)
- `RUNTIME-LOCK-CI-GOVERNED-E2E-001` — promote to controlled CI when docker-local is stable
