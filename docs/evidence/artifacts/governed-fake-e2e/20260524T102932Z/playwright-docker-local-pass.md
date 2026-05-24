# Playwright docker-local — governed-fake-e2e-docker-live

**Date:** 2026-05-24  
**Config:** `ontogony-frontend/playwright.docker-local.config.ts`  
**Base URL:** `http://localhost:5175`

## Result

3 passed (serial suite `GOVERNED-FAKE-E2E-001 live Docker stack`)

1. Start governed fake run via API and capture identifiers
2. Evidence Spine resolves governed fake graph from run id
3. Agent Interaction opens live run stream, not fixture replay

## Prerequisite

Rebuild `ontogony-frontend` Docker image after Agent Interaction live-summary work (`AgentInteractionWorkbenchPage` bundle ~73 KB). Stale image (~56 KB) fails test 3 with missing `agent-interaction-live-summary`.

## Command

```powershell
npx playwright test -c playwright.docker-local.config.ts governed-fake-e2e-docker-live
```
