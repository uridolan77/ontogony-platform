# Cursor implementation prompt — graduation Phase 2

Copy everything below the line into a new Cursor agent session.

---

# PHENOMENOLOGICAL-AUTHORITY-CERTIFICATION-GRADUATION-001 — Phase 2 implementation

Complete the **remaining graduation work** for package `PHENOMENOLOGICAL-AUTHORITY-CERTIFICATION-GRADUATION-001`.

**Baseline:** Phase 1 implementation is done (Kanon callbacks, Aisthesis ledger logic, Allagma Postgres store, frontend components, fixture certification PASS). Live E2E, Postgres proof, operator wiring, contract gates, and closeout hygiene remain.

## Read first

1. `ontogony-platform/docs/evidence/PHENOMENOLOGICAL_AUTHORITY_GRADUATION_001_REMAINING_PLAN.md` — master plan
2. `ontogony-platform/docs/evidence/ONTOGONY_PHENOMENOLOGICAL_AUTHORITY_GRADUATION_001_SYSTEM_CLOSEOUT.md` — current system status
3. Package (reference): `aisthesis-dotnet/docs/_incoming/_active/PHENOMENOLOGICAL-AUTHORITY-CERTIFICATION-GRADUATION-001/PHENOMENOLOGICAL-AUTHORITY-CERTIFICATION-GRADUATION-001/`
4. Per-repo closeout docs under `docs/evidence/` in each repo

## Repos in scope

- `aisthesis-dotnet`
- `allagma-dotnet`
- `ontogony-frontend`
- `ontogony-platform`
- `kanon-dotnet` (Phase 2 contract snapshots only)
- `ontogony-ui` — optional; skip unless FE-UI-INFRA extraction is explicitly needed

## Execution order (strict)

### Step 1 — Live five-service E2E (`ontogony-platform` + `allagma-dotnet`)

Implement `scripts/system/run-phenomenological-memory-authority-live-e2e.ps1`:

1. Health-check Kanon, Aisthesis, Allagma (`$env:KANON_BASE_URL`, `AISTHESIS_BASE_URL`, `ALLAGMA_BASE_URL`).
2. Trigger deterministic authority probe run (fixed fixture; use `ONTOGONY_PHENOM_AUTH_LIVE_TRIGGER_URL` if documented).
3. Poll `GET /allagma/v0/runs/{runId}/phenomenological-projection` until terminal.
4. Poll `GET /aisthesis/v1/memory/mutations/{mutationId}/authority-status`.
5. Assert `decisionRecordUrl` contains `/ontology/v0/decision-records/` (never `/decisions/`).
6. Assert callback outcome is `applied`, `rejected`, or `pending_review` per fixture.
7. Write `summary.json` with `schemaVersion=phenomenological-authority-certification-v2`, `mode=Live`, `status=PASS`.

Run:

```powershell
cd ontogony-platform
powershell -File scripts/system/run-phenomenological-memory-authority-certification.ps1 -Mode Live
powershell -File scripts/system/validate-phenomenological-memory-authority-summary.ps1 -SummaryPath <artifact>/summary.json
```

**Do not claim Live PASS unless a real stack proves it.**

### Step 2 — Aisthesis Postgres restart proof (`aisthesis-dotnet`)

1. Add integration test: `AuthorityCallback*Postgres` using Testcontainers or existing Postgres fixture.
2. Prove callback ledger survives restart; idempotent replay succeeds; conflicting replay throws.
3. Reconcile EF migration snapshot for `phenomenological_authority_callbacks` if CI drifts.
4. Update `docs/evidence/AISTHESIS_VALIDATION_AUTOMATION_GRADUATION_001_CLOSEOUT.md`.

### Step 3 — Allagma Postgres restart proof (`allagma-dotnet`)

1. Add integration test: `PhenomenologicalProjection*Postgres` using `PostgresPersistenceFixture`.
2. Prove projection status survives restart; route `GET /allagma/v0/runs/{runId}/phenomenological-projection` reads durable store.
3. Reconcile EF model snapshot for `allagma_phenomenological_projection_status`.
4. Update `docs/evidence/ALLAGMA_PROJECTION_STATUS_GRADUATION_001_CLOSEOUT.md`.

