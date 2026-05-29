# PHENOMENOLOGICAL-AUTHORITY-CERTIFICATION-GRADUATION-001 — Remaining work plan

**Package:** `PHENOMENOLOGICAL-AUTHORITY-CERTIFICATION-GRADUATION-001`  
**Status as of:** 2026-05-29  
**Baseline:** Phase 2 implementation complete; fixture certification PASS; **graduation blocked on Live PASS**  
**Related closeout:** [`ONTOGONY_PHENOMENOLOGICAL_AUTHORITY_GRADUATION_001_SYSTEM_CLOSEOUT.md`](./ONTOGONY_PHENOMENOLOGICAL_AUTHORITY_GRADUATION_001_SYSTEM_CLOSEOUT.md)

## Completion definition

Do **not** mark the package graduated until all items in **Phase 1** and **Phase 2** are done and evidenced. Phase 3 is required for ≥9.1 scorecard targets.

---

## Phase 1 — Graduation blockers (required)

### 1.1 Live five-service E2E certification

**Owner:** `ontogony-platform` (scripts) + `allagma-dotnet` (probe trigger)  
**Files:**

- `ontogony-platform/scripts/system/run-phenomenological-memory-authority-live-e2e.ps1`
- `ontogony-platform/scripts/system/run-phenomenological-memory-authority-certification.ps1`

**Tasks:**

1. Define deterministic authority probe run (fixed ontology, mutation fixture, known trace/run ids).
2. Implement live script sequence:
   - Health-check Kanon, Aisthesis, Allagma.
   - Trigger probe run via Allagma (or documented `ONTOGONY_PHENOM_AUTH_LIVE_TRIGGER_URL`).
   - Poll `GET /allagma/v0/runs/{runId}/phenomenological-projection` until terminal state.
   - Poll `GET /aisthesis/v1/memory/mutations/{mutationId}/authority-status`.
   - Assert `decisionRecordUrl` contains `/ontology/v0/decision-records/` (never `/decisions/`).
   - Assert callback outcome ∈ `{applied, rejected, pending_review}` as appropriate for fixture.
   - Write `summary.json` with `mode=Live`, `status=PASS`.
3. Run `-Mode Live` against local/docker stack; attach artifact path to system closeout.

**Acceptance:**

```powershell
# With stack up and env set:
$env:KANON_BASE_URL = "http://localhost:5081"
$env:AISTHESIS_BASE_URL = "http://localhost:5085"
$env:ALLAGMA_BASE_URL = "http://localhost:5083"
powershell -File scripts/system/run-phenomenological-memory-authority-certification.ps1 -Mode Live
powershell -File scripts/system/validate-phenomenological-memory-authority-summary.ps1 -SummaryPath <summary.json>
# Expect: status = PASS
```

---

### 1.2 Postgres restart proof — Aisthesis callback ledger

**Owner:** `aisthesis-dotnet`  
**Files:**

- `src/Aisthesis.Infrastructure.Postgres/Persistence/Phenomenology/PostgresMutationValidationCallbackLedger.cs`
- `tests/Aisthesis.Infrastructure.Postgres.Tests/` (new or extend existing Postgres fixture tests)

**Tasks:**

1. Add integration test using Testcontainers (or shared Postgres fixture pattern):
   - Apply migration `20260529120000_AddPhenomenologicalAuthorityCallbacks`.
   - Record `pending_review` callback via service.
   - Simulate restart (new DbContext factory / new host).
   - Replay same idempotency key → no conflict; state unchanged.
   - Conflicting replay → `InvalidOperationException`.
2. Fix EF model snapshot if CI enforces migration drift (`dotnet ef migrations add` reconcile).

**Acceptance:**

```text
dotnet test --filter "FullyQualifiedName~AuthorityCallback.*Postgres"
```

**Note:** Separate `phenomenological_authority_status` table from skeleton is **optional** if authority status remains durable via `graph_mutations` metadata + callback ledger. Document chosen approach in Aisthesis closeout.

---

### 1.3 Postgres restart proof — Allagma projection status

**Owner:** `allagma-dotnet`  
**Files:**

- `src/Allagma.Infrastructure/Aisthesis/PostgresPhenomenologicalProjectionStatusStore.cs`
- `tests/Allagma.Tests/PhenomenologicalBridge/` (extend)
- `src/Allagma.Infrastructure/Persistence/Migrations/20260529130000_PhenomenologicalProjectionStatusGraduation001.cs`

