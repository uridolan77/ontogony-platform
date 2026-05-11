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
- Verified solution hygiene commands pass:
	- dotnet restore Ontogony.Platform.sln
	- dotnet build Ontogony.Platform.sln --no-restore
	- dotnet test Ontogony.Platform.sln --no-build

PR2 hardening updates:

- Added idempotency-safe retry controls in Ontogony.Http transport resilience options.
- Retry defaults now prioritize safe methods and require Idempotency-Key for unsafe methods.
- Added bounded content buffering and no-retry behavior for multipart and streaming request content.
- Final transient exceptions now contribute to circuit failure tracking.
- Added targeted resilience tests for method safety, idempotency key behavior, oversized payload, multipart content, and exception-driven circuit opening.
- Verified commands pass after hardening:
	- dotnet restore Ontogony.Platform.sln
	- dotnet build Ontogony.Platform.sln --no-restore
	- dotnet test Ontogony.Platform.sln --no-build

PR3 error contract updates:

- Expanded exception mappings with safe public messages, optional exception-message inclusion, log severity control, and optional details inclusion.
- Hardened Ontogony exception middleware to avoid exposing unmapped exception messages and to skip writing when responses have already started.
- Added instance path support to ApiError and introduced ApiError <-> ProblemDetails bridge helpers.
- Added focused middleware and ProblemDetails bridge tests in Ontogony.Infrastructure.Tests.

PR4 event contract updates:

- Stabilized OntogonyEnvelope<TPayload> with deterministic JSON serialization and metadata round-trip support.
- Added reference types for protocol-neutral payloads: ArtifactRef, SubjectRef, TraceRef (extends ActorRef, EntityRef).
- Added OntogonyEventMetadata record for structured context beyond required envelope fields.
- Implemented CloudEvents 1.0 bridge: OntogonyEnvelope<T>.ToCloudEvent() and CloudEvent.ToOntogonyEnvelope<T>().
- Expanded OntogonyEventTypes with agent.run.started, agent.run.completed, agent.step.completed, cost.recorded, provider.failed event type constants.
- Added comprehensive PR4 tests for envelope determinism, metadata preservation, CloudEvents conversion, and extension round-trips.
- Verified commands pass:
	- dotnet restore Ontogony.Platform.sln
	- dotnet build Ontogony.Platform.sln --no-restore
	- dotnet test Ontogony.Platform.sln --no-build

PR5 hashing and idempotency v1:

- Confirmed canonical JSON serialization with recursive key sorting, stable unicode handling, null preservation.
- Added EnvelopePayloadHasher for deterministic envelope payload hashing and operation fingerprinting.
- Enhanced IdempotencyKeyBuilder with format {namespace}:{operation}:{version}:{hash} (e.g., ontogony:agentor.run.start:v1:abc123...).
- Introduced IdempotencyKeyOptions with configurable namespace, version, max length, and safe character validation.
- Added BuildKey() and BuildKeyFromJson() methods for flexible key construction with safe-character enforcement.
- Key length constraints with automatic truncation (max 256 chars by default).
- Service-specific namespace support (athanor, agentor, conexus prefixes with version awareness).
- Added 18 hashing tests (canonical JSON determinism, envelope payload hashing, operation fingerprinting, hash stability).
- Added 34 idempotency tests (key format, safe characters, truncation, namespace isolation, backwards compatibility).
- All 121 tests pass (69 existing + 52 new).

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
