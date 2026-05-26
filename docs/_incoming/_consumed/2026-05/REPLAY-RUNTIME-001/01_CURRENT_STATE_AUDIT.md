# 01 — Current-state audit

## Audit caveat

This audit was prepared from current GitHub `main` refs, not by running local `C:\dev` builds. Cursor must re-check local working trees before implementation.

## 1. Ontogony Platform

### Existing replay-like concepts

Platform does not own replay execution today. It already owns the mechanics needed to make replay coherent:

- Evidence identifier contracts and taxonomy.
- System Evidence Spine contract.
- Cross-service evidence bundle schemas and fixtures.
- System evidence-spine resolution matrix and validator scripts.
- Governed fake E2E evidence artifacts.
- Runtime-lock governed fake smoke wrapper.
- Six-repo compatibility gate.
- Contract discipline standard and check scripts.
- System protocol registry.

### Existing artifacts/routes/docs to preserve

- `docs/contracts/EVIDENCE_IDENTIFIER_CONTRACT.md`
- `docs/operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md`
- `docs/operators/SYSTEM_EVIDENCE_SPINE_CONTRACT.md`
- `schemas/contracts/evidence-identifier.schema.json`
- `docs/schemas/ontogony-cross-service-evidence-spine-bundle-v1.schema.json`
- `docs/system/system-evidence-spine-resolution.matrix.json`
- `docs/system/schemas/system-evidence-spine-resolution.matrix.schema.json`
- `scripts/validate-system-evidence-spine-contract.ps1`
- `docker/local-working-system/scripts/run-evidence-spine-docker-local-verification.ps1`
- `scripts/smoke/run-runtime-lock-governed-fake-e2e.ps1`
- `.github/workflows/governed-fake-e2e-runtime-lock.yml`
- `docs/operators/RUNTIME_LOCK_GOVERNED_FAKE_E2E.md`
- `src/Ontogony.SystemCompatibility/SixRepoCompatibilityGate.cs`
- `scripts/check/check-contract-discipline.ps1`
- `docs/contracts/CONTRACT_DISCIPLINE_STANDARD.md`
- `docs/evidence/GOVERNED_FAKE_E2E_001_PASS_20260524T102932Z.md`
- `docs/evidence/artifacts/governed-fake-e2e/20260524T102932Z/evidence-graph.json`

### Missing pieces

- Shared replay runtime contract docs/schemas.
- Replay mode taxonomy registered beside Evidence Spine taxonomy.
- Replay target identifier schema.
- Replay evidence bundle schema.
- Replay result/delta schema.
- Compatibility gate awareness for replay protocol version.

### Stale claims to ignore

Ignore any incoming package or stale doc that assumes Evidence Spine is still fake-only or absent. Current platform has real Evidence Spine contracts, schemas, validators, and governed fake evidence artifacts.

## 2. Allagma.NET

### Existing replay-like concepts

Allagma already has the core runtime replay seed:

- `ReplayAgentRunService`.
- `POST /allagma/v0/runs/{runId}/replay`.
- Required idempotency key for replay.
- Terminal-run-only replay manifest rule.
- `RunReplayRequested` event.
- Replay manifest fingerprint.
- Audit route linkage: `/allagma/v0/runs/{runId}/audit`.
- Kanon replay bundle ID collection from run decisions.
- Run lifecycle, retry, cancel, resume.
- Run events query and topology summary.
- Interaction events NDJSON export and SSE stream.
- Run operations view.
- Evaluation hooks and eval scenario replayer.
- Kanon replay evidence recorder port/infrastructure.
- Governed fake E2E smoke scripts and artifact writer.
- Runtime-lock docs and cross-repo compatibility/test matrices.

### Existing endpoints/routes

Observed current API routes include:

- `POST /allagma/v0/runs`
- `GET /allagma/v0/runs/{runId}`
- `GET /allagma/v0/runs/{runId}/events`
- `GET /allagma/v0/runs/{runId}/interaction-events`
- `GET /allagma/v0/runs/{runId}/interaction-events/stream`
- `GET /allagma/v0/runs/{runId}/audit`
- `POST /allagma/v0/runs/{runId}/resume`
- `GET /allagma/v0/runs/{runId}/operations`
- `POST /allagma/v0/runs/{runId}/retry`
- `POST /allagma/v0/runs/{runId}/cancel`
- `POST /allagma/v0/runs/{runId}/replay`
- `GET /allagma/v0/runs/{runId}/evaluations`
- `GET /allagma/v0/evaluations`
- `GET /allagma/v0/capabilities`
- `GET /allagma/v0/model-purposes`
- `GET /allagma/v0/runtime/posture`

### Existing evidence artifacts

