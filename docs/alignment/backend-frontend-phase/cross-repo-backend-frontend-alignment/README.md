# Cross-Repo Backend/Frontend Alignment Package

This package plans the next alignment phase after `ontogony-frontend` Phase I.

The frontend is now a 9+ operator-console release-candidate implementation. It has strong route coverage, E2E coverage, release provenance, redaction gates, performance budgets, and evidence-export consistency. The next system-level value is not more frontend UI. It is making the backend repos expose the contracts that the frontend is already prepared to consume honestly.

Target repos:

| Repo | Role |
|---|---|
| `uridolan77/ontogony-platform` | Shared headers, errors, tracing, OpenTelemetry, integration primitives |
| `uridolan77/conexus-dotnet` | LLM gateway, chat, streaming, request diagnostics, projects/keys/aliases |
| `uridolan77/kanon-dotnet` | Semantic authority, facts, plans, decisions, provenance, replay bundles |
| `uridolan77/allagma-dotnet` | Governed runtime, runs, gates, replay/evidence, execution operations |
| `uridolan77/ontogony-frontend` | Contract consumer, operator UI, CI/release evidence gate |

## Core thesis

Frontend Phase I proved the operator workflows. Backend alignment should now make those workflows fully live, typed, observable, and reproducible across repos.

Do not add fake endpoints. Do not weaken frontend limitation banners. Only remove a limitation when the backend route exists, is documented in OpenAPI, has integration tests, and is consumed through generated/frontend contract checks.

## Recommended execution order

1. Platform shared contract baseline.
2. Conexus request observability/list/search contract.
3. Allagma run operation and typed replay contract.
4. Kanon provenance/replay schema hardening.
5. Cross-service trace metadata conformance.
6. Backend OpenAPI/provenance publication.
7. Local stack + CI cohesion test.
8. Frontend snapshot refresh and limitation cleanup.
9. End-to-end system certification.
