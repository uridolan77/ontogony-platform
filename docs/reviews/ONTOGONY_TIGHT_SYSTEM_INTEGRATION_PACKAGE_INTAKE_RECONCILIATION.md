# Ontogony Tight System Integration Package — intake reconciliation

**Package:** `docs/_incoming/Ontogony_Tight_System_Integration_Package_2026-05-21.zip`  
**Unpacked to:** `docs/_incoming/Ontogony_Tight_System_Integration_Package_2026-05-21/ontogony_tight_integration_package/`  
**Reconciliation date:** 2026-05-21  
**Intake verdict:** **ACCEPT with scope trim** — package direction matches post–`SYSTEM-ALPHA-006` reality; several items are already shipped or superseded by prior programs (Evidence Spine, Kanon Connect, Conexus post-lock classification). Active work should tighten operator journeys and release discipline, not reopen Kanon/Conexus capability sprints.

**Locked baseline (authoritative):** `SYSTEM-ALPHA-006` per [`allagma-dotnet/docs/system/ontogony-runtime.lock.json`](../../allagma-dotnet/docs/system/ontogony-runtime.lock.json). Do **not** rename the lock baseline to `SYSTEM-TIGHT-001` without a dedicated lock-promotion PR, fresh evidence, and updated closeout docs.

---

## 1. Package contents inventory

| Path | Purpose |
|------|---------|
| `README.md` | Package overview, baseline assumptions, sprint name |
| `00_EXECUTIVE_VERDICT.md` | Strategic verdict: enter tight integration phase |
| `01_REPO_STATE_REVIEW.md` | Per-repo strengths/risks/roles |
| `02_TARGET_ARCHITECTURE.md` | Authority boundaries and evidence chain |
| `03_GAP_REGISTER.md` | Prioritized gaps P0–P3 |
| `04_PR_SEQUENCE.md` | `SYS-TIGHT-001` … `008` proposed PRs |
| `05_ACCEPTANCE_MATRIX.md` | P0/P1/P2 gates and commands |
| `06_RUNTIME_LOCK_AND_COMPATIBILITY_SPEC.md` | Future lock shape (`SYSTEM-TIGHT-001`) |
| `07_E2E_SCENARIO_SPECS.md` | Scenarios A–H (run, idempotency, gates, assistance, fallback, streaming, restart, operator audit) |
| `08_OPERATOR_EVIDENCE_SPINE_PLAN.md` | Resolver graph + operator panels |
| `09_STREAMING_AND_MODEL_PURPOSE_PLAN.md` | Model-purpose / streaming evidence policy |
| `10_IDENTITY_AND_SECURITY_ROADMAP.md` | Production identity roadmap (planning) |
| `11_OBSERVABILITY_AND_SLO_PLAN.md` | System observability (planning) |
| `12_CI_RELEASE_GATES.md` | PR / release-candidate gate design |
| `issue-cards/SYS-TIGHT-001` … `008` | Implementation-ready cards |
| `matrices/sys_tight_acceptance_matrix.json` | Machine-readable gate list |
| `matrices/system_identifier_resolution_matrix.json` | Identifier → route hints |
| `templates/EVIDENCE_SUMMARY_TEMPLATE.md` | Closeout template |
| `templates/POST_LOCK_DELTA_TEMPLATE.md` | Post-lock delta entry template |
| `scripts/validate-package-structure.ps1` | Package self-check (expects files at package root; see §6) |

**Naming:** No `Agentor` references in the package (compliant with Allagma-as-runtime-owner rule).

---

## 2. Current repo baseline vs package assumptions

### Runtime lock (`SYSTEM-ALPHA-006`)

