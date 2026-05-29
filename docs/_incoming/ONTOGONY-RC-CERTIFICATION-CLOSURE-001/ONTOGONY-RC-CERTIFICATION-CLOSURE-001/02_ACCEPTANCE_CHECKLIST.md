# Acceptance Checklist

## Runtime port lock

- [ ] Runtime lock declares the canonical five-service port map.
- [ ] Docker compose bindings match the lock.
- [ ] Every live-cert script uses the same source of truth.
- [ ] A preflight fails if `GET :5084/ready` is not Metabole.
- [ ] A preflight fails if `GET :5085/ready` is not Aisthesis.
- [ ] Evidence file records service identity for ports 5081–5085.

## Metabole producer

- [ ] `POST /metabole/v0/pipeline-runs/schema-profile` returns 2xx in live stack.
- [ ] Response includes `traceId`.
- [ ] Response includes `pipelineRunId`.
- [ ] Pipeline completes.
- [ ] SLOD candidates are generated.
- [ ] Kanon validation edge is present when Kanon HTTP is enabled.
- [ ] Aisthesis observes `producer=metabole`.
- [ ] Aisthesis bundle contains required Metabole edges.

## Allagma producer

- [ ] `POST /allagma/v0/runs` returns 2xx in live stack.
- [ ] Response includes `traceId`.
- [ ] Response includes `runId`.
- [ ] Run completes or reaches governed terminal state.
- [ ] Aisthesis observes `producer=allagma`.
- [ ] Aisthesis bundle contains required Allagma edges.

## Allagma phenomenological bridge drift

- [ ] `phenomenological-projection` route appears in route inventory if implemented.
- [ ] OpenAPI snapshot is refreshed.
- [ ] Feature connection matrix is refreshed.
- [ ] Event vocabulary includes bridge events.
- [ ] Run audit evidence docs are refreshed.
- [ ] `dotnet test Allagma.sln -c Release` no longer fails on this drift.

## Kanon package-mode

- [ ] `ReplayTarget` and related replay DTOs are in the correct package assembly.
- [ ] `Kanon.Contracts` packs successfully.
- [ ] Allagma package-mode build passes.
- [ ] Package-mode tests do not rely on sibling source references.

## Conexus cache metrics

- [ ] `RecordCache_instruments_emit_lookup_hit_and_miss` passes.
- [ ] Cache lookup hit/miss instruments emit stable names.
- [ ] Tags are stable and documented.
- [ ] Full Conexus gateway hardening acceptance wrapper passes.

## Aisthesis diagnostics

- [ ] Live-cert failure summary includes trigger profile/url/body hash/status/body excerpt.
- [ ] Service identity preflight is written to summary.
- [ ] Missing producer trace is classified as `producer_trigger_no_trace`.
- [ ] Missing edges are grouped by producer and edge type.

## Full rerun

- [ ] dotnet tests pass for all relevant repos.
- [ ] package-mode gates pass.
- [ ] five-service live cert passes.
- [ ] system cohesion acceptance passes.
- [ ] Production readiness remains not claimed.
