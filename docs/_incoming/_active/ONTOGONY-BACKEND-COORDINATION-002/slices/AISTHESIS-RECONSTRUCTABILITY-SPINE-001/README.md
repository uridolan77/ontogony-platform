# Slice 7 — AISTHESIS-RECONSTRUCTABILITY-SPINE-001

**Owner:** `aisthesis-dotnet`  
**Depends on:** Slice 6  
**May absorb:** `AISTHESIS-LIVE-FIVE-SERVICE-PASS-009`  
**Prompt:** [`../prompts/P07_AISTHESIS_RECONSTRUCTABILITY_SPINE_001.md`](../prompts/P07_AISTHESIS_RECONSTRUCTABILITY_SPINE_001.md)

## Goal

**Live** five-service reconstructability certification — not fixture-only ingestion.

## Pass criteria

```json
{
  "mode": "Live",
  "status": "PASS",
  "requiredEdges": { "present": 10, "missing": 0 },
  "reconstructabilityGrade": "complete"
}
```

## Deliverables

1. SDK/`global.json` aligned with workspace policy
2. Optional `docs/system/` matrices for Aisthesis boundaries
3. Native producer edges from Allagma, Kanon, Conexus, Metabole
4. Live certification script PASS with committed evidence
5. Runbook updated with exact commands

## Evidence

`aisthesis-dotnet/docs/evidence/AISTHESIS_RECONSTRUCTABILITY_SPINE_001_CLOSEOUT.md`

## Boundary

Aisthesis observes and reconstructs — does not own semantic decisions or execution.
