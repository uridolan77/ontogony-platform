# EVIDENCE-SPINE-009A — Docker-live QA evidence

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PARTIAL PASS** — frontend provenance + workbench route + Allagma/Kanon/route-decision APIs verified; Conexus model-call GET 404 on seeded IDs  
**Follow-up to:** [EVIDENCE_SPINE_009_CLOSEOUT_EVIDENCE.md](./EVIDENCE_SPINE_009_CLOSEOUT_EVIDENCE.md)

## Goal

Execute Docker-local verification after EVIDENCE-SPINE-009 closeout: rebuild frontend with provenance, re-seed the stack, and prove live resolver backing APIs (not only mocked Playwright).

## Prerequisites fixed (build gate)

`npm run build` in `ontogony-frontend` failed before this pass (TypeScript errors in evidence-spine adapters/tests). Minimal fixes applied so Docker image build succeeds:

- `RetryAgentRunRequestDto` default `copyContext: true`
- Evidence spine panels: `OperatorApiError` cast for `ProductLiveQueryState`
- Kanon correlation views: default `links: []`
- Test fixture shapes aligned with OpenAPI DTOs

```text
npm run build → PASS (2026-05-20)
```

## Docker provenance (DOCKER-LOCAL-VERIFY-001)

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\run-evidence-spine-docker-local-verification.ps1 -Build
```

| Check | Result |
| --- | --- |
| Frontend image rebuild | **PASS** |
| Container recreate | **PASS** |
| Provenance probe | **PASS** — git `cf09a04` at `http://localhost:5175` |
| Checklist artifact | `docker/local-working-system/artifacts/evidence-spine-008-docker-local-checklist.json` |
| Provenance report | `docker/local-working-system/artifacts/docker-local-verify-001-report.json` |

**Script fix:** `run-evidence-spine-docker-local-verification.ps1` line 56 — PowerShell parsed `(7-char prefix)` as subtraction; switched to single-quoted string.

## Stack seed (ENV-SEED-001)

```powershell
.\docker\local-working-system\scripts\seed-and-verify-local-working-system.ps1
```

**PASS** — fresh IDs written to `env-seed-001-report.json`.

## Live API verification (resolver backing routes)

Host → local compose ports. Tokens from seed script defaults.

| Lookup kind | Example ID (from seed) | Endpoint | Result |
| --- | --- | --- | --- |
| Allagma run | `run_48a8e0e958b3415284fd395ace9aa887` | `GET /allagma/v0/runs/{runId}` | **PASS** |
| Allagma eval | `eval_e6a8e380e84a4296bd9cade351188d68` | `GET /allagma/v0/evaluations/{evaluationRunId}` | **PASS** |
| Baseline comparison | `cmp_603dd5d1a11344df86cd137a76ff33ca` | `GET /allagma/v0/evaluations/baseline-comparisons/{id}` | **PASS** |
| Kanon decision | `decision_ced8c904c93f48fc8624cbf90cb2f90d` | `GET /ontology/v0/decision-records/{decisionId}` | **PASS** |
| Conexus route decision | `rd-0HNLM25K3A4HC-00000002` | `GET /admin/v0/route-decisions/{id}` | **PASS** |
| Conexus model call | `chatcmpl-…` (seed subject/baseline) | `GET /admin/v0/model-calls/{id}` | **FAIL** — HTTP 404 |
| Conexus model-call list | — | `GET /admin/v0/model-calls?limit=5` | **FAIL** — HTTP 404 |

Artifact: `docker/local-working-system/artifacts/evidence-spine-009a-live-api-report.json`

### Model-call 404 analysis

- Seed report records `subjectModelCallId` / `baselineModelCallId` from Allagma run responses.
- Route-decision admin GET succeeds for the same flow (`rd-0HNLM25K3A4HC-00000002`).
- Model-call admin list and detail return **404** on the running `conexus-api` image — likely **stale Conexus image** without CONEXUS-DEEPEN-001/002 admin routes, or model-call persistence not aligned with fake-provider seed path.
- **Operator impact:** paste model-call ID at `/system/evidence-spine` may show partial graph (route/trace via execution-run bridge) until Conexus model-call admin is live on the stack.

## Frontend route smoke

| Check | Result |
| --- | --- |
| `GET http://localhost:5175/system/evidence-spine` | **PASS** — 200, SPA shell |
| `GET http://localhost:5175/provenance.json` | **PASS** — matches `cf09a04` |

**008A** (`run-evidence-spine-008a-docker-live-verification.ps1`) now automates browser paste/export/schema checks for seeded IDs. This 009A pass predates that script; re-run 008A for full browser sign-off.

## Recommended operator follow-up

1. Rebuild `conexus-api` in `docker/local-working-system` (CONEXUS-DEEPEN admin routes).
2. Re-run seed, then paste seed IDs at `http://localhost:5175/system/evidence-spine`.
3. Record screenshots or checklist rows in a future 009B if full browser sign-off is required.

## Sign-off

- [x] Frontend Docker rebuild + provenance PASS
- [x] ENV-SEED-001 PASS after rebuild
- [x] Allagma + Kanon + route-decision live GET PASS
- [ ] Conexus model-call admin GET PASS on Docker stack
- [ ] Manual browser paste/export walkthrough recorded