| Field | Lock file | Package `06_*` spec | Match |
|-------|-----------|---------------------|-------|
| `baseline` | `SYSTEM-ALPHA-006` | Example uses `SYSTEM-TIGHT-001` | **Future only** — do not apply without lock PR |
| `packageVersions` | Ontogony `0.3.0-alpha.1`, Kanon `0.1.0-alpha.0`, Conexus `0.1.0-alpha.1` | Same | **Match** |
| `apiPrefixes` | `/allagma/v0`, `/ontology/v0`, `/v1/chat/completions` | Same | **Match** |
| `requiredScenarios` | 9 scenarios (incl. `correlation_chain`, no explicit `streaming_lifecycle` / `operator_audit_journey` names) | Adds `streaming_lifecycle`, `operator_audit_journey` | **Additive naming** — behaviors largely covered by ALPHA-006 cohesion evidence |
| `evidence` paths | Pinned 20260520 artifacts | Adds `operatorAuditSummary` | **Not in lock yet** — valid follow-up for next cut |

### Moving-main drift (2026-05-21)

All four runtime repos are **ahead of** `lockedCommits` (expected after ALPHA-006):

| Repo | Lock SHA | Current `HEAD` | Status |
|------|----------|----------------|--------|
| `ontogony-platform` | `23ba850…` | `2bcd3b2…` | DRIFT |
| `allagma-dotnet` | `6dbf087…` | `edb1d4e…` | DRIFT |
| `kanon-dotnet` | `515c7aa…` | `31b471d…` | DRIFT |
| `conexus-dotnet` | `100c33d…` | `8db7729…` | DRIFT |

Companion pins in [`SYSTEM_ALPHA_006_CLOSEOUT.md`](../../allagma-dotnet/docs/evidence/SYSTEM_ALPHA_006_CLOSEOUT.md) (`ontogony-frontend`, `ontogony-ui`) are informational; they are not in `ontogony-runtime.lock.json` but matter for operator gates.

### Machine-readable matrix cross-check

| Artifact | Package / intake | Repo truth | Verdict |
|----------|------------------|------------|---------|
| [`allagma-feature-connection.matrix.json`](../../allagma-dotnet/docs/system/allagma-feature-connection.matrix.json) | Package assumes matrix exists | Present; `lastUpdated` 2026-05-21; known gaps: streaming UI, Kanon chain panel | **Aligned** |
| [`KANON_COMPATIBILITY_MANIFEST.json`](../../kanon-dotnet/docs/generated/KANON_COMPATIBILITY_MANIFEST.json) | v1.1, 61 routes, frozen v0 | Manifest v1.1; `evidence_spine_handoff` gate present | **Aligned** |
| [`system-protocol-registry.json`](../system/system-protocol-registry.json) | Package wants first-class registry | Present; `baseline: SYSTEM-ALPHA-006`; validator `scripts/validate-system-protocol-registry.ps1` | **Largely satisfied** (SYS-TIGHT-001 should link deltas to registry, not recreate) |
| `system_identifier_resolution_matrix.json` | `modelCallId` → `/conexus/v0/model-calls/{id}` | Conexus also exposes `/admin/v0/model-calls/{id}`; FE taxonomy uses **admin** for operator spine | **Partially stale** — prefer admin + evidence-links for operator matrix |
| `sys_tight_acceptance_matrix.json` | `targetBaseline: SYSTEM-TIGHT-001` | Lock remains `SYSTEM-ALPHA-006` | **Sprint name ≠ lock baseline** — treat as program label only |

---

## 3. Already satisfied (do not re-implement)

