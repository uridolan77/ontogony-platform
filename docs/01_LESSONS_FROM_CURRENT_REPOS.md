# 01 — Lessons From Current Repos

## Agentor lessons

Agentor is the best donor for service runtime mechanics.

Take:

- request tracing middleware
- AsyncLocal correlation context
- ActivitySource/Meter diagnostics
- fake/http/disabled integration modes
- named HTTP clients with correlation headers
- durable queue/outbox/leases/distributed operation ledger pattern
- structured tool payload/fingerprint discipline
- auth modes: Fake/Header/Jwt
- release smoke and repo-truth discipline

Do not take:

- agent plan execution semantics
- policy rule meaning
- human review semantics
- tool orchestration domain

## Athanor lessons

Athanor is the best donor for provenance discipline.

Take:

- append-only mindset
- source/evidence/provenance vocabulary as inspiration
- content hashing
- PostgreSQL as canonical write authority
- graph projection as read model only
- startup guards
- review/canonical-risk invariants as a product principle

Do not take:

- canonization engine into shared packages
- contradiction/snapshot logic
- project authority semantics as common infrastructure

## Conexus lessons

Conexus is the best donor for operational gateway posture.

Take as requirements:

- project API keys
- request ID per gateway call
- request logs with provider/model/latency/tokens/cost/errors
- readiness checks
- secret encryption at rest
- production hardening: no default secrets, no wildcard CORS
- OpenAI-compatible gateway surface as a product boundary

Do not take:

- Python implementation into .NET packages
- provider routing internals
- BO page logic

## KGB lessons

KGB is historical.

Take:

- chunk hashing
- pipeline stage structure
- human review UI ideas
- export formats

Do not use it as the new infrastructure foundation.
