# Ontogony terminology glossary

**Item:** `TERMINOLOGY-CLEANUP-001`  
**Scope:** Operator-facing and cross-repo integration docs. **Not production readiness.**

Use this glossary when writing or reviewing docs, status boards, runbooks, and evidence. Historical planning archives may retain old names only when explicitly marked **historical**.

## Program boundary

| Term | Meaning |
| --- | --- |
| **First Dockerized local working system** | The closed ENV-DOCKER milestone: compose stack, guided flow, frontend walkthrough, deterministic seed — **not** production readiness. |
| **Post-Docker-local hardening** | Work after `ENV-DOCKER-CLOSEOUT-001` (persistence, Kanon ops, trace contract, frontend gates, packaging status, terminology). Still **not** production readiness. |
| **Production readiness** | Separate future program (`PROD-READINESS-*`). Do not imply the Docker-local program satisfies production gates. |

## Product and module names

| Term | Use |
| --- | --- |
| **Allagma** | Governed execution service (runs, gates, eval, replay evidence). **Current** name. |
| **Agentor** | **Historical** name for Allagma. Allowed only in migration notes, legacy HTTP header aliases, or archival docs marked historical. Active operator docs use **Allagma**. |
| **Kanon** | Semantic authority (ontology, decisions, topology authorization, provenance). |
| **Conexus** | Model gateway (routing, fake/local provider in Docker-local program, execution journal). |
| **Athanor** | **Historical** semantic product name. Do not use in active operator docs. |
| **ontogony-frontend** | Unified operator console (Conexus + Kanon + Allagma routes). **Current** product frontend. |
| **conexus-frontend** | **Historical** split frontend. Do not use for current integration docs. |

## `@ontogony/ui` packaging

| Term | Meaning |
| --- | --- |
| **`@ontogony/ui`** | Private npm library (`ontogony-ui` repo). Built to `dist/`; consumed via public subpaths. |
| **Sibling `file:` dependency** | `"@ontogony/ui": "file:../ontogony-ui"` in `ontogony-frontend` — **not** an npm workspace root. |
| **DevRoot / six-repo layout** | Sibling folders under `C:\dev\` (platform, backends, frontend, ui). This is a **filesystem workspace**, not `npm workspaces`. |
| **`npm pack` / packed tarball** | Validation rehearsal for future registry publish — **not** an assumption that the package is published today. |
| **Build order** | `npm run build` in `ontogony-ui` **before** frontend `dev`/`build`/Docker image build. Frontend bundles **`dist/`**, not `src/`. |

See `ontogony-ui/docs/development/PACKAGING_STATUS.md`.

## Health and readiness (Conexus especially)

| Endpoint / term | Meaning |
| --- | --- |
| **Liveness** | Process up; lightweight checks. Conexus: `/health`, `/health/live`, `/live`. Use for **compose startup** and wait scripts. |
| **Readiness** | Strict dependency checks (Postgres, credentials, durable stores, provider invariants). Conexus: `/ready`. |
| **Pre-bootstrap `/ready` 503** | **Expected** before seed/bootstrap — not a compose failure. |
| **`/health` (Kanon, Allagma)** | Startup/liveness for those services in the Docker-local program. Do not conflate with Conexus strict `/ready`. |

## Trace, correlation, and decision IDs

| ID | Scope | Notes |
| --- | --- | --- |
| **`X-Ontogony-Trace-Id`** | End-to-end request lineage | Echoed on responses; Kanon `traceId` on decision records. |
| **`X-Ontogony-Correlation-Id`** | Operation-level correlation | Distinct from trace when both are set. Conexus journal `correlation_id`. |
| **Legacy trace aliases** | Migration only | `X-Agentor-Trace-Id`, `X-Athanor-Trace-Id`, `X-Conexus-Request-Id`, `X-Correlation-ID` — accepted inbound during migration; prefer canonical headers in new docs. |
| **`planningDecisionId`** | Kanon decision for run **planning** | From Allagma run start / audit bundle; **not** the same as topology authorization. |
| **`topologyAuthorizationDecisionId`** | Kanon decision for **topology authorization** | May be **null by design** on `single_workflow` / low-risk baseline paths. |
| **`routeDecisionId`** | Conexus routing decision for a model call | Expected on baseline + subject model calls in live Docker-local sanity; fixture replay may omit. |
| **`modelCallId`** | Conexus model-call record id | Links telemetry/journal to a specific provider invocation. |
| **`requestId`** | Conexus request id | Used in admin diagnostics (`execution-runs/by-request-id/{requestId}`). |

See `docs/operators/TRACE_CORRELATION_CONTRACT.md` and `post-closeout-hardening/KANON-OP-001.md`.

## Frontend: fixture vs live

| Mode | Meaning |
| --- | --- |
| **Fixture** | Query-param demo data (`dashboardFixture`, `evalFixture`, `topologyFixture`, `sandboxFixture`). Banner visible; live API skipped for that surface. |
| **Live** | No fixture query param; hooks call backends (or honest empty/error). |
| **Live sample** | Evaluations list sampled from recent Allagma runs — not a global eval catalog API. |
| **System health fallback** | `systemFixtures.ts` when `/health` probes have not returned — not operator fixture mode. |

See `ontogony-frontend/docs/operators/FRONTEND_FIXTURE_LIVE_BOUNDARY.md`.

## Provider and execution claims

| Claim | Docker-local truth |
| --- | --- |
| **Fake / local provider** | Conexus uses **fake** provider in the default program — not real external LLM execution. |
| **Real provider smoke** | Optional `ENV-REAL-PROVIDER-001` only — explicit keys; still not production. |
| **No real external execution** | Default program does not prove production provider paths. |

## Config and deploy (frontend)

| Term | Meaning |
| --- | --- |
| **`VITE_*`** | Compile-time env vars baked into the SPA at **build** — not nginx runtime injection. |
| **Secrets** | Never `VITE_*`; operator browser settings only. |
| **Runtime nginx env injection** | **Deferred** — do not document as available. |

See `ontogony-frontend/docs/operators/FRONTEND_CONFIG_OPERATOR_CONTRACT.md`.

## Related docs

| Doc | Repo |
| --- | --- |
| Trace/correlation operator contract | `docs/operators/TRACE_CORRELATION_CONTRACT.md` |
| UI packaging status | `ontogony-ui/docs/development/PACKAGING_STATUS.md` |
| Frontend fixture/live | `ontogony-frontend/docs/operators/FRONTEND_FIXTURE_LIVE_BOUNDARY.md` |
| Historical product naming (frontend `src/`) | `ontogony-frontend/docs/migration/HISTORICAL_PRODUCT_NAMING.md` |
| Evidence | `docs/evidence/TERMINOLOGY_CLEANUP_001_EVIDENCE.md` |
