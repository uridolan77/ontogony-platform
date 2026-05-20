# EVIDENCE-SPINE-002 — Frontend unified resolver evidence (platform index)

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS**  
**Statement:** Cross-service evidence resolver v1 implemented in `ontogony-frontend`; platform taxonomy doc from 001 remains authoritative for ID kinds.

## Delivered

| Repo | Artifact |
| --- | --- |
| `ontogony-frontend` | [`docs/evidence/EVIDENCE_SPINE_002_FRONTEND_UNIFIED_RESOLVER_EVIDENCE.md`](https://github.com/uridolan77/ontogony-frontend/blob/main/docs/evidence/EVIDENCE_SPINE_002_FRONTEND_UNIFIED_RESOLVER_EVIDENCE.md) |
| `ontogony-frontend` | `src/evidence-spine/resolveEvidenceSpine.ts` + tests |
| `ontogony-platform` | This evidence file |

## Operator contract (summary)

```ts
resolveEvidenceSpine(settings, { raw, kind?, includeExports?, includeEvents?, includeRelated? })
  → EvidenceResolutionResult { graph, attemptedSources, completeness, warnings }
```

`includeExports` / `includeEvents` / `includeRelated` are accepted on the input type for forward compatibility; v1 resolver does not yet load audit or export nodes unless expanded in a follow-up.

## Related docs

- [`docs/operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md`](../operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md)
- [`docs/reviews/EVIDENCE_SPINE_000_CURRENT_STATE_AUDIT.md`](../reviews/EVIDENCE_SPINE_000_CURRENT_STATE_AUDIT.md)

## Next platform touchpoint

Update `TRACE_CORRELATION_CONTRACT.md` when the evidence spine workbench route ships (EVIDENCE-SPINE-006).
