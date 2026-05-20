# ALLAGMA-ACTION-001 — Closeout (start run workbench)

**Date:** 2026-05-20  
**Verdict:** **PASS** (functional); **001A cleanup** applied for review follow-ups  
**Prior audit:** [ALLAGMA_ACTION_000_CURRENT_STATE_AUDIT.md](./ALLAGMA_ACTION_000_CURRENT_STATE_AUDIT.md)

## Delivered

| Capability | Status |
| --- | --- |
| Route `/allagma/runs/start` | Shipped |
| `POST /allagma/v0/runs` client + idempotency header | Shipped |
| Presets + operator form + result card | Shipped |
| OpenAPI `modelPurpose` + extended `AgentRunResponse` | Shipped |

## Evidence index

| Repo | Artifact |
| --- | --- |
| `ontogony-frontend` | [ALLAGMA_ACTION_001_START_RUN_WORKBENCH_EVIDENCE.md](https://github.com/uridolan77/ontogony-frontend/blob/main/docs/evidence/ALLAGMA_ACTION_001_START_RUN_WORKBENCH_EVIDENCE.md) |
| `ontogony-platform` | [ALLAGMA_ACTION_001_START_RUN_WORKBENCH_EVIDENCE.md](../evidence/ALLAGMA_ACTION_001_START_RUN_WORKBENCH_EVIDENCE.md) |
| `ontogony-platform` | [ALLAGMA_ACTION_001A_START_RUN_CLEANUP_EVIDENCE.md](../evidence/ALLAGMA_ACTION_001A_START_RUN_CLEANUP_EVIDENCE.md) |

## Review follow-ups (001A)

| Finding | Resolution |
| --- | --- |
| `blocked` not routed to human gates | `resolveStartRunNextAction` uses `isAllagmaResumeAvailable` |
| Streaming preset UX | Renamed/clarified: backend smoke, no live chunk UI |
| Preset-only context | Explicit copy on workbench |
| Platform evidence index | This doc + `docs/evidence/README.md` section |
| Semantic Conexus aliases | `risk-summary-v0` / `risk-summary-stream-v0` in Allagma config + local seed bootstrap |
| OpenAPI required fields | `AgentRunResponse` requires `ontologyVersionId`, `actorId` |
| Submit error handling | `try/catch` on `mutateAsync` |
| Reset from settings | Button on workbench |

## CI

`ontogony-frontend/.github/workflows/ci.yml` runs typecheck, Vitest, and Playwright on `main` push and app-path PRs. Verify on the merge commit via GitHub Actions.

## Next

**ALLAGMA-ACTION-002** — Human-gate resume workbench polish.
