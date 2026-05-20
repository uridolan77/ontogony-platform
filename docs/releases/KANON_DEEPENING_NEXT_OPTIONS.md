# Kanon deepening — next options



After v1 closeout (000–006) and v2 slices **007–014** (including Docker browser QA), consider these follow-ups.



| ID | Theme | Status (2026-05-20) | Rationale |

|---|---|---|---|

| **KANON-DEEPEN-007** | Conexus assistance operator UI | **Done** (build + browser) | `/kanon/assistance` — [014 evidence](../evidence/KANON_DEEPEN_014_BROWSER_QA_BASELINE_CANDIDATE_EVIDENCE.md) |

| **KANON-DEEPEN-008** | Durable fact/plan history | **Done** (build + browser) | GET history routes + workbench panels — 014 evidence |

| **KANON-DEEPEN-009** | Policy/gate explanation | **Done** (build + browser) | `/kanon/policies` explain/simulate — 014 evidence |

| **KANON-DEEPEN-010** | Domain-pack diff and impact | **Done** (build + browser) | Diff/impact/migration/simulate — 014 evidence |

| **KANON-DEEPEN-011** | Semantic evidence graph | **Done** (build + browser) | Semantic graph + decisions panel — 014 evidence |

| **KANON-DEEPEN-012** | Source-binding / ontology quality | **Done** (build + browser) | Quality loop on `/kanon/source-bindings` — 014 evidence |

| **KANON-DEEPEN-013** | Hardening / coherence cleanup | **Done** (build; browser N/A) | Route inventory, auth matrix, OpenAPI catalog — [013 evidence](../../../kanon-dotnet/docs/evidence/KANON_DEEPEN_013_SEMANTIC_AUTHORITY_HARDENING_EVIDENCE.md) |
| **KANON-DEEPEN-014A** | Status wording reconciliation | **Done** (docs) | [014A evidence](../evidence/KANON_DEEPEN_014A_STATUS_WORDING_RECONCILIATION.md) |

| **EVIDENCE-SPINE-001** | Unified cross-service evidence resolver | Deferred | Supersedes identifier-only links from 005 |

| **KANON-DEEPEN-014** | Docker/browser QA + baseline candidate | **Done** | Playwright 12/12 — see [014 evidence](../evidence/KANON_DEEPEN_014_BROWSER_QA_BASELINE_CANDIDATE_EVIDENCE.md) |



## Recommended order



1. **FE-EVIDENCE-SPINE-002** — fix B-013 Kanon `kanon.decision` node in evidence spine Docker-live graph.

2. Rebuild `allagma-api` / `conexus-api` compose images after SDK pin alignment (CORS header parity in source).

3. Defer **EVIDENCE-SPINE-001** until the spine package is scheduled — do not expand 005 into a spine implementation ad hoc.


