# EVAL-PRODUCT-001 — Query/list contract evidence (platform index)

**Date:** 2026-05-19  
**Decision:** **Option A** — `GET /allagma/v0/evaluations` with cursor pagination and dashboard summary DTOs.

## Not production readiness

Product hardening only. Does not assert production readiness, real provider mode, or cloud deployment.

## Cross-repo deliverables

| Repo | Evidence |
| --- | --- |
| allagma-dotnet | [EVAL_PRODUCT_001_QUERY_LIST_CONTRACT_EVIDENCE.md](https://github.com/uridolan77/allagma-dotnet/blob/main/docs/evidence/EVAL_PRODUCT_001_QUERY_LIST_CONTRACT_EVIDENCE.md) |
| ontogony-frontend | [EVAL_PRODUCT_001_FRONTEND_QUERY_LIST_CONTRACT_EVIDENCE.md](https://github.com/uridolan77/ontogony-frontend/blob/main/docs/evidence/EVAL_PRODUCT_001_FRONTEND_QUERY_LIST_CONTRACT_EVIDENCE.md) |
| ontogony-platform | Gap matrices `03`–`08` under `docs/product-hardening/eval-alignment-frontend-depth/` |

## Package matrix updates

- `03_EVAL_PRODUCT_GAP_MATRIX.md` — global list + dashboard model closed for P0
- `04_BACKEND_FRONTEND_ALIGNMENT_GAP_MATRIX.md` — dashboard list **aligned**
- `06_CONTRACT_AND_OPENAPI_STATE.md` — global route in snapshot + FE client
- `07_TEST_AND_EVIDENCE_STATE.md` — new evidence links
- `08_KNOWN_LIMITATIONS.md` — removed “no global list”; added filter/limit caveats

## Next implementation (unchanged sequence)

`ALIGN-PRODUCT-001` **done** — see `ALIGN_PRODUCT_001_CONTRACT_MATRIX_REFRESH_EVIDENCE.md`. Next: **FE-PRODUCT-001**.
