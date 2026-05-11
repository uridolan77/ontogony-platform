# Migration Note: Primitives and HTTP Resilience Extraction

Date: 2026-05-11

## Summary

This migration extracts generic infrastructure mechanics from donor implementations into Ontogony packages:

- Clock and ID primitives moved into Ontogony.Primitives.
- Correlation HTTP propagation and resilience mechanics consolidated in Ontogony.Http.
- Integration HTTP error redaction and status-preserving exception shaping added.
- Generic authentication startup safety guards added to Ontogony.Security.

## Breaking/Behavioral Changes

1. New package dependency: Ontogony.Primitives.
- Consumers that previously depended only on Ontogony.Persistence for IClock/IIdGenerator should migrate to Ontogony.Primitives interfaces.
- Ontogony.Persistence still provides compatibility shims marked Obsolete.

2. HTTP resilience behavior expanded.
- Resilience can be enabled/disabled via TransportResilienceOptions.Enabled.
- Retry status codes are configurable via TransportResilienceOptions.RetryableStatusCodes.
- In-memory circuit-breaker behavior now applies per named client through TransportResilienceRegistry.
- Retry now supports buffered content payloads for non-streaming requests.

3. Trace compatibility behavior retained.
- Standard trace header remains X-Ontogony-Trace-Id.
- Legacy acceptance/echo compatibility preserved for:
  - X-Athanor-Trace-Id
  - X-Agentor-Trace-Id
  - X-Conexus-Request-Id

4. Startup auth safety guards added.
- Disabled/unvalidated JWT modes are blocked by default outside Development/Test unless explicit override options are set.

## Required Repo Changes

- Athanor: Prefer Ontogony.Primitives for clock/id abstractions; keep legacy usage temporarily via shims.
- Agentor: Replace local transport resilience helper patterns with Ontogony.Http registry/options where applicable.
- Conexus: No .NET runtime auth/HTTP donor code import required; use shared Ontogony APIs where integrating with .NET services.

## Validation

Extraction behaviors are covered with tests under tests/Ontogony.Infrastructure.Tests.
