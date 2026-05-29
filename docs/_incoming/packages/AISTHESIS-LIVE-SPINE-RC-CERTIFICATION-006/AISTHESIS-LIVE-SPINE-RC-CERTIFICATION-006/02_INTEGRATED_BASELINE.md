# Integrated baseline

## Current Aisthesis baseline

Aisthesis is already a strong alpha / pre-RC evidence spine.

Known green areas:

- health/readiness/capabilities;
- evidence envelope and edge ingestion;
- batch ingestion with whole-batch idempotency;
- producer-native identifier fields;
- lookup by trace/correlation/run/decision/semanticPlan/modelCall/routeDecision/dataProfile/mappingCandidate/pipelineRun/subject/producer/type/time range;
- timeline, graph, reconstructability legacy/v1/v2;
- trace bundle export and stable bundle fingerprint;
- in-memory and Postgres evidence stores;
- Postgres idempotency store;
- producer-token auth with body-level producer enforcement;
- Krisis evaluation runs;
- typed Aisthesis.Client for most evidence routes;
- metrics meters;
- route inventory, OpenAPI snapshot, client coverage truth gates;
- required-edge matrix v1 with 10 minimum edges;
- fixture-based five-service reconstructability smoke;
- producer-native emitters present in Allagma/Kanon/Conexus/Metabole.

## Known partial/deferral areas from RC-readiness-005

- live mode can honestly return `NOT_RUN` when live orchestration is absent;
- LES-002 Metabole-first path may remain partial around ~0.82 with zero blockers;
- frontend live evidence-spine backing is handoff-only unless separately proven;
- production IAM is deferred;
- retention/erasure automation is deferred;
- distributed OTel trace export is deferred;
- lockRequired remains false/candidate pending evidence.

## Additional gaps from the architectural review

- Need true five-service live certification, not only fixture harness.
- Need required-edge matrix v2 covering human gates, side effects, provider fallback/error/cost, Metabole review/transform lifecycle, replay bundles, canonical facts/source bindings, and frontend-facing diagnostics.
- Need durable evaluation job semantics instead of fire-and-forget evaluation after envelope ingestion.
- Need Aisthesis.Client coverage for evaluation routes or explicit server-only classification.
- Need stronger direct edge-write authorization or relation-scoped edge ownership.
- Need a shared error-envelope direction consistent with the rest of Ontogony.
- Need traceparent/OTel bridge.
- Need retention/erasure behavior that preserves auditability and reconstructability semantics.

## Baseline decision

Treat current Aisthesis as stable enough for hardening. Do not rebuild core concepts unless a gate fails.
