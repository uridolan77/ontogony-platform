# Docker local working system — compose tree

**Boundary:** first Dockerized local working system. Development credentials only — not for staging or production.

**Wave 7 canonical entry** (from `ontogony-platform` repo root):

```powershell
./scripts/start-local-ontogony-system.ps1 -Build -OpenBrowser
./scripts/validate-local-ontogony-system.ps1
```

Golden journey: [`docs/operators/OPERATOR_V1_DEMO_GUIDE.md`](../../docs/operators/OPERATOR_V1_DEMO_GUIDE.md)

Terminology: [`docs/operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md`](../../docs/operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md) (liveness vs readiness, decision IDs, fake provider).

Programs: Docker-local and post-hardening operator milestones are **CLOSED**; see [`docs/KNOWN_LIMITATIONS.md`](../../docs/KNOWN_LIMITATIONS.md) — not production readiness.

## Layout

```text
docker/local-working-system/
  README.md
  docker-compose.yml
  .env.example
  postgres/init/          # ENV-DB-001 — first-init bootstrap SQL
  scripts/
    start-local-working-system.ps1
    wait-local-working-system.ps1
    reset-local-working-system.ps1
    verify-postgres-bootstrap.ps1
    seed-and-verify-local-working-system.ps1
    run-docker-guided-main-flow.ps1
    validate-docker-guided-main-flow.ps1
    validate-conexus-persistence-bootstrap.ps1
    run-conexus-persistence-durability-regression.ps1
    validate-conexus-persistence-durability-report.ps1
    inspect-kanon-topology-evidence.ps1
    validate-kanon-topology-evidence-report.ps1
    diagnose-kanon-topology-ops.ps1
    validate-kanon-topology-diagnostics-report.ps1
    inspect-trace-correlation-evidence.ps1
    validate-trace-correlation-evidence-report.ps1
    inspect-frontend-browser-provenance.ps1
    validate-frontend-browser-provenance-report.ps1
    verify-frontend-browser-provenance.ps1
    start-observability-stack.ps1
    verify-observability-stack.ps1
    _docker-local-env.ps1
```

Copy `.env.example` to `.env` to override placeholders locally without committing changes:

```powershell
cd C:\dev\ontogony-platform\docker\local-working-system
Copy-Item .env.example .env
```

## Compose orchestration (ENV-COMPOSE-001)

Start stack:

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\start-local-working-system.ps1 -Build
```

If Docker restore fails with NuGet TLS chain errors (`NU1301`, `PartialChain`), the
start script now probes NuGet from the SDK container and auto-injects a matching
trusted root CA from Windows cert store into .NET Docker **build and runtime** stages
(Conexus `Dockerfile` applies the same `EXTRA_CA_CERT_BASE64` at runtime for outbound
provider HTTPS). This keeps TLS verification enabled and avoids committing local CA material.

**Real provider live validation (RP-003A):** after a classified `provider_transport_error` in Docker,
run `scripts/run-rp-003a-live-provider-validation.ps1` with a local `CONEXUS_PROVIDER_OPENAI_API_KEY`.

Backend-only startup (skip frontend build when isolating backend defects):

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\start-local-working-system.ps1 -Build -SkipFrontend
```

Starts **postgres**, **kanon-api**, **conexus-api**, **aisthesis-api**, **metabole-api**, and **allagma-api** (not the frontend).

### Five-service backend ports

| Service | Host port | Health |
| --- | --- | --- |
| Kanon | 5081 | `GET /health` |
| Conexus | 5082 | `GET /health/live` |
| Allagma | 5083 | `GET /health` |
| Aisthesis | 5085 | `GET /health` |
| Metabole | 5084 | `GET /health` |

Canonical certification ports per [`metabole-dotnet/docs/system/ONTOGONY_FIVE_SERVICE_PORT_MATRIX.md`](../../../metabole-dotnet/docs/system/ONTOGONY_FIVE_SERVICE_PORT_MATRIX.md). Metabole and Aisthesis `dotnet run` defaults match **5084** and **5085** respectively.

### Frontend browser freshness (DOCKER-LOCAL-VERIFY-001)

