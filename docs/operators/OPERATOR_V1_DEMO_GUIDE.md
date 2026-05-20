# Operator V1 demo guide

**Issue:** `SYSTEM-DEMO-FLOWS-001`  
**Baseline:** `SYSTEM-ALPHA-006`  
**Catalog:** [`operator-demo-flow-catalog.json`](./operator-demo-flow-catalog.json)

This walkthrough helps a new operator understand Ontogony in about ten minutes. It is **Docker-local / operator scope**, not production readiness.

## Safety (read first)

```text
Real external tool execution remains blocked.
Model assistance is draft-only and non-authoritative.
Kanon remains the only semantic authority.
```

## Two ways to run

| Mode | When to use | Prerequisites |
| --- | --- | --- |
| **Mocked Playwright** | CI and fast UI proof without APIs | `cd ontogony-frontend && npm run test:e2e:demo-flows` |
| **Docker-live** | Show real cross-service ids | Compose stack on `:5175` / `:5081`–`:5083`, then demo prep below |

### Docker-live prep

```powershell
cd C:\dev\ontogony-platform
# Start compose per docker/local-working-system/README.md, then:
powershell -NoProfile -ExecutionPolicy Bypass `
  -File .\docker\local-working-system\scripts\run-operator-v1-demo-prep.ps1
```

Writes:

```text
docker/local-working-system/artifacts/operator-v1-demo-ids.json
```

Open the frontend at `http://localhost:5175`. Configure operator settings (tokens/keys) per [`ENV-SETUP-001`](../evidence/ENV_SETUP_001_LOCAL_OPERATOR_SANITY_DOCS.md).

### Mocked fixture ids (Playwright)

When APIs are mocked, use these stable ids from `ontogony-frontend` contract fixtures:

| Kind | Id |
| --- | --- |
| Run | `run-123` |
| Human gate | `gate-456` |
| Kanon decision | `decision-plan-001` |
| Conexus model call | `chatcmpl-123` |
| Route decision | `route-decision-001` |

---

## 1. System posture (`system-posture`)

**Goal:** See service health, runtime baseline, and real-external-blocked posture on one page.

| Step | Action |
| --- | --- |
| 1 | Open `/system` (or `/` — same home). |
| 2 | Confirm Conexus, Kanon, and Allagma health cards. |
| 3 | Read **Real external execution: Blocked by default** (`data-testid=operator-home-real-external-blocked`). |
| 4 | Open **Allagma runtime posture** — sandbox/tool mode labels stay observational. |

**Expected:** Healthy or honest degraded states; no claim that external tools are enabled.

**Evidence:** `ontogony-frontend/docs/evidence/SYSTEM_DEMO_FLOWS_001_EVIDENCE.md` — Playwright `system-posture`.

---

## 2. Simple governed run (`simple-governed-run`)

**Goal:** Start a governed run and open run detail.

| Step | Action |
| --- | --- |
| 1 | From home, **Start governed run** → `/allagma/runs/start`. |
| 2 | Review idempotency key and confirmation phrase **START RUN**. |
| 3 | Submit; note `runId` in the result panel. |
| 4 | **Open run detail** — timeline, Kanon/Conexus evidence links. |

**Docker-live ids:** `operator-v1-demo-ids.json` → `flows.simple-governed-run.runId` (subject run from seed).

**Evidence:** Playwright `simple-governed-run`; see also `ALLAGMA_ACTION_001` evidence for workbench depth.

---

## 3. Human gate approve (`human-gate-approve`)

**Goal:** Resolve a waiting gate with approval and resume.

| Step | Action |
| --- | --- |
| 1 | Open `/allagma/gates`. |
| 2 | Select a waiting gate; click **Approve**. |
| 3 | Type **APPROVE GATE**; confirm **Approve and resume**. |
| 4 | On partial failure scenarios, UI shows Kanon resolved but Allagma resume failed (honest boundary). |

**Mocked success path:** scenario `allagma-run-ops` completes resume.  
**Partial failure demo:** scenario `allagma-gate-partial-failure` (golden journey).

**Evidence:** Playwright `human-gate-approve`.

---

## 4. Human gate deny (`human-gate-deny`)

**Goal:** Deny a gate with explicit confirmation (no silent bypass).

