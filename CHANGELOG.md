# Changelog

## Unreleased

- Added planning package for PR26-PR35 infrastructure roadmap under `docs/planning/ontogony-platform-next-prs/`.

PR25 — Platform release hardening (this repo only):

- **Packaging:** `scripts/pack-all.ps1` requires `PACKAGE_VERSION` (no implicit `0.1.0-starter`). CI packs with an explicit version.
- **Observability:** `OntogonyCorrelationContext.FromHeaders` accepts canonical trace header name plus accepted alias list; `RequestTracingMiddleware` wires `OntogonyObservabilityOptions` so `AcceptedIncomingTraceHeaders` is honored.
- **Time:** `IClock` injected for `ServiceIdentityCurrentActorAccessor` (timestamp skew), `InMemoryOutboxStore` (dead-letter time), and optional `CloudEventConversionOptions.Clock` for missing CloudEvent `time` (`Ontogony.Contracts` references `Ontogony.Primitives`).
- **Security:** `ServiceIdentityOptions.RequirePreloadedBodyHashForHmacBodies` gates sync body rehash fallback for HMAC requests.
- **Messaging:** `IEventPublisherWithResult`; `InMemoryEventPublisher` implements it.
- **HTTP:** `RespectRetryAfterHeader`, `BackoffJitterFraction`, `HttpRequestMessage.Options` copied on retry clones; `ResilientIntegrationDelegatingHandler` uses `IClock` for `Retry-After` HTTP-date parsing.
- **Docs:** `docs/migrations/2026-05-12-pr25-platform-release-hardening.md`; `docs/Sprint3.md` current status vs historical archive.

## 0.2.0

Comprehensive shared-infrastructure extraction pass.

PR22 — Packaging and consumer migration docs:

- **NuGet metadata:** `Directory.Build.props` adds shared `PackageProjectUrl` and `PackageTags` for shipping packages.
- **`scripts/pack-all.ps1`:** resolves repo root, optional `-NoBuild` / `-IncludeSymbols`, fails when no `.nupkg` is produced, prints package list.
- **Adoption:** `docs/adoption/consumer-package-migration.md` as the single index linking feed setup, versioning, CI samples, and hashing note; cross-links from `private-nuget-feed.md`, `package-versioning.md`, and `local-repo-layout-and-ci.md`.
- **CI samples:** `.github/workflows/samples/multi-checkout.yml` and `consume-internal-feed.yml` (`workflow_dispatch` only; copy into consumer repos).

PR21 — Athanor hashing (documentation in this repo):

- Migration note `docs/migrations/2026-05-11-pr21-athanor-hashing-stage1.md` (UTF-8 SHA-256 delegation to `Ontogony.Hashing`; canonical JSON parity limits for escaped strings). Implementation and golden vectors live in the Athanor repository.

PR20 — Ontogony.Errors JSON wire shape and mapping hooks:

- **`OntogonyExceptionMappingOptions`:** `ErrorCodeJsonKey`, `DetailsJsonKey`, `IncludeInstanceInJson`, `UnhandledErrorCode` for legacy-compatible JSON without changing `ApiError` in-memory shape.
- **`ExceptionMapping` / `Map<T>`:** optional `resolveErrorCode`, `resolvePublicMessage`, `detailsFactory`, and **`resolveStatusCode`** (per-instance HTTP status) for exceptions such as operational rejections with non-default status codes.
- **`OntogonyExceptionHandlingMiddleware`:** builds JSON from a dictionary so wire keys follow options; unmapped failures use `UnhandledErrorCode`; `ResolveErrorCode` applies only to mapped exceptions.
- Tests extended in `Ontogony.Infrastructure.Tests` for custom unhandled codes and `error` / `errors` wire keys.
- Migration note: `docs/migrations/2026-05-11-pr20-ontogony-errors-json-and-mapping.md`.

**Observability default:** `OntogonyObservabilityOptions.EchoLegacyHeaders` now defaults to **false** (canonical `X-Ontogony-Trace-Id` only on responses). Services that still need legacy response aliases set `EchoLegacyHeaders = true` during rollout. Migration note: `docs/migrations/2026-05-11-echo-legacy-trace-headers-default.md`.

