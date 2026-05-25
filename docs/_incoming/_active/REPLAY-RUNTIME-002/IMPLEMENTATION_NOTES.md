# REPLAY-RUNTIME-002 — Implementation notes

## Package

- **Name:** REPLAY-RUNTIME-002 (follow-up slice; no incoming zip)
- **Scope:** REPLAY-RUNTIME-001 deferred Stages 3–4 — cross-service orchestration + Conexus dry-run replay
- **Parent:** [REPLAY-RUNTIME-001](../REPLAY-RUNTIME-001/IMPLEMENTATION_NOTES.md)

## Goal

Wire Kanon replay bundles and Conexus model-call dry-run evidence into Allagma replay orchestration when a terminal run is replayed (`evidence_only` / `manifest_only`), without invoking real providers or tools.

## Repos touched

| Repo | Status |
| --- | --- |
| conexus-dotnet | Conexus admin replay routes + eligibility/dry-run services + tests |
| kanon-dotnet | `IKanonReplayRuntimeClient` on Kanon.Client |
| allagma-dotnet | `CrossServiceReplayCoordinator`, Conexus replay HTTP client, orchestration wiring |
| ontogony-platform | This note + contract doc addendum |

## Delivered

### Conexus (`/admin/v0/replay/*`)

- `POST /admin/v0/replay/eligibility`
- `POST /admin/v0/replay/model-calls/{modelCallId}/dry-run`
- `POST /admin/v0/replay/route-decisions/{routeDecisionId}/dry-run`
- `GET /admin/v0/replay/model-calls/{modelCallId}/evidence`

Default safety: `forbid_real_providers`. Dry-run never calls provider adapters.

### Kanon.Client

- `IKanonReplayRuntimeClient` → `POST /ontology/v0/replay/eligibility`

### Allagma

- `ICrossServiceReplayCoordinator` collects Kanon decision replay bundle attempts (list/prepare) and optional Conexus model-call dry-run attempts when `Conexus:AdminApiKey` or `Conexus:Admin:ApiKey` is configured.
- `ReplayOrchestrationService` appends downstream `ReplayServiceAttempt` rows on terminal run replay create.
- `ReplayDeltaBuilder` notes updated for REPLAY-RUNTIME-002 cross-service linking.

## Configuration

Allagma Conexus replay client (optional):

```json
{
  "Conexus": {
    "BaseUrl": "http://localhost:5082",
    "AdminApiKey": "<admin read key>"
  }
}
```

Falls back to `Conexus:Admin:ApiKey`. When unset, Conexus attempts are skipped (Kanon still attempted when reachable).

## Tests run

| Check | Result |
| --- | --- |
| `Conexus.Api.Tests` `ConexusReplayRuntime*` | **PASS** (2) |
| `Allagma.Tests` `ReplayRuntime*` | **PASS** (5) |

## Still deferred (REPLAY-RUNTIME-003+)

- Live governed-fake replay E2E on docker-local stack (`run-governed-fake-replay-e2e.ps1` exists; optional lock gate)
- Postgres `ReplayRecord` persistence
- Merged cross-service eligibility on `POST /allagma/v0/replay/eligibility`
- **Route-decision dry-run from Allagma orchestration** — Conexus endpoint shipped; `CrossServiceReplayCoordinator` calls **model-call dry-run only** (documented in `docs/contracts/REPLAY_RUNTIME_CONTRACT.md` and `allagma-dotnet/docs/contracts/CROSS_SERVICE_REPLAY.md`)

## REPLAY-RUNTIME-002A (contract and safety hardening)

| Fix | Status |
| --- | --- |
| Regenerate `CONEXUS_ROUTE_INVENTORY.json` + `ConexusRouteCatalog` | **Done** |
| Admin OpenAPI snapshot (`UPDATE_CONTRACT_SNAPSHOTS=true`) | **Done** |
| `ConexusReplaySafetyPolicyValidator` + 400 on bad policy/mode | **Done** |
| Route-decision dry-run defaults to `reconstructed`; blocks real providers with explicit reasons | **Done** |
| Allagma respects Kanon eligibility before bundle prepare | **Done** |
| Coordinator + orchestration bundle tests | **Done** |

Tests: Conexus replay/inventory/OpenAPI; Allagma `Replay*` + `CrossServiceReplay*`.

## Package closure

**Closed for scoped frontend replay wiring** (ontogony-frontend, verified 2026-05-25). Backend cross-service slice (Kanon bundles + Conexus model-call dry-run from Allagma orchestration) remains as documented above.

**Orchestration caveat (do not miss for UI):** wire Conexus **route-decision** dry-run via Conexus admin OpenAPI if the panel needs it; Allagma replay results will not include that attempt until REPLAY-RUNTIME-003+.

**Frontend follow-up (REPLAY-RUNTIME-002A):** `resolveAllagmaReplayTarget` is codegen-ready but `/allagma/replay` still uses local `buildReplayLookupRoot`; prefer backend resolver for target kind inference or document the exception.

Full REPLAY-RUNTIME-001 acceptance headline still requires governed fake replay smoke (REPLAY-RUNTIME-005+).

## Documentation alignment

| Document | Orchestration scope |
| --- | --- |
| `docs/contracts/REPLAY_RUNTIME_CONTRACT.md` | Platform contract + orchestration table |
| `allagma-dotnet/docs/contracts/CROSS_SERVICE_REPLAY.md` | Allagma coordinator behavior |
| `docs/_incoming/packages/REPLAY-RUNTIME-001/04_CROSS_SERVICE_REPLAY_FLOW.md` | Target flow (step 7 notes partial Conexus wiring) |
| `docs/_incoming/packages/REPLAY-RUNTIME-001/05_BACKEND_IMPLEMENTATION_PLAN.md` | Stage 4–5 status callouts |
