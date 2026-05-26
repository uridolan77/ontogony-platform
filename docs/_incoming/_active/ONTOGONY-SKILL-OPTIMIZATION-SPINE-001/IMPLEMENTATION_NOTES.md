# ONTOGONY-SKILL-OPTIMIZATION-SPINE-001 — Implementation Notes

**Package unpacked:** 2026-05-26  
**Active directory:** `docs/_incoming/_active/ONTOGONY-SKILL-OPTIMIZATION-SPINE-001/`  
**Original zip preserved:** `docs/_incoming/ONTOGONY-SKILL-OPTIMIZATION-SPINE-001 (1).zip`

---

## Package files discovered

| File | Purpose |
|------|---------|
| `README.md` | Core thesis, repository targets, non-negotiable rules |
| `01_EXECUTIVE_BRIEF.md` | Strategic rationale and success criteria |
| `02_TARGET_ARCHITECTURE.md` | Cross-service data ownership and evidence graph shape |
| `03_contracts/*.md` (8 files) | Canonical contract specs for all SkillOpt objects |
| `04_backend/ALLAGMA_IMPLEMENTATION.md` | Allagma orchestration roles and endpoints |
| `04_backend/KANON_IMPLEMENTATION.md` | Kanon governance roles and validation service |
| `04_backend/CONEXUS_IMPLEMENTATION.md` | Conexus optimizer/target model roles and injection metadata |
| `04_backend/MIGRATION_AND_STORAGE_NOTES.md` | DB migration guidance |
| `04_backend/PLATFORM_INTEGRATION.md` | Platform alignment notes |
| `05_frontend/*.md` (3 files) | Skill Lab UX, state, console integration |
| `06_testing/*.md` (4 files) | Acceptance gates, fixtures, evaluation harness, contract drift |
| `07_rollout/*.md` (3 files) | Phased execution plan, risks, implementation order |
| `08_source_notes/SKILLOPT_PAPER_MAPPING.md` | Mapping from SkillOpt paper to Ontogony |
| `10_prompts/*.md` (5 files) | Repo-specific implementation prompts |
| `99_CURSOR_FINAL_CHECKLIST.md` | Final acceptance checklist |
| `MANIFEST.json` | Package manifest with file checksums |
| `templates/*.json` (8 files) | JSON schemas and fixture templates |
| `examples/*.md/.json` (4 files) | Example skill, evaluation, and rejection artifacts |

---

## Current repo SHA at implementation start

| Repo | SHA | Branch |
|------|-----|--------|
| `ontogony-platform` | `5c296d83c501466816887f0b8639cc1387e440ba` | `main` |
| `allagma-dotnet` | `cf55cb5bf926e4b92ab20922f637fb0d65d0ceb3` | `main` |
| `kanon-dotnet` | `cd64caffaa14a6a2d20d1fd6d51ddf28df3b5477` | `main` |
| `conexus-dotnet` | `75e3bac010e0982d944cf7dcb08ad84020939c7a` | `main` |
| `ontogony-frontend` | `2c81fb5d6293614e6c68a23527560a914e202c84` | `main` |
| `ontogony-ui` | `1cedf18b32f0290c2679405eb99a4be201d29ef5` | `main` |

---

## Current-state assessment (as of 2026-05-26)

### 1. Decision Reconstructability Closure — CLOSED

The reconstructability spine is fully operational across all three backend repos.

| Component | Status | Key files |
|-----------|--------|-----------|
| Allagma decision-event emission | ✅ | `DecisionEventsEndpoints.cs`, `AllagmaDecisionEventProjector.cs` |
| Allagma replay evidence recorder | ✅ | `IKanonReplayEvidenceRecorder.cs`, `KanonReplayEvidenceRecorder.cs` |
| Conexus decision-event emitters | ✅ | commit `75e3bac` "enhance decision event handling and Kanon classifier integration" |
| Kanon reconstructability classifier | ✅ | `DecisionReconstructabilityClassifier.cs`, `DecisionReconstructabilityService.cs` |
| Kanon classify-batch endpoint | ✅ | `ReconstructabilityEndpoints.cs` |
| Kanon report artifact persistence | ✅ | `PostgresReconstructabilityReportArtifactRepository.cs` |
| Golden trace scripts and evidence | ✅ | `docs/evidence/ONTOGONY_RECONSTRUCTABILITY_CLOSURE_OPTION1_CLOSEOUT.md` |

