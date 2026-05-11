# Changelog

## 0.2.0

Comprehensive shared-infrastructure extraction pass.

Included updates:

- Added Ontogony.Primitives package for clock and ID generation abstractions.
- Moved correlation propagation handler into Ontogony.Http.
- Added transport resilience registry and expanded options in Ontogony.Http.
- Added integration HTTP error redaction and status-preserving exception helper in Ontogony.Http.
- Added generic authentication startup safety guards in Ontogony.Security.
- Added integration and behavior coverage in Ontogony.Infrastructure.Tests.
- Added migration note: docs/migrations/2026-05-11-primitives-and-http-resilience-extraction.md.

## 0.1.0-starter

Initial starter package.

Included packages:

- Ontogony.Contracts
- Ontogony.Observability
- Ontogony.Configuration
- Ontogony.Errors
- Ontogony.Http
- Ontogony.Hashing
- Ontogony.Idempotency
- Ontogony.Messaging
- Ontogony.Security
- Ontogony.Persistence
- Ontogony.Testing

Primary intent: provide a stable extraction base for shared infrastructure currently duplicated or implied across Athanor, Agentor, and Conexus.