| Package item | Evidence in repos |
|--------------|-------------------|
| Alpha governed runtime baseline | [`SYSTEM_ALPHA_006_CLOSEOUT.md`](../../allagma-dotnet/docs/evidence/SYSTEM_ALPHA_006_CLOSEOUT.md) — cohesion, restart, observability, lock validators PASS at cut |
| Runtime lock + compatibility matrix | [`ontogony-runtime.lock.json`](../../allagma-dotnet/docs/system/ontogony-runtime.lock.json), [`SYSTEM_COMPATIBILITY_MATRIX.md`](../../allagma-dotnet/docs/system/SYSTEM_COMPATIBILITY_MATRIX.md) |
| Feature connection matrix | [`allagma-feature-connection.matrix.json`](../../allagma-dotnet/docs/system/allagma-feature-connection.matrix.json) + `scripts/validate-feature-connection-matrix.ps1` |
| Kanon v0 freeze + manifest gates | [`KANON_COMPATIBILITY_MANIFEST.json`](../../kanon-dotnet/docs/generated/KANON_COMPATIBILITY_MANIFEST.json), [`KANON_V0_CONTRACT_FREEZE.md`](../../kanon-dotnet/docs/contracts/KANON_V0_CONTRACT_FREEZE.md) |
| Kanon evidence-spine handoff | [`KANON_EVIDENCE_SPINE_ENTRYPOINTS.json`](../../kanon-dotnet/docs/generated/KANON_EVIDENCE_SPINE_ENTRYPOINTS.json) |
| Cross-service evidence spine (client resolver) | [`CROSS_SERVICE_EVIDENCE_SPINE_CLOSEOUT.md`](../releases/CROSS_SERVICE_EVIDENCE_SPINE_CLOSEOUT.md), `ontogony-frontend/src/evidence-spine/`, `/system/evidence-spine` |
| Identifier taxonomy | [`EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md`](../operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md) — supersedes need for a separate `SYSTEM_EVIDENCE_SPINE_CONTRACT.md` body; at most add a thin alias doc |
| Conexus route-preview + quota APIs | Implemented; classified at ALPHA-006 in [`CONEXUS_POSTLOCK_001_CLASSIFICATION.md`](../../conexus-dotnet/docs/evidence/CONEXUS_POSTLOCK_001_CLASSIFICATION.md) |
| Conexus model-call evidence routes | [`MODEL_CALL_EVIDENCE_FLOW.md`](../../conexus-dotnet/docs/operators/MODEL_CALL_EVIDENCE_FLOW.md) |
| Allagma streaming + `StreamAsync` | Documented in compatibility matrix (`ALLAGMA-STREAM-001` / `AGM-STREAM-002`); cohesion evidence includes streaming |
| Package-mode CI gate | `allagma-package-mode` job + `scripts/run-package-mode-build.ps1` (SYS-PKG-001) |
| Real tool execution blocked | `scripts/validate-real-tools-block.ps1`, SANDBOX / trust-model docs |
| System protocol registry (alpha) | [`system-protocol-registry.json`](../system/system-protocol-registry.json), `SYSTEM_PROTOCOL_REGISTRY_001` evidence |
| Kanon Connect + route parity + Docker smoke | Platform `KANON_CONNECT_*` evidence; FE `kanon:route-parity:check` |
| Partial Conexus operator consumption (moving-main) | FE: `useConexusRoutePreview`, `ConexusOverviewPage`, `conexusGovernance.test.ts` — **post-dates** Conexus post-lock doc “not wired” note |

---

## 4. Stale or superseded

| Package claim | Why stale |
|---------------|-----------|
| “Start a cross-service evidence spine program” (SYS-TIGHT-002 scope) | **Superseded** by Evidence Spine 000–009 closeout (2026-05-20) |
| “Route-preview/quota not surfaced” (01_REPO_STATE, SYS-TIGHT-004) | **Partially superseded** on moving-main FE; Conexus post-lock doc still says “not wired” at lock SHAs |
| `06_*` example lock `baseline: SYSTEM-TIGHT-001` | **Premature** — conflicts with instruction to keep `SYSTEM-ALPHA-006` until evidence-backed promotion |
| `system_identifier_resolution_matrix.json` operator routes | **Incomplete** vs [`EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md`](../operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md) (admin model-call paths, eval/baseline ids) |
| “Do not open KANON-NEXT-019” | **Still valid** — aligns with Kanon AGENTS.md v0 freeze |
| Package `01_REPO_STATE` “frontend route parity baseline blocker” | **Superseded at ALPHA-006** by Kanon Connect 006/007 PASS |
| Unified backend evidence aggregator (08 plan implied) | **Explicitly deferred** in Evidence Spine closeout — client-side v1 is canonical |

