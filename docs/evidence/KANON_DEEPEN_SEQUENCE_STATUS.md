# Kanon deepening sequence — status index

**Last updated:** 2026-05-20 (009 closeout index)  
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
| KANON-DEEPEN-007 | Done (source/unit/frontend-build; Docker browser pending) | [007 evidence](../../../kanon-dotnet/docs/evidence/KANON_DEEPEN_007_CONEXUS_ASSISTANCE_WORKBENCH_EVIDENCE.md) (canonical) | `kanon-dotnet` + `ontogony-frontend` — Conexus assistance workbench `/kanon/assistance` |
| KANON-DEEPEN-008 | Done (API + unit/frontend-build; Docker browser pending) | [008 evidence](../../../kanon-dotnet/docs/evidence/KANON_DEEPEN_008_DURABLE_FACT_AND_PLAN_HISTORY_EVIDENCE.md) (canonical); [008A frontend reconciliation](../../../ontogony-frontend/docs/evidence/KANON_DEEPEN_008A_RECONCILIATION_EVIDENCE.md) | `kanon-dotnet` + `ontogony-frontend` — durable fact/plan history GET routes and workbench panels |
| KANON-DEEPEN-009 | Done (API + unit/frontend-build; Docker browser pending) | [009 evidence](../../../kanon-dotnet/docs/evidence/KANON_DEEPEN_009_POLICY_GATE_EXPLANATION_EVIDENCE.md) (canonical) | `kanon-dotnet` + `ontogony-frontend` — policy/gate explanation workbench `/kanon/policies` |

**Closeout bundle (v1):** [`docs/releases/KANON_DEEPENING_CLOSEOUT.md`](../releases/KANON_DEEPENING_CLOSEOUT.md)

## Browser verification posture

| Item | Automated tests recorded | Docker browser walkthrough |
|---|---|---|
| 002–005 | Unit + mocked E2E (see per-item evidence) | **Pending** — requires `docker compose build ontogony-frontend kanon-api` and operator walkthrough |
| 006 | Closeout checklist documented | Same — see [006 evidence](./KANON_DEEPEN_006_CLOSEOUT_AND_MANUAL_QA_EVIDENCE.md) |
| 007 | Unit/API + frontend adapter tests; workbench build | **Pending** — enable `Kanon:ConexusAssistance` + Conexus URL/key; walk `/kanon/assistance` (see 007 evidence) |
| 008 | Fact/plan history API + adapter tests; workbench build | **Pending** — walk `/kanon/facts` and `/kanon/plans` history panels (see 008 evidence) |
| 009 | Policy explain/simulate API + adapter tests; workbench build | **Pending** — walk `/kanon/policies` explain/simulate and Allagma gate cross-links (see 009 evidence) |

Do not claim end-to-end browser verification until a rebuilt frontend image has been exercised against the manual checklist.

## UI-HARDEN overlap

Some Kanon pages adopted `@ontogony/ui` feedback/navigation primitives during the same period as UI-HARDEN (limitation cards, route patterns). That adoption is **opportunistic shared UI** — ownership remains:

- **Kanon deepening:** semantic workbenches, Kanon API mapping, cross-service semantic links.
- **UI-HARDEN:** shared primitive contracts in `ontogony-ui` (documented under UI-HARDEN evidence).

## Not in scope / follow-up (v2 package)

- Full **Cross-Service Evidence Spine** graph resolver (`EVIDENCE-SPINE-*` package).
- **KANON-DEEPEN-007A** — Docker-local browser validation for `/kanon/assistance` (operator-owned; not claimed in 007 evidence).
- **KANON-DEEPEN-007B** — OpenAPI/generated-client alignment for assistance DTOs (optional cleanup).
- **KANON-DEEPEN-010** — domain-pack diff and impact (next slice).
- **KANON-DEEPEN-014** — Docker/browser QA before baseline candidate.