**Tasks:**

1. Add Postgres integration test (reuse `PostgresPersistenceFixture` pattern):
   - Upsert projection status for `runId`.
   - New store instance (restart simulation).
   - `GET` equivalent via store → same record.
2. Add API-level test: `GET /allagma/v0/runs/{runId}/phenomenological-projection` returns persisted status after host recycle (optional E2E variant).
3. Reconcile EF model snapshot for `allagma_phenomenological_projection_status`.

**Acceptance:**

```text
dotnet test --filter "FullyQualifiedName~PhenomenologicalProjection.*Postgres"
```

---

### 1.4 Frontend operator wiring — full chain view

**Owner:** `ontogony-frontend`  
**Files:**

- `src/aisthesis/components/PhenomenologicalProjectionPanel.tsx` (exists)
- `src/aisthesis/api/phenomenologicalProjectionClient.ts` (exists)
- `src/allagma/pages/` or run detail panel (wire target — TBD by existing run UX)
- `src/app/route-workflow-catalog.json`
- `docs/generated/BACKEND_ENDPOINT_UI_COVERAGE.json` (if FE-UI-INFRA gate applies)

**Tasks:**

1. Add `usePhenomenologicalProjectionStatus(runId)` hook (mirror `useMutationAuthorityStatus`).
2. Wire `PhenomenologicalProjectionPanel` into operator UI where `runId` is known:
   - Preferred: Allagma run detail / run evidence page alongside existing run panels.
   - Alternate: phenomenological memory page when `?runId=` query param present.
3. Ensure single screen shows: decision link, review/human gate, callback state, fingerprints, Allagma projection status.
4. Update route-workflow catalog + endpoint coverage matrix.
5. Add page-level test (or extend existing run page test).

**Acceptance:**

```text
npm test -- --run src/aisthesis/components/PhenomenologicalProjectionPanel.test.tsx
# Plus integration test on wired page
```

---

### 1.5 Allagma bridge grade-gate tests

**Owner:** `allagma-dotnet`  
**Files:**

- `src/Allagma.Application/PhenomenologicalBridge/PhenomenologicalBridgeTriggerService.cs`
- `tests/Allagma.Tests/PhenomenologicalBridge/PhenomenologicalBridgeTriggerServiceTests.cs` (new)

**Tasks:**

1. Test projected path: Aisthesis returns grade ≥ minimum → `ProjectionStatus=projected`, event emitted.
2. Test skipped path: below grade + `FailIfBelowGrade=false` → `skipped`.
3. Test failed path: below grade + `FailIfBelowGrade=true` → `failed`, error recorded.
4. Test pending_authority path: Kanon authority required, decisions pending.
5. Test fail-open: bridge disabled / Aisthesis unreachable → run still terminal, status recorded as skipped/failed per options.
6. Test fail-closed: if product requires hard fail, assert behavior matches `PhenomenologicalMemoryBridgeOptions`.

**Acceptance:**

```text
dotnet test --filter "FullyQualifiedName~PhenomenologicalBridgeTrigger"
```

---

## Phase 2 — Contract truth & CI gates (001F)

**Owner:** split by repo; coordinated from `ontogony-platform`

| Repo | Tasks |
|------|--------|
| `kanon-dotnet` | Update OpenAPI snapshot for callback payload v2 fields (`reviewItemId`, `reviewQueueUrl`, `humanGateId`, `policy_missing`, `pending_review`). |
| `aisthesis-dotnet` | Snapshot `/authority-status` response shape; callback request contract tests. |
| `allagma-dotnet` | Snapshot `GET /runs/{runId}/phenomenological-projection`; validate against `schemas/json/allagma-projection-status-v2.schema.json`. |
| `ontogony-frontend` | Regenerate Kanon/Allagma client types if needed; update `BACKEND_ENDPOINT_UI_COVERAGE.json`. |
| `ontogony-platform` | Add certification summary schema validation to CI; fail on drift. |

**Acceptance:** CI green on contract drift tests in each repo.

---

## Phase 3 — Security, observability, scorecard polish (001G + 001H)

### 3.1 Security & retention (001G)

**Owner:** `aisthesis-dotnet` + `ontogony-platform`

1. Implement real `raw-payload-leakage-check.ps1` (package skeleton exists under prior bundle); wire into certification `-Mode Live`, not fixture stub.
2. Ensure callback ledger stores fingerprints/metadata only — no raw episode payloads.
3. Add retention metadata columns if required by skeleton (`received_at_utc`, tombstone flags).
4. Test: episode erasure redacts summaries but preserves `kanon_decision_id` / `decision_record_url`.

