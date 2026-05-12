# Changelog

## Unreleased

PR41 / PR42 — outbox boundary cleanup and package README consistency:

- **Breaking:** Removed `Ontogony.Messaging` duplicate outbox surface (`IOutboxStore`, `OutboxMessage`, `NoOpOutboxStore`). Use `Ontogony.Persistence` outbox contracts only. Migration: `docs/migrations/2026-05-12-pr41-remove-messaging-outbox-duplicates.md`.
- **Docs:** Updated `docs/persistence/outbox-contract.md`, `docs/testing/conformance-kits.md`, `docs/02_PACKAGE_BOUNDARIES.md`, `docs/start-here.md`, and `docs/packages/index.md` for the Messaging vs Persistence split.
- **Packages:** Added packed `README.md` (with “What this is” / “What this is not”) to every shipping library under `src/` that was missing one; aligned `Ontogony.Hosting` with packed README; clarified XML remarks on `OutboxMessage` (opaque strings) and on in-memory messaging/idempotency types (tests / single-process).

PR39 / PR40 — repository truth and package-level enforcement:

- **README:** Positions the repo as mechanical infrastructure for new Ontogony services (pre–v1, first target Conexus.NET); lists all 18 `src/` packages; removes contradictory HTTP resilience notes; adds layer summary, “do not add product semantics,” and links to `docs/architecture/package-levels.md`.
- **Docs:** `docs/architecture/package-levels.md` — levels 0–3, forbidden dependency rules, and a dependency matrix aligned with the golden map.
- **CI:** `scripts/validate-package-levels.ps1` validates Ontogony `ProjectReference` edges under `src/`; workflow step in `.github/workflows/ci.yml`.

PR38.1 — Ontogony.Execution journal port and docs:

- **`IExecutionJournal` / `InMemoryExecutionJournal`:** Append-only mechanical journal with `GetRunAsync` and list APIs (`ListStepsAsync`, `ListAttemptsAsync`, `ListTransitionsAsync`, `ListCheckpointsAsync`). Duplicate primary ids throw `InvalidOperationException`; lists return append order.
- **DI:** `AddOntogonyInMemoryExecutionJournal()` registers the in-memory journal.
- **Contracts:** `ExecutionCheckpointRecord.PayloadArtifactId` (optional opaque id, often same as `Ontogony.Artifacts.ArtifactRef.ArtifactId`). XML remarks on journal DTOs state that lifecycle consistency is not validated here.
- **Docs:** `README.md` (packed), `docs/packages/Ontogony.Execution.md`, and `docs/packages/index.md` (18 packages, dependency graph, selection matrix).
- **Tests:** `ExecutionJournalTests` cover append/read, list ordering, duplicate ids, empty lists, and DI registration.

PR38 — Ontogony.Execution (execution journal contracts):

- **New package `Ontogony.Execution`:** `ExecutionRunRecord`, `ExecutionStepRecord`, `ExecutionAttemptRecord`, `ExecutionStateTransitionRecord`, and `ExecutionCheckpointRecord` — serialization-friendly DTOs with opaque string kinds, statuses, and hashes; optional scope and metadata dictionaries. No orchestration engine, storage backend, or product-specific vocabulary.
- **Tests:** `Ontogony.Execution.Tests` — deterministic `Ontogony.Hashing.CanonicalJson` round-trips, metadata key order independence, boundary cases (negative attempt number, `long` sequence extremes), and a reflection guard on exported names.
- **Script:** `scripts/validate-ai-runtime-boundaries.ps1` scan roots include `Ontogony.Execution` sources and tests.
- **Solution:** `Ontogony.Platform.sln` references `Ontogony.Execution` and `Ontogony.Execution.Tests`.

PR37.1 — Ontogony.Artifacts hardening:

