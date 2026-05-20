# Conexus deepening sequence — status index

**Last updated:** 2026-05-20  
**Package:** `Ontogony-Conexus-Deep-Enhancement-Package-v1`  
**Audit:** [`docs/reviews/CONEXUS_DEEPEN_000_CURRENT_STATE_AUDIT.md`](../reviews/CONEXUS_DEEPEN_000_CURRENT_STATE_AUDIT.md)  
**Backend audit:** `conexus-dotnet/docs/reviews/CONEXUS_DEEPEN_000_CURRENT_STATE_AUDIT.md`

| Item | Status | Primary evidence |
| --- | --- | --- |
| CONEXUS-DEEPEN-000 | Done | [000 evidence](./CONEXUS_DEEPEN_000_CURRENT_STATE_AUDIT_EVIDENCE.md) |
| CONEXUS-DEEPEN-001 | Done | [001 evidence](./CONEXUS_DEEPEN_001_REQUEST_LIFECYCLE_LIST_EVIDENCE.md) |
| CONEXUS-DEEPEN-002 | Done | [002 evidence](./CONEXUS_DEEPEN_002_MODEL_CALL_DETAIL_EVIDENCE.md) |
| CONEXUS-DEEPEN-003 | Done | [003 evidence](./CONEXUS_DEEPEN_003_ROUTE_DECISION_EXPLORER_EVIDENCE.md) |
| CONEXUS-DEEPEN-004 | Done | [004 evidence](./CONEXUS_DEEPEN_004_USAGE_COST_WORKBENCH_EVIDENCE.md) |
| CONEXUS-DEEPEN-005 | Done | [005 evidence](./CONEXUS_DEEPEN_005_CROSS_SERVICE_EVIDENCE_SPINE_EVIDENCE.md) |
| CONEXUS-DEEPEN-006 | Done | [006 evidence](./CONEXUS_DEEPEN_006_FRONTEND_OBSERVABILITY_V2_EVIDENCE.md) |
| CONEXUS-DEEPEN-007 | Done (docs/validation; browser QA checklist) | [007 closeout evidence](./CONEXUS_DEEPENING_CLOSEOUT_EVIDENCE.md) |

**Closeout bundle:** [`docs/releases/CONEXUS_DEEPENING_CLOSEOUT.md`](../releases/CONEXUS_DEEPENING_CLOSEOUT.md)

## Implemented admin / governance contracts

| Contract | Method | Purpose |
| --- | --- | --- |
| Model-call list | `GET /admin/v0/model-calls` | Recent requests, filters, cursor pagination |
| Model-call detail | `GET /admin/v0/model-calls/{modelCallId}` | Summary, attempts, tokens, route id |
| Evidence links | `GET /admin/v0/model-calls/{modelCallId}/evidence-links` | Cross-service spine slots |
| Route decision detail | `GET /admin/v0/route-decisions/{routeDecisionId}` | Routing explanation |
| Usage summary | `GET /v1/governance/usage` | Window aggregates + drill-down |
| Diagnostics summary | `GET /admin/v0/diagnostics/summary` | Table counts, warnings |
| Execution run lookup | `GET /admin/v0/diagnostics/execution-runs/by-request-id/{requestId}` | Per-request journal |

## Browser verification posture

| Item | Automated tests recorded | Docker browser walkthrough |
| --- | --- | --- |
| 001–006 | Unit + API integration + mocked Playwright E2E | **Pending** — operator checklist in 007 evidence |
| 007 | Closeout docs + validation commands | Same — rebuild `ontogony-frontend` + seed Conexus |

Do not claim full Docker-local browser verification until the [007 manual checklist](./CONEXUS_DEEPENING_CLOSEOUT_EVIDENCE.md) is executed and recorded.

## Not in scope for 000–007

- Unified **Cross-Service Evidence Spine** graph resolver (platform `EVIDENCE-SPINE-*` program).
- Raw prompt/completion operator UI (hash-only / redacted exports only).
- Production dashboards, alerting, or multi-tenant admin IAM.
