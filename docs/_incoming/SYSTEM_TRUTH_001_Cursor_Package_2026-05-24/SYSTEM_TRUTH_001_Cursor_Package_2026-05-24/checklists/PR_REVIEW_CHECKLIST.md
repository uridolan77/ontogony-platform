# PR Review Checklist — SYSTEM-TRUTH-001

## Truth semantics

- [ ] Healthy is not used as synonym for ready.
- [ ] Ready is not used as synonym for release-ready.
- [ ] Unknown is never green.
- [ ] Fixture/generated states are visibly non-authoritative.
- [ ] Partial states list missing reasons.

## Backend

- [ ] `/health` validates against health.v1.
- [ ] `/ready` validates against ready.v1.
- [ ] Version metadata present.
- [ ] Conexus not-ready/degraded state is explained.
- [ ] Optional provider warnings do not block local fake path unless required.

## Frontend

- [ ] Home is not contradictory.
- [ ] Settings shows exact health/readiness/contract state.
- [ ] Topology separates current/planned/fixture.
- [ ] Release readiness does not overstate generated artifacts.
- [ ] Data-source badges are present.

## Tests

- [ ] Parser tests added.
- [ ] Aggregation tests added.
- [ ] Smoke script passes or produces expected warning.
- [ ] No fixture-only value contributes to passed release posture.