| Step | Action |
| --- | --- |
| 1 | On `/allagma/gates`, click **Deny**. |
| 2 | Type **DENY GATE**; confirm **Deny and resume**. |
| 3 | Confirm UI returns to queue or shows resolve outcome (no secret leakage in errors). |

**Evidence:** Playwright `human-gate-deny`.

---

## 5. Conexus fallback (`conexus-fallback`)

**Goal:** Inspect alias fallback chain at route-decision time (gateway semantics, not Kanon meaning).

| Step | Action |
| --- | --- |
| 1 | Open `/conexus/observability` → **Lookup** tab. |
| 2 | Enter **route decision id** (seed: `routeEvidence.subjectRouteDecisionId` or mock `route-decision-001`). |
| 3 | In **Route decision explorer**, read **Fallback chain at decision time** and fallback provider readiness. |

**Docker-live:** `flows.conexus-fallback.observabilityUrl` in demo ids file.

**Evidence:** Playwright `conexus-fallback`.

---

## 6. Kanon assistance — draft only (`kanon-assistance`)

**Goal:** Request non-authoritative draft assistance; follow evidence links.

| Step | Action |
| --- | --- |
| 1 | Open `/kanon/assistance`. |
| 2 | Confirm **draft-only** / non-authoritative banners. |
| 3 | Type **DRAFT ASSIST**; **Request draft**. |
| 4 | Open Conexus observability and Evidence Spine links from the result card. |

**Docker-live:** optional `topology.subjectTopologyAuthorizationDecisionId` for decision drill-down.

**Evidence:** Playwright `kanon-assistance`; live assistance may be slow — draft path is mocked in CI.

---

## 7. Sandbox / tool-mode posture (`sandbox-posture`)

**Goal:** See sandbox execute evidence and real-external-blocked labels on a run.

| Step | Action |
| --- | --- |
| 1 | Open run detail for a seeded or mocked run with sandbox timeline. |
| 2 | Find **Sandbox evidence** panel (`allagma-sandbox-evidence-panel`). |
| 3 | Confirm **real external blocked** callout and replay-safe indicators. |

**Docker-live:** `flows.sandbox-posture.route` (baseline run from seed).  
**Mocked:** `/allagma/runs/run-123` with scenario `allagma-sandbox-evidence`.

**Evidence:** Playwright `sandbox-posture`; `ALLAGMA-SANDBOX` workbench evidence.

---

## 8. Evidence Spine trace (`evidence-spine-trace`)

**Goal:** Resolve a run id into a cross-service graph (read-only).

| Step | Action |
| --- | --- |
| 1 | Open `/system/evidence-spine`. |
| 2 | Paste **run id** (seed baseline or mock `run-123`). |
| 3 | **Resolve** — graph shows Kanon decision and Conexus model call nodes. |
| 4 | Optional: export preview (redacted bundle). |

**Docker-live:** `flows.evidence-spine-trace.lookupRunId`.  
**Deep verification:** `npm run test:e2e:docker-live:evidence-spine` in `ontogony-frontend`.

**Evidence:** Playwright `evidence-spine-trace`; `EVIDENCE_SPINE_008` / `FE_EVIDENCE_SPINE_002`.

---

## Automated validation

```powershell
# Mocked (no APIs)
cd C:\dev\ontogony-frontend
npm run test:e2e:demo-flows

# Optional Docker-live spine only
npm run test:e2e:docker-live:evidence-spine
```

## Related docs

- [`SYS_OPERATOR_HOME_001`](../../../ontogony-frontend/docs/evidence/SYS_OPERATOR_HOME_001_EVIDENCE.md) — unified home
- [`ENV_SEED_001`](../evidence/ENV_SEED_001_DETERMINISTIC_BOOTSTRAP_EVIDENCE.md) — deterministic seed
- [`FRONTEND_DOCKER_LOCAL_CONTRACT`](../../../ontogony-frontend/docs/operators/FRONTEND_DOCKER_LOCAL_CONTRACT.md) — ports and contracts

## Known limitations

- Demo prep does not enable real external tools or production IAM.
- Conexus assistance invoke on live stack can be slow; CI uses mocks.
- Demo ids change when seed is re-run; always use the latest `operator-v1-demo-ids.json`.