---

## 5. Still valid (active gaps)

| ID | Gap | Owner | Notes |
|----|-----|-------|-------|
| **SYS-TIGHT-001** | Unified **four-repo** post-lock delta register + release-mode validator enforcement | Allagma + Platform | Conexus has repo-local classification only; Allagma has **no** `post-lock-deltas.json` / `POST_LOCK_DELTA_REGISTER.md`; `validate-runtime-lock.ps1` has **no** delta checks today |
| **SYS-TIGHT-003** | Single **run-centric** operator audit journey (not only ID-paste workbench) | Frontend + UI | Run detail has audit export + embedded panels; no one flow combining timeline + Kanon + Conexus + stream + failure diagnosis per package §08 |
| **SYS-TIGHT-004** | Route-preview/quota **in audit/cohesion context** | Frontend + Conexus docs | APIs exist; ensure governance/quota visible in operator paths beyond Conexus overview; link into evidence spine |
| **SYS-TIGHT-005** | Dedicated **stream lifecycle operator panel** | Allagma + Frontend | Backend events shipped; `allagma-feature-connection.matrix.json` lists `fe-streaming-evidence-ui` as known gap |
| **SYS-TIGHT-006** | Cross-service **failure taxonomy adapter** (operator-normalized, non-breaking public APIs) | Platform + Allagma + Frontend | Not present as shared adapter; service-specific errors remain by design |
| **SYS-TIGHT-007** | Release candidate gate **bundle** explicit in one runbook | Allagma | Pieces exist; package wants single release-candidate checklist including operator Playwright |
| **SYS-TIGHT-008** | Production-readiness **separation** backlog | Platform-led planning | Valid; planning-only |
| Lock promotion | Classify moving-main drift → next `SYSTEM-ALPHA-007` (or named cut) | Allagma | Requires SYS-TIGHT-001 + full P0 revalidation — **not** silent lock edit |

---

## 6. Risks and contradictions

1. **Baseline name collision:** Package sprint `SYS-TIGHT-001`, acceptance matrix `targetBaseline: SYSTEM-TIGHT-001`, and lock spec example baseline **must not** overwrite `SYSTEM-ALPHA-006` in `ontogony-runtime.lock.json` without a lock PR.
2. **Double evidence-spine work:** SYS-TIGHT-002 would duplicate Evidence Spine 001–009; risk of divergent taxonomy if rewritten from scratch.
3. **Post-lock doc drift:** Conexus classification dated 2026-05-20 says route-preview/quota “not wired”; frontend moving-main now implements clients — update classifications on next delta pass, not by reverting FE.
4. **Identifier matrix vs taxonomy:** Package matrix under-specifies Conexus admin routes and Allagma eval identifiers; could mislead new resolver code if taken alone.
5. **Moving-main vs lock validation:** Running `-ReleaseMode` on current `HEAD` may fail until deltas are classified or SHAs are re-pinned; expected, not a package defect.
6. **Kanon expansion pressure:** Package correctly refuses new Kanon product work; any SYS-TIGHT change in Kanon must stay additive compatibility / evidence metadata only.
7. **Package validator path:** `validate-package-structure.ps1` fails when run from `scripts/` because required files live under `ontogony_tight_integration_package/` subdirectory (validator expects flat layout).

---

## 7. Recommended PR sequence (adjusted)

Prefer **docs/validators/gates/operator glue** only. Kanon and Conexus PRs only when an existing safe API needs docs/tests or additive operator metadata.

