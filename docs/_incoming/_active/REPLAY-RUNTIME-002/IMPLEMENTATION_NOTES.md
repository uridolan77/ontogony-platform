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

- Governed fake replay E2E smoke (`run-governed-fake-replay-e2e.ps1`)
- Frontend OpenAPI codegen for new Allagma/Conexus replay routes
- Postgres `ReplayRecord` persistence
- Merged cross-service eligibility on `POST /allagma/v0/replay/eligibility`
- Route-decision dry-run invoked from Allagma orchestration (model-call dry-run only today)
- Conexus route inventory regeneration (manual when promoting)

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

**Ready for frontend client generation (backend contract slice)** — route inventory and admin OpenAPI snapshots include `/admin/v0/replay/*`. Full REPLAY-RUNTIME-001 acceptance headline still requires smoke + frontend wiring. Conexus route-decision dry-run from Allagma orchestration remains deferred.