### 3.2 Observability & operations (001H)

**Owner:** per service

| Service | Metrics |
|---------|---------|
| Kanon | callback dispatch count, callback failure count, `pending_review` vs `allow`/`deny` |
| Aisthesis | callback received, idempotent replay, conflict, `pending_review` no-apply |
| Allagma | projection triggered, projected, skipped, failed, pending_authority |

Add runbook: `docs/operations/PHENOMENOLOGICAL_AUTHORITY_CHAIN_TRIAGE.md` (platform or per-repo).

---

## Phase 4 — Repo hygiene & closeout

### 4.1 Migration hygiene

```text
# Aisthesis
cd aisthesis-dotnet
dotnet ef migrations add ReconcilePhenomenologicalAuthorityCallbacks --project src/Aisthesis.Infrastructure.Postgres

# Allagma
cd allagma-dotnet
dotnet ef migrations add ReconcilePhenomenologicalProjectionStatus --project src/Allagma.Infrastructure
```

Verify snapshots match hand-written SQL before merging.

### 4.2 Package intake (platform rules)

1. Promote durable content from package to canonical `docs/`.
2. Move `docs/_incoming/_active/PHENOMENOLOGICAL-AUTHORITY-CERTIFICATION-GRADUATION-001/` → `docs/_incoming/_consumed/2026-05/PHENOMENOLOGICAL-AUTHORITY-CERTIFICATION-GRADUATION-001/`.
3. Add `CONSUMED.md`; update `MANIFEST.md` in platform and aisthesis.

### 4.3 Per-repo commits (suggested order)

1. `kanon-dotnet` — callback + tests + closeout update
2. `aisthesis-dotnet` — ledger + callback handling + Postgres tests + closeout
3. `allagma-dotnet` — Postgres store + bridge tests + closeout
4. `ontogony-frontend` — cockpit wiring + coverage + closeout
5. `ontogony-platform` — live E2E + CI gates + system closeout (final)

### 4.4 Closeout doc updates

After each phase, update repo evidence docs with honest status (no Live PASS until 1.1 succeeds):

| Repo | Evidence doc |
|------|----------------|
| kanon-dotnet | `docs/evidence/KANON_PHENOMENOLOGICAL_AUTHORITY_GRADUATION_001_CLOSEOUT.md` |
| aisthesis-dotnet | `docs/evidence/AISTHESIS_VALIDATION_AUTOMATION_GRADUATION_001_CLOSEOUT.md` |
| allagma-dotnet | `docs/evidence/ALLAGMA_PROJECTION_STATUS_GRADUATION_001_CLOSEOUT.md` |
| ontogony-frontend | `docs/evidence/FRONTEND_AUTHORITY_COCKPIT_GRADUATION_001_CLOSEOUT.md` |
| ontogony-platform | `docs/evidence/ONTOGONY_PHENOMENOLOGICAL_AUTHORITY_GRADUATION_001_SYSTEM_CLOSEOUT.md` |

---

## Execution order (recommended)

```text
1.1 Live E2E wiring          ← unblocks Live PASS claim
1.2 Aisthesis Postgres tests
1.3 Allagma Postgres tests
1.5 Allagma grade-gate tests
1.4 Frontend wiring
2.x Contract truth gates
3.x Security + observability (parallel after Live PASS)
4.x Hygiene + consume package + commits
```

---

## Optional / deferred

| Item | Notes |
|------|--------|
| `ontogony-ui` shared components | Optional; frontend owns cockpit unless FE-UI-INFRA extraction is requested |
| `phenomenological_authority_status` table | Defer if mutation metadata + ledger suffice; document in closeout |
| Live mode without docker | Requires manually running five-service stack; document env vars in runbook |

---

## Tracking checklist

Use this checklist to mark graduation complete:

- [ ] Live E2E PASS on running stack
- [ ] Aisthesis Postgres callback ledger restart test
- [ ] Allagma Postgres projection restart test
- [ ] Allagma bridge grade-gate tests
- [ ] Frontend projection panel wired to operator page
- [ ] Contract/OpenAPI/coverage drift CI green
- [ ] Real raw-payload scan in Live certification
- [ ] EF migration snapshots reconciled
- [ ] Package consumed per intake rules
- [ ] All five repo closeout docs updated with Live evidence paths
