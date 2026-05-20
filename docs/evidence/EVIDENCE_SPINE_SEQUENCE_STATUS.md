# Cross-service evidence spine sequence status

**Package:** Ontogony-Cross-Service-Evidence-Spine-Package-v1  
**Last updated:** 2026-05-20 (EVIDENCE-SPINE-009A Docker-live QA)

| ID | Theme | Status | Platform evidence |
| --- | --- | --- | --- |
| EVIDENCE-SPINE-000 | Current state audit | Done | [000](./EVIDENCE_SPINE_000_CURRENT_STATE_AUDIT_EVIDENCE.md), [review](../reviews/EVIDENCE_SPINE_000_CURRENT_STATE_AUDIT.md) |
| EVIDENCE-SPINE-001 | Resolver contract + ID taxonomy | Done | [001](./EVIDENCE_SPINE_001_RESOLVER_CONTRACT_EVIDENCE.md) |
| EVIDENCE-SPINE-002 | Frontend unified resolver v1 | Done | [002](./EVIDENCE_SPINE_002_FRONTEND_UNIFIED_RESOLVER_EVIDENCE.md) |
| EVIDENCE-SPINE-003 | Allagma evidence normalization | Done | `ontogony-frontend/docs/evidence/EVIDENCE_SPINE_003_*` |
| EVIDENCE-SPINE-004 | Conexus request + route linking | Done | `ontogony-frontend/docs/evidence/EVIDENCE_SPINE_004_*` |
| EVIDENCE-SPINE-005 | Kanon decision provenance linking | Done | `ontogony-frontend/docs/evidence/EVIDENCE_SPINE_005_*` |
| EVIDENCE-SPINE-006 | Graph workbench UI | Done | [006 platform index](./EVIDENCE_SPINE_006_GRAPH_WORKBENCH_UI_EVIDENCE.md), frontend detail |
| EVIDENCE-SPINE-007 | Export bundle + diagnostic pack | Done | [007](./EVIDENCE_SPINE_007_EXPORT_BUNDLE_EVIDENCE.md) |
| EVIDENCE-SPINE-008 | E2E / browser verification | Done | [008](./EVIDENCE_SPINE_008_E2E_BROWSER_VERIFICATION_EVIDENCE.md) |
| EVIDENCE-SPINE-009 | Closeout | Done | [009](./EVIDENCE_SPINE_009_CLOSEOUT_EVIDENCE.md) |
| EVIDENCE-SPINE-009A | Docker-live QA | Done (partial) | [009A](./EVIDENCE_SPINE_009A_DOCKER_LIVE_QA_EVIDENCE.md) |

## Verification matrix

| Item | Unit tests | Schema / platform | Playwright E2E | Docker-local browser |
| --- | --- | --- | --- | --- |
| 001–002 | Frontend `src/evidence-spine` | Taxonomy doc | — | — |
| 003–005 | Per-service adapter tests | — | — | — |
| 006–007 | Workbench + export builder | `EvidenceSpineExportBundleSchemaTests` | — | — |
| 008 | Included in spine suite | — | **PASS** (mocked) | Checklist scripted |
| 009 | Closeout aggregation | Re-run on closeout date | — | — |
| 009A | — | — | — | **PARTIAL** — provenance PASS; model-call admin 404 |

## Operator entry point

`/system/evidence-spine` — paste any supported known ID → resolved graph, source attempts, missing edges, redacted export bundle.

Closeout: [`docs/releases/CROSS_SERVICE_EVIDENCE_SPINE_CLOSEOUT.md`](../releases/CROSS_SERVICE_EVIDENCE_SPINE_CLOSEOUT.md)
