# Header Compatibility Matrix

This matrix documents the use, scope, and compatibility guarantees for Ontogony headers across service boundaries.

## HTTP Headers

| Header | Purpose | Ontology | Athanor | Agentor | Conexus | Future Services | Breaking |
|--------|---------|----------|---------|---------|---------|-----------------|----------|
| `X-Ontogony-Trace-Id` | Correlation ID (primary) | ✓ Primary | ✓ Pass-through | ✓ Pass-through | ✓ Pass-through | ✓ Pass-through | Stable |
| `X-Athanor-Trace-Id` | Legacy trace ID (deprecated) | — | ✓ Legacy | ✓ Legacy emit | — | ⚠ Phase-out | Stable (legacy) |
| `X-Agentor-Trace-Id` | Legacy trace ID (deprecated) | — | ✓ Legacy emit | ✓ Legacy | — | ⚠ Phase-out | Stable (legacy) |
| `X-Conexus-Request-Id` | Conexus-specific request ID | — | — | — | ✓ Primary | — | Stable |
| `X-Ontogony-Tenant-Id` | Tenant isolation | ✓ Optional | ✓ Optional | ✓ Optional | ✓ Optional | ✓ Optional | Stable |
| `X-Ontogony-Workspace-Id` | Workspace isolation | ✓ Optional | ✓ Optional | ✓ Optional | ✓ Optional | ✓ Optional | Stable |
| `X-Ontogony-Project-Id` | Project isolation | ✓ Optional | ✓ Optional | ✓ Optional | ✓ Optional | ✓ Optional | Stable |
| `X-Ontogony-Actor-Id` | Current user/service | ✓ Optional | ✓ Optional | ✓ Optional | ✓ Optional | ✓ Optional | Stable |
| `X-Ontogony-Session-Id` | Session correlation | ✓ Optional | ✓ Optional | ✓ Optional | ✓ Optional | ✓ Optional | Stable |
| `Idempotency-Key` | Idempotent request deduplication | ✓ Optional | ✓ Optional | ✓ Optional | ✓ Optional | ✓ Optional | Stable |
| `traceparent` (W3C) | W3C trace context (optional) | ✓ Optional | ✓ Optional | ✓ Optional | ✓ Optional | ✓ Optional | Stable |
| `tracestate` (W3C) | W3C trace state (optional) | ✓ Optional | ✓ Optional | ✓ Optional | ✓ Optional | ✓ Optional | Stable |

## Envelope Extension Fields (CloudEvents)

When converting to/from CloudEvents, these fields are preserved as extensions:

| Extension | Type | Scope | Stability |
|-----------|------|-------|-----------|
| `traceId` | string | Cross-service | Stable |
| `spanId` | string | Cross-service | Stable |
| `parentSpanId` | string | Cross-service | Stable |
| `tenantId` | string | Tenant isolation | Stable |
| `workspaceId` | string | Workspace isolation | Stable |
| `projectId` | string | Project isolation | Stable |
| `actorId` | string | Actor tracking | Stable |
| `sessionId` | string | Session correlation | Stable |
| `protocol` | string | Protocol preservation | Stable |
| `payloadHash` | string (hex) | Integrity | Stable |
| `schemaVersion` | string | Schema versioning | Stable |
| `metadata` | object | Custom metadata | Stable |

## W3C Trace Context (Optional)

Headers `traceparent` and `tracestate` are optional and follow W3C Trace Context specification.

- **`traceparent`**: Format `version-traceId-parentId-traceFlags` (e.g., `00-abc123-def456-01`)
- **`tracestate`**: Vendor-specific vendor-string values separated by commas

Not all Ontogony services require W3C headers; they are accepted but not mandatory for correlation.

## Legacy Header Deprecation Plan

**Phase 1 (Current)**: `X-Athanor-Trace-Id` and `X-Agentor-Trace-Id` are accepted and pass through.

**Phase 2 (TBD)**: Services emit only `X-Ontogony-Trace-Id`; legacy headers still accepted.

**Phase 3 (TBD)**: Legacy headers deprecated; will be removed.

A migration note will be published 2 quarters before removal.

## Compatibility Guarantees

- **Stable**: Header is required to remain unchanged. Any modification requires PR review and migration documentation.
- **Stable (legacy)**: Header is stable but scheduled for deprecation. Accept but do not emit in new code.
- **Phase-out**: Header is being phased out. Plan for removal.

## Adding New Headers

To add a new header:

1. Use the `X-Ontogony-*` prefix for shared headers.
2. Use service-specific prefix (e.g., `X-Conexus-*`) only for service-local headers.
3. Add to this matrix and update tests in `HeaderConstantsSnapshotTests.cs`.
4. Update `OntogonyEventHeaders` constant.
5. Update documentation and CHANGELOG.
6. Plan for adoption across consuming services.

## Custom Headers in Ontogony Metadata

Service-specific or application-specific metadata should be placed in the `Metadata` dictionary of `OntogonyEnvelope`, not as separate HTTP headers. This keeps the header namespace clean and services loosely coupled.
