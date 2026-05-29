# ALLAGMA-AISTHESIS-LIVE-TRIGGER-001

## Owner repo

- `allagma-dotnet`

## Problem

Aisthesis live cert attempted `POST /allagma/v0/runs` and received `500 Internal Server Error`. No `traceId` was produced, and Aisthesis observed no Allagma producer.

## Required behavior

The live Allagma trigger must return 2xx for the cert workflow, include `traceId`, include `runId`, emit/forward Allagma evidence edges to Aisthesis when the producer is enabled, and return normalized orchestration errors with trace/correlation IDs on failure.

## Minimum Aisthesis edges

```text
allagma.run.created
allagma.run.planning_started
allagma.kanon.plan_requested
allagma.conexus.completion_requested
allagma.run.completed
allagma.evidence_bundle.exported
```

## Acceptance

```powershell
cd C:\devllagma-dotnet
pwsh .\scriptsun-allagma-cohesion-option-a.ps1 -Strict
pwsh .\scripts\systemun-system-cohesion-acceptance.ps1 -UseExistingServices -Quick
```
