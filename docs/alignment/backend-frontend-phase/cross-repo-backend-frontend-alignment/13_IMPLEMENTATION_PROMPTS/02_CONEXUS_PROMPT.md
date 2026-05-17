# Prompt — BFA-002 Conexus Request Observability

Repo: uridolan77/conexus-dotnet

Goal:
Add a real admin request search/list API.

Tasks:
1. Inspect current diagnostics execution-run by request-id endpoint.
2. Add paged admin list/search endpoint.
3. Support filters: requestId, modelCallId, traceId, runId, decisionId, projectId, provider, model, status, time range.
4. Return redacted summaries.
5. Add detail endpoint if current one is insufficient.
6. Add OpenAPI docs/schemas.
7. Add tests for success, no results, unauthorized, validation, paging, redaction.

Validation: `dotnet test`; generated OpenAPI contains paths and schemas.
