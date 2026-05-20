# Conexus deepening — closeout

**Program:** Ontogony-Conexus-Deep-Enhancement-Package-v1 (items 000–007)  
**Date:** 2026-05-20  
**Posture:** Docker-local gateway operator observability — **not production readiness**

## Summary

The Conexus deepening sequence made the LLM gateway inspectable without pre-known ids: admin model-call list and detail, route decision explorer, usage/cost drill-down, cross-service evidence links, and a tabbed observability workbench in `ontogony-frontend`. Backend contracts live in `conexus-dotnet`; shared mechanics remain in `ontogony-platform`.

**Browser end-to-end verification** is operator-owned: rebuild the frontend image, seed the local stack, and run the [007 manual QA checklist](../evidence/CONEXUS_DEEPENING_CLOSEOUT_EVIDENCE.md).

## Sequence outcomes

| ID | Outcome |
| --- | --- |
| 000 | Grounded gap matrix — audit |
| 001 | `GET /admin/v0/model-calls` + Recent requests UI |
| 002 | Model-call detail + provider attempts workbench |
| 003 | Route decision explorer |
| 004 | Usage/cost workbench + drill-down to request rows |
| 005 | Evidence-links resolver + cross-service spine panel |
| 006 | Tabbed observability v2 (Recent, Lookup, Routes, Usage, Provider, Diagnostics) |
| 007 | Closeout docs, scorecard, limitations, manual QA checklist |

## Evidence index

[`docs/evidence/CONEXUS_DEEPEN_SEQUENCE_STATUS.md`](../evidence/CONEXUS_DEEPEN_SEQUENCE_STATUS.md)

## Operator surface

| Surface | Location |
| --- | --- |
| Primary workbench | `http://localhost:5175/conexus/observability` (compose port may vary) |
| Traffic generation | `/conexus/chat`, Allagma governed flows, `POST /admin/v0/dev/bootstrap` |
| API reference | `ontogony-frontend/openapi/conexus.v0.json` |

**Note:** `conexus-dotnet/docs/operations/KNOWN_LIMITATIONS.md` still states “No admin UI” for the **Conexus API product** (no first-party Conexus-hosted admin portal). Operator UI is delivered by **ontogony-frontend**, not inside the gateway repo.

## Related programs (out of scope)

- **EVIDENCE-SPINE** — unified cross-service evidence resolver (future; 005 is Conexus-native links + correlation only).
- **KANON-DEEPEN** — semantic authority workbenches (linked from Conexus spine).
- **UI-HARDEN** — shared `@ontogony/ui` primitives used by observability tabs.

## Sign-off criteria

- [x] 001–006 have platform evidence with implementation on main
- [x] 007 closeout docs, scorecard, and limitations exist
- [x] Automated validation recorded (API + frontend unit tests)
- [x] Honest gaps documented (in-memory mode, no raw payloads, browser QA pending)
- [ ] Browser walkthrough executed and recorded (operator)