PR16–PR17 — Security HMAC hardening and Contracts polish:

- **Security:** `ServiceIdentityOptions` adds `MaxSignedBodyBytes` and `AllowUnsignedEmptyBody`; `IRequestBodyHashProvider` now returns `RequestBodyHashResult` from `TryComputeSha256HexLower` (bounded reads; no unbounded synchronous copy). Default `Sha256RequestBodyHashProvider` is DI-friendly; `Sha256RequestBodyHashProvider.Instance` removed. `ServiceIdentityBodyHashPreloadMiddleware` + `UseOntogonyServiceIdentityBodyHashPreload()` preload body hashes asynchronously with the same cap. `InMemoryNonceReplayStore` gains `InMemoryNonceReplayStoreOptions` (retention + max entries), optional clock func, and documents distributed-store requirement for production clusters.
- **Contracts:** `DefaultEnvelopeValidator` requires absolute URI `Source` with consistent error text. `CloudEventConversionOptions.AllowNullCloudEventData` controls null `data` when converting to `OntogonyEnvelope`. JSON schema `Source` description tightened.
- **Adoption docs:** `docs/adoption/local-repo-layout-and-ci.md`, `docs/adoption/private-nuget-feed.md`; cross-links from platform adoption guides (`athanor-platform-adoption.md`, `agentor-observability-adoption.md`) for CI and internal feed strategy.
- Migration note: `docs/migrations/2026-05-11-pr16-pr17-security-contracts.md`.

PR15 — Envelope validation and CloudEvents hardening:

- Added `IEnvelopeValidator`, `EnvelopeValidationResult`, `EnvelopeValidationError`, `DefaultEnvelopeValidator`, and `EnvelopeValidatorOptions` (mechanical rules: safe `EventId`/`TraceId`, `protocol.entity.verb` event types, absolute URI `Source`, optional `PayloadHash` as 64 lowercase hex, optional protocol allowlist).
- CloudEvents: `ToCloudEvent` now emits `schemaVersion` extension; `ToOntogonyEnvelope` validates `specversion` is 1.0 and supports `CloudEventConversionOptions` / `CloudEventTraceIdPolicy` for missing `traceId` (generate vs reject).
- Added `schemas/ontogony-envelope.schema.json` and `docs/contracts/schema-versioning.md`.
- Updated test fixtures (`TestEnvelopeFactory`, `EnvelopeFixtureBuilder`) and tests to use valid sample event types, URI sources, and protocol names.
- Migration note: `docs/migrations/2026-05-11-pr15-envelope-validation.md`.
- Completed per-package semantic stubs under `docs/packages/` for Primitives, Hashing, Idempotency, Observability, Errors, Testing, and Configuration (alongside existing Contracts/Http/Messaging/Persistence/Security).

PR14 — Outbox in-memory reference and dead-letter contracts:

- Added `InMemoryOutboxStore` implementing `IOutboxWriter`, `IOutboxReader`, `IOutboxDispatcher`, and `IProcessedMessageStore` with documented ordering (`AvailableAt` then `OccurredAt`), idempotent `MarkDispatchedAsync`, `MarkFailedAsync` incrementing `AttemptCount`, and optional dead-letter emission via `InMemoryOutboxStoreOptions` + `IDeadLetterWriter`.
- Added `DeadLetterMessage`, `IDeadLetterWriter`, and `InMemoryDeadLetterWriter` for tests.
- Added `AddOntogonyInMemoryOutboxStore` DI registration.
- Expanded `docs/persistence/outbox-contract.md` with in-memory semantics and dead-letter threshold documentation.
- Migration note: `docs/migrations/2026-05-11-pr14-in-memory-outbox.md`.

PR11–PR13 (Sprint 2) platform semantics and hardening:

