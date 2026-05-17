# conexus-dotnet Alignment Plan

## Objective

Make Conexus fully observable from the frontend and stable as the LLM gateway contract.

## PRs

### CONEXUS-BFA-001 — Request list/search API
Add `GET /admin/v0/diagnostics/execution-runs` with filters for requestId, modelCallId, traceId, runId, decisionId, projectId, provider, model, status, and time range.

### CONEXUS-BFA-002 — Request detail contract
Ensure detail includes usage/tokens, provider/model, project, trace/run/decision metadata, status/failure reason, redacted warnings/errors, and timestamps.

### CONEXUS-BFA-003 — Streaming OpenAPI contract
Document SSE response behavior or add a companion streaming contract.

### CONEXUS-BFA-004 — Project list API or explicit unsupported capability
Expose project list or return explicit capability metadata saying unavailable.

### CONEXUS-BFA-005 — Frontend consumption
Refresh frontend OpenAPI, add request list/search UI, update E2E, remove only relevant limitation banners.

## Acceptance

- Admin request search/list has tests.
- Search does not leak prompts/headers/secrets.
- Frontend can navigate modelCallId ↔ requestId ↔ traceId.
