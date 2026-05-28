# AISTHESIS producer alignment 004 — live proof status

**Date:** 2026-05-28 (updated)

## Fixture mode (Aisthesis harness)

| Field | Value |
|---|---|
| `mode` | `Fixture` |
| `status` | **PASS** |
| `requiredEdges.present` | 10 |
| `requiredEdges.missing` | 0 |
| `reconstructabilityGrade` | `complete` |
| Artifact | `aisthesis-dotnet/artifacts/five-service-aisthesis-live-smoke/20260528T080350Z/summary.json` |

Fixture ingestion uses `fixtures/CROSS_SYSTEM_TRACE_REQUIRED_EDGES_V1.evidence.json`. This validates the Aisthesis evaluator and harness; it is **not** native producer live proof.

## Live mode (five native services)

| Field | Value |
|---|---|
| `mode` | `Live` |
| `status` | **PASS** |
| Package | `ONTOGONY-LIVE-EVIDENCE-SPINE-001` |
| Artifact | `allagma-dotnet/docs/evidence/ONTOGONY_LIVE_EVIDENCE_SPINE_001_20260528_PASS.json` |
| `producersObserved` | allagma, kanon, conexus, metabole |
| `reconstructabilityV2Grade` | `complete` |
| `blockingFindings` | 0 |

Coordinated live native-emitter proof succeeded via Allagma → Metabole SLOD orchestration (LES-001) and Metabole-first schema-profile entry (LES-002).

### LES-001 (Allagma-first)

| Field | Value |
|---|---|
| Package | `ONTOGONY-LIVE-EVIDENCE-SPINE-001` |
| Artifact | `allagma-dotnet/docs/evidence/ONTOGONY_LIVE_EVIDENCE_SPINE_001_20260528_PASS.json` |
| `reconstructabilityV2Grade` | `complete` |
| `blockingFindings` | 0 |

### LES-002 (Metabole-first)

| Field | Value |
|---|---|
| Package | `ONTOGONY-LIVE-EVIDENCE-SPINE-002` |
| Artifact | `allagma-dotnet/docs/evidence/ONTOGONY_LIVE_EVIDENCE_SPINE_002_20260528_PASS.json` |
| traceId | `ad40b7bbb0b54b108d31168c33268a68` |
| `reconstructabilityV2Grade` | `partial` (0.82) |
| `blockingFindings` | 0 |
| Driver | `-Workflow MetaboleFirstSlod` |

## Runtime lock

`SYSTEM-RC-003` / Aisthesis `lockRequired` promotion: **Aisthesis lock review closed 2026-05-28** — `liveProducerSmoke: pass`, `lockRequired` remains **false** until five-service CI + ReleaseMode gates (`SYSTEM_RC_003_AISTHESIS_LOCK_REVIEW_CLOSEOUT.md`). Full baseline RC-003 promotion still deferred (`SYSTEM_RC_003_DRY_RUN_DEFERRAL.md`).
