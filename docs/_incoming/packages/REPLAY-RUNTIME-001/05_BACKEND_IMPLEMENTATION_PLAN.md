# 05 — Backend implementation plan

## Stage 1 — Contracts only

### Ontogony Platform

Add:

- `docs/contracts/REPLAY_RUNTIME_CONTRACT.md`
- `schemas/contracts/replay-target-v1.schema.json`
- `schemas/contracts/replay-eligibility-v1.schema.json`
- `schemas/contracts/replay-request-v1.schema.json`
- `schemas/contracts/replay-result-v1.schema.json`
- `schemas/contracts/replay-delta-v1.schema.json`
- `schemas/contracts/replay-evidence-bundle-v1.schema.json`

Update:

- `docs/operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md`
- `docs/contracts/EVIDENCE_IDENTIFIER_CONTRACT.md`
- `docs/schemas/ontogony-cross-service-evidence-spine-bundle-v1.schema.json`
- `docs/system/system-protocol-registry.json`
- `docs/system/schemas/system-protocol-registry.schema.json`
- `src/Ontogony.SystemCompatibility/SixRepoCompatibilityGate.cs` only if protocol version becomes gated.

Tests:

- schema fixture validation;
- identifier taxonomy validation;
- system evidence spine contract validation;
- contract discipline check.

### Allagma

Add shared contract projections in `src/Allagma.Contracts` only:

- `ReplayRuntimeContracts.cs`
- `ReplayMode.cs`
- `ReplayTargetKind.cs`
- `ReplayEligibilityResponse.cs`
- `ReplayResultResponse.cs`
- `ReplayEvidenceReference.cs`
- `ReplaySafetyPosture.cs`

Do not add behavior yet beyond serialization/snapshot tests.

### Kanon

Add Kanon-specific replay contract mapping types if needed:

- `KanonReplayEligibilityResponse`
- `KanonReplayAttemptResult`

Wire no new behavior yet.

### Conexus

Add Conexus-specific replay contract mapping types if needed:

- `ConexusReplayEligibilityResponse`
- `ConexusRouteDecisionReplayRequest`
- `ConexusModelCallDryRunReplayRequest`

Wire no new behavior yet.

## Stage 2 — Allagma replay record + replay trigger normalization

### Goal

Upgrade current Allagma replay from a manifest-only endpoint to a replay record workflow while preserving existing behavior.

### Files likely to change

- `src/Allagma.Contracts/ReplayAgentRunRequest.cs`
- `src/Allagma.Contracts/ReplayAgentRunResponse.cs`
- `src/Allagma.Application/ReplayAgentRunService.cs`
- `src/Allagma.Application/RunOperationsIdempotency.cs`
- `src/Allagma.Application/AgentRunEventPayloadContracts.cs`
- `src/Allagma.Domain/AgentRun.cs`
- `src/Allagma.Domain/AgentRunEventTypes.cs`
- `src/Allagma.Application/Ports.cs`
- `src/Allagma.Infrastructure/Persistence/*Replay*`
- `src/Allagma.Api/Program.cs`
- tests under `tests/Allagma.Tests`.

### Add

- `ReplayRecord` domain model.
- `IReplayRecordRepository`.
- In-memory implementation.
- Postgres implementation/migration if Allagma persistent replay records are enabled in this stage.
- `ReplayEligibilityClassifier`.
- `ReplayOrchestrationService`.
- `ReplayRecordQueryService`.

### Routes

Preserve:

- `POST /allagma/v0/runs/{runId}/replay`

Add:

- `POST /allagma/v0/replay/resolve`
- `POST /allagma/v0/replay/eligibility`
- `POST /allagma/v0/replay/requests`
- `GET /allagma/v0/replay/requests/{replayId}`
- `GET /allagma/v0/replay/requests/{replayId}/bundle`
- `GET /allagma/v0/replay/requests/{replayId}/delta`

### Behavior

- Existing run replay remains terminal-run-only.
- Existing `manifest_only` maps to `evidence_only` with `legacyMode = manifest_only` for compatibility.
- Replay record captures root identifier, target kind, requested mode, actor context, safety policy, source evidence refs, service attempts, result refs.
- Replay event payloads include `replayId`, `replayMode`, `safetyPosture`, and `evidenceBundleRef`.

### Tests

- replay requires idempotency key;
- terminal run evidence-only replay creates replay record;
- non-terminal run replay is blocked with stable error envelope;
- idempotent replay request returns same replay record;
- different payload under same key conflicts;
- existing `POST /runs/{runId}/replay` contract remains compatible.

## Stage 3 — Kanon decision/provenance replay bundle integration

> **Implementation status (REPLAY-RUNTIME-002):** Wired via `CrossServiceReplayCoordinator` (eligibility gate + bundle list/prepare).

### Goal

Use Kanon’s existing replay bundle/provenance infrastructure from Allagma replay orchestration.

### Kanon files likely to change

- `src/Kanon.Api/Endpoints/ProvenanceReplayEndpoints.cs`
- `src/Kanon.Application/Provenance/ReplayBundleFactory.cs`
- `src/Kanon.Application/Provenance/DecisionProvenanceService.cs`
- `src/Kanon.Application/Abstractions.cs`
- `src/Kanon.Contracts/DecisionProvenanceContracts.cs`
- `src/Kanon.Client/IKanonDecisionProvenanceClient.cs`
- `src/Kanon.Client/KanonDecisionProvenanceClient.cs`
- route inventory/OpenAPI/generated docs/tests.

### Add routes

Exact final route names should follow existing Kanon route style, but the shape should be:

- `POST /ontology/v0/replay/eligibility`
- `POST /ontology/v0/replay/decision-records/{decisionId}`
- `GET /ontology/v0/replay/bundles/{replayBundleId}/summary`

Alternative: extend existing provenance replay endpoints if they already cover these semantics.

### Behavior

- Return Kanon replay eligibility for decision/provenance/semantic-plan targets.
- Return replay bundle refs and semantic snapshot completeness.
- Do not call Conexus assistance during replay unless the mode explicitly permits advisory dry-run and real providers remain blocked.
- Preserve Kanon as authority over semantic replay claims.

### Allagma integration

Add Kanon replay client call in Allagma orchestration:

- collect Kanon decision IDs from run;
- request eligibility;
- request replay bundle summary/export;
- include attempt in cross-service replay result.

### Tests

- Kanon semantic decision replay acceptance remains green;
- new route inventory and OpenAPI snapshots updated;
- Allagma replay result includes Kanon service attempt and replay bundle refs.

## Stage 4 — Conexus model-call/route-decision dry-run replay integration

> **Implementation status (REPLAY-RUNTIME-002):** Conexus ships all four `/admin/v0/replay/*` routes. Allagma orchestration calls **model-call dry-run only**; route-decision dry-run remains a direct Conexus admin concern until REPLAY-RUNTIME-003+. See `docs/contracts/REPLAY_RUNTIME_CONTRACT.md` and `allagma-dotnet/docs/contracts/CROSS_SERVICE_REPLAY.md`.

### Goal

Let replay reason about Conexus evidence without silently calling real providers.

### Conexus files likely to change

- `src/Conexus.Api/Endpoints/*Replay*Endpoints.cs` new or integrated with admin endpoints.
- `src/Conexus.Admin.Contracts/*Replay*Dtos.cs`
- `src/Conexus.Application/Replay/IChatCompletionReplayStore.cs`
- `src/Conexus.Application/Chat/ChatCompletionIdempotencyCoordinator.cs`
- `src/Conexus.Application/Telemetry/ModelCallEvidenceBundleMapping.cs`
- `src/Conexus.Application/Telemetry/ModelCallEvidenceLinksMapping.cs`
- `src/Conexus.Api/Endpoints/RouteDecisionAdminEndpoints.cs`
- `docs/contracts/CONEXUS_MODEL_CALL_EVIDENCE_BUNDLE.md`
- `docs/contracts/CONEXUS_EVIDENCE_SPINE_CONTRACT.md`
- `docs/generated/CONEXUS_ROUTE_INVENTORY.json`
- `openapi/conexus-admin-v0.snapshot.json`

### Add routes

- `POST /admin/v0/replay/eligibility`
- `POST /admin/v0/replay/model-calls/{modelCallId}/dry-run`
- `POST /admin/v0/replay/route-decisions/{routeDecisionId}/dry-run`
- `GET /admin/v0/replay/model-calls/{modelCallId}/evidence`

### Behavior

- Default `providerExecutionPolicy = forbid_real_providers`.
- Route decision replay may re-run routing logic against preserved config snapshots if present.
- Model-call replay may use fake/local provider only when the original or requested replay mode is fake/local and deterministic.
- Real provider attempts return `unavailable` or `safety_blocked` unless a future trust model enables explicit controlled execution.
- Provider attempt replay is evidence-only by default.

### Tests

- real provider dry-run does not invoke provider adapter;
- fake provider deterministic simulation produces stable fingerprint;
- route-decision dry-run compares selected provider/model alias;
- route inventory/OpenAPI/client coverage updated.

## Stage 5 — Cross-service replay bundle

> **Implementation status (REPLAY-RUNTIME-002):** `CrossServiceReplayBundleBuilder`, `ReplayDeltaBuilder`, and coordinator wiring are in allagma-dotnet; Conexus route-decision attempts are not collected by the coordinator yet.

### Goal

Allagma composes a replay bundle from service attempts.

### Allagma additions

- `CrossServiceReplayBundleBuilder`
- `ReplayDeltaBuilder`
- `ReplayEvidenceLinkBuilder`
- `ReplayExportService`

### Bundle contents

- replay request;
- root target;
- resolved Evidence Spine graph snapshot summary;
- service attempts;
- original evidence refs;
- replay evidence refs;
- safety posture;
- redaction metadata;
- runtime/build metadata;
- delta;
- summary.

### Tests

- bundle contains all required top-level fields;
- missing service data becomes skipped/unavailable, not failure unless required;
- Evidence Spine link set is deterministic;
- bundle redaction metadata is present.

## Stage 6 — Runtime-lock smoke evidence

### Goal

Prove a governed fake replay path without making replay a required gate too early.

### Add scripts

In Allagma or Platform, depending on existing smoke ownership:

- `scripts/smoke/run-governed-fake-replay-e2e.ps1`
- `scripts/check-governed-fake-replay-summary.ps1`

In Platform runtime-lock wrapper:

- optionally extend `scripts/smoke/run-runtime-lock-governed-fake-e2e.ps1` to include replay smoke behind `-IncludeReplay`.

### Evidence output

- `replay-request.json`
- `replay-result.json`
- `replay-evidence-bundle.json`
- `replay-delta.json`
- `replay-summary.json`
- `replay-summary.md`

### Gate policy

- Stage 1: required only contract/schema tests.
- Stage 2: Allagma replay unit/integration tests required.
- Stage 3: Kanon replay acceptance required for Kanon changes.
- Stage 4: Conexus replay/dry-run tests required for Conexus changes.
- Stage 5: cross-service replay smoke optional/manual.
- Stage 6: governed fake replay smoke may become release evidence, not default PR gate.