- **Stream-based writes:** Added `ArtifactStreamPutRequest` (with `ContentStream`, `ExpectedSizeBytes`, `ExpectedContentHash`) and an `IArtifactStore.PutAsync(ArtifactStreamPutRequest, …)` overload so future durable implementations are not forced to buffer entire payloads in memory. `InMemoryArtifactStore` drains the stream, verifies expected size/hash when supplied, and shares the deterministic SHA-256 fingerprint with the byte-buffer path.
- **Encoding-aware dedupe:** `InMemoryArtifactStore` dedupe identity now includes `ContentEncoding` alongside content hash, tenant/workspace/project, media type, and classification. Same raw bytes labelled `identity` vs `gzip` no longer collapse into one artifact. `StorageTier` and `Uri` remain hint metadata.
- **Defensive reads & writes:** `GetAsync` / `TryGetAsync` return a copy of the stored bytes so consumers cannot mutate the backing array. `PutAsync(ArtifactPutRequest)` snapshots the caller's buffer before storing, so subsequent caller mutation does not change stored content.
- **Docs:** `ArtifactRef` XML doc and `docs/packages/Ontogony.Artifacts.md` now spell out the identity-vs-hint split (`ContentEncoding` is identity; `StorageTier` and `Uri` are hints) and that `ContentHash` is computed over the stored raw bytes. `ArtifactNotFoundException` now documents that hosts should map it through their error middleware before exposing it.
- **Tests:** `Ontogony.Artifacts.Tests` covers encoding-aware dedupe (no collapse), hint-only differences (dedupe preserved), stream/buffer hash parity, stream put/get round trip, stream put dedupe within scope, `ExpectedSizeBytes` and `ExpectedContentHash` enforcement (mismatch + match), defensive copy on read, and input-buffer immutability after put.
- **Explicit non-scope:** Still no cloud provider bindings, no retention/eviction policy, no PII or sensitivity registry, no agent/canon/routing semantics.

PR37 — Ontogony.Artifacts (artifact reference contracts and in-memory store):

- **New package `Ontogony.Artifacts`:** `ArtifactRef` (serialization-friendly reference DTO with id, content hash, size, opaque media type / encoding / storage tier / classification, optional scope and locator URI), `ArtifactPutRequest`, `ArtifactPutResult`, `ArtifactContent`, `IArtifactStore`, `ArtifactNotFoundException`, and thread-safe `InMemoryArtifactStore` (lowercase hex SHA-256 content addressing via `Ontogony.Hashing.Sha256ContentHashService`, dedup by content hash + tenant/workspace/project/media-type/classification).
- **DI helper:** `AddOntogonyInMemoryArtifactStore()` registers `IArtifactStore`, `IClock`, `IIdGenerator`, and `IContentHashService` for tests and single-process hosts.
- **Tests:** `Ontogony.Artifacts.Tests` — deterministic `CanonicalJson` round-trips, `OntogonyEnvelope<ArtifactRef>` validation via `DefaultEnvelopeValidator`, in-memory store put/get/dedupe/scope/collision behavior, SHA-256 fingerprint vector, and reflection boundary checks (no cloud provider, routing, or product-meaning tokens on public surface).
- **Docs:** Added `docs/packages/Ontogony.Artifacts.md`; expanded `docs/packages/index.md` (package count 17, dependency graph, quick selection matrix).
- **Script:** Extended `scripts/validate-ai-runtime-boundaries.ps1` scan roots to include `Ontogony.Artifacts` sources and tests.
- **Solution:** `Ontogony.Platform.sln` now references `Ontogony.Artifacts` and `Ontogony.Artifacts.Tests`.
- **Explicit non-scope:** No cloud provider (S3/Azure/GCS) bindings, no retention/eviction/replication policy, no PII or sensitivity registry — `MediaType`, `ContentEncoding`, `StorageTier`, and `Classification` remain opaque caller-defined strings.

PR35.1 — Documentation accuracy and validation:

- **Stale API cleanup:** Aligned adoption, architecture, examples, operations, packages, and start-here guides with current extension points (`AddOntogonyIntegrationHttpClient`, `TransportResilienceOptions`, `UseOntogonyRequestTracing`, `ProtocolNames.GenericJson`, messaging `IEventPublisher`, etc.).
- **Security guide rewrite:** Replaced obsolete `OntogonySecurityOptions` / manual signing examples with the PR29 model (`ServiceIdentityOptions`, `IServiceSigningSecretResolver`, `OntogonyServiceIdentitySigningHandler`, `OntogonyServiceIdentityHeaders`, body-hash preload, key-id rotation).
- **ProtocolIngress / persistence / HTTP accuracy:** Removed nonexistent `ProtocolIngress.ToProto` / gRPC samples; clarified outbox-only persistence scope (not a general ORM); fixed operations snippets for Postgres outbox and resilience options.
- **Links:** Repaired broken references (e.g. conformance kits path, ADR filenames, ADR 0001 from start-here).
- **Validation scripts:** Added `scripts/validate-docs-links.ps1` and `scripts/validate-docs-api-names.ps1`; documented usage in `docs/start-here.md` and CI.

PR36 — Ontogony.AI.Contracts (AI mechanical substrate, contracts only):

- **New package `Ontogony.AI.Contracts`:** `LlmRequestEnvelope`, `LlmResponseEnvelope`, `LlmStreamChunk`, `LlmUsageRecord` (with `ResolveTotalTokensOrSum`), `LlmCostRecord`, `LlmProviderError`, `ToolCallRecord`, `ModelCapabilityDescriptor`; depends on `Ontogony.Contracts` only.
- **Tests:** `Ontogony.AI.Contracts.Tests` — deterministic `CanonicalJson` round-trips, usage/cost/mechanical assertions, `DefaultEnvelopeValidator` coverage for `OntogonyEnvelope<LlmRequestEnvelope>`, reflection boundary checks on exported contract surface (types, properties, methods, enums).
- **Docs:** `docs/ai-runtime/boundary-guardrails.md`, `docs/ai-runtime/implementation-order.md`, `docs/planning/ai-runtime-prs/pr-specs/`, `docs/planning/ai-runtime-prs/prompts/`, ADRs `ADR-0036`–`ADR-0040`, package page `docs/packages/Ontogony.AI.Contracts.md`.
- **Script:** `scripts/validate-ai-runtime-boundaries.ps1` (scoped scan of AI.Contracts sources).
- **Explicit non-scope:** Does not implement routing, agents, KB meaning, or provider SDKs; PR37+ packages remain out of this change.

PR36.1 — Polish (docs, packaging, boundary tests):

- **NuGet README path:** `Ontogony.AI.Contracts` uses `PackagePath="/"` for packed `README.md` (aligned with other packages).
- **Docs:** Package index installation wording matches artifact-first release; `Provider` / `Model` documented as opaque strings (package README, XML remarks, `docs/packages/Ontogony.AI.Contracts.md`, packages index).
- **Contracts:** Clarified `LlmProviderError.SafeMessage` depends on caller redaction; pointed to future redaction mechanics.
- **Tests:** Boundary scan extended to public properties, methods, fields, events, nested types, and enum member names.
- **Scripts:** `validate-ai-runtime-boundaries.ps1` documents extending scan roots when PR37+ packages land.

PR35 — Developer experience documentation (5 comprehensive guides):

- **`docs/start-here.md`** — Main entry point covering overview, quick links, and what Ontogony is
- **`docs/packages/index.md`** — Reference for all 16 packages, use cases, and dependency graph
- **`docs/adoption/index.md`** — Step-by-step integration guide (7 phases from setup to conformance testing)
- **`docs/operations/index.md`** — Deployment, monitoring, database management, security practices, troubleshooting
- **`docs/architecture/index.md`** — Design decisions, package boundaries, testing strategy, future improvements
- **`docs/security/index.md`** — Authentication, HMAC signing, secrets management, compliance, incident response
- **`docs/examples/index.md`** — Complete service example, code patterns, event publishing, error handling, testing