| Order | ID | Repo(s) | Type | Rationale |
|-------|-----|---------|------|-----------|
| **1** | **SYS-TIGHT-001** | Allagma, Platform | docs + validator + CI | **First active work** — doc-heavy, no runtime behavior; closes moving-main confusion. Add `docs/system/POST_LOCK_DELTA_REGISTER.md`, `post-lock-deltas.json`, extend `validate-runtime-lock.ps1` / cross-ref; link from protocol registry. |
| **2** | **SYS-TIGHT-002** (trimmed) | Platform | docs-only alias | Publish `docs/operators/SYSTEM_EVIDENCE_SPINE_CONTRACT.md` as **index** pointing to Evidence Spine taxonomy, graph schema, and FE resolver — do not rebuild resolver. |
| **3** | **SYS-TIGHT-003** | Frontend, UI | UI + e2e | Run-centric audit page/flow from `runId`/`traceId`; reuse `resolveEvidenceSpine`, run audit bundle, existing panels. |
| **4** | **SYS-TIGHT-004** | Frontend, Conexus docs | UI + tests | Promote route-preview/quota into audit journey + spine links; refresh Conexus post-lock “FE status” rows. |
| **5** | **SYS-TIGHT-005** | Allagma, Frontend | UI + smoke | Stream lifecycle panel; confirm audit bundle fields; optional cohesion streaming flag in release checklist. |
| **6** | **SYS-TIGHT-006** | Platform, Allagma, Frontend | contract + adapter tests | Operator failure taxonomy mapping layer only. |
| **7** | **SYS-TIGHT-007** | Allagma | CI/docs | Consolidate existing gates into `docs/releases/SYSTEM_TIGHT_RELEASE_CANDIDATE_CHECKLIST.md`. |
| **8** | **SYS-TIGHT-008** | Platform | planning | Production-readiness backlog split; link from alpha closeouts. |
| **—** | **Lock promotion** | Allagma | dedicated PR | After P0 gates on classified deltas: cut `SYSTEM-ALPHA-007` (or next alpha id), update lock SHAs + evidence paths — **not** bundled into feature PRs. |

**Intake-safe first PR:** **SYS-TIGHT-001** is documentation and validation only (explicitly “no runtime code changes” in package). Safe to start when approved; this intake does **not** implement it.

---

## 8. Do not implement (from package + repo policy)

- Rename or bump `ontogony-runtime.lock.json` `baseline` to `SYSTEM-TIGHT-001` without lock-promotion PR and evidence.
- New **Kanon** feature sequences (`KANON-NEXT-019`, ontology v1, replay execution in Kanon).
- Move orchestration into Kanon or model routing into Allagma.
- **Real external tool execution** or `RealExecutionEnabled=true` without SANDBOX graduation.
- **Enterprise IAM / production SLO / managed observability** as part of this sprint (SYS-TIGHT-008 planning only).
- **Backend unified evidence resolve API** (deferred by Evidence Spine v1).
- Reintroduce **Agentor** naming in new docs, routes, or packages.
- Silent edits to `packageVersions` or `lockedCommits` during feature PRs.

---

## 9. Validation commands

Run from a machine with Docker/local stack as noted. On moving-main, expect lock release-mode to fail until deltas are classified or SHAs refreshed.

### Package intake

```powershell
cd C:\dev\ontogony-platform\docs\_incoming\Ontogony_Tight_System_Integration_Package_2026-05-21\ontogony_tight_integration_package
.\scripts\validate-package-structure.ps1
# Note: script expects files in cwd; run from package root or fix paths — currently fails from scripts/ subfolder.
```

### P0 — runtime / Allagma

