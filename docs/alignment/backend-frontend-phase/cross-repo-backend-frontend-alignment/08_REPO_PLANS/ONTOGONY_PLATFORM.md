# ontogony-platform Alignment Plan

## Objective

Make the platform package the single source of truth for cross-service headers, metadata keys, error envelopes, OpenTelemetry naming, and OpenAPI service metadata.

## PRs

### PLATFORM-BFA-001 — Contract constants and metadata envelope
Add shared constants for trace/correlation/run/decision/model-call/request/human-gate/replay IDs.

### PLATFORM-BFA-002 — Error envelope finalization
Stabilize the shared JSON error payload and minimal API helper tests.

### PLATFORM-BFA-003 — OpenAPI service metadata helper
Provide helper to add service name, version, contract version, source commit, and build time to health/OpenAPI metadata.

### PLATFORM-BFA-004 — Cross-service contract test package
Add reusable test helpers for metadata propagation.

## Acceptance

- No service-specific duplicate constants for cross-service ID names.
- Shared tests prove header propagation.