PR34.1 — Release automation SemVer support:

- **Manifest regex fix:** Updated `scripts/generate-package-manifest.ps1` to support SemVer prerelease versions (`0.3.0-alpha.1`, `0.3.0-local`, etc.) in addition to release versions
- **Release automation tests:** Added `NupkgFilenameParser_ExtractsPrereleaseVersions` test covering alpha, local, and rc variants
- **Clarification:** Documented that PR34 release workflow currently does artifact-only release (GitHub Releases, not real `dotnet nuget push`)

PR34 — Release automation and quality gates:

- **LICENSE file:** Added MIT license to repository root and NuGet package metadata.
- **Package metadata enhancement:** Updated `Directory.Build.props` to include `PackageLicenseExpression`, `RepositoryType`, `IncludeSymbols`, `SymbolPackageFormat`, `PublishRepositoryUrl`, and `EmbedUntrackedSources`.
- **Package README files:** Added curated README.md to key packages (`Ontogony.Contracts`, `Ontogony.Observability`, `Ontogony.Testing`) and configured `.csproj` to include them in NuGet packages via `PackageReadmeFile`.
- **Manifest generation script:** Added `scripts/generate-package-manifest.ps1` to produce `PACKAGE_MANIFEST.json` with version, commit hash, and per-package metadata (id, version, filename, size, SHA256 hash).
- **Changelog validation script:** Added `scripts/validate-changelog.ps1` to verify `CHANGELOG.md` has been updated (soft gate in CI, strict in release workflow).
- **Release workflow:** Added `.github/workflows/release-packages.yml` triggered on tag push or manual dispatch; performs restore/build/test, validates changelog, generates manifest, and uploads artifacts to GitHub Releases.
- **CI enhancement:** Enhanced `.github/workflows/ci.yml` to generate manifest on every push/PR for validation.
- **Release automation tests:** Added `ReleaseAutomationPr34Tests` covering manifest schema, version parsing, filename extraction, and changelog validation.
- **Quality gates:** Manifest generation fails if no packages produced, version mismatch detected, or invalid package names encountered.

PR33 — Testing conformance kits:



- **TracingConformanceAssertions:** helpers to assert `RequestTracingMiddleware` echoes `X-Ontogony-Trace-Id`, generates a trace id when none is present, and propagates tenant id into `OntogonyCorrelationContext`.
- **ErrorShapeConformanceAssertions:** helpers to assert unmapped exceptions produce 500 with canonical JSON shape, mapped exceptions produce the expected status and code, and no internal exception message leaks.
- **EnvelopeConformanceAssertions:** helpers to assert envelope source follows `ontogony://` scheme, event type is namespaced, schema version is present, and payload hash matches content.
- **HmacConformanceAssertions:** helpers to assert HMAC canonical string format, signature round-trips, and tampered signatures are rejected.
- **OutboxConformanceHarness:** helpers to assert write-then-read, mark-dispatched removes from queue, duplicate write throws, and mark-failed is idempotent against any `IOutboxWriter`/`IOutboxReader`/`IOutboxDispatcher`.
- **HttpResilienceConformanceHarness:** helpers to build resilient `HttpClient` instances backed by `StubHttpMessageHandler` and assert retry-on-failure, no-retry-on-success, and circuit-breaker open behaviour.
- **New project references:** `Ontogony.Testing` now references `Ontogony.Http` and `Ontogony.Persistence` to support outbox and resilience conformance helpers.
- **Self-tests:** `ConformanceKitPr33Tests` in `Ontogony.Infrastructure.Tests` covers positive and negative cases for all six kits.
- **Docs:** added `docs/testing/conformance-kits.md` with full API reference and service adoption snippets.

PR32 — Advanced HTTP resilience policies:

