# Docker local working system — compose tree

**Boundary:** first Dockerized local working system. Development credentials only — not for staging or production.

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

Init SQL creates logical databases and service users:

| Database | User | Password (dev) |
| --- | --- | --- |
| `allagma_local` | `allagma_local` | `allagma_local_pw` |
| `kanon_local` | `kanon_local` | `kanon_local_pw` |
| `conexus_local` | `conexus_local` | `conexus_local_pw` |

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

## Seed/bootstrap (ENV-SEED-001)

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

## Troubleshooting

### Conexus `/health/live` returns 404 but container is healthy

Usually **not** a stale Docker image. On Windows, a local `Conexus.Api` dev process may bind `127.0.0.1:5082` while Docker binds `0.0.0.0:5082`. `localhost` probes then hit the wrong process.

```powershell
netstat -ano | findstr :5082
```

Stop the stale local process, or set `CONEXUS_HOST_PORT` in `.env` to an unused port.

**Operator rule:** before Docker-local health checks, stop local services on **5081**, **5082**, **5083**, **5175**, or override host ports in `.env`.

### Health probes (summary)

See [Conexus persistence & bootstrap (operator)](#conexus-persistence--bootstrap-operator) for full `/ready` semantics.

```text
/health, /health/live, /live  → liveness (Docker startup / wait scripts)
/ready                        → strict readiness (503 before bootstrap is expected)
```

### Postgres host port

Default in `.env.example` is **55433** (avoids collision with local Postgres on **5432**). Change `POSTGRES_HOST_PORT` in `.env` if needed.

## Docs

Canonical plan: `docs/environments/docker-local-working-system/`.