### Step 4 — Allagma bridge grade-gate tests (`allagma-dotnet`)

Add `PhenomenologicalBridgeTriggerServiceTests` covering:

- `projected` / `skipped` / `failed` / `pending_authority`
- fail-open vs fail-closed per `PhenomenologicalMemoryBridgeOptions`
- event schema for projected/skipped/failed paths

### Step 5 — Frontend operator wiring (`ontogony-frontend`)

1. Add `usePhenomenologicalProjectionStatus(runId)` hook.
2. Wire `PhenomenologicalProjectionPanel` into run-scoped operator UI (prefer Allagma run detail page).
3. Single view must show: decision link, review item, human gate, callback state, fingerprints, Allagma projection.
4. Update `route-workflow-catalog.json` and `docs/generated/BACKEND_ENDPOINT_UI_COVERAGE.json`.
5. Add/extend page tests.
6. Update `docs/evidence/FRONTEND_AUTHORITY_COCKPIT_GRADUATION_001_CLOSEOUT.md`.

### Step 6 — Contract truth gates (001F, all repos)

- Kanon: OpenAPI snapshot for callback payload v2 fields
- Aisthesis: authority-status + callback contract tests
- Allagma: projection route snapshot + JSON schema validation
- Frontend: endpoint coverage matrix
- Platform: certification summary schema in CI

### Step 7 — Security + observability (001G, 001H) — after Live PASS

- Real raw-payload leakage scan in Live certification (not fixture stub)
- Callback/projection metrics per service
- Runbook: `docs/operations/PHENOMENOLOGICAL_AUTHORITY_CHAIN_TRIAGE.md`

### Step 8 — Hygiene & closeout

1. Reconcile EF migrations (Aisthesis + Allagma).
2. Update all five repo closeout docs with Live artifact paths.
3. Consume package per `ontogony-platform/docs/_incoming/README.md`:
   - Move to `docs/_incoming/_consumed/2026-05/PHENOMENOLOGICAL-AUTHORITY-CERTIFICATION-GRADUATION-001/`
   - Add `CONSUMED.md`, update manifests

## Hard rules

- Preserve service boundaries (Kanon = meaning, Allagma = execution, Aisthesis = phenomenology, platform = mechanics).
- Decision links **must** use `/ontology/v0/decision-records/{decisionId}`.
- `pending_review` must not apply graph mutations.
- Postgres mode must use durable stores (not in-memory) for callback ledger and projection status.
- LiveOrExplain must stay honest: `NOT_RUN` when stack/env missing; never fake Live PASS.
- Do not commit unless explicitly asked.

## Do not mark graduated unless

- [ ] Live E2E PASS on running stack
- [ ] Aisthesis Postgres callback ledger restart test passes
- [ ] Allagma Postgres projection restart test passes
- [ ] Allagma bridge grade-gate tests pass
- [ ] Frontend projection panel wired to operator page
- [ ] Contract/OpenAPI/coverage drift CI green
- [ ] Real raw-payload scan in Live mode (Phase 3)
- [ ] EF migration snapshots reconciled
- [ ] Package consumed per intake rules
- [ ] All closeout docs updated with evidence paths

## Test commands (run before claiming done)

```powershell
# Kanon (if contract changes)
cd kanon-dotnet
dotnet test --filter "FullyQualifiedName~AisthesisMemoryMutation"

# Aisthesis
cd aisthesis-dotnet
dotnet test --filter "FullyQualifiedName~PhenomenologicalAuthority"
dotnet test --filter "FullyQualifiedName~AuthorityCallback"

# Allagma
cd allagma-dotnet
dotnet test --filter "FullyQualifiedName~Phenomenological"
dotnet test --filter "FullyQualifiedName~PhenomenologicalBridgeTrigger"

# Frontend
cd ontogony-frontend
npm test -- --run src/aisthesis

# Platform certification
cd ontogony-platform
powershell -File scripts/system/run-phenomenological-memory-authority-certification.ps1 -Mode Fixture
powershell -File scripts/system/run-phenomenological-memory-authority-certification.ps1 -Mode LiveOrExplain
# Live only when stack is up:
powershell -File scripts/system/run-phenomenological-memory-authority-certification.ps1 -Mode Live
```
