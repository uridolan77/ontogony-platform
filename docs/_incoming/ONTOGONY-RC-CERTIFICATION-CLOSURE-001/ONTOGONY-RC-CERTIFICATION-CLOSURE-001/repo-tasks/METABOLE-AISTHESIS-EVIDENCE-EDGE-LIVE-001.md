# METABOLE-AISTHESIS-EVIDENCE-EDGE-LIVE-001

## Owner repo

- `metabole-dotnet`

## Problem

Aisthesis live cert attempted `POST /metabole/v0/pipeline-runs/schema-profile` and received `502 Bad Gateway`. No `traceId` was produced, and Aisthesis observed no Metabole producer.

## Required behavior

The live stack Metabole schema-profile trigger must return 2xx, return `traceId`, return `pipelineRunId`, complete the schema-profile pipeline, generate SLOD candidates, call Kanon validation when Kanon HTTP is enabled, and emit required Aisthesis edges when Aisthesis producer is enabled.

## Minimum Aisthesis edges

```text
metabole.pipeline_run.created
metabole.schema_profile.started
metabole.schema_profile.completed
metabole.data_quality.evaluated
metabole.slod_candidates.generated
metabole.kanon_validation.requested
metabole.kanon_validation.completed
metabole.evidence_bundle.exported
```

## Acceptance

```powershell
cd C:\dev\metabole-dotnet
pwsh .\scripts\smokeun-metabole-five-service-certification.ps1 -DevRoot C:\dev -RequireLivePeers -RequirePass
pwsh .\scripts\systemerify-metabole-aisthesis-evidence-edges.ps1 -TraceId <traceId> -WriteEvidence
```