- **Retry classifier extension point:** added `IRetryClassifier` interface to allow custom retry decision logic beyond status codes and exception types.
- **Default retry classifier:** added `DefaultRetryClassifier` implementing standard retry logic for backward compatibility.
- **Retry decision enum:** added `RetryDecision` with options `DoNotRetry`, `Retry`, and `RetryBypassingBudget` for fine-grained control.
- **Retry budget:** added `RetryBudgetPerMinute` to `TransportResilienceOptions` to limit retries per client per minute, protecting downstream services from retry storms.
- **Total timeout:** added `TotalTimeout` to bound the entire request lifecycle (all attempts + backoff delays).
- **Per-attempt timeout:** added `AttemptTimeout` to cancel individual attempts that exceed threshold, treating as transient errors for retry.
- **Observability flag:** added `EmitAttemptMetrics` to `TransportResilienceOptions` to indicate metric emission intent.
- **Retry budget tracking:** `TransportResilienceRegistry` now tracks retry budget per client per minute via new `TryConsumeRetryBudget` method.
- **Enhanced handler:** `ResilientIntegrationDelegatingHandler` now accepts optional `IRetryClassifier`, enforces timeouts, and respects retry budgets.
- **Comprehensive tests:** `AdvancedHttpResilienceTests` covers retry budget exhaustion, total/per-attempt timeouts, custom classifiers, and budget bypass.

PR31 — Schema governance and compatibility:

- **Fixture-driven testing:** added `schemas/fixtures/valid/*.json` and `schemas/fixtures/invalid/*.json` golden vectors for envelope validation.
- **JSON schema validation tests:** `SchemaFixtureValidationTests` validates fixtures against `ontogony-envelope.schema.json` and ensures `DefaultEnvelopeValidator` behavior aligns.
- **CloudEvents round-trip tests:** `CloudEventsRoundTripTests` verifies all required and optional fields survive `ToCloudEvent()` + `ToOntogonyEnvelope<T>()` conversions.
- **Header snapshot tests:** `HeaderConstantsSnapshotTests` ensures `OntogonyEventHeaders` and `ProtocolNames` constants remain stable across releases.
- **Compatibility policy:** added `docs/contracts/compatibility-policy.md` with breaking-change definition, testing requirements, versioning strategy, and PR checklist.
- **Header compatibility matrix:** added `docs/contracts/header-compatibility-matrix.md` with cross-service header usage, deprecation plans, and adoption guidance.
- **Stability guarantees:** formalized how PRs modifying envelopes, headers, or validation logic must include fixture updates, test coverage, and migration documentation.

PR30 — Observability operations pack:

- **Operational docs:** added `docs/observability/operations-pack.md` with OTLP exporter wiring, local collector usage, log correlation patterns, and rollout checks.
- **Local stack:** added `docs/observability/local-collector/` with Docker Compose, collector config, and Prometheus scrape config for local traces/metrics.
- **Queries and alerts:** added `docs/observability/dashboard-queries.md` and `docs/observability/alerts.prometheus.rules.yml`.
- **Catalogs:** added `docs/observability/metrics-catalog.md` and `docs/observability/trace-attributes.md`.
- **Trace migration checks:** added `docs/observability/trace-header-burn-in-checks.md`.
- **Stability tests:** added smoke tests pinning activity source/meter names, metric instrument names, and span attribute keys.
- **Package docs:** updated `docs/packages/Ontogony.Observability.md` and `docs/packages/Ontogony.Http.md` with observability operations references and metrics interoperability guidance.

PR29 — Security production hardening:

