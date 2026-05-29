# Service Coverage Matrix

| Service / repo | Required automation areas | First target coverage |
|---|---|---:|
| Ontogony.Platform | shared error envelope, idempotency primitives, hashing, HTTP utilities, telemetry contracts, architecture boundary tests | 70% |
| Conexus.NET | chat completion, streaming policy, aliases, provider fallback, quotas, cost, auth, admin routes, idempotency, retention, metrics | 75% |
| Kanon.NET | ontology versions, source bindings, semantic plans, canonical facts, action policy, human gates, decisions, replay bundles, Conexus assistance | 75% |
| Allagma.NET | run lifecycle, event stream, Kanon integration, Conexus integration, human gate pause/resume, idempotency, restart, evidence export | 80% |
| Metabole.NET | data-spine/evolution/transformation flows, versioning, provenance, failure cases, integration with Kanon/Allagma where applicable | 65% |
| Aisthesis.NET | phenomenological memory write/read/retrieve, temporal traces, reflection, privacy boundaries, Kanon alignment | 65% |
| Ontogony Frontend | page coverage, service coverage, API calls, error states, human gate UX, run evidence UX | 60% |
| Ontogony UI | shared components, tables/forms/modals/state components, accessibility, visual state coverage, contract with frontend | 70% |

## Coverage definition

Coverage here does not mean only line coverage. It means **manual regression coverage**:

```text
A human who used to manually check the same service behavior can stop checking it every time because the harness now checks it reliably.
```

## Coverage scoring dimensions

Each service gets a score from 0-10 in each dimension:

1. Startup and readiness.
2. Auth and authorization.
3. API happy paths.
4. API negative paths.
5. Persistence and restart.
6. Idempotency/replay.
7. Error envelope consistency.
8. Observability.
9. Cross-service integration.
10. UI coverage.

A service reaches `manual replacement ready` when:

- no score is below 6;
- critical dimensions are 8+;
- evidence bundles are generated;
- tests run in one command locally and in CI/manual workflow.