Code review does not prove the browser shows your latest frontend commit. After changing
`ontogony-frontend`, rebuild and verify served provenance matches `git rev-parse HEAD`:

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\verify-frontend-browser-provenance.ps1 -Build
```

`-Build` is **frontend-only** when Postgres is already up: `docker compose build
ontogony-frontend` (without `--with-dependencies`), recreate that container, wait on `:5175`
only. First-time or cold stack: backends start once via `-SkipFrontend`, then the frontend image
builds. Do not pass `--with-dependencies` on build unless you intend to rebuild APIs too.

This runs CA-aware build (explicit `docker compose build ontogony-frontend` with your repo
`git` SHA, then recreates the container), probes
`GET /provenance.json` and `index.html` meta tags, checks the main JS bundle is not stale,
validates the Docker image label, and writes `artifacts/docker-local-verify-001-report.json`.

If verify reports `gitSha: local`, the frontend image was not rebuilt. The verify script must pass
`-Build` as a **switch** (`@{ Build = $true }`), not `@("-Build")` (that binds as a positional
string and skips the frontend build). For a full no-cache frontend rebuild after TLS fixes:
`start-local-working-system.ps1 -Build -ForceFrontendNoCache`.

Inspect only (stack already running):

```powershell
.\docker\local-working-system\scripts\inspect-frontend-browser-provenance.ps1
.\docker\local-working-system\scripts\validate-frontend-browser-provenance-report.ps1
```

The operator shell header shows **Build {short-sha}**; Settings → Environment lists the full SHA;
diagnostics export includes `frontend.gitSha`.

Wait-only (if stack already running):

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\wait-local-working-system.ps1
```

Reset stack + volumes:

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\reset-local-working-system.ps1 -Force
```

## Postgres bootstrap (ENV-DB-001)

Init SQL creates logical databases and service users on **first volume init only**. After adding `aisthesis_local` / `metabole_local`, reset the Postgres volume once so init SQL re-runs:

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\reset-local-working-system.ps1 -Force
```

Init SQL creates logical databases and service users:

| Database | User | Password (dev) |
| --- | --- | --- |
| `allagma_local` | `allagma_local` | `allagma_local_pw` |
| `kanon_local` | `kanon_local` | `kanon_local_pw` |
| `conexus_local` | `conexus_local` | `conexus_local_pw` |
| `aisthesis_local` | `aisthesis_local` | `aisthesis_local_pw` |
| `metabole_local` | `metabole_local` | `metabole_local_pw` |

Admin bootstrap (compose / verify script): `ontogony_admin` / `ontogony_admin_pw`.

Mount `postgres/init` read-only to `/docker-entrypoint-initdb.d` (runs only on empty data directory).

