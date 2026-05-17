# Execution Sequence

## BFA-000 — Alignment package registration
Register this package under a coordination docs folder. No product code changes.

## BFA-001 — Shared platform contract baseline
Repo: `ontogony-platform`

Define shared headers, metadata keys, error envelopes, and OpenAPI service metadata. This is the foundation for all service alignment.

## BFA-002 — Conexus request observability API
Repo: `conexus-dotnet`

Add paged admin request list/search and stable request detail schemas. Support requestId, modelCallId, traceId, runId, decisionId, projectId, provider, model, status, and time range filters.

## BFA-003 — Allagma run operation contracts
Repo: `allagma-dotnet`

Decide and implement or explicitly mark unsupported: resume, retry, cancel, replay trigger, start run. Expose capability metadata.

## BFA-004 — Allagma typed replay/evidence schema
Repo: `allagma-dotnet`

Add stable typed replay/evidence response schemas, replay status, idempotency summary, decision/provenance references, Conexus model-call references, and unsupported-operation metadata.

## BFA-005 — Kanon provenance/replay contract hardening
Repo: `kanon-dotnet`

Stabilize replay bundle lookup contracts by decisionId, traceId, entity reference, and runId where applicable. Include domain-pack lifecycle decision IDs and source-binding/provenance references.

## BFA-006 — Cross-service trace metadata conformance
Repos: all backend repos

Adopt platform constants and prove the same envelope flows through Allagma, Kanon, and Conexus.

## BFA-007 — Backend OpenAPI provenance publication
Repos: backend repos + frontend

Every backend CI build publishes OpenAPI JSON and provenance JSON. Frontend snapshot refresh records backend repo, commit, snapshot SHA, and refresh date.

## BFA-008 — Local stack compatibility harness
Repo: `ontogony-platform` or dedicated system scripts

Start Conexus/Kanon/Allagma with test providers and produce a compatibility report.

## BFA-009 — Frontend snapshot refresh and limitation cleanup
Repo: `ontogony-frontend`

Refresh OpenAPI, regenerate clients, update provenance, and remove limitation banners only where backend support is real.

## BFA-010 — Cross-repo release certification
Record all five SHAs, OpenAPI hashes, CI runs, compatibility report, and frontend `check:full`.
