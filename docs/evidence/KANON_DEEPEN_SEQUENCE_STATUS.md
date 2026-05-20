# Kanon deepening sequence — status index

**Last updated:** 2026-05-20 (014A status wording cleanup; 014 Docker/browser QA 12/12 PASS)  
**Packages:** `Ontogony-Kanon-Deep-Enhancement-Package-v1` (000–006); `Ontogony-Kanon-Courageous-Enhancement-Package-v2` (007+)  
**Audit:** [`docs/reviews/KANON_DEEPEN_000_CURRENT_STATE_AUDIT.md`](../reviews/KANON_DEEPEN_000_CURRENT_STATE_AUDIT.md)

| Item | Status | Primary evidence | Implementation commits (main) |
|---|---|---|---|
| KANON-DEEPEN-000 | Done | Platform audit | (audit only) |
| KANON-DEEPEN-001 | Done | [001 evidence](./KANON_DEEPEN_001_LOCAL_OPERATOR_AUTH_AND_READ_WORKBENCH_EVIDENCE.md) | frontend + platform + kanon (see 001 doc) |
| KANON-DEEPEN-002 | Done | [002 evidence](./KANON_DEEPEN_002_DOMAIN_PACK_LIFECYCLE_WORKBENCH_EVIDENCE.md) | `ontogony-frontend` `5bff111`, `kanon-dotnet` `a4eec4a`, platform `f3fea49` |
| KANON-DEEPEN-003 | Done | [003 evidence](./KANON_DEEPEN_003_DECISION_PROVENANCE_EXPLORER_EVIDENCE.md) | `ontogony-frontend` `0f0bde6d`, `664eb04` |
| KANON-DEEPEN-004 | Done (superseded by 008 for history browse) | [004 evidence](./KANON_DEEPEN_004_FACTS_PLANS_BINDINGS_EXPLORER_EVIDENCE.md) | `ontogony-frontend` `65fe83c` |
| KANON-DEEPEN-005 | Done | [005 evidence](./KANON_DEEPEN_005_CROSS_SERVICE_SEMANTIC_LINKS_EVIDENCE.md) | `ontogony-frontend` `c5c4bfc`, platform `68f6e90` |
| KANON-DEEPEN-006 | Done (docs/validation; browser QA checklist) | [006 evidence](./KANON_DEEPEN_006_CLOSEOUT_AND_MANUAL_QA_EVIDENCE.md) | platform closeout docs (this pass) |
| KANON-DEEPEN-007 | Done (build + Docker browser via 014) | [007 evidence](../../../kanon-dotnet/docs/evidence/KANON_DEEPEN_007_CONEXUS_ASSISTANCE_WORKBENCH_EVIDENCE.md) (canonical); browser: [014](./KANON_DEEPEN_014_BROWSER_QA_BASELINE_CANDIDATE_EVIDENCE.md) | `kanon-dotnet` + `ontogony-frontend` — Conexus assistance workbench `/kanon/assistance` |
| KANON-DEEPEN-008 | Done (build + Docker browser via 014) | [008 evidence](../../../kanon-dotnet/docs/evidence/KANON_DEEPEN_008_DURABLE_FACT_AND_PLAN_HISTORY_EVIDENCE.md) (canonical); [008A](../../../ontogony-frontend/docs/evidence/KANON_DEEPEN_008A_RECONCILIATION_EVIDENCE.md); browser: [014](./KANON_DEEPEN_014_BROWSER_QA_BASELINE_CANDIDATE_EVIDENCE.md) | durable fact/plan history GET routes and workbench panels |
| KANON-DEEPEN-009 | Done (build + Docker browser via 014) | [009 evidence](../../../kanon-dotnet/docs/evidence/KANON_DEEPEN_009_POLICY_GATE_EXPLANATION_EVIDENCE.md) (canonical); [009A](./KANON_DEEPEN_009A_RECONCILIATION_EVIDENCE.md); browser: [014](./KANON_DEEPEN_014_BROWSER_QA_BASELINE_CANDIDATE_EVIDENCE.md) | policy/gate explanation workbench `/kanon/policies` |
| KANON-DEEPEN-010 | Done (build + Docker browser via 014) | [010 evidence](../../../kanon-dotnet/docs/evidence/KANON_DEEPEN_010_DOMAIN_PACK_IMPACT_EVIDENCE.md) (canonical); [010A](./KANON_DEEPEN_010A_RECONCILIATION_EVIDENCE.md); browser: [014](./KANON_DEEPEN_014_BROWSER_QA_BASELINE_CANDIDATE_EVIDENCE.md) | domain-pack diff/impact panel on `/kanon/domain-packs` |
| KANON-DEEPEN-011 | Done (build + Docker browser via 014) | [011 evidence](../../../kanon-dotnet/docs/evidence/KANON_DEEPEN_011_SEMANTIC_EVIDENCE_GRAPH_EVIDENCE.md) (canonical); [011A](./KANON_DEEPEN_011A_RECONCILIATION_EVIDENCE.md); browser: [014](./KANON_DEEPEN_014_BROWSER_QA_BASELINE_CANDIDATE_EVIDENCE.md) | semantic evidence graph + decisions panel |
| KANON-DEEPEN-012 | Done (build + Docker browser via 014) | [012 evidence](../../../kanon-dotnet/docs/evidence/KANON_DEEPEN_012_SOURCE_BINDING_QUALITY_LOOP_EVIDENCE.md) (canonical); [012A](./KANON_DEEPEN_012A_RECONCILIATION_EVIDENCE.md); browser: [014](./KANON_DEEPEN_014_BROWSER_QA_BASELINE_CANDIDATE_EVIDENCE.md) | source-binding quality loop on `/kanon/source-bindings` |
| KANON-DEEPEN-013 | Done (API + unit/frontend-build; Docker browser via 014) | [013 evidence](../../../kanon-dotnet/docs/evidence/KANON_DEEPEN_013_SEMANTIC_AUTHORITY_HARDENING_EVIDENCE.md) (canonical) | `kanon-dotnet` + `ontogony-frontend` — route inventory, auth matrix, OpenAPI catalog hardening |
| KANON-DEEPEN-014 | Done (Docker browser QA 12/12; baseline candidate) | [014 evidence](./KANON_DEEPEN_014_BROWSER_QA_BASELINE_CANDIDATE_EVIDENCE.md); [frontend 014](../../../ontogony-frontend/docs/evidence/KANON_DEEPEN_014_BROWSER_MANUAL_QA_EVIDENCE.md) | `ontogony-frontend` Playwright + platform evidence; `kanon-dotnet` CORS fix |

