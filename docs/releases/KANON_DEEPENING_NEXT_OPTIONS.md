# Kanon deepening — next options



After v1 closeout (000–006) and v2 slices **007–011** at source/API/unit/frontend-build level, consider these follow-ups.



| ID | Theme | Status (2026-05-20) | Rationale |

|---|---|---|---|

| **KANON-DEEPEN-007** | Conexus assistance operator UI | **Done** (build); Docker browser pending | `/kanon/assistance` — see 007 evidence |

| **KANON-DEEPEN-008** | Durable fact/plan history | **Done** (build); Docker browser pending | GET history routes + workbench panels — see 008 evidence |

| **KANON-DEEPEN-009** | Policy/gate explanation | **Done** (build); Docker browser pending | `/kanon/policies` explain/simulate — see 009 evidence |

| **KANON-DEEPEN-010** | Domain-pack diff and impact | **Done** (build); Docker browser pending | Diff/impact/migration/simulate on `/kanon/domain-packs` — see 010 evidence |

| **KANON-DEEPEN-011** | Semantic evidence graph | **Done** (build); Docker browser pending | `GET /ontology/v0/semantic-graph` + decisions panel — see 011 evidence |

| **KANON-DEEPEN-012** | Source-binding / ontology quality | **Next** | Mapping quality, contradictions, review priorities |

| **EVIDENCE-SPINE-001** | Unified cross-service evidence resolver | Deferred | Supersedes identifier-only links from 005 |

| **KANON-DEEPEN-014** | Docker/browser QA + baseline candidate | Planned | Batch 007–011 browser walkthrough |



## Recommended order



1. Execute **007–011 Docker browser manual QA** (rebuilt `kanon-api` + `ontogony-frontend` images).

2. Proceed with **KANON-DEEPEN-012** (source-binding and ontology quality loop).

3. Defer **EVIDENCE-SPINE-001** until the spine package is scheduled — do not expand 005 into a spine implementation ad hoc.


