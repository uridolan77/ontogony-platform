# REPLAY-RUNTIME-001 — Implementation notes

## Package

- **Name:** REPLAY-RUNTIME-001
- **Source:** `docs/_incoming/REPLAY-RUNTIME-001.zip` → `docs/_incoming/packages/REPLAY-RUNTIME-001/`
- **Local HEADs at implementation:** platform `9b615e7`, allagma `460815d`, kanon `1772f385`, conexus `9b11c5ad`, frontend `e6ec3a56`

## Repos touched

| Repo | Status |
| --- | --- |
| ontogony-platform | Stage 1 contracts/schemas/docs/tests |
| allagma-dotnet | Stage 2 replay orchestration + routes + inventory |
| kanon-dotnet | Stage 3 minimal eligibility endpoint |
| conexus-dotnet | Not changed (deferred dry-run routes) |
| ontogony-frontend | Minimal `src/replay` mode labels + tests; existing `AllagmaReplayEvidenceWorkbench` retained |
| ontogony-ui | No changes (canonical primitives already sufficient) |

## Instruction triage

| Instruction | Status |
| --- | --- |
| Shared replay vocabulary/schemas (Stage 1) | **current_and_actionable** — implemented |
| Allagma replay record + `/allagma/v0/replay/*` routes (Stage 2) | **current_and_actionable** — implemented (in-memory records) |
| Preserve `POST /runs/{runId}/replay` | **already_done** — preserved; orchestration wraps it |
| `manifest_only` → `evidence_only` mapping | **current_and_actionable** — implemented with `legacyMode` |
| Kanon replay bundle integration from Allagma (Stage 3) | **needs_adjustment** — eligibility endpoint only; no Allagma→Kanon orchestration call yet |
| Conexus dry-run replay routes (Stage 4) | **deferred** — not implemented |
| Cross-service replay bundle with Kanon/Conexus attempts (Stage 5) | **partial** — Allagma-only bundle/delta; service attempts list stubbed for non-run targets |
| Governed fake replay smoke (Stage 6) | **deferred** — script not added |
| Frontend Replay Workbench `/system/replay` (Stage 6 plan) | **stale_skip** as new dense page — `allagma/replay` + Evidence Spine already exist; added shared mode labels only |
| OpenAPI/generated TS client for new routes | **deferred** — route inventory updated for Allagma; frontend OpenAPI generation not run |
| Postgres replay record persistence | **deferred** — in-memory repository only |

## Stale assumptions

- Package README GitHub SHAs differ from local HEAD; local trees trusted.
- “Frontend needs brand-new evidence console” — **stale_skip**; Evidence Spine + `ReplayEvidencePage` already present.
- “No replay in Allagma” — **already_done** manifest replay path.

## Implementation decisions

1. Canonical C# types live in `Ontogony.Replay.Contracts`; Allagma HTTP DTOs in `Allagma.Contracts` reference them.
2. Allagma owns orchestration records; Kanon exposes only `POST /ontology/v0/replay/eligibility` in this slice.
3. Legacy run replay remains on `ReplayAgentRunService`; `ReplayOrchestrationService` creates records and calls it for `evidence_only`/`manifest_only` run targets.
4. JSON schema validation uses standalone eligibility/bundle schemas for tooling; `replay-runtime-v1.schema.json` holds full `$defs`.

## Files changed (summary)

### ontogony-platform

- `docs/contracts/REPLAY_RUNTIME_CONTRACT.md`
- `docs/operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md`
- `schemas/contracts/replay-*.schema.json`, fixtures
- `src/Ontogony.Replay.Contracts/*` (runtime vocabulary)
- `tests/Ontogony.Infrastructure.Tests/ReplayRuntimeSchemaTests.cs`

### allagma-dotnet

- `src/Allagma.Contracts/ReplayRuntimeContracts.cs`
- `src/Allagma.Domain/ReplayRecord.cs`
- `src/Allagma.Application/Replay/*`
- `src/Allagma.Infrastructure/InMemoryReplayRecordRepository.cs`
- `src/Allagma.Api/Program.cs` (replay routes)
- `tests/Allagma.Tests/ReplayRuntimeServiceTests.cs`
- `tests/Allagma.Tests/AllagmaV0RouteCatalog.cs`
- `docs/generated/ALLAGMA_V0_ROUTE_INVENTORY.json`
- `eng/Ontogony.References.props`, `Directory.Packages.props`

### kanon-dotnet

- `src/Kanon.Contracts/KanonReplayRuntimeContracts.cs`
- `src/Kanon.Application/Provenance/KanonReplayEligibilityService.cs`
- `src/Kanon.Api/Endpoints/ReplayRuntimeEndpoints.cs`
- `tests/Kanon.Tests/KanonReplayEligibilityServiceTests.cs`

### ontogony-frontend

- `src/replay/replayModeLabels.ts`, `replayModeLabels.test.ts`

## Tests / checks run

| Check | Result |
| --- | --- |
| `dotnet test` Ontogony.Infrastructure.Tests `ReplayRuntime*` | **PASS** (3) |
| `dotnet test` Allagma.Tests `ReplayRuntime*` | **PASS** (4) |
| `dotnet test` Allagma.Tests route inventory update | **PASS** (regenerated JSON) |
| `dotnet test` Kanon.Tests `KanonReplayEligibility*` | **PASS** (1) |
| `npm run test -- src/replay/replayModeLabels.test.ts` | **PASS** (2) |
| Full `dotnet test` per repo | **Not run** |
| `contracts:discipline` frontend | **Not run** |
| `check-contract-discipline.ps1` platform | **Not run** |
| Governed fake replay smoke | **Not run** (deferred) |

## Acceptance status

| Criterion | Status |
| --- | --- |
| Shared replay modes | **Done** |
| Eligibility explains unavailable modes | **Done** (Allagma + Kanon decision) |
| No silent real providers/tools | **Done** (default policies + classifier messages) |
| Existing run replay compatible | **Done** |
| Cross-service orchestration with Kanon/Conexus attempts | **Partial** |
| Governed fake E2E replay smoke | **Deferred** |
| Evidence Spine replay links in UI | **Partial** (taxonomy/docs; UI uses existing workbench) |
| Contract discipline full matrix | **Deferred** |

## Deferred / follow-up

1. Conexus `POST /admin/v0/replay/*` dry-run endpoints.
2. Allagma orchestration calls to Kanon replay bundles and Conexus evidence.
3. Postgres `ReplayRecord` persistence.
4. Frontend OpenAPI codegen + Replay Workbench panels wired to new Allagma routes.
5. `run-governed-fake-replay-e2e.ps1` smoke + runtime-lock optional gate.
6. Kanon route inventory regeneration for `/ontology/v0/replay/eligibility`.

## Known caveats

- `GET /replay/requests/{id}/bundle` rebuilds from stored record without re-attaching legacy `ReplayAgentRunResponse` to delta comparisons.
- Target resolver heuristics may mis-classify opaque IDs as `platform.trace`.
- Replay evidence bundle JSON schema is intentionally loose for `request`/`result` nested objects until OpenAPI generation lands.

## Package closure

**Partially closed** — Stages 1–2 and minimal Stage 3 eligibility are implemented with tests; Stages 4–6 and full acceptance headline remain open.
