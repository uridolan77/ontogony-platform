# Acceptance Gates

## Safety

- [ ] real external execution remains disabled
- [ ] sandbox production block remains
- [ ] no raw prompts/completions/secrets/marker content in artifacts/UI
- [ ] manual eval POST remains gated
- [ ] fixture/live distinction visible

## Backend

- [ ] OpenAPI duplicate-key checks pass
- [ ] OpenAPI snapshots/provenance updated
- [ ] durable persistence tests pass where applicable
- [ ] eval CI artifacts validate
- [ ] route evidence safe

## Frontend

- [ ] OpenAPI check passes
- [ ] adapters/components tested
- [ ] dashboards distinguish fixture/live
- [ ] unknown metrics/events safe
- [ ] E2E sanity route passes

## System

- [ ] full sanity report produced
- [ ] scorecard produced
- [ ] known limitations documented
