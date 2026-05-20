# EVIDENCE-SPINE-006 — Graph workbench UI (platform index)

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS**  
**Statement:** Operators paste identifiers at `/system/evidence-spine` and inspect resolved graphs with source attempts, missing edges, page links, and export.

## Delivered

| Artifact | Path |
| --- | --- |
| Frontend evidence | `ontogony-frontend/docs/evidence/EVIDENCE_SPINE_006_GRAPH_WORKBENCH_UI_EVIDENCE.md` |
| Workbench route | `/system/evidence-spine` |
| Core UI | `ontogony-frontend/src/evidence-spine/components/EvidenceSpineWorkbench.tsx` |

## Validation

```powershell
cd C:\dev\ontogony-frontend
npm test -- src/evidence-spine/components/EvidenceSpineWorkbench.test.tsx src/app/nav.test.ts
```

## Next

- EVIDENCE-SPINE-007 — export bundle
- EVIDENCE-SPINE-009 — closeout — [009 evidence](./EVIDENCE_SPINE_009_CLOSEOUT_EVIDENCE.md)
