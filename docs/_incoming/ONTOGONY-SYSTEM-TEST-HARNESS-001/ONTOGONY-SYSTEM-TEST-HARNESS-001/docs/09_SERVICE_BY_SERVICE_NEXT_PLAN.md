# Service-by-Service Next Plan

After this starter harness is accepted, go service by service and convert the broad matrix into exact route-level plans.

## Order

1. Allagma.NET — because it is the runtime orchestrator and owns the most important system E2E flows.
2. Kanon.NET — because semantic authority/policy/human-gates are high-risk.
3. Conexus.NET — because model gateway/fallback/idempotency failures are expensive.
4. Metabole.NET — calibrate transformation/evolution/data-spine flows.
5. Aisthesis.NET — calibrate phenomenological memory flows and Kanon alignment.
6. Ontogony.Platform — shared primitives, error envelope, tracing, idempotency.
7. Ontogony Frontend — service coverage and endpoint usage.
8. Ontogony UI — shared infrastructure UI components, accessibility, state coverage.

## Deliverable per service

For each service produce:

```text
<SERVICE>-TEST-PLAN-001.md
<SERVICE>-ROUTE-MATRIX.yml
<SERVICE>-TEST-CASES.yml
<SERVICE>-FIXTURES/
<SERVICE>-HARNESS-IMPLEMENTATION-PROMPT.md
```

## Minimum sections per service-specific plan

1. Service definition and testing boundaries.
2. Current route/API inventory.
3. Current test inventory.
4. Missing coverage analysis.
5. Manual testing currently performed.
6. Manual testing candidates for automation.
7. Required seed data and fixtures.
8. Auth/role matrix.
9. Happy-path tests.
10. Negative-path tests.
11. Idempotency/retry/replay tests.
12. Persistence/restart tests.
13. Observability tests.
14. Cross-service integration tests.
15. UI coverage mapping.
16. CI gating recommendation.
17. Acceptance criteria.

## First service-specific plan recommendation

Start with **Allagma.NET**:

- It coordinates Kanon and Conexus.
- It exposes the human-visible run lifecycle.
- It is the best place to measure whether manual system testing is actually reduced.
- Its E2E suite will become the backbone for subsequent service plans.
