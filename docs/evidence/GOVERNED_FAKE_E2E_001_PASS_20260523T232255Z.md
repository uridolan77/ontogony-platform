# GOVERNED-FAKE-E2E-001 — Evidence Closure (initial)

**Status:** PASS (superseded for closure index by [GOVERNED_FAKE_E2E_001_PASS_20260524T102932Z.md](./GOVERNED_FAKE_E2E_001_PASS_20260524T102932Z.md))  
**Timestamp:** 2026-05-23T23:22:55Z  
**Package:** `GOVERNED_FAKE_E2E_001_Cursor_Package_2026-05-24` (consumed under [`docs/_incoming/_consumed/2026-05/`](../_incoming/_consumed/2026-05/GOVERNED_FAKE_E2E_001_Cursor_Package_2026-05-24/))

---

## Identity Fields (dynamic per run)

| Field | Source | Notes |
|---|---|---|
| `runId` | `POST /allagma/v0/runs` response | Captured live, not fixture |
| `traceId` | Request header `X-Ontogony-Trace-Id` | Pattern: `gov-fake-e2e-{timestamp}` |
| `correlationId` | Request header `X-Ontogony-Correlation-Id` | Pattern: `gov-fake-corr-{timestamp}` |
| `planningDecisionId` | Run response body | Kanon planning decision |
| `modelCallId` | Run response body | Conexus model call |
| `routeDecisionId` | `GET /admin/v0/model-calls/{modelCallId}` | Resolved via Conexus admin API |
| `conexusModelAlias` | Run response body | Persisted at creation time |

---

## Run Parameters

```json
{
  "ontologyVersionId": "gaming-core@0.1.0",
  "actorId": "local-operator",
  "actorType": "human",
  "actorRoles": ["Admin"],
  "modelPurpose": "summarize-player-risk",
  "context": { "playerId": "123" }
}
```

---

## Provider Resolution

| Field | Expected | Source |
|---|---|---|
| `selectedProviderKey` | `fake` | Conexus model-call record |
| `selectedProviderModel` | `fake.chat` | Conexus model-call record |
| `routeDecisionId` | non-null | Resolved via `/admin/v0/route-decisions/{id}` |

---

## Evidence Spine PASS

Graph resolved from `runId` via `/system/evidence-spine?id={runId}&kind=allagmaRunId`.

**Required nodes — all present:**

| Node Kind | Present |
|---|---|
| `allagma.run` | ✓ |
| `platform.trace` | ✓ |
| `platform.correlation` | ✓ |
| `kanon.decision` | ✓ |
| `conexus.modelCall` | ✓ |
| `conexus.routeDecision` | ✓ |
| `conexus.providerAttempt` | ✓ |

Node count: 7 required node kinds, all resolved.

---

## Agent Interaction PASS

Opened via `/system/agent-interaction?runId={runId}`.

**Assertions verified:**

- Workbench rendered (`agent-interaction-workbench` visible)
- No "fixture replay" text in status header
- Status header shows "live stream" or "live lookup"
- No `run-demo-001` text on page
- Live `runId` visible in status header
- `agent-interaction-live-summary` visible and contains `summarize-player-risk`, `gaming-core@0.1.0`
- `agent-interaction-provider-panel` visible and contains `fake`, `fake.chat`
- "Kanon plan compiled" visible
- "Conexus model completed" visible
- "Tool intent blocked" visible

---

## Commands Run

```bash
# Docker stack — services required
docker compose up allagma conexus kanon

# E2E test (Playwright, Docker-local profile)
npx playwright test e2e/governed-fake-e2e-docker-live.spec.ts --project=docker-live
```

---

## Implementing Commits

| Repo | SHA | Description |
|---|---|---|
| `allagma-dotnet` | `e9c6761` | Introduce traceId and correlationId for improved run tracking |
| `allagma-dotnet` | `e1281de` | Ensure correct alias usage in run responses |
| `allagma-dotnet` | `b59908c` | Persist ConexusModelAlias on AgentRun |
| `allagma-dotnet` | `5d61264` | Enhance run context propagation and model purpose features |
| `ontogony-frontend` | `6a5c540` | Enhance agent interaction features and improve E2E test coverage |
| `ontogony-frontend` | `cea676f` | Enhance agent interaction components and improve E2E test assertions |

---

## Known Caveats

- Run IDs in this doc are dynamic (generated per test run, not fixed).
- `AGENT-INTERACTION-LIVE-001` is next: the live workbench exists but needs deeper panels (timeline, state snapshots, Evidence Spine links per event, tool intent/result events).
- `fake` provider remains blocked in Production environment if that guard is active.
- No real OpenAI calls are made at any point in this proof.
- Docker local stack is required — this test does not run in CI without the governed-fake-e2e Docker profile.
