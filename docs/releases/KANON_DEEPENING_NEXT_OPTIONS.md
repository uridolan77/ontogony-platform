# Kanon deepening — next options

After v1 closeout (000–006) and v2 slices **007–009** at source/API/unit/frontend-build level, consider these follow-ups.

| ID | Theme | Status (2026-05-20) | Rationale |
|---|---|---|---|
| **KANON-DEEPEN-007** | Conexus assistance operator UI | **Done** (build); Docker browser pending | `/kanon/assistance` — see 007 evidence |
| **KANON-DEEPEN-008** | Durable fact/plan history | **Done** (build); Docker browser pending | GET history routes + workbench panels — see 008 evidence |
| **KANON-DEEPEN-009** | Policy/gate explanation | **Done** (build); Docker browser pending | `/kanon/policies` explain/simulate — see 009 evidence |
| **KANON-DEEPEN-010** | Domain-pack diff and impact | **Next** | Safe ontology/domain evolution review |
| **KANON-DEEPEN-011** | Semantic evidence graph | Planned | Provenance as navigable graph |
| **EVIDENCE-SPINE-001** | Unified cross-service evidence resolver | Deferred | Supersedes identifier-only links from 005 |
| **KANON-DEEPEN-014** | Docker/browser QA + baseline candidate | Planned | Batch 007–009 browser walkthrough |

## Recommended order

1. Execute **007–009 Docker browser manual QA** (rebuilt `kanon-api` + `ontogony-frontend` images).
2. Proceed with **KANON-DEEPEN-010** (domain-pack diff/impact).
3. Defer **EVIDENCE-SPINE-001** until the spine package is scheduled — do not expand 005 into a spine implementation ad hoc.