Verify without full compose:

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\verify-postgres-bootstrap.ps1 -Recreate
```

## Conexus persistence & bootstrap (operator)

**Boundary:** fake/local provider only; development credentials; **not production readiness**.

Conexus in this stack uses **PostgreSQL** (`conexus_local`). There is no `Conexus__Persistence__Mode` compose flag — persistence is enabled when `ConnectionStrings__ConexusPostgres` is set (see `docker-compose.yml` and `.env.example`).

| Setting | Default (dev) | Role |
| --- | --- | --- |
| `ConnectionStrings__ConexusPostgres` | `Host=postgres;…;Database=conexus_local;…` | Durable Conexus store |
| `CONEXUS_DEV_PROJECT_API_KEY` | `cx-dev-key-change-me` | Bootstrap issues this key on **fresh** projects (see below) |
| `CONEXUS_ADMIN_API_KEY` | `cx-conexus-admin-dev` | Admin header `X-Conexus-Admin-Key` for bootstrap |
| `CONEXUS_PROJECT_API_KEY_FOR_ALLAGMA` | `cx-dev-key-change-me` | Allagma `Conexus__ProjectApiKey` — must match dev project key |

**Migrations:** with `ASPNETCORE_ENVIRONMENT=Development`, Conexus applies EF migrations on startup. See `conexus-dotnet/docs/deployment/MIGRATION_RUNBOOK.md` and `STARTUP_AND_READINESS.md`.

### Fresh volume vs existing volume

| Volume state | Postgres init | Conexus schema | API bootstrap | Operator action |
| --- | --- | --- | --- | --- |
| **Fresh** (empty data dir) | `postgres/init` creates DB/users | Migrations on first Conexus start | **Required** — no project keys, aliases, or route evidence yet | Run seed or guided flow after `wait-local-working-system.ps1` |
| **Existing** | Skipped | Already migrated | Rows may exist (projects, API keys, providers, aliases, telemetry) | Re-seed may fail key/bootstrap checks; use `reset-local-working-system.ps1 -Force` for clean state |

### Dev bootstrap — `POST /admin/v0/dev/bootstrap`

Local/dev-only endpoint (404 in Production). Called by `seed-and-verify-local-working-system.ps1` with:

```json
{
  "projectId": "dev-project",
  "displayName": "Development Project",
  "modelAlias": "gpt-4o-mini",
  "providerKey": "fake",
  "providerModel": "fake.chat",
  "createProjectKey": true
}
```

It upserts the fake provider, model alias, price catalog entry, and dev project. When the project has **no** active API keys, it issues a key using `CONEXUS_DEV_PROJECT_API_KEY` (not a random key). That alignment fix prevents fresh-volume mismatches with Allagma’s `Conexus__ProjectApiKey`.

If the project **already has** API keys, bootstrap skips issuing a new key (warning in response). Re-running seed against an already-bootstrapped volume can then fail the `apiKey` check — reset volumes instead of fighting durable state.

### Health vs readiness

| Endpoint | Use in Docker-local | Meaning |
| --- | --- | --- |
| `/health`, `/health/live`, `/live` | **Compose healthcheck** and `wait-local-working-system.ps1` | Liveness — process and HTTP pipeline up |
| `/ready` | Manual / seed report only — **not** compose startup | Strict readiness: provider credentials, Postgres, durable stores, enabled aliases with runtime clients |

**Before bootstrap:** `/ready` returning **503 is expected** — do not point compose `healthcheck` at `/ready`.

**After bootstrap:** `/ready` may be **200** or still **503** depending on strict invariants; **route/model evidence** is the stronger local proof (see verification below).

### Operator verification

```powershell
# 1) Liveness (expect 200 when conexus-api is healthy)
curl -s http://localhost:5082/health/live

# 2) Readiness (503 before bootstrap is OK)
curl -s -w "\nHTTP %{http_code}\n" http://localhost:5082/ready

# 3) Full seed + evidence (bootstrap, runs, routeDecisionId, evals)
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\seed-and-verify-local-working-system.ps1

# 4) Guided flow report (includes baselineRouteDecisionId, subjectRouteDecisionId)
.\docker\local-working-system\scripts\run-docker-guided-main-flow.ps1
.\docker\local-working-system\scripts\validate-docker-guided-main-flow.ps1
Get-Content .\docker\local-working-system\artifacts\docker-guided-main-flow-report.json |
  ConvertFrom-Json | Select-Object baselineRouteDecisionId, subjectRouteDecisionId, verdict
```

Seed exports `docker/local-working-system/artifacts/env-seed-001-report.json` (bootstrap block + `conexusReadinessStatusAfterBootstrap`). Guided flow writes `docker-guided-main-flow-report.json` with non-empty `baselineRouteDecisionId` and `subjectRouteDecisionId` when route evidence is present.

### Conexus troubleshooting

| Symptom | Likely cause | Action |
| --- | --- | --- |
| `/health/live` **404** but container healthy | Host port **5082** hit by stale local `Conexus.Api`, not Docker | `netstat -ano \| findstr :5082`; stop host process or set `CONEXUS_HOST_PORT` in `.env` |
| `/ready` **503** before seed | Expected — no bootstrap yet | Run `seed-and-verify-local-working-system.ps1` |
| `/ready` **503** after seed | Provider rows, aliases, runtime clients, or project overrides | Inspect admin routing state; re-bootstrap after `reset … -Force` if corrupted |
| Seed fails `apiKey` mismatch | `CONEXUS_DEV_PROJECT_API_KEY` ≠ Allagma key, or existing keys on volume | Align `.env` keys; or reset volume |
| Missing `routeDecisionId` | Bootstrap/route not established; runs didn’t reach Conexus | Re-run seed; check Allagma→Conexus with project bearer key |

More Conexus detail: `conexus-dotnet/docs/development/DOCKER_LOCAL.md`, `conexus-dotnet/docs/deployment/STARTUP_AND_READINESS.md`.

### Conexus persistence validation (CONEXUS-PERSIST-002)

Repeatable validation for Postgres, migrations, key alignment, bootstrap state, and route evidence:

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\validate-conexus-persistence-bootstrap.ps1
```