Skill optimization **must** link to these artifacts for evidence reconstructability. Do not bypass them.

### 2. Evaluation Infrastructure — IMPLEMENTED (Allagma)

Allagma has a full offline evaluation harness ready for skill optimization to build on.

| Component | Status |
|-----------|--------|
| Eval harness runner (`EvalHarnessRunner`) | ✅ |
| Eval judge orchestrator (`EvalJudgeOrchestrator`) | ✅ |
| Evaluation datasets with persistence | ✅ `PostgresManagedEvaluationDatasetStore` |
| Baseline comparison service | ✅ `BaselineComparisonService` |
| Eval evidence export bundle | ✅ `GetEvalEvidenceExportBundleService` |
| Eval CI regression gate | ✅ `EvalCiRegressionGateEvaluator` |
| Platform evaluation contracts | ✅ `Ontogony.Evaluation.Contracts` |

The `SkillEvaluationGate` in this package maps directly onto Allagma's existing gate/eval infrastructure. The skill optimization run should **link** evaluation run IDs rather than invent a parallel scoring system.

### 3. Budget / Cost Governance — IMPLEMENTED

| Component | Status | Location |
|-----------|--------|----------|
| Allagma budget policies and run snapshots | ✅ | `BudgetEndpoints.cs`, `RunBudgetGovernor.cs` |
| Allagma model-purpose budget policy | ✅ | `ModelPurposeBudgetPolicy.cs` |
| Conexus usage-cost admin routes | ✅ | `UsageCostAdminEndpoints.cs`, `UsageCostDrilldownDtos.cs` |
| Conexus budget governance endpoints | ✅ | `BudgetGovernanceEndpoints.cs` |

Cost/usage for optimizer model calls must flow through existing Conexus telemetry. Do not fabricate or duplicate.

### 4. Platform 001A — COMPLETE

The 001A slice (contract docs + JSON schemas + fixtures + schema tests) is already done in `ontogony-platform`:

| Item | Path |
|------|------|
| Protocol index | `docs/protocols/SKILL_OPTIMIZATION_SPINE.md` |
| Lifecycle doc | `docs/protocols/SKILL_ARTIFACT_LIFECYCLE.md` |
| 8 contract docs | `docs/contracts/SKILL_*_V0.md` |
| 7 JSON schemas | `docs/schemas/skill-optimization/` |
| Fixtures | `docs/schemas/fixtures/skill-optimization/` |
| Schema tests | `tests/Ontogony.Infrastructure.Tests/SkillOptimizationSpineSchemaTests.cs` |

**001A is not a pending todo.** Verify it passes before proceeding with 001B.

---

## Contract home decision

The platform's stated charter is "shared mechanics only, no product semantics." SkillArtifact governance is product-level.

**Decision: service-local contracts, not a new platform library.**

| Contract family | Home |
|-----------------|------|
| `SkillArtifactContract`, `SkillVersionContract`, `SkillOptimizationRunContract`, `SkillEvaluationGateContract`, `RejectedSkillEditBufferContract`, `SkillDeploymentBindingContract` | `Allagma.Contracts` (Allagma orchestrates these objects) |
| `SkillEditEvaluationRequest`, `SkillEditEvaluationResult`, `SkillEditDecisionRecord`, skill governance decision types | `Kanon.Contracts` (Kanon governs skill edits) |
| Skill injection metadata on model calls | `Conexus.Admin.Contracts` (Conexus owns model-call metadata) |

This is consistent with how existing `AllagmaEvaluationContracts.cs` and `KanonDecisionLifecycleContracts.cs` are organized. A shared platform library would be appropriate only if multiple services need to deserialize the same contract types — which is not true here.

**If that changes (e.g., frontend or another service deserializes these directly), revisit and extract to `Ontogony.SkillOptimization.Contracts` at that time.**

---

## Explicit non-goals for this entire package

1. No live self-modifying agents.
2. No production/live skill mutation.
3. No optimized skill candidates affecting real runs without explicit deployment binding.
4. No hidden chain-of-thought storage.
5. No fabricated scores, cost data, or reconstructability evidence.
6. Allagma does not own model routing or provider pricing.
7. Conexus does not own semantic governance.
8. Kanon does not orchestrate execution.
9. No live deployment binding active by default for any candidate version.