```powershell
cd C:\dev\allagma-dotnet
.\scripts\validate-runtime-lock.ps1 -RequireEvidence -ReleaseMode
.\scripts\validate-release-lock-crossref.ps1
.\scripts\validate-feature-connection-matrix.ps1
.\scripts\validate-real-tools-block.ps1
dotnet test Allagma.sln -c Release --filter "Category!=CrossRepo&Category!=PersistenceSmoke"
.\scripts\run-package-mode-build.ps1
.\scripts\architecture-conformance\run-cross-repo-conformance.ps1
.\scripts\run-system-cohesion-smoke.ps1 -UseExistingServices -IncludeKanonAssistance -IncludeConexusFallback
# Restart (canonical): Docker path per SYSTEM_ALPHA_006_CLOSEOUT / restart-e2e scripts
```

### P0 — Kanon

```powershell
cd C:\dev\kanon-dotnet
dotnet test --filter "FullyQualifiedName~KanonCompatibilityManifestTests|FullyQualifiedName~OntologyV0RouteInventoryTests|FullyQualifiedName~OpenApiBaselineTests|FullyQualifiedName~KanonV0ContractFreezeTests|FullyQualifiedName~KanonEvidenceSpineHandoffTests"
```

### P0 — Conexus

```powershell
cd C:\dev\conexus-dotnet
dotnet test --filter "FullyQualifiedName~RoutePreview|FullyQualifiedName~QuotaStatus"
dotnet test Conexus.sln -c Release
```

### P0 — Platform

```powershell
cd C:\dev\ontogony-platform
.\scripts\validate-system-protocol-registry.ps1
dotnet test Ontogony.sln -c Release
```

### P1 — operator / frontend (when stack up)

```powershell
cd C:\dev\ontogony-frontend
npm run kanon:route-parity:check
npm run inventory:check
npm run test
npm run test:e2e:docker-live:evidence-spine
npm run test:e2e:docker-live:kanon-connect-007
npx playwright test e2e/protocol-delta-golden-journeys.spec.ts
```

### Cross-repo manifest conformance (Allagma tests)

```powershell
cd C:\dev\allagma-dotnet
dotnet test --filter "FullyQualifiedName~KanonCompatibilityManifestConformanceTests"
```

---

## 10. Intake decision

| Question | Answer |
|----------|--------|
| Accept package for planning? | **Yes** |
| Start implementation in this intake? | **No** — reconciliation and planning pointers only |
| Next sprint label | `SYS-TIGHT-001 — Ontogony Tight Integration Baseline` (program name) |
| Next lock baseline name | Keep **`SYSTEM-ALPHA-006`** until promotion PR; do not use `SYSTEM-TIGHT-001` as lock `baseline` field |
| Planning folder | [`docs/_planning/tight-system-integration/`](../_planning/tight-system-integration/README.md) |

---

## 11. Related repo docs (canonical over package matrices)

- [`allagma-dotnet/docs/system/ontogony-runtime.lock.json`](../../allagma-dotnet/docs/system/ontogony-runtime.lock.json)
- [`allagma-dotnet/docs/system/SYSTEM_COMPATIBILITY_MATRIX.md`](../../allagma-dotnet/docs/system/SYSTEM_COMPATIBILITY_MATRIX.md)
- [`allagma-dotnet/docs/system/allagma-feature-connection.matrix.json`](../../allagma-dotnet/docs/system/allagma-feature-connection.matrix.json)
- [`kanon-dotnet/docs/generated/KANON_COMPATIBILITY_MANIFEST.json`](../../kanon-dotnet/docs/generated/KANON_COMPATIBILITY_MANIFEST.json)
- [`conexus-dotnet/docs/evidence/CONEXUS_POSTLOCK_001_CLASSIFICATION.md`](../../conexus-dotnet/docs/evidence/CONEXUS_POSTLOCK_001_CLASSIFICATION.md)
- [`ontogony-platform/docs/releases/CROSS_SERVICE_EVIDENCE_SPINE_CLOSEOUT.md`](../releases/CROSS_SERVICE_EVIDENCE_SPINE_CLOSEOUT.md)
- [`ontogony-platform/docs/system/system-protocol-registry.json`](../system/system-protocol-registry.json)
