# 01 — Current state reconciliation

## Source facts to reconcile

### Sprint 4 evidence exists

The following Sprint 4 items should be treated as closed if current service evidence is present:

| Item | Repo | Evidence / proof surface |
| --- | --- | --- |
| `ALLAGMA-EVIDENCE-001` | `allagma-dotnet` | `docs/evidence/ALLAGMA_EVIDENCE_001_EVIDENCE.md` |
| `ALLAGMA-STREAM-001` | `allagma-dotnet` | `docs/evidence/ALLAGMA_STREAM_001_EVIDENCE.md` |
| `CONEXUS-RETENTION-001` | `conexus-dotnet` | `docs/evidence/CONEXUS_RETENTION_001_EVIDENCE.md` |
| `KANON-CONEXUS-ASSIST-001` | `kanon-dotnet` | `docs/evidence/KANON_CONEXUS_ASSIST_001_EVIDENCE.md` |
| `KANON-DOMAINPACK-GOV-001` | `kanon-dotnet` | `docs/reviews/KANON_DOMAINPACK_GOV_001_VALIDATION_REPORT.md` and lifecycle governance docs |

### Platform sprint plan may still lag

`ontogony-platform/docs/_incoming/curated-review-package/ontogony_curated_review_package/02_SPRINT_PLAN.md` should be updated so Sprint 4 is no longer “deferred but important” if all above evidence exists.

### Runtime lock is stale

`allagma-dotnet/docs/system/ontogony-runtime.lock.json` still declares:

```json
"baseline": "SYSTEM-ALPHA-003"
```

and points to old evidence artifact paths. This should remain historical until a new full validation pass is completed.

## Correct next-state language

Use:

```text
Sprint 4 source/evidence closeout: closed.
Runtime baseline promotion: pending SYSTEM-ALPHA-004.
Production readiness: not claimed.
```

Do not use:

```text
Sprint 4 production complete
System production ready
Full live E2E complete
```

## Immediate reconciliation tasks

1. Update platform sprint plan.
2. Update platform acceptance matrix.
3. Add Sprint 4 closeout addendum.
4. Cross-link service evidence files.
5. Add explicit deferred follow-up:
   - `CONEXUS-RETENTION-002 — durable maintenance-run history`
6. Keep `SYSTEM-ALPHA-003` as historical until `SYSTEM-ALPHA-004-CUT`.
