# Conexus support planning for Ontogony.Platform

This directory tracks platform work discovered while building Conexus.NET.

Conexus.NET must not grow reusable mechanics locally. When Conexus needs a generic capability that also belongs to Agentor, Athanor, or future Ontogony services, add it here first.

## Rule

`ontogony-platform` supplies mechanics. `conexus-dotnet` supplies gateway meaning.

## Current platform role for Conexus

Required mechanics include:

- hosting/service defaults
- request tracing and structured logging
- redaction
- secret references, masking, fingerprints, and possibly generic secret resolution
- quota ledger mechanics
- errors/problem mapping
- HTTP resilience
- security/current actor context
- idempotency ledger
- canonical hashing/fingerprints
- protocol-neutral contracts
- AI telemetry DTOs
- artifact references/storage
- execution journal facts
- optional persistence/outbox and test fixtures

## This directory contains

- `INFRASTRUCTURE_BACKLOG.md` — candidate platform PRs.
- `BOUNDARY_DECISION_TREE.md` — where a change belongs.
- `CONEXUS_SUPPORT_ROADMAP.md` — recommended order.
- `pr-specs/` — platform PR specs required or likely useful for Conexus.
- `templates/INFRA_PR_TEMPLATE.md` — template for platform PRs driven by Conexus needs.
