# Operator V1 demo guide — golden journey

**Wave 7 — Real local operator system**  
**Boundary:** Docker-local development credentials only. **Not production readiness.**

This is the canonical alpha demo for the six-repo Ontogony stack: one governed loop from Allagma run through Kanon decision, Conexus model call, operator UI, evidence spine, and replay/export.

---

## Prerequisites

Six sibling repos under one dev root (default `C:\dev`):

```text
ontogony-platform/
allagma-dotnet/
kanon-dotnet/
conexus-dotnet/
ontogony-frontend/
ontogony-ui/
```

Stop host processes on **5081**, **5082**, **5083**, and **5175** before starting Docker compose, or override ports in `docker/local-working-system/.env`.

---

## One command start

From `ontogony-platform`:

```powershell
./scripts/start-local-ontogony-system.ps1 -Build -OpenBrowser
```

This starts Postgres, Kanon, Conexus, Allagma, and `ontogony-frontend`, seeds governed demo data, proves Allagma restart durability, and writes `docker/local-working-system/artifacts/operator-v1-demo-ids.json`.

Quick stack-only start (no seed):

```powershell
./scripts/start-local-ontogony-system.ps1 -Quick -Build
```

---

## One command validation

```powershell
./scripts/validate-local-ontogony-system.ps1
```

Optional full evidence-spine Playwright proof:

```powershell
./scripts/validate-local-ontogony-system.ps1 -IncludeEvidenceSpineLive
```

Report: `docker/local-working-system/artifacts/local-ontogony-system-validation-report.json`

---

## Golden journey (browser)

After start, open **http://localhost:5175**. Demo IDs are in `artifacts/operator-v1-demo-ids.json`.

### 1. System posture

Route: `/system`

Confirm live health badges for Kanon, Conexus, and Allagma. The shell header shows the frontend build SHA.

### 2. Operator settings

Route: `/settings`

Confirm service base URLs point at `http://localhost:5081`–`5083`. Tokens and API keys stay in browser localStorage only — never `VITE_*` env vars.

Default Kanon roles for read workbenches: `Auditor,ProvenanceReader`.

### 3. Start governed run (Allagma)

Route: `/allagma/runs/start` or open the seeded subject run:

`/allagma/runs/{subjectRunId}`

You should see:

- run status **Completed**
- `planningDecisionId` (Kanon planning)
- `modelCallId` (Conexus fake provider)
- topology summary (subject uses `centralized_orchestrator` override)

### 4. Kanon plan / decision

Route: `/kanon/decisions`

Paste `subjectTopologyAuthorizationDecisionId` from the demo ids file or guided flow report.

Confirm a Kanon `topology_policy_evaluation` decision record with provenance link.

### 5. Conexus model call

Route: `/conexus/observability`

Open the subject model call or route decision from demo ids:

- `subjectRouteDecisionId`
- `subjectModelCallId`

Confirm route decision metadata and execution journal correlation fields.

### 6. Human gate (when applicable)

Route: `/allagma/gates`

Human gates are first-class outcomes. The seeded baseline/subject runs complete without a gate; use this route to inspect pending gates when your scenario triggers `human_gate`.

### 7. Run audit / evaluations

Routes:

- `/allagma/evaluations`
- `/allagma/evaluations/{subjectEvaluationRunId}`
- `/allagma/evaluations/baseline-comparisons/{baselineComparisonId}`

Confirm eval write/list and baseline comparison survived the Allagma container restart (ENV-DOCKER-RUN-001).

Fixture-only dashboard (labelled): `/allagma/evaluations?dashboardFixture=ci-suite`

### 8. Evidence spine graph

Route: `/system/evidence-spine`

Paste IDs from demo ids (`lookupRunId`, `modelCallId`, `routeDecisionId`, `decisionId`).

Confirm cross-service graph nodes, source attempts, and page links resolve against live APIs.

Export bundle: copy or download JSON with schema `ontogony-cross-service-evidence-spine-bundle-v1`.

### 9. Replay / export

Route: `/allagma/replay?runId={subjectRunId}`

Confirm replay posture and export bundle for the governed run.

---

## Trace / correlation proof

Automated gate: `inspect-trace-correlation-evidence.ps1` (TRACE-CONTRACT-001).

Operator check: note `X-Ontogony-Trace-Id` and `X-Ontogony-Correlation-Id` on Allagma run responses and follow the same IDs into Kanon decision records and Conexus execution journal metadata.

---

## Machine artifacts

| Artifact | Purpose |
| --- | --- |
| `artifacts/env-seed-001-report.json` | Seed/bootstrap proof |
| `artifacts/docker-guided-main-flow-report.json` | Governed flow + restart durability |
| `artifacts/operator-v1-demo-ids.json` | Browser walkthrough ID map |
| `artifacts/trace-contract-001-evidence-report.json` | Trace/correlation proof |
| `artifacts/local-ontogony-system-validation-report.json` | Wave 7 validation summary |

---

## Related docs

| Document | Purpose |
| --- | --- |
| [`../docker/local-working-system/README.md`](../docker/local-working-system/README.md) | Compose tree and troubleshooting |
| [`TRACE_CORRELATION_CONTRACT.md`](./TRACE_CORRELATION_CONTRACT.md) | Header contract |
| [`SYSTEM_EVIDENCE_SPINE_CONTRACT.md`](./SYSTEM_EVIDENCE_SPINE_CONTRACT.md) | Evidence spine index |
| [`CANONICAL_RESTART_PATH.md`](./CANONICAL_RESTART_PATH.md) | Allagma restart proof |
| [`ontogony-frontend/docs/LOCAL_DEV.md`](../../ontogony-frontend/docs/LOCAL_DEV.md) | Frontend local dev |

---

## Mocked vs Docker-local confidence

| Tier | Command | Use |
| --- | --- | --- |
| Mocked E2E | `ontogony-frontend`: `npm run test:e2e:demo-flows` | Fast UI regression |
| Docker-local | `./scripts/validate-local-ontogony-system.ps1` | Live API + SPA proof |
| Live provider | `run-rp-003a-live-provider-validation.ps1` | Optional real Conexus provider |

Wave 8 adds explicit tier labeling across CI and operator docs.
