# Rollback plan

## Safe rollback principles

- Do not delete existing evidence docs or artifacts.
- Do not remove RC-readiness-005 artifacts if they are referenced by closeouts.
- Prefer additive docs/contracts/scripts.
- Code changes should be isolated behind config when possible.

## Rollback by slice

| Slice | Rollback |
|---|---|
| Matrix v2 | Keep v1 evaluator active; remove v2 from certification gate only |
| Live certification script | Revert script addition; keep fixture smoke |
| Client coverage | Revert client methods if API unchanged; update coverage doc |
| Evaluation jobs | Disable worker/scheduler via config; keep direct evaluation route |
| Edge auth | Revert to previous auth only if no producer workflows depend on stricter mode |
| Retention | Disable destructive actions; keep tombstone docs |
| OTel | Disable exporter/activity source via config |
| Frontend | Revert UI route wiring; keep docs handoff |

## Evidence rollback

If a bad evaluator or edge matrix misclassifies traces, record:

```text
docs/evidence/AISTHESIS_RC_CERTIFICATION_006_ROLLBACK_NOTE.md
```

Include affected trace IDs, wrong classification, fix commit, and regenerated summary.