Report: `docker/local-working-system/artifacts/conexus-persist-002-report.json` (local only; redacts passwords and API keys — no raw secrets).

Options:

- `-InvokeBootstrap` — force dev bootstrap when fake provider/alias missing
- `-SkipBootstrap` — detect only; fail if bootstrap state absent
- `-RequireRouteEvidence` — require `baselineRouteDecisionId` / `subjectRouteDecisionId` in guided or seed artifacts

Run after `wait-local-working-system.ps1`. For full route proof, run `run-docker-guided-main-flow.ps1` first (or pass `-RequireRouteEvidence` only when artifacts exist).

### Conexus durability regression (CONEXUS-PERSIST-003)

Proves **conexus-api** restart does not lose Postgres-backed bootstrap/routing state:

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\run-conexus-persistence-durability-regression.ps1 -SkipFrontend
```

Report: `docker/local-working-system/artifacts/conexus-persist-003-durability-report.json` (local only; no raw secrets).

Requires route evidence artifacts from seed or guided flow. Runs `CONEXUS-PERSIST-002` validator before and after restart.

## Kanon topology decision evidence (KANON-OP-001)

**Boundary:** operator visibility for Docker-local topology authorization; **not production readiness**.

### What reports contain

| Report | Topology-related fields |
| --- | --- |
| `artifacts/env-seed-001-report.json` | `topology.baselineTopologyAuthorizationDecisionId` (null), `topology.subjectTopologyAuthorizationDecisionId`, selected topologies |
| `artifacts/docker-guided-main-flow-report.json` | `subjectTopologyAuthorizationDecisionId`, `baselineRunId`, `subjectRunId` |

`planningDecisionId` is on the Allagma run object, not in guided-flow JSON. Use the inspect script or `GET /allagma/v0/runs/{runId}`.

### Why baseline auth ID is null

Baseline uses `single_workflow` without `topologyOverride`. Topology selection sets `requiresKanonAuthorization=false`, so Allagma does **not** call Kanon `POST /ontology/v0/execution-topologies/evaluate`. `topologyAuthorizationDecisionId` stays null **by design**.

### Why subject auth ID is set

Subject run context includes `topologyOverride=centralized_orchestrator`. Selection requires Kanon authorization → Allagma records `TopologyAuthorizationRequested` / `Completed` → `topologyAuthorizationDecisionId` points at Kanon `topology_policy_evaluation` decision record.

### Operator trace

```text
docker-guided-main-flow-report.json
  → subjectRunId, subjectTopologyAuthorizationDecisionId
GET /allagma/v0/runs/{subjectRunId}/events?includeTopologySummary=true
GET /ontology/v0/decision-records/{subjectTopologyAuthorizationDecisionId}   (Kanon :5081)
GET /ontology/v0/decision-records/{id}/provenance   (optional, ProvenanceReader)
```

### Inspect script

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\inspect-kanon-topology-evidence.ps1
.\docker\local-working-system\scripts\validate-kanon-topology-evidence-report.ps1
```

Report: `artifacts/kanon-op-001-topology-evidence-report.json` (local, redacted; no raw dev tokens).

Requires prior seed or guided flow and live Kanon/Allagma on host ports **5081** / **5083**.

Tokens and host ports are read from `.env` or `.env.example` (`ALLAGMA_SERVICE_TOKEN`, `KANON_SERVICE_TOKEN`, `ALLAGMA_HOST_PORT`, `KANON_HOST_PORT`). Parameter overrides remain supported.

More detail: `kanon-dotnet/docs/operators/TOPOLOGY_DECISION_EVIDENCE.md`, `docs/environments/compose-to-docker-closeout-package-v2/post-closeout-hardening/KANON-OP-001.md`.

## Kanon topology operational diagnostics (KANON-OP-002)

