# Cross-service evidence spine — closeout

**Program:** Ontogony-Cross-Service-Evidence-Spine-Package-v1 (EVIDENCE-SPINE-000 through EVIDENCE-SPINE-009)  
**Date:** 2026-05-20  
**Posture:** Docker-local operator console — **not production readiness**  
**Overall verdict:** **PASS** (mocked e2e + unit/schema gates); Docker-live QA **PARTIAL** — [009A evidence](../evidence/EVIDENCE_SPINE_009A_DOCKER_LIVE_QA_EVIDENCE.md)

## Summary

The evidence spine package delivers a single operator pattern: **paste any supported known ID → resolve the cross-service execution graph**. Implementation is frontend-first (`ontogony-frontend/src/evidence-spine/`) using existing Allagma, Conexus, and Kanon GET routes—no new backend aggregator. Platform owns taxonomy, JSON Schema for export bundles, and evidence governance.

The workbench at `/system/evidence-spine` shows nodes, edges, source attempts, missing-link reasons, page links, and a redacted export bundle validated as `ontogony-cross-service-evidence-spine-bundle-v1`.

## Sequence outcomes

| ID | Outcome |
| --- | --- |
| 000 | Cross-repo audit: trace resolver vs unified spine gaps |
| 001 | Canonical ID taxonomy, graph types, parser, platform operator doc |
| 002 | `resolveEvidenceSpine` — multi-root resolver with source attempts |
| 003 | Allagma run/eval/baseline/audit normalization + embedded panels |
| 004 | Conexus model-call, route-decision, execution-run graph adapters |
| 005 | Kanon decision/provenance linking; `forbidden` attempt status |
| 006 | Evidence spine workbench UI + deep links from product surfaces |
| 007 | Export bundle builder + platform JSON Schema + redaction policy |
| 008 | Playwright e2e (mocked APIs) + Docker verification script |
| 009 | Closeout, scorecard, limitations, next options |

## Evidence index

- Sequence: [`docs/evidence/EVIDENCE_SPINE_SEQUENCE_STATUS.md`](../evidence/EVIDENCE_SPINE_SEQUENCE_STATUS.md)
- Closeout QA: [`docs/evidence/EVIDENCE_SPINE_009_CLOSEOUT_EVIDENCE.md`](../evidence/EVIDENCE_SPINE_009_CLOSEOUT_EVIDENCE.md)
- Taxonomy: [`docs/operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md`](../operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md)
- Audit: [`docs/reviews/EVIDENCE_SPINE_000_CURRENT_STATE_AUDIT.md`](../reviews/EVIDENCE_SPINE_000_CURRENT_STATE_AUDIT.md)

## Operator surface

| Surface | Location |
| --- | --- |
| Primary workbench | `http://localhost:5175/system/evidence-spine` |
| Embedded panels | Allagma run/eval detail, Conexus model-call/route panels, Kanon provenance workbench |
| Legacy trace lookup | System overview, Conexus observability (`resolveTraceCorrelation`) — retained |
| Export schema | `docs/schemas/ontogony-cross-service-evidence-spine-bundle-v1.schema.json` |

## Related programs (out of scope)

- **ALLAGMA-ACTION** — run operations v2 (retry/cancel/replay); separate workbench
- **CONEXUS-DEEPEN / KANON-DEEPEN** — per-service observability; spine consumes their APIs
- **Backend unified resolve API** — intentionally deferred; v1 is client-side orchestration

## Sign-off criteria

- [x] EVIDENCE-SPINE-000–008 have per-slice evidence
- [x] EVIDENCE-SPINE-009 closeout, scorecard, limitations, next-options
- [x] Operator can paste supported IDs and resolve graph (unit + mocked e2e)
- [x] Limitations and next gaps documented
- [x] Export bundle schema validated in platform tests
- [x] Docker-live provenance + API QA (009A) — partial (Conexus model-call admin 404)
- [ ] Manual browser paste/export walkthrough (009B)
