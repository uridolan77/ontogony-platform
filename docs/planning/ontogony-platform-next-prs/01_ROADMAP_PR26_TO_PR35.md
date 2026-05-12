# Roadmap — PR26 to PR35

## Phase A — Make the platform easy and safe to host

### PR26 — Ontogony.Hosting service defaults

Add a new package for common ASP.NET service wiring:

- `AddOntogonyServiceDefaults`
- `UseOntogonyServiceDefaults`
- middleware ordering for tracing/errors/security preload
- health/readiness endpoints
- standard JSON options
- production config validation summary

No domain semantics.

## Phase B — Make event delivery durable

### PR27 — Ontogony.Persistence.Postgres.Outbox

Add a separate provider package implementing the existing outbox contracts over Postgres.

- SQL schema/migration script
- `FOR UPDATE SKIP LOCKED` or equivalent claim semantics
- retry/dead-letter handling
- idempotent consumer store
- clock injection
- integration tests via Testcontainers if acceptable, or documented local Docker path

## Phase C — Make protocol recording real

### PR28 — Ontogony.ProtocolIngress

Add a mechanical normalization package for external protocol events:

- AG-UI event adapter
- MCP event adapter
- A2A event adapter
- CloudEvents normalization helpers
- OpenTelemetry span/log/event projection helpers
- validation to `OntogonyEnvelope<T>`

This package must not decide what a tool call, agent run, or canonical claim means.

## Phase D — Make service identity production-operational

### PR29 — Security production hardening

Add secret rotation and key-id mechanics:

- key id header
- multi-secret resolver contract
- current/previous secret validation
- distributed nonce store guidance/provider sample
- HMAC signing client handler
- conformance vectors for signature validation

## Phase E — Make observability operational

### PR30 — Observability operations pack

Add concrete docs and examples for:

- OpenTelemetry exporters
- dashboards
- alert rules
- trace/header burn-in checks
- correlation smoke tests
- service-level metrics naming

## Phase F — Make contracts governable

### PR31 — Schema governance and compatibility

Add compatibility discipline:

- schema version policy
- breaking-change detector script
- JSON schema golden fixtures
- envelope/CloudEvents round-trip vectors
- package API surface snapshot

## Phase G — Make HTTP resilience production-grade

### PR32 — Advanced HTTP resilience policies

Improve `Ontogony.Http`:

- per-client named policies
- retry budget
- timeout policy
- hedging explicitly rejected or very constrained
- richer metrics for attempts, retries, circuit open, retry-after source
- response-body redaction tests

## Phase H — Make consumers prove compatibility

### PR33 — Ontogony.Testing conformance kits

Add test packages/fixtures for services adopting platform packages:

- tracing conformance
- error shape conformance
- envelope conformance
- HMAC conformance
- outbox conformance
- HTTP resilience conformance

## Phase I — Make releases safe

### PR34 — Release automation and quality gates

Add:

- GitHub Actions release workflow
- package publish dry-run
- package manifest generation
- SourceLink/package metadata
- SBOM if desired
- package compatibility matrix

## Phase J — Make the platform learnable

### PR35 — Documentation-first developer experience

Create a coherent docs site structure:

- start-here path
- package path
- hosting path
- adoption path
- ops path
- security path
- architecture decision records
- examples index

## Rule of execution

Each PR must be shippable alone. Do not batch PR26–PR35 into one mega-PR.