- **Key-id support:** added `X-Ontogony-Service-Key-Id`, `ServiceIdentityOptions.ServiceKeyIdHeaderName`, and explicit key-id policy via `RequireKeyIdForHmacSignature`.
- **Multi-secret resolver contract:** added `IServiceSigningSecretResolver`, `ServiceSigningSecret`, `ServiceSigningSecretSet`, and default `OptionsServiceSigningSecretResolver` with legacy fallback support.
- **Client signing:** added `OntogonyServiceIdentitySigningHandler` for outbound canonical HMAC header stamping.
- **Middleware diagnostics:** `ServiceIdentityBodyHashPreloadMiddleware` now detects unsafe order (after endpoint selection) with configurable throw/log behavior.
- **Tests:** added coverage for current/previous key validation, unknown key-id rejection, explicit missing key-id behavior, signing handler canonical headers, body-hash/server verification parity, and middleware-order diagnostics.
- **Docs:** added production hardening guide, deterministic signing vectors, rotation sequence, and deployment checklist.

PR28.2 — ProtocolIngress adoption hardening:

- **Source normalization:** preserves already-absolute source URIs and prefixes only non-URI identifiers.
- **Hash semantics:** `RawProtocolPayload` now includes `RawPayloadHash` (exact raw bytes) and `CanonicalPayloadHash` (canonical JSON).
- **CloudEvents tracing:** `CloudEventsProtocolAdapter` now preserves `traceparent` and `tracestate` extensions in envelope metadata.
- **DI helper:** added `AddOntogonyProtocolIngress()` to register dependencies and all ingress adapters.
- **Tests/docs:** expanded adapter and DI registration tests, plus protocol-ingress docs for source and hash semantics.

PR28.1 (Phase 2) — ProtocolIngress P1 clarifications:

- **Event type policy:** adapters now emit mechanical envelope event types (`{protocol}.ingress.normalized`) while preserving protocol-native event types in `RawProtocolPayload.RawEventType`.
- **CloudEvents consolidation:** `CloudEventsProtocolAdapter` now uses the canonical `CloudEventEnvelope` from `Ontogony.Contracts.Events`; internal duplicate CloudEvent DTO removed.
- **Hash semantics:** canonical payload hashing is now explicit via `CanonicalPayloadHash`; envelope `PayloadHash` continues to carry canonical SHA-256 identity.
- **Durability guard:** `RawProtocolPayload.ParsedObject` is now marked non-durable (`JsonIgnore`) to prevent accidental persistence/serialization of runtime-only object graphs.
- **Tests:** adapter tests updated for mechanical event type expectations and raw event type preservation.

PR28.1 (Phase 3) — ProtocolIngress polish and edge-case coverage:

- **Dead-code cleanup:** removed an unused base-adapter import to keep the ingress core minimal.
- **Advanced edge-case tests:** added coverage for source URI normalization idempotency, context timestamp fallback precedence, whitespace-only trace handling under strict policy, CloudEvents invalid-time fallback behavior, CloudEvents `JsonElement` trace extension extraction, and non-durable `ParsedObject` serialization exclusion.
- **Verification:** solution build and full test suite pass with expanded coverage.

PR28 — Ontogony.ProtocolIngress mechanical protocol normalization:

- **New package:** `Ontogony.ProtocolIngress` adds mechanical protocol-to-envelope normalization without product semantics.
- **Core abstractions:** `IProtocolIngressAdapter<TRaw>`, `ProtocolIngressResult`, `ProtocolIngressContext`, `RawProtocolPayload`, `ProtocolIngressError`.
- **Adapters:** `GenericJsonProtocolAdapter`, `CloudEventsProtocolAdapter`, `McpProtocolAdapter`, `A2aProtocolAdapter`, `AgUiProtocolAdapter` for diverse event protocols.
- **Mechanics:** Payload preservation, trace ID policy (RequireProvided or GenerateIfMissing), timestamp normalization, deterministic payload hash, distributed tracing metadata.
- **Validation:** Required field checking, structured error reporting, integration with `OntogonyEnvelope<RawProtocolPayload>`.
- **Tests:** Golden payload fixtures, deterministic hash verification, missing trace ID handling, raw payload preservation.
- **Docs:** `docs/protocol-ingress/overview.md`, `docs/protocol-ingress/cloudevents.md`, `docs/protocol-ingress/mcp.md`, `docs/protocol-ingress/a2a.md`, `docs/protocol-ingress/ag-ui.md`.
- **Boundary:** Does not include Agentor run orchestration, Athanor canonization, Conexus approval logic, or product-specific interpretation.

