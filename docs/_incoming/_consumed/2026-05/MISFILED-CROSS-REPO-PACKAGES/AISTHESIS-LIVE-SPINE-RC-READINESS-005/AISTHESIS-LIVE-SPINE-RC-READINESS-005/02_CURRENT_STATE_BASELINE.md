# Current state baseline

## Aisthesis state before this package

Aisthesis is currently a strong alpha / pre-RC evidence spine.

Known green areas:

- health/readiness/capabilities;
- evidence envelope and edge ingestion;
- batch ingestion with idempotency;
- producer-native identifier fields;
- lookup, timeline, graph;
- reconstructability v1/v2;
- trace bundle export;
- stable bundle fingerprint;
- required-edge evaluator with 10 rules;
- complete and missing-edge fixtures;
- in-memory and Postgres persistence modes;
- producer-token auth;
- typed client;
- metrics meters;
- route inventory and OpenAPI/client coverage gates;
- Aisthesis fixture/harness validation green after 003A;
- cross-repo producer alignment 004 landed;
- LES-001 live proof complete with four producers observed.

## Known non-green / not final areas

- Aisthesis repo live mode is not itself the orchestrator.
- LES-002 Metabole-first path is partial at approximately 0.82 with zero blockers.
- Full five-service CI smoke is not yet established as a repeatable gate.
- Full clean Release solution gates across all repos were not uniformly recorded after stopping all APIs.
- Frontend live backing is handoff only.
- Production IAM is not complete.
- Retention/erasure APIs/automation are not complete.
- Full OpenTelemetry distributed trace export is not complete.
- `lockRequired` remains false pending RC review and gates.

## Baseline truth

This package should treat Aisthesis as stable enough to harden, not as a repo needing broad new abstractions.
