# Allagma actionability workbench — scorecard

**Date:** 2026-05-20  
**Scale:** 1–5 (5 = strong for Docker-local operator scope)

| Category | Score | Notes |
| --- | ---: | --- |
| Backend contract alignment | 5 | OpenAPI snapshot + provenance; operations v2 routes in `Program.cs` |
| Frontend operator usefulness | 4 | Start, resume, compare, export, operations panel; gated by contract |
| Unsupported-operation honesty | 5 | Capability metadata + panel lists unavailable ops with reasons |
| Automated test confidence | 4 | Backend 10/10; frontend allagma 173/173 after 007 test fix; mocked E2E pass |
| Docker integration | 4 | Rebuild succeeded; `GET …/operations` + ENV-SEED-001 pass; bootstrap required on fresh stack |
| Browser / manual QA confidence | 4 | 007A Playwright @ :5175 — 11/11; start POST blocked when Kanon unhealthy |
| Documentation traceability | 5 | Per-slice evidence + sequence index + closeout set |
| **Overall (package closeout)** | **4.5** | Docker-local operator scope complete |

## Strengths

- End-to-end wiring for **existing** v0 mutations before inventing new ones.
- Operations v2 follows explicit design doc and state machine guards.
- Export UX on run and eval detail is operator-grade (copy/download).
- No fake retry/cancel/replay when OpenAPI or `GET …/operations` says unavailable.

## Gaps

- Running Docker `allagma-api` predates ACTION-006 until image rebuild succeeds.
- Browser manual QA not recorded on closeout date.
- Conexus OpenAPI catalog drift in `openApiSnapshotCatalog.test.ts` (orthogonal).
- Deny gate, promote baseline, bulk export remain intentionally unsupported.