- Added planning package for PR26-PR35 infrastructure roadmap under `docs/planning/ontogony-platform-next-prs/`.

PR27.5 — PostgreSQL provider hardening:

- **Lifecycle:** `AddOntogonyPostgresOutbox(...)` now registers one shared `NpgsqlDataSource` singleton consumed by `PostgresOutboxStore` and `PostgresDeadLetterWriter`.
- **Schema:** outbox index names are generated from configured outbox table name to avoid collisions when multiple table names are used in the same schema.
- **Ownership APIs:** `IPostgresOutboxClaimStore` adds `MarkDispatchedIfOwnedAsync(...)` and `MarkFailedIfOwnedAsync(...)` for claim-owner-only updates; existing `IOutboxDispatcher` methods remain compatibility methods.
- **Dead-letter semantics:** docs now explicitly state that atomic outbox+dead-letter write is guaranteed only with `PostgresDeadLetterWriter`; external dead-letter writers may observe partial completion.
- **Docs/migration:** `docs/migrations/2026-05-12-pr27-5-postgres-provider-hardening.md`, package/provider docs updated.
- **README:** updated stale HTTP resilience note to reflect Retry-After and jitter support.

PR27 — Ontogony.Persistence.Postgres outbox provider:

- **New package:** `Ontogony.Persistence.Postgres` with durable PostgreSQL implementation for `IOutboxWriter`, `IOutboxReader`, `IOutboxDispatcher`, `IProcessedMessageStore`, and `IDeadLetterWriter`.
- **Schema:** adds provider-owned tables `ontogony_outbox_messages`, `ontogony_processed_messages`, and `ontogony_dead_letter_messages`.
- **Concurrency:** `ReadAvailableAsync` performs atomic claim leasing with PostgreSQL locking (`FOR UPDATE SKIP LOCKED`) and preserves ordering (`AvailableAt`, then `OccurredAt`).
- **Reliability:** idempotent `MarkDispatchedAsync`, idempotent processed-message insert, retry scheduling updates via `MarkFailedAsync`, and optional dead-letter threshold handling.
- **Lease controls:** added provider-specific `IPostgresOutboxClaimStore` (`ClaimAvailableAsync`, `TryClaimAsync`, `RenewClaimAsync`, `ReleaseClaimAsync`) for explicit worker claim management.
- **Startup:** optional `PostgresOutboxOptions.EnsureSchemaOnStartup` to run schema initialization through hosted service registration.
- **CI:** `.github/workflows/ci.yml` now provisions PostgreSQL and sets `ONTOGONY_POSTGRES_TEST_CONNECTION` so Postgres integration tests run in CI.
- **Docs:** `docs/persistence/postgres-outbox-provider.md`, `docs/packages/Ontogony.Persistence.Postgres.md`.
- **Migration:** `docs/migrations/2026-05-12-pr27-postgres-outbox-provider.md`.

PR26 — Ontogony.Hosting service defaults:

- **New package:** `Ontogony.Hosting` adds `AddOntogonyServiceDefaults(...)`, `UseOntogonyServiceDefaults()`, and `MapOntogonyHealthEndpoints()` for mechanical host composition only.
- **Defaults:** Observability + errors registration by default; middleware ordering keeps request tracing before exception handling so trace IDs flow into error payloads.
- **Security:** Optional service-identity body-hash preload middleware (`UseServiceIdentityBodyHashPreload`) remains explicit opt-in.
- **Health:** Configurable health/readiness endpoint mapping (`/health`, `/ready` by default).
- **Docs:** Added package + hosting docs and minimal example.
- **Migration:** `docs/migrations/2026-05-12-pr26-ontogony-hosting-service-defaults.md`.

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