- Added package-level semantic docs under `docs/packages/` plus `docs/invariants.md`, `docs/messaging/delivery-semantics.md`, and `docs/security/service-identity.md`.
- Documented `InMemoryEventSink` as published-event capture (not a delivery ledger); clarified header actor trusted-upstream semantics.
- Implemented production **HMAC-SHA256** service identity verification (`RequireHmacSignature`, `OntogonyServiceIdentityHeaders`, `IServiceSecretResolver`, `INonceReplayStore`, `IRequestBodyHashProvider`, `ServiceIdentityHmacSignatureHelper`, `InMemoryNonceReplayStore`); kept **StaticSharedSecret** mode behind `RequireSignatureVerification` without HMAC.
- Default service identity id header is now `X-Ontogony-Service-Id` (see migration note).
- Messaging: added `EventPublisherOperationMode`, `EventPublishResult` / handler dispatch records, `PublishWithResultAsync`, and separated publish vs dispatch OpenTelemetry metrics (`ontogony.event.publish.count`, `ontogony.event.dispatch.count`, `ontogony.event.dispatch.failure.count`, `ontogony.event.handler.duration.ms`); removed `ontogony.event.published.count` / `ontogony.event.handled.count`.
- `Ontogony.Messaging` now references `Ontogony.Observability` for metrics emission (opt out via `EventDispatchOptions.RecordObservabilityMetrics`).
- Migration note: `docs/migrations/2026-05-11-pr11-13-semantics-service-identity-messaging-metrics.md`.

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

PR6 security primitives v1:

- Added OntogonyRoleNames with generic role constants: HumanOperator, Service, Agent, Admin (no business-specific roles).
- Implemented AllRoles collection and AreAllValid() validation for role checking.
- Added OntogonySecurityHeaders for HTTP header constants (Authorization, X-Ontogony-Actor-Id, X-Ontogony-Roles, etc.).
- Added ExtractBearerToken() helper for JWT Bearer token parsing.
- Implemented ClaimsCurrentActorAccessor for JWT claims-based actor extraction with configurable claim type priorities.
- Added ClaimsCurrentActorAccessorOptions with claim type configuration, actor type detection, role filtering, and strict role validation mode.
- Implemented ServiceIdentityCurrentActorAccessor for service-to-service authentication with optional signature verification.
- Added ServiceIdentityOptions for service secret management and signature validation configuration.
- Enhanced OntogonyAuthenticationOptions with comprehensive XML documentation and validation method.
- Updated OntogonyAuthenticationMode enum with security warnings about Header and Disabled modes.
- All three modes documented: Disabled (dev only), Header (trusted-upstream only), JWT (token-based).
- OntogonyAuthenticationOptions.Validate() enforces safety constraints:
  - Disabled mode blocked in Production unless explicitly allowed
  - Header mode requires configured header name
  - JWT mode requires authority or explicit unvalidated-token allowance
  - Unvalidated JWT blocked in Production unless explicitly allowed
- Added 34 comprehensive PR6 tests:
  - Role name constants and validation (5 tests)
  - Security header constants and utilities (8 tests)
  - JWT claims extraction with multiple claim types, roles, tenant IDs (10 tests)
  - Service-to-service authentication with optional signature verification (6 tests)
  - Authentication options validation with all three modes (5 tests)
- All 155 tests pass (121 existing + 34 new).

PR6.1 foundation correctness hardening:

- Added OntogonyActorTypes (human, service, agent) to keep actor type semantics separate from role semantics.
- Fixed ServiceIdentityCurrentActorAccessor to map tenant/workspace/project from dedicated headers instead of misusing TenantId for actor delegation.
- Switched ServiceIdentityCurrentActorAccessor signature comparison to constant-time equality.
- Updated HeaderCurrentActorAccessor to use OntogonySecurityHeaders constants for roles and actor-type headers.
- Added DI helpers for additional security accessor modes:
	- AddOntogonyClaimsActorContext(...)
	- AddOntogonyServiceIdentityActorContext(...)