**Boundary:** troubleshooting only; **not production readiness**; no new topology policy behavior.

The diagnostics script classifies common failure modes:

| Diagnosis | Meaning |
| --- | --- |
| `BASELINE_NULL_BY_DESIGN` | Baseline `requiresKanonAuthorization=false` — null auth ID expected |
| `ALLAGMA_NO_KANON_CALL` | Subject should require auth but `requiresKanonAuthorization=false` |
| `SUBJECT_MISSING_AUTH_ID` | Auth required but `topologyAuthorizationDecisionId` missing on Allagma |
| `KANON_DECISION_NOT_FOUND` | HTTP 404 on decision-record lookup |
| `KANON_AUTH_FAILURE` | HTTP 401/403 — token or `ProvenanceReader` role |
| `KANON_UNAVAILABLE` | Kanon health probe failed |
| `KANON_DENY` / `KANON_HUMAN_GATE` | Policy outcome not allow |
| `ARTIFACT_MISSING` / `REPORT_STALE` | Guided/seed report missing or IDs out of date |

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\diagnose-kanon-topology-ops.ps1
.\docker\local-working-system\scripts\validate-kanon-topology-diagnostics-report.ps1
```

Report: `artifacts/kanon-op-002-topology-diagnostics-report.json` (local, redacted).

Run after seed/guided flow. Pair with KANON-OP-001 inspect when validating happy path.

More detail: `kanon-dotnet/docs/operators/TOPOLOGY_DIAGNOSTICS.md`, `docs/environments/compose-to-docker-closeout-package-v2/post-closeout-hardening/KANON-OP-002.md`.

## Cross-service trace/correlation (TRACE-CONTRACT-001)

**Boundary:** operator contract proof for Docker-local Allagma → Kanon → Conexus; **not production readiness**.

The inspect script runs a **correlation probe** (`POST /allagma/v0/runs` with distinct `X-Ontogony-Trace-Id` and `X-Ontogony-Correlation-Id`) and verifies:

| Service | Evidence |
| --- | --- |
| Allagma | Response echoes trace; `RunCreated` event payloads |
| Kanon | Planning decision + `GET /decision-records/by-trace/{traceId}` |
| Conexus | Execution journal metadata `allagma_run_id`, `correlation_id` |

When a guided/seed report exists, the script also replays the **subject run** trace against Kanon by-trace.

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\wait-local-working-system.ps1 -SkipFrontend
.\docker\local-working-system\scripts\inspect-trace-correlation-evidence.ps1
.\docker\local-working-system\scripts\validate-trace-correlation-evidence-report.ps1
```

Report: `artifacts/trace-contract-001-evidence-report.json` (local, redacted).

Env: `ALLAGMA_SERVICE_TOKEN`, `KANON_SERVICE_TOKEN`, `CONEXUS_ADMIN_API_KEY`, host ports **5081** / **5082** / **5083**.

More detail: `docs/operators/TRACE_CORRELATION_CONTRACT.md`, `docs/environments/compose-to-docker-closeout-package-v2/post-closeout-hardening/TRACE-CONTRACT-001.md`.

## Operator V1 demo prep (SYSTEM-DEMO-FLOWS-001)

After the stack is up and ENV-SEED-001 has passed at least once:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass `
  -File .\docker\local-working-system\scripts\run-operator-v1-demo-prep.ps1
```

Writes `artifacts/operator-v1-demo-ids.json` for the walkthrough in `docs/operators/OPERATOR_V1_DEMO_GUIDE.md`.

## Seed/bootstrap (ENV-SEED-001)

**Allagma schema:** with `ASPNETCORE_ENVIRONMENT=Development` and `Allagma__Persistence__Mode=Postgres`, `allagma-api` applies EF migrations at container startup. A fresh volume does not require running `allagma-dotnet/scripts/apply-allagma-postgres-migration.ps1` before seed.

`seed-and-verify-local-working-system.ps1` performs deterministic API/bootstrap steps:

