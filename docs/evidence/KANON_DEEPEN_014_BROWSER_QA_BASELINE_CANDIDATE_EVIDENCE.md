# KANON-DEEPEN-014 — Browser QA and baseline candidate

**Date:** 2026-05-20  
**Repo(s):** ontogony-platform, ontogony-frontend, kanon-dotnet, allagma-dotnet, conexus-dotnet  
**Slice:** Docker-local browser QA for Kanon deepening v2 (007–013) + baseline-candidate closeout  
**Verdict:** PASS (Docker-local operator scope)

## Summary

Rebuilt the Docker-local `ontogony-frontend` image after OpenAPI/type alignment, fixed Development CORS to allow `X-Ontogony-Roles` (browser requests from the operator console were blocked on Kanon routes that send role headers), re-ran ENV-SEED-001, and executed the 12-step Playwright manual QA suite against compose on `:5175` / `:5081` / `:5082` / `:5083`.

Kanon deepening v2 is **PASS for Docker-local operator scope**. This is **not** a production-readiness claim. Real external tool execution remains blocked. Model assistance is draft-only and non-authoritative. **`ontogony-runtime.lock.json` was not updated** — baseline SHA promotion remains a separate SYSTEM-ALPHA step after commit SHAs are pinned.

## Files changed

| Repo | Path | Purpose |
|---|---|---|
| kanon-dotnet | `src/Kanon.Api/Options/KanonCorsApplicationBuilderExtensions.cs` | Allow `X-Ontogony-Roles` in Development CORS |
| allagma-dotnet | `src/Allagma.Api/Options/AllagmaCorsApplicationBuilderExtensions.cs` | Align actor/trace headers for browser (compose rebuild pending SDK pin) |
| conexus-dotnet | `src/Conexus.Api/Program.cs` | Align Ontogony actor headers in Development CORS |
| ontogony-frontend | `e2e/kanon-deepen-014-docker-local-manual-qa.spec.ts` | 12-step Docker-local Playwright QA |
| ontogony-frontend | `src/kanon/api/kanonClient.ts`, adapters, `tsconfig.app.json` | OpenAPI-aligned DTOs; exclude `*.test.ts` from production `tsc` |
| ontogony-frontend | `playwright.docker-local.config.ts` | Register 014 spec |

## API/contract changes

| Route/contract | Change | OpenAPI updated? |
|---|---|---|
| (none new) | CORS allow-list only (mechanical browser access) | N/A |

## Tests run

```powershell
# ENV-SEED-001 (prerequisite)
cd C:\dev\ontogony-platform\docker\local-working-system
.\scripts\seed-and-verify-local-working-system.ps1

# Frontend production build (host)
cd C:\dev\ontogony-frontend
npm run build

# Docker images
cd C:\dev\ontogony-platform\docker\local-working-system
docker compose build ontogony-frontend kanon-api
docker compose up -d ontogony-frontend kanon-api

# Playwright — KANON-DEEPEN-014 (12/12)
$env:ENV_SEED_001_REPORT = 'C:\dev\ontogony-platform\docker\local-working-system\artifacts\env-seed-001-report.json'
cd C:\dev\ontogony-frontend
npx playwright test e2e/kanon-deepen-014-docker-local-manual-qa.spec.ts --config=playwright.docker-local.config.ts
```

| Command | Result |
|---|---|
| `seed-and-verify-local-working-system.ps1` | PASS (`env-seed-001-report.json`) |
| `npm run build` (frontend) | PASS |
| `docker compose build ontogony-frontend kanon-api` | PASS |
| Playwright `kanon-deepen-014-docker-local-manual-qa.spec.ts` | **12 passed** (~7s) |

### Seed anchors (2026-05-20 run)

| Field | Value |
|---|---|
| `baselineRunId` | `run_ec99e2f3310d48d8b80b7480ae0e8893` |
| `baselineModelCallId` | `chatcmpl-0HNLME1HTTHS7-00000001` |
| Ontology version (operator settings) | `gaming-core@0.1.0` |

## Manual/Docker/browser verification

**Executed:** Docker compose stack + Playwright Chromium against `http://localhost:5175`.

| # | Route / flow | Result |
|---|---|---|
| 1 | `/kanon` overview metrics + operator context | PASS |
| 2 | `/kanon/ontologies` | PASS |
| 3 | `/kanon/source-bindings` quality loop | PASS |
| 4 | `/kanon/facts` history panel | PASS |
| 5 | `/kanon/plans` history/workbench | PASS |
| 6 | `/kanon/decisions` provenance + semantic graph | PASS |
| 7 | `/kanon/domain-packs` lifecycle/evolution | PASS |
| 8 | `/kanon/assistance` draft-only posture | PASS |
| 9 | `/kanon/policies` explain (ApproveWithdrawal) | PASS |
| 10 | Allagma run → Kanon decision link | PASS |
| 11 | Conexus observability model-call lookup | PASS |
| 12 | Assistance safety (no execution CTAs) | PASS |

### Root-cause note (test 1 initial failure)

Browser `fetch` to Kanon routes with `X-Ontogony-Roles` failed CORS preflight because Development CORS did not list that header. `/health` succeeded (no role header). Fixed in `KanonCorsApplicationBuilderExtensions` and rebuilt `kanon-api` only.

## Safety

- Real external tool execution remains blocked.
- Conexus assistance is draft-only and non-authoritative on `/kanon/assistance`.
- Kanon remains semantic authority; policy explain/simulate does not persist decisions.

## Known limitations

- `docker compose build allagma-api` / `conexus-api` failed in this environment (SDK `9.0.203` vs image `9.0.314` in `allagma-dotnet/global.json`); Allagma/Conexus CORS header alignment is in source but not deployed in compose until images rebuild.
- Operator banner may still show `E2E` label while settings use Docker-local service URLs (cosmetic; URLs/tokens seeded correctly).
- Runtime lock not updated; SYSTEM-ALPHA baseline promotion requires pinned commit SHAs + separate evidence.

## Follow-up

- Rebuild `allagma-api` and `conexus-api` after SDK pin alignment; re-smoke cross-service browser flows.
- Optional: `KANON_DEEPEN_V2_*` closeout bundle docs per courageous package `06_BASELINE_AND_RELEASE_PLAN.md`.
- SYSTEM-ALPHA-004/005 candidate when lock SHAs are chosen.
