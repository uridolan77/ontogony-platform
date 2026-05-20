# Conexus deepening — known limitations

Consolidated from CONEXUS-DEEPEN-000 through 007 evidence. **Not** a production readiness statement. For gateway alpha caveats see `conexus-dotnet/docs/operations/KNOWN_LIMITATIONS.md`.

## Persistence and telemetry

- **In-memory mode** (`persistenceMode: in_memory`): request list, route decisions, and usage aggregates are **lost on gateway process restart**. UI warns via persistence banners.
- **Postgres mode** (`persistenceMode: postgres`): durable in Docker-local when `CONEXUS_POSTGRES_CONNECTION_STRING` is set and migrations applied.
- **Retention jobs** may skip work when persistence is disabled (`persistence_disabled` warnings).
- EF list queries with combined post-filters may scan a bounded batch (see 001 evidence).

## Redaction and exports

- Admin list/detail DTOs are **redacted summaries** — no raw prompts or completions by default.
- Model-call evidence export (`conexus.model-call.evidence.v1`) includes structured metadata and links, not raw provider payloads.
- Raw payload capture depends on gateway artifact policy; hash-only artifact refs may be empty.
- Diagnostic export uses operator redaction rules; secrets must not appear in bundles.

## Routing and provider attempts

- `routeDecisionId` may be absent on older or pre-routing telemetry rows.
- Route decision `modelCallId` prefers related telemetry ids; `chatcmpl-{requestId}` is a fallback only when list is empty.
- Provider readiness on route detail reflects **current** gateway config, not historical config at request time.
- Real-provider traffic requires explicit local opt-in; fake provider is the Docker-default path.

## Usage and cost

- `GET /v1/governance/usage` requires **project API key** (separate from admin key).
- Failed requests are counted separately and excluded from token/cost totals.
- `requestsMissingTokenUsage` is explained in UI; drill-down uses `tokenUsage=missing` filter — values are not invented.

## Cross-service links (005)

- **Not** the platform Cross-Service Evidence Spine package.
- Allagma run GET may omit `planningDecisionId` / `actionEvaluationDecisionId`; links are best-effort from fields present at render time.
- Kanon decision id may be unresolved; trace-scoped Kanon search link is offered when trace is known.
- Correlation resolver still requires known ids for some targets; Recent requests reduces that gap.

## Operator UI (006)

- Admin HTTP APIs remain the Conexus product surface; **ontogony-frontend** hosts `/conexus/observability` (not a Conexus-repo UI).
- Tab navigation uses URL query params; bookmarking works; no separate routes per tab.
- Route decisions tab needs `routeDecisionId` unless opened from a recent row or detail link.

## Auth and environment

- Admin APIs use Conexus admin key (`admin:read` minimum for observability routes). No per-operator human identity in Conexus audit rows (alpha auth model).
- CORS / 403 / unreachable gateway must be distinguished in operator settings and live-query empty states.
- Swagger and dev bootstrap are Development-oriented; Production has stricter validation.

## Testing and verification

- Automated E2E for observability uses **mocked** Conexus admin responses.
- Docker-local browser checklist in 007 evidence is **not marked complete** in this closeout pass.
- Load/soak evidence uses fake provider harness — not production traffic proof.

## Documentation drift

- `conexus-dotnet/docs/operations/KNOWN_LIMITATIONS.md` § “No admin UI” means no **Conexus-hosted** admin portal; update mental model to include **frontend operator console** for observability.
