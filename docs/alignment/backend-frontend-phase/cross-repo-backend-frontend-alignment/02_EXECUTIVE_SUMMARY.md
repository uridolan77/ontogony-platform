# Executive Summary

`ontogony-frontend` is now ahead of the backend surface in several places. This is good: the UI has become an executable specification for what an operator-grade Ontogony system needs.

## Current strong points

- The frontend has release-route, E2E, evidence, redaction, provenance, and performance gates.
- Conexus has streaming client work and request-id diagnostic lookup.
- Kanon has domain-pack lifecycle decision tracking and semantic governance depth.
- Allagma has system cohesion evidence and increasingly rich execution telemetry.
- `ontogony-platform` has shared correlation headers, error helpers, and OpenTelemetry infrastructure.

## Main remaining backend/frontend gaps

1. Conexus request observability: request list/search/filter APIs for operator investigation.
2. Allagma run operations: decide and implement or explicitly reject retry/cancel/replay-trigger/start-run.
3. Typed Allagma replay/evidence contracts: move from flexible frontend parsing to typed backend schemas.
4. Cross-service trace shape: one canonical metadata envelope across Conexus, Kanon, and Allagma.
5. Backend OpenAPI provenance: backend artifacts should publish canonical OpenAPI snapshots with source commit evidence.
6. System CI: a cross-repo local stack test that proves frontend snapshot + backend OpenAPI + runtime endpoints agree.

## Intended result

- Frontend limitation banners correspond to explicit backend decisions, not unknowns.
- Every backend route used by the frontend has OpenAPI, tests, auth/error contracts, and trace metadata.
- Every cross-service ID is carried in a shared envelope.
- CI can produce a system evidence bundle linking frontend build provenance to backend commit/OpenAPI provenance.