---

## Implementation sequence

```text
001A — Platform contract docs/schemas/fixtures  [DONE]
001B — Kanon fake governance vertical slice      [IN PROGRESS]
001C — Allagma offline optimization run registry [DEFERRED]
001D — Conexus optimizer/target metadata         [DEFERRED]
001E — Frontend Skill Lab                        [DEFERRED]
```

---

## Slice 001A — Contracts and fixtures (COMPLETE)

| Item | Path |
|------|------|
| Protocol index | `docs/protocols/SKILL_OPTIMIZATION_SPINE.md` |
| Lifecycle | `docs/protocols/SKILL_ARTIFACT_LIFECYCLE.md` |
| Contracts (8) | `docs/contracts/SKILL_*_V0.md` |
| JSON schemas (7) | `docs/schemas/skill-optimization/` |
| Fixture | `docs/schemas/fixtures/skill-optimization/fixture-skill-optimization-run-example.json` |
| Schema tests | `tests/Ontogony.Infrastructure.Tests/SkillOptimizationSpineSchemaTests.cs` |

**Verification:**
```powershell
cd C:\dev\ontogony-platform
dotnet test tests/Ontogony.Infrastructure.Tests -c Release --filter FullyQualifiedName~SkillOptimizationSpineSchemaTests
```

---

## Slice 001B — Kanon fake governance vertical slice (IN PROGRESS)

**Scope:** Kanon owns the governance decision shape for skill edits. This slice defines that shape with a deterministic fake validation service and a single stubbed endpoint. No Allagma orchestration, no Conexus injection, no frontend.

**Hard boundaries for 001B:**
- No live skill deployment.
- No autonomous mutation.
- No Allagma orchestration endpoints.
- No Conexus runtime injection.
- No production optimizer loop.
- No duplicate contracts from 001A platform docs.

### Intended changes

#### `kanon-dotnet`

1. **`src/Kanon.Contracts/SkillGovernanceContracts.cs`** — New file  
   - `SkillEditOperations` (static class: `add`, `delete`, `replace`)
   - `SkillEditRiskClasses` (static class: `low`, `medium`, `high`, `blocked`)
   - `SkillEditDecisionStatuses` (static class: `accepted`, `rejected`, `deferred`)
   - `SkillEditRejectionReasons` (static class: codes)
   - `SkillEditEvaluationRequest` (record)
   - `SkillEditProposalContract` (record: bounded edit proposal)
   - `SkillEditEvidenceRef` (record: typed reference to evidence)
   - `SkillEditEvaluationResult` (record)
   - `SkillEditDecisionRecord` (record: governed outcome with safe rationale only)

2. **`src/Kanon.Application/SkillGovernance/SkillEditGovernanceService.cs`** — New file  
   Deterministic fake validation service. No live model calls.
   - Validates known operation types
   - Rejects full-rewrite proposals (no section path)
   - Rejects missing evidence refs
   - Rejects protected-section mutations
   - Defers when evidence count is insufficient
   - Returns accepted for valid bounded edits with sufficient evidence

3. **`src/Kanon.Api/Endpoints/SkillGovernanceEndpoints.cs`** — New file  
   `POST /ontology/v0/skill-edits/evaluate`

4. **`src/Kanon.Api/Endpoints/OntologyV0Endpoints.cs`** — Add `.MapSkillGovernanceEndpoints()`

5. **`src/Kanon.Infrastructure/DependencyInjection.cs`** — Register `SkillEditGovernanceService`

6. **`tests/Kanon.Tests/SkillEditGovernanceTests.cs`** — New test file  
   11 deterministic tests (see test list below)

7. **`docs/generated/ONTOLOGY_V0_ROUTE_INVENTORY.json`** — Add 1 new route (103 total)
8. **`docs/generated/KANON_COMPATIBILITY_MANIFEST.json`** — Update route count
9. **`docs/evidence/ONTOGONY_SKILL_OPTIMIZATION_SPINE_001B.md`** — New evidence doc

#### `ontogony-platform`