- Conexus `POST /admin/v0/dev/bootstrap` (dev project + fake provider alias)
- baseline + subject Allagma runs
- Kanon topology evidence checks (`single_workflow` null baseline auth ID; centralized subject auth decision)
- Conexus route-decision evidence checks
- Allagma eval write/list + baseline comparison create/fetch
- JSON evidence report export under `docker/local-working-system/artifacts/`

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\seed-and-verify-local-working-system.ps1
```

## Dockerized guided main flow (ENV-DOCKER-RUN-001)

Runs wait → seed/bootstrap → `allagma-api` restart → persistence re-fetch → machine report validation.

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\run-docker-guided-main-flow.ps1
.\docker\local-working-system\scripts\validate-docker-guided-main-flow.ps1
```

Report: `docker/local-working-system/artifacts/docker-guided-main-flow-report.json`

## Frontend operator walkthrough (ENV-DOCKER-FE-001)

After ENV-DOCKER-RUN-001, verify the compose frontend against live report IDs:

```powershell
cd C:\dev\ontogony-frontend
.\scripts\docker\open-docker-local-operator-pages.ps1
```

Walkthrough: `ontogony-frontend/docs/development/DOCKER_LOCAL_OPERATOR_WALKTHROUGH.md`  
Evidence: `ontogony-frontend/docs/evidence/ENV_DOCKER_FE_001_OPERATOR_WALKTHROUGH_EVIDENCE.md`

## Frontend hardening (FE-HARDEN-001)

Automated HTTP SPA checks + Playwright fixture/correlation coverage beyond the manual walkthrough:

```powershell
cd C:\dev\ontogony-frontend
.\scripts\docker\inspect-docker-local-operator-frontend.ps1
.\scripts\docker\validate-docker-local-operator-frontend-report.ps1
```

Contract: `ontogony-frontend/docs/operators/FRONTEND_DOCKER_LOCAL_CONTRACT.md`  
Evidence: `ontogony-frontend/docs/evidence/FE_HARDEN_001_FRONTEND_HARDENING_EVIDENCE.md`

## Operator runtime config (RUNTIME-CONFIG-DEV-001B)

Service base URLs for the browser are served at **`/operator-runtime-config.json`** (nginx no-cache route). The file is generated before compose up and bind-mounted — **no frontend image rebuild** when changing host ports.

```powershell
# Regenerate after editing .env host ports or override file
powershell -File .\scripts\write-operator-runtime-config.ps1

# Smoke (frontend must be up on FRONTEND_HOST_PORT, default 5175)
powershell -File ..\..\scripts\smoke\assert-operator-runtime-config.ps1 -BaseUrl http://localhost:5175
```

- Generator: `ontogony-frontend/scripts/runtime-config/generate-operator-runtime-config.mjs` (profile `docker-local-nginx`).
- Output: `generated/operator-runtime-config.json` → mounted into `ontogony-frontend` nginx.
- Optional overrides: copy `config/operator-runtime-config.local.override.example.json` → `operator-runtime-config.local.override.json` (gitignored).

Docs: `ontogony-frontend/docs/development/DOCKER_RUNTIME_CONFIG.md`

## Browser API access (FE-LOCAL-CORS-001)

The operator frontend at **http://localhost:5175** calls Kanon, Conexus, and Allagma **directly from the browser** (no IIS/proxy shim). Service tokens and API keys stay in **browser localStorage** only.

When `ASPNETCORE_ENVIRONMENT=Development`, each API enables a **narrow CORS policy** for:

- `http://localhost:5175`
- `http://127.0.0.1:5175`

Allowed methods: `GET`, `POST`, `OPTIONS`. Allowed headers include `Authorization`, `Content-Type`, Ontogony actor/context headers (`X-Ontogony-Actor-Id`, `X-Ontogony-Actor-Type`, trace/correlation/tenant/idempotency), and Conexus admin/project keys. Production images keep CORS **disabled** unless explicitly configured — this is **not** production readiness.

Docker-local compose sets `Kanon__Cors__*`, `Conexus__Cors__*`, and `Allagma__Cors__*` via `.env` (`FRONTEND_CORS_ORIGIN_*`). If DevTools shows `No Access-Control-Allow-Origin`, rebuild API containers after pulling CORS changes and confirm the frontend origin matches `.env`.

Evidence: `docs/evidence/FE_LOCAL_CORS_001_DOCKER_LOCAL_BROWSER_API_EVIDENCE.md`, `docs/evidence/FE_LOCAL_OPERATOR_UX_001_EVIDENCE.md`

