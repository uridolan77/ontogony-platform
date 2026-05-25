# REPLAY-RUNTIME-001 — Implementation notes

> **Follow-up:** Cross-service wiring and Conexus admin replay are documented in [REPLAY-RUNTIME-002](../REPLAY-RUNTIME-002/IMPLEMENTATION_NOTES.md). Platform contract: `docs/contracts/REPLAY_RUNTIME_CONTRACT.md`. Allagma orchestration scope: `allagma-dotnet/docs/contracts/CROSS_SERVICE_REPLAY.md`.

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
| Kanon replay bundle integration from Allagma (Stage 3) | **superseded by REPLAY-RUNTIME-002** — coordinator calls eligibility + bundle list/prepare |
| Conexus dry-run replay routes (Stage 4) | **superseded by REPLAY-RUNTIME-002** — four `/admin/v0/replay/*` routes in conexus-dotnet |
| Cross-service replay bundle with Kanon/Conexus attempts (Stage 5) | **partial (002)** — Kanon + Conexus model-call attempts; route-decision dry-run not from Allagma |
| Governed fake replay smoke (Stage 6) | **scripted (005)** — `run-governed-fake-replay-e2e.ps1` + platform wrapper; lock evidence optional |
| Frontend Replay Workbench `/system/replay` (Stage 6 plan) | **stale_skip** as new dense page — `allagma/replay` + Evidence Spine already exist; operator panels wired in 002A |
| OpenAPI/generated TS client for new routes | **closed (2026-05-25)** — Allagma/Conexus replay paths in frontend OpenAPI + `contracts:discipline` green |
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
| `contracts:discipline` frontend | **Pass** (2026-05-25) |
| `check-contract-discipline.ps1` platform | **Not run** |
| Governed fake replay smoke | **Not run** (deferred) |

## Acceptance status

| Criterion | Status |
| --- | --- |
| Shared replay modes | **Done** |
| Eligibility explains unavailable modes | **Done** (Allagma + Kanon decision) |
| No silent real providers/tools | **Done** (default policies + classifier messages) |
| Existing run replay compatible | **Done** |
| Cross-service orchestration with Kanon/Conexus attempts | **Partial** — Kanon bundles + Conexus model-call dry-run; route-decision dry-run not from Allagma (see REPLAY-RUNTIME-002) |
| Governed fake E2E replay smoke | **Scripted** — live stack run still operator-optional |
| Evidence Spine replay links in UI | **Done** (operator replay panels + Conexus posture) |
| Contract discipline full matrix | **Done** (frontend, 2026-05-25) |

## Deferred / follow-up

1. Postgres `ReplayRecord` persistence (in-memory repository today).
2. Live governed-fake replay E2E on docker-local stack (script exists; not lock-required).
3. Merged cross-service eligibility on `POST /allagma/v0/replay/eligibility` (REPLAY-RUNTIME-003+).
4. Route-decision dry-run from Allagma orchestration (Conexus endpoint shipped; coordinator calls model-call dry-run only).

## REPLAY-RUNTIME-001B (post second review)

| Issue | Fix |
| --- | --- |
| Delta false-divergence when snapshot built before `ManifestFingerprint` set | Pass `legacyResponse.EventFingerprint` into `ReplayDeltaBuilder` at snapshot build time |
| Bundle builder hardcoded redaction metadata | `CrossServiceReplayBundleBuilder` reads policies from `ReplayRecord` |
| Custom redaction allowed but bundle said `operator_default` | Validator now only allows `operator_default` |
| Kanon inventory `GeneratedAt` stale (`2026-05-20`) | Test pinned to `2026-05-24`; inventory regenerated |

Tests: `Get_bundle_after_create` asserts `manifest_fingerprint` comparison is `matched`.

## Follow-up fixes (post-review, same branch)

| Issue | Fix |
| --- | --- |
| Conexus ID misclassification (`chatcmpl-` / `rd-` before hyphen heuristic) | `ReplayTargetResolver` check order fixed; `ReplayTargetResolverTests` added |
| Kanon eligibility overstates available source data | `KanonReplayEligibilityService` now uses `IDecisionRecordRepository` + `IReplayBundleRepository` |
| Bundle/delta GET loses execution evidence | `ReplayExecutionSnapshot` JSON persisted on `ReplayRecord`; GET routes rehydrate |
| Safety policies ignored on create | `ReplaySafetyPolicyValidator` validates and stores policies on record + snapshot |
| Kanon route inventory stale | `OntologyV0RouteCatalog` + regenerate via `KANON_UPDATE_ROUTE_INVENTORY=1` |

## Known caveats

- Replay evidence bundle JSON schema is intentionally loose for nested `request`/`result` until OpenAPI generation lands.
- Frontend still uses legacy `POST /runs/{runId}/replay` only; new replay runtime routes not in generated client yet.
- Conexus replay eligibility/dry-run still absent.

## Package closure

**Partially closed** — Stages 1–2 and evidence-backed minimal Stage 3; correctness fixes applied for resolver, Kanon truthfulness, and Allagma rehydration. Stages 4–6 and full acceptance headline remain open.