10. **`docs/evidence/ONTOGONY_SKILL_OPTIMIZATION_SPINE_001B.md`** — New evidence doc  
    Update IMPLEMENTATION_NOTES.md to mark 001B complete.

### Tests for 001B

1. `Valid bounded add edit with evidence → accepted`
2. `Valid bounded replace edit with before/after hash → accepted`
3. `Delete edit without evidence → rejected (rejectedMissingEvidence)`
4. `Edit with unknown operation → rejected (rejectedInvalidOperation)`
5. `Edit targeting protected section → rejected (rejectedProtectedSectionMutation)`
6. `Edit with no section path → rejected (rejectedUnboundedProposal)`
7. `Edit with insufficient evidence count → deferred`
8. `Edit proposal with budget exceeded → rejected (rejectedBudgetExceeded)`
9. `Accepted decision has safe rationale, no hidden reasoning`
10. `Rejected decision has rejection reason code, not empty`
11. `No deployment side effects on any decision outcome`

---

## Slice 001C — Allagma offline optimization run registry (DEFERRED)

Allagma will own the `SkillOptimizationRun` lifecycle, trajectory selection, candidate version creation, and evidence export. This slice is blocked on 001B: the Allagma run must call the Kanon governance endpoint to accept/reject candidate edits.

Key planned files (do not implement yet):
- `src/Allagma.Contracts/SkillContracts.cs`
- `src/Allagma.Application/SkillOptimization/`
- `src/Allagma.Api/SkillOptimizationEndpoints.cs`

---

## Slice 001D — Conexus optimizer/target model evidence (DEFERRED)

Blocked on 001C. Conexus will add `SkillInjectionMetadata` to model-call admin DTOs once the Allagma run shape is stable.

---

## Slice 001E — Frontend Skill Lab (DEFERRED)

Blocked on 001C/001D. Not to be started until backend contracts are stable.

---

## Assumptions

1. Kanon's existing decision record lifecycle (accept/reject/supersede) is a natural parent for skill edit decisions. The new `SkillEditDecisionRecord` is not a replacement — it's a specialized decision type that can eventually be linked into existing `DecisionRecordService` if needed.
2. The fake validation service uses deterministic rules only. No live model calls, no external I/O.
3. Safe rationale storage follows `ReasoningEvidenceKinds.SafeRationale` already defined in `Kanon.Contracts`.
4. Budget/cost unknown is the valid state when no optimizer call has been made. Never fabricate.
5. The platform's `Ontogony.Evaluation.Contracts` shapes are the canonical types for evaluation scores; the Kanon governance result references these by ID, not by value copy.

---

## Verification commands

```powershell
# 001A platform schema tests
cd C:\dev\ontogony-platform
dotnet test tests/Ontogony.Infrastructure.Tests -c Release --filter FullyQualifiedName~SkillOptimizationSpineSchemaTests

# 001B Kanon governance tests
cd C:\dev\kanon-dotnet
dotnet test tests/Kanon.Tests -c Release --filter FullyQualifiedName~SkillEditGovernance

# Full Kanon test suite
cd C:\dev\kanon-dotnet
dotnet test tests/Kanon.Tests -c Release

# Route inventory regeneration (after adding endpoint)
cd C:\dev\kanon-dotnet
pwsh .\scripts\update-route-doc-fragments.ps1
```

---

## Final evidence links (updated as slices complete)

| Slice | Evidence doc | Status |
|-------|-------------|--------|
| 001A (platform) | `ontogony-platform/docs/evidence/ONTOGONY_SKILL_OPTIMIZATION_SPINE_001A.md` | Pending |
| 001B (Kanon governance) | `kanon-dotnet/docs/evidence/ONTOGONY_SKILL_OPTIMIZATION_SPINE_001B.md` | Pending |
| 001C (Allagma runs) | `allagma-dotnet/docs/evidence/ONTOGONY_SKILL_OPTIMIZATION_SPINE_001C.md` | Deferred |
| 001D (Conexus metadata) | `conexus-dotnet/docs/evidence/ONTOGONY_SKILL_OPTIMIZATION_SPINE_001D.md` | Deferred |
| 001E (frontend) | `ontogony-frontend/docs/evidence/ONTOGONY_SKILL_OPTIMIZATION_SPINE_001E.md` | Deferred |