- `docs/RESTART_REPLAY_EVIDENCE.md`
- `docs/evidence/ALLAGMA_EVIDENCE_001_EVIDENCE.md`
- `docs/evidence/ALLAGMA_9_003_EVIDENCE.md`
- `docs/evidence/AGM_EVAL_001_EVAL_HARNESS_EVIDENCE.md`
- `docs/contracts/EVIDENCE_EXPORT_CANONICAL_FIELDS.md`
- `docs/contracts/AGENT_INTERACTION_AGUI_SPINE_CONTRACT.md`
- `docs/system/ONTOGONY_RUNTIME_LOCK.md`
- `docs/system/SYSTEM_COMPATIBILITY_MATRIX.md`
- `docs/system/SYSTEM_ROUTE_MATRIX.md`
- `docs/system/SYSTEM_TEST_MATRIX.md`

### Missing pieces

- Replay record entity/repository/query surface.
- Replay eligibility classifier.
- Shared replay mode enum instead of free-form `Mode` defaulting to `manifest_only`.
- Cross-service replay request/result bundle.
- Service attempt normalization for Kanon and Conexus replay subcalls.
- Delta comparison contract.
- Evidence Spine link for replay result as a first-class node.
- Replay smoke evidence generated from governed fake E2E.

### Stale claims to ignore

Ignore claims that Allagma has no replay. It has replay today, but it is manifest/evidence replay, not full execution replay.

## 3. Kanon.NET

### Existing replay-like concepts

Kanon already owns semantic replay/provenance primitives:

- Decision records.
- Decision provenance contracts and clients.
- Replay bundle factory.
- Replay bundle persistence, including Postgres repository and migrations.
- Provenance replay endpoints.
- Replay options.
- Replay export verification docs.
- Semantic decision replay acceptance scripts.
- Evidence Spine handoff docs and generated Evidence Spine entrypoints.
- Domain packs, semantic plans, canonical facts, source bindings, source-binding quality, review queue, and assistance records.

### Existing docs/files to preserve

- `docs/DECISION_PROVENANCE_AND_REPLAY.md`
- `docs/operations/REPLAY_EXPORT_VERIFICATION.md`
- `docs/e2e/KANON_SEMANTIC_DECISION_REPLAY_ACCEPTANCE.md`
- `scripts/run-semantic-decision-replay-acceptance.ps1`
- `scripts/run-semantic-decision-replay-live-acceptance.ps1`
- `db/migrations/007_replay_bundle.sql`
- `db/migrations/008_replay_bundle_integrity.sql`
- `src/Kanon.Application/Provenance/ReplayBundleFactory.cs`
- `src/Kanon.Api/Endpoints/ProvenanceReplayEndpoints.cs`
- `src/Kanon.Application/Provenance/KanonReplayOptions.cs`
- `src/Kanon.Infrastructure/Persistence/Postgres/PostgresReplayBundleRepository.cs`
- `src/Kanon.Contracts/DecisionProvenanceContracts.cs`
- `src/Kanon.Client/KanonDecisionProvenanceClient.cs`
- `docs/generated/KANON_EVIDENCE_SPINE_ENTRYPOINTS.json`
- `docs/generated/ONTOLOGY_V0_ROUTE_INVENTORY.json`
- `docs/api/kanon-openapi-v1.json`

### Missing pieces

- Explicit replay eligibility endpoint per semantic target.
- Clear distinction between semantic deterministic simulation and evidence-only replay.
- Response metadata that Allagma can embed directly into a cross-service replay result.
- Replay result link type back into Evidence Spine.
- Route inventory/OpenAPI/client coverage for any new replay endpoint.

### Stale claims to ignore

Ignore claims that Kanon replay is only planned. It has real replay bundle/provenance infrastructure. The missing piece is cross-service replay orchestration.

## 4. Conexus.NET

### Existing replay-like concepts

Conexus has replay-adjacent model gateway primitives:

- Chat completion replay store for idempotency/replay.
- EF and in-memory replay store implementations.
- Replay persistence migration.
- Idempotency acceptance script and evidence.
- Streaming documentation and streaming evidence contracts.
- Model-call evidence bundle contracts and mapping.
- Model-call evidence links metadata.
- Route-decision admin endpoints and stores.
- Governance drill-down contracts.
- Model-call interaction event projection/export.
- Usage/cost drill-down mapping.
- Provider fallback contract and provider health dashboards.
- Route inventory, OpenAPI snapshot, route catalog tests, and operator UI coverage docs.

### Existing files to preserve

