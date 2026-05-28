# AISTHESIS producer alignment 004 — live proof status

**Date:** 2026-05-28

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
| `status` | **NOT_RUN** |
| Reason | Coordinated live run with all five services emitting native evidence was not executed in this alignment session. Metabole was not running on 5085 during fixture smoke readiness probe. |

## Next step for live PASS

1. Start Kanon (5081), Conexus (5082), Allagma (5083), Aisthesis (5084), Metabole (5085) with Aisthesis emission enabled.
2. Execute a governed run that touches Kanon plan/decision, Conexus model call, and Metabole pipeline lifecycle.
3. Run:

```powershell
cd C:\Dev\aisthesis-dotnet
./scripts/system/run-five-service-aisthesis-live-smoke.ps1 -Mode Live
```

4. Require summary:

```json
{
  "mode": "Live",
  "status": "PASS",
  "requiredEdges": { "present": 10, "missing": 0 },
  "reconstructabilityGrade": "complete"
}
```

Do not claim live proof until that summary exists.
