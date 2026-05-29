# AISTHESIS-LIVE-CERT-DIAGNOSTICS-001

## Owner repo

- `aisthesis-dotnet`

## Problem

Aisthesis live certification failed with no trace and empty `producersObserved`. The summary needs to make upstream failures immediately actionable.

## Required implementation

Enhance live cert summary with trigger profile, trigger URL, method, request body hash, HTTP status, response body excerpt, extracted traceId, extracted producer run ID, service identity preflight result, failure kind, producers observed, and missing edge groups.

## Failure kind taxonomy

```text
service_identity_mismatch
producer_trigger_http_failure
producer_trigger_no_trace
producer_trace_not_ingested
producer_edges_missing
producer_edges_incomplete
certification_pass
```

## Acceptance

```powershell
cd C:\devisthesis-dotnet
pwsh .\scripts\systemun-aisthesis-reconstructability-spine-001.ps1 -WorkspaceRoot C:\dev
pwsh .\scripts\systemun-five-service-live-certification.ps1 -Mode Live
```
