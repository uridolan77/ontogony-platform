# ReleaseMode gate

## Goal

Ensure Aisthesis behavior is compatible with realistic deployment assumptions, not only local developer defaults.

## Required ReleaseMode assumptions

| Area | Requirement |
|---|---|
| Persistence | Postgres mode supported and documented |
| Auth | producer-token mode supported |
| Idempotency | durable idempotency when Postgres is configured |
| Payload refs | secret/protected refs warned but not dereferenced |
| Logs | no tokens, raw prompts, or sensitive payloads in evidence closeouts |
| Failures | no fake PASS when producers are unavailable |
| Bundle | deterministic fingerprint independent of export time |
| Reconstructability | missing edges produce diagnostics, not silent success |

## Proposed environment

```powershell
$env:ASPNETCORE_ENVIRONMENT = "Production"
$env:ConnectionStrings__Aisthesis = "<postgres connection>"
$env:Aisthesis__Auth__Mode = "producer-token"
$env:Aisthesis__Auth__Producers__allagma__Token = "<secret>"
$env:Aisthesis__Auth__Producers__allagma__CanWrite = "true"
$env:Aisthesis__Auth__Producers__kanon__Token = "<secret>"
$env:Aisthesis__Auth__Producers__kanon__CanWrite = "true"
$env:Aisthesis__Auth__Producers__conexus__Token = "<secret>"
$env:Aisthesis__Auth__Producers__conexus__CanWrite = "true"
$env:Aisthesis__Auth__Producers__metabole__Token = "<secret>"
$env:Aisthesis__Auth__Producers__metabole__CanWrite = "true"
```

## Acceptance

ReleaseMode is accepted only if:

- API starts;
- `/ready` passes;
- ingestion works with producer tokens;
- lookup/timeline/graph/bundle/reconstructability work;
- smoke summary is redacted;
- idempotent replay remains stable.
