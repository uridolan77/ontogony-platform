# 07 — Acceptance Criteria

## Backend acceptance

- [ ] Conexus `/health` returns valid `health.v1`.
- [ ] Kanon `/health` returns valid `health.v1`.
- [ ] Allagma `/health` returns valid `health.v1`.
- [ ] Conexus `/ready` returns valid `ready.v1` with detailed checks.
- [ ] Kanon `/ready` returns valid `ready.v1` with detailed checks.
- [ ] Allagma `/ready` returns valid `ready.v1` with detailed checks.
- [ ] Every service exposes service name, version, baseline, environment, checked timestamp.
- [ ] Readiness checks have stable IDs and structured statuses.
- [ ] Conexus not-ready condition is explainable from `/ready`.
- [ ] Missing optional provider credentials are warnings, not hidden unknowns.
- [ ] Health endpoints remain safe for local unauthenticated read if that is current project convention.

## Frontend acceptance

- [ ] Home no longer shows "Live with fixture fallback" as page headline.
- [ ] Home no longer says "All services are healthy" when Conexus readiness is not ready.
- [ ] Home separates connectivity, readiness, contract health, compatibility, and data source.
- [ ] Settings shows why Conexus is not ready.
- [ ] Topology separates current, planned, fixture/demo edges.
- [ ] Release readiness is downgraded or renamed if generated/fixture-only.
- [ ] Compatibility artifact missing is shown as warning/unknown with remediation.
- [ ] Service version alignment is not claimed when version metadata is missing.
- [ ] Data-source badges appear on cards that use fixture/generated/imported data.
- [ ] Unknown values are labeled: unknown provider, unknown compatibility, unknown task type, etc.

## Artifact acceptance

- [ ] A compatibility manifest or summary artifact exists.
- [ ] It has schema version.
- [ ] It records generated timestamp.
- [ ] It records expected baseline.
- [ ] It records actual service health/readiness/contract state.
- [ ] It records warnings/failures.
- [ ] The frontend can read it or gracefully report it as missing.
- [ ] A smoke script can regenerate or validate it locally.

## Test acceptance

- [ ] Unit tests cover health parser valid/invalid payloads.
- [ ] Unit tests cover readiness parser valid/invalid payloads.
- [ ] Unit tests cover status aggregation rules.
- [ ] Integration/smoke tests hit `/health` and `/ready` for all services.
- [ ] UI tests assert that Conexus not-ready is not summarized as healthy.
- [ ] UI tests assert fixture-only does not count as release-ready.