**Closeout bundle (v1):** [`docs/releases/KANON_DEEPENING_CLOSEOUT.md`](../releases/KANON_DEEPENING_CLOSEOUT.md)

## v2 courageous package — current slice

| Slice | Source/API/unit/frontend-build | Docker browser |
|---|---|---|
| 007 Conexus assistance | Done | Done (014 suite) |
| 008 Durable fact/plan history | Done | Done (014 suite) |
| 009 Policy/gate explanation | Done | Done (014 suite) |
| 010 Domain-pack diff/impact | Done | Done (014 suite) |
| 011 Semantic evidence graph | Done | Done (014 suite) |
| 012 Source-binding / ontology quality | Done | Done (014 suite) |
| **013** Hardening / coherence cleanup | Done | — |
| **014** QA / baseline candidate | Done | [014 evidence](./KANON_DEEPEN_014_BROWSER_QA_BASELINE_CANDIDATE_EVIDENCE.md) |

## Browser verification posture

**Scope:** Docker-local operator console only — **not** production readiness.

| Item | Automated tests recorded | Docker browser walkthrough |
|---|---|---|
| 002–005 | Unit + mocked E2E (see per-item evidence) | v1 paths covered by mocked E2E; full compose walk optional |
| 006 | Closeout checklist documented | Checklist in [006 evidence](./KANON_DEEPEN_006_CLOSEOUT_AND_MANUAL_QA_EVIDENCE.md); v2 batch walk superseded by 014 for 007–012 |
| 007–012 | Unit/API + frontend adapter tests; workbench build | **Done via KANON-DEEPEN-014** Docker-local Playwright suite (12/12) — [014 evidence](./KANON_DEEPEN_014_BROWSER_QA_BASELINE_CANDIDATE_EVIDENCE.md) |
| 013 | Route inventory, auth matrix, API surface tests | N/A (hardening slice; no dedicated browser walk) |
| 014 | Playwright `kanon-deepen-014-docker-local-manual-qa.spec.ts` | **Done** — rebuilt `ontogony-frontend` + `kanon-api`; ENV-SEED-001; CORS `X-Ontogony-Roles` fix |

014 verifies Kanon v2 in Docker-local operator scope. Runtime lock promotion remains **SYSTEM-ALPHA-owned** (`ontogony-runtime.lock.json` not updated by 014).

Do not claim production browser verification or full multi-service compose rebuild until SYSTEM-ALPHA lock cut (Allagma/Conexus image SDK pin still outstanding in 014 evidence).

## UI-HARDEN overlap

Some Kanon pages adopted `@ontogony/ui` feedback/navigation primitives during the same period as UI-HARDEN (limitation cards, route patterns). That adoption is **opportunistic shared UI** — ownership remains:

- **Kanon deepening:** semantic workbenches, Kanon API mapping, cross-service semantic links.
- **UI-HARDEN:** shared primitive contracts in `ontogony-ui` (documented under UI-HARDEN evidence).

## Not in scope / follow-up (v2 package)

- Full **Cross-Service Evidence Spine** graph resolver (`EVIDENCE-SPINE-*` package).
- **SYSTEM-ALPHA-004/005** — pin commit SHAs, rebuild all compose services (resolve Allagma/Conexus SDK image drift), update `ontogony-runtime.lock.json`.
- **KANON-DEEPEN-007B** — OpenAPI/generated-client alignment for assistance DTOs (optional cleanup).
- **Conexus `openApiSnapshotCatalog` drift** (013 note) — quarantine as non-Kanon until system baseline cut.
- Index reconciliation: [009A](./KANON_DEEPEN_009A_RECONCILIATION_EVIDENCE.md), [010A](./KANON_DEEPEN_010A_RECONCILIATION_EVIDENCE.md), [011A](./KANON_DEEPEN_011A_RECONCILIATION_EVIDENCE.md), [012A](./KANON_DEEPEN_012A_RECONCILIATION_EVIDENCE.md), [014A](./KANON_DEEPEN_014A_STATUS_WORDING_RECONCILIATION.md).