- Added TransportResilienceOptions.CountOnlyRetryableResponsesAsCircuitFailures (default true) and updated HTTP handler to avoid counting non-retryable final responses (for example 400/404) as circuit failures.
- Updated TransportResilienceRegistry to use IClock from Ontogony.Primitives and wired clock injection through HTTP DI registration.
- Hardened request buffering fallback in ResilientIntegrationDelegatingHandler so unknown-length/failed buffering disables retry rather than throwing from buffer-load path.
- Corrected RequestTracingMiddleware request/error metrics to emit after pipeline execution using final response status (including non-exception 5xx responses).
- Hardened CloudEvents extension extraction to handle JsonElement values after JSON round-trip and restored protocol from CloudEvents extension when present.
- Hardened IdempotencyKeyBuilder operation handling:
	- operation now safe-character validated when enabled
	- payload hash is preserved intact when key-length constraints are applied
	- operation component is compacted instead of truncating the payload hash
- Added focused regression tests for all above behaviors.
- All 168 tests pass.

PR7 messaging v1:

- Added EventDispatchOptions to control dispatch behavior:
	- ContinueOnHandlerException
	- ComputePayloadHash
	- ValidateRequiredEnvelopeFields
- Added IEventSerializer and JsonEventSerializer for envelope serialization/deserialization.
- Added InMemoryEventSink for capture/read/clear workflows in local runs and tests.
- Added InMemoryEventPublisher with:
	- envelope publishing
	- in-memory sink capture
	- registered handler dispatch
	- optional payload hash computation
	- required field validation guard
	- configurable handler exception behavior
- Updated InMemoryEventBus to remain backward-compatible as a wrapper over InMemoryEventPublisher.
- Added Ontogony.Testing helpers:
	- PublishedEventRecorder
	- FakeEventPublisher
	- EnvelopeAssertions
	- legacy InMemoryEventRecorder now forwards to PublishedEventRecorder
- Added PR7 coverage for publish capture, handler dispatch/fan-out, exception policy, metadata preservation, payload-hash option, validation behavior, serializer round-trip, and testing helpers.

PR8 persistence foundations v1:

- Added SQL-agnostic persistence contracts in Ontogony.Persistence:
	- IOutboxWriter
	- IOutboxReader
	- IOutboxDispatcher
	- IProcessedMessageStore
	- IUnitOfWorkBoundary
- Added neutral records:
	- OutboxMessage
	- ProcessedMessage
	- OutboxDispatchResult
- Added mechanical helper surface in OutboxContracts:
	- message validation
	- deterministic processed-message key construction
	- retry next-available scheduling
	- deterministic metadata JSON serialization
- Added documentation:
	- docs/persistence/outbox-contract.md
	- docs/persistence/idempotent-consumer.md
- Added PR8 tests for outbox validation, processed key construction, retry schedule calculation, and metadata serialization.
- All 187 tests pass.

PR9 testing package v1:

- Expanded Ontogony.Testing fixture surface with:
	- FakeIdGenerator
	- FakeCurrentActorAccessor
	- StubHttpMessageHandler
	- RecordingHttpMessageHandler
	- TestCorrelationScope
	- EnvelopeFixtureBuilder<TPayload>
	- ApiErrorAssertions
	- CanonicalJsonAssertions
	- MiddlewareTestHarness (default HttpContext creation, pipeline run, JSON/header assertions)
- Added framework and package references needed by fixture helpers in Ontogony.Testing.
- Added fixture self-tests in Ontogony.Infrastructure.Tests covering deterministic time/ids, actor access, HTTP scripting/recording, correlation scope push/pop, envelope building, API error assertions, canonical JSON assertions, and middleware harness behavior.

PR10 documentation and examples v1:

- Added adoption guides:
	- docs/adoption/agentor-observability-adoption.md
	- docs/adoption/athanor-observability-adoption.md
	- docs/adoption/conexus-event-emission-adoption.md
	- docs/adoption/http-client-adoption.md
	- docs/adoption/package-versioning.md
- Existing guide retained:
	- docs/adoption/error-middleware-adoption.md
- Added example directories with explicit documentation-only status:
	- examples/IntegrationHttpClientExample/
	- examples/EventEnvelopeExample/
	- examples/ErrorMiddlewareExample/
- Added do-not-do guidance in new adoption docs/examples to reinforce package boundary and safety rules.
- All 197 tests pass.

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