### Kanon operator actor (KANON-DEEPEN-001)

Docker-local frontend build defaults (also in `.env.example`):

```text
FRONTEND_VITE_KANON_ONTOLOGY_ID=gaming-core
FRONTEND_VITE_KANON_ONTOLOGY_VERSION_ID=gaming-core@0.1.0
FRONTEND_VITE_KANON_DEFAULT_ACTOR_ID=local-operator
FRONTEND_VITE_KANON_DEFAULT_ACTOR_ROLES=Auditor,ProvenanceReader
```

The operator UI sends these as `X-Ontogony-Actor-Id` / `X-Ontogony-Roles` on Kanon calls. **Auditor** grants domain-pack read; **ProvenanceReader** grants provenance read; validate/load/promote still require **Admin** or **System** on Kanon.

If `/kanon/domain-packs` shows a role card instead of packs, open **Settings → Allagma defaults** and set roles to `Auditor,ProvenanceReader`, or reset operator settings. A 403 here is **authorization**, not Kanon being down.

Evidence: `docs/evidence/KANON_DEEPEN_001_LOCAL_OPERATOR_AUTH_AND_READ_WORKBENCH_EVIDENCE.md`

Operator console health badges distinguish **live**, **readiness strict (not ready)**, **browser blocked (CORS)**, and **not configured** — not generic “degraded” without explanation. Sidebar groups collapse/expand; **Settings** stays in the pinned System section.

## Troubleshooting

### Conexus `/health/live` returns 404 but container is healthy

Usually **not** a stale Docker image. On Windows, a local `Conexus.Api` dev process may bind `127.0.0.1:5082` while Docker binds `0.0.0.0:5082`. `localhost` probes then hit the wrong process.

```powershell
netstat -ano | findstr :5082
```

Stop the stale local process, or set `CONEXUS_HOST_PORT` in `.env` to an unused port.

**Operator rule:** before Docker-local health checks, stop local services on **5081**, **5082**, **5083**, **5084**, **5085**, **5175**, or override host ports in `.env`.

### Health probes (summary)

See [Conexus persistence & bootstrap (operator)](#conexus-persistence--bootstrap-operator) for full `/ready` semantics.

```text
/health, /health/live, /live  → liveness (Docker startup / wait scripts)
/ready                        → strict readiness (503 before bootstrap is expected)
```

### Postgres host port

Default in `.env.example` is **55433** (avoids collision with local Postgres on **5432**). Change `POSTGRES_HOST_PORT` in `.env` if needed.

## Observability stack (SYSTEM-DASH-002)

Grafana, Jaeger, Prometheus, and the OTEL collector for the **three-node runtime** are defined in **`allagma-dotnet`** (`docker-compose.observability.yml`, `dashboards/grafana/ontogony-alpha-runtime.json`).

**Canonical operator entry:** [`docs/operations/SYSTEM_DASHBOARD_SLO_INDEX.md`](../../docs/operations/SYSTEM_DASHBOARD_SLO_INDEX.md).

```powershell
cd C:\dev\ontogony-platform
# Runtime APIs (Docker-local)
.\docker\local-working-system\scripts\start-local-working-system.ps1 -Build
.\docker\local-working-system\scripts\wait-local-working-system.ps1

# Observability UI (separate compose; assets in allagma-dotnet)
.\docker\local-working-system\scripts\start-observability-stack.ps1
.\docker\local-working-system\scripts\verify-observability-stack.ps1
```

- Grafana: `http://localhost:3000` (override: `$env:GRAFANA_HOST_PORT` or `-GrafanaHostPort` on start script) → **Ontogony → Ontogony Alpha Runtime**
- Jaeger: `http://localhost:16686` · Prometheus: `http://localhost:9090`

OTLP export on Docker-local API containers is **not** enabled by default. See the index § *Enable OTLP on Docker-local APIs* before expecting live metrics panels.

Evidence: [`docs/evidence/SYSTEM_DASH_002_EVIDENCE.md`](../../docs/evidence/SYSTEM_DASH_002_EVIDENCE.md).

## Docs

Canonical plan: `docs/environments/docker-local-working-system/`.