- `docs/IDEMPOTENCY_AND_REPLAY.md`
- `src/Conexus.Application/Replay/IChatCompletionReplayStore.cs`
- `src/Conexus.Infrastructure/Replay/InMemoryChatCompletionReplayStore.cs`
- `src/Conexus.Persistence/EfChatCompletionReplayStore.cs`
- `src/Conexus.Persistence/Migrations/20260512221047_AddConexusChatCompletionReplay.cs`
- `docs/contracts/CONEXUS_IDEMPOTENCY_ACCEPTANCE.md`
- `scripts/run-idempotency-acceptance.ps1`
- `docs/contracts/CONEXUS_MODEL_CALL_EVIDENCE_BUNDLE.md`
- `src/Conexus.Application/Telemetry/ModelCallEvidenceBundleMapping.cs`
- `src/Conexus.Application/Telemetry/ModelCallEvidenceLinksMapping.cs`
- `docs/operators/MODEL_CALL_EVIDENCE_FLOW.md`
- `docs/operators/MODEL_CALL_INTERACTION_TIMELINE_MAPPING.md`
- `src/Conexus.Api/Endpoints/RouteDecisionAdminEndpoints.cs`
- `src/Conexus.Infrastructure/Telemetry/InMemoryRouteDecisionStore.cs`
- `src/Conexus.Persistence/EfRouteDecisionStore.cs`
- `docs/generated/CONEXUS_ROUTE_INVENTORY.json`
- `openapi/conexus-admin-v0.snapshot.json`

### Missing pieces

- A Conexus replay eligibility API for model calls, route decisions, and provider attempts.
- Explicit dry-run replay endpoints that never call real providers by default.
- Model-call replay mode classification: fake provider deterministic simulation vs real provider unavailable/evidence-only.
- Provider-attempt replay safety posture.
- Replay result snippets suitable for Allagma cross-service bundle assembly.

### Stale claims to ignore

Ignore claims that Conexus has no replay at all. It has idempotency replay storage, but it does not yet have operator replay semantics for cross-service runtime replay.

## 5. Ontogony Frontend

### Existing surfaces

The unified frontend already has most of the UI substrate:

- Evidence Spine page and workbench.
- Evidence Spine lookup bar, resolver, links, export bundle builder, export panel, fixtures, and tests.
- Agent Interaction adapters and evidence graph panels.
- Allagma Run Detail page.
- Allagma Run Audit Journey page.
- Allagma Human Gates page.
- Allagma run/gate/sandbox workbench adapters.
- Cross-service timeline event adapter.
- Kanon decision ID links and workbench adapters.
- Conexus governance/evidence panels inside Allagma audit surfaces.
- Route-workflow inventory script.
- Frontend contract discipline scripts.
- Evidence Spine Docker live E2E and golden journey E2E.

### Existing files to preserve

- `src/system/pages/EvidenceSpinePage.tsx`
- `src/evidence-spine/components/EvidenceSpineWorkbench.tsx`
- `src/evidence-spine/components/EvidenceSpineLookupBar.tsx`
- `src/evidence-spine/components/EvidenceSpineExportPanel.tsx`
- `src/evidence-spine/resolveEvidenceSpine.ts`
- `src/evidence-spine/buildEvidenceSpineExportBundle.ts`
- `src/allagma/pages/RunDetailPage.tsx`
- `src/allagma/pages/RunAuditJourneyPage.tsx`
- `src/allagma/pages/HumanGatesPage.tsx`
- `src/agent-interaction/adapters/evidenceSpineInteractionAdapter.ts`
- `src/agent-interaction/evidence/resolveAgentInteractionEvidenceGraph.ts`
- `src/app/routes.tsx`
- `scripts/lib/route-workflow-inventory.mjs`
- `e2e/evidence-spine-docker-live.spec.ts`

### Missing pieces

- Replay Workbench domain module.
- Replay eligibility summary panel.
- Replay mode selector and safety preview.
- Replay result/delta comparison panel.
- Replay result Evidence Spine links.
- Generated TypeScript schemas/client support for replay contracts.
- Route-workflow catalog entries for replay.

### Stale claims to ignore

Ignore claims that the frontend needs a brand-new evidence console. Evidence Spine and Agent Interaction already exist; replay should be integrated into them.

## 6. Ontogony UI

### Existing canonical primitives

The UI library already provides the required primitives:

- `OperatorPageFrame`
- `OperatorSignalSummary`
- `OperatorDisclosure`
- `ConfirmDialog`
- `DestructiveConfirmDialog`
- `OperatorDataTable`
- AppShell contracts and package export tests.

### Existing files to preserve

- `src/components/layout/OperatorPageFrame.tsx`
- `src/components/layout/OperatorSignalSummary.tsx`
- `src/components/layout/OperatorDisclosure.tsx`
- `src/components/dialogs/ConfirmDialog.tsx`
- `src/components/dialogs/DestructiveConfirmDialog.tsx`
- `src/components/data/OperatorDataTable.test.tsx`
- `src/components/data/OperatorDataTable.stories.tsx`
- `docs/COMPONENT_CONTRACTS.md`
- `docs/PACKAGE_EXPORTS.md`
- `src/packaging/consumer-contract.imports.test.ts`

### Missing pieces

No required new primitive is obvious. If a replay-specific composition is needed, keep it in frontend first. Promote it to UI only after repeated use proves it is general.

### Stale claims to ignore

Ignore claims that canonical UI does not exist. It exists; replay must use it.
