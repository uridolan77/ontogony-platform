# System compatibility gate (PLATFORM-9-001)

Platform-owned mechanical gate that proves the Ontogony alpha system has not drifted across backend repos, frontend snapshots, and shared package versions.

## Package

| Path | Role |
| --- | --- |
| `src/Ontogony.SystemCompatibility/` | Validator library |
| `tests/Ontogony.SystemCompatibility.Tests/` | Fixture tests + optional DevRoot integration |
| `scripts/run-system-compatibility-gate.ps1` | Operator entrypoint |

## Inputs (read-only)

| Source | Repo | Path |
| --- | --- | --- |
| Protocol registry | `ontogony-platform` | `docs/system/system-protocol-registry.json` |
| Runtime lock | `allagma-dotnet` | `docs/system/ontogony-runtime.lock.json` |
| Kanon manifest | `kanon-dotnet` | `docs/generated/KANON_COMPATIBILITY_MANIFEST.json` |
| Conexus manifest + OpenAPI | `conexus-dotnet` | `docs/generated/CONEXUS_COMPATIBILITY_MANIFEST.json`, `openapi/*.snapshot.json` |
| Allagma feature matrix | `allagma-dotnet` | `docs/system/allagma-feature-connection.matrix.json` |
| Environment matrix | `allagma-dotnet` | `docs/system/SYSTEM_ENVIRONMENT_MATRIX.md` |
| Frontend route/client matrix | `ontogony-frontend` | `docs/system/ALLAGMA_FRONTEND_COVERAGE_MATRIX.md`, `docs/generated/ROUTE_WORKFLOW_INVENTORY.md`, `openapi/*.json`, `npm run route-client-drift:check` (SYSTEM-9A-005) |
| Platform package version | `ontogony-platform` | `Directory.Build.props` `<Version>` |

## Outputs

```text
artifacts/system-compat/system-compatibility-summary.json
artifacts/system-compat/system-compatibility-summary.md
```

Schema: `ontogony-system-compatibility-summary-v1`.

## Frozen propagation headers (PLATFORM-9-003)

The gate validates the frozen header set via [`HEADER_PROPAGATION_CONTRACT.md`](./HEADER_PROPAGATION_CONTRACT.md) and [`propagation-header.matrix.json`](../system/propagation-header.matrix.json).

| Header | Notes |
| --- | --- |
| `traceparent` | W3C trace context |
| `X-Correlation-ID` | Legacy correlation alias (also emits `X-Ontogony-Correlation-Id`) |
| `X-Ontogony-Actor-Id` | Actor identity |
| `X-Ontogony-Actor-Type` | Actor classifier |
| `X-Ontogony-Actor-Roles` | Comma-separated roles |
| `X-Ontogony-Idempotency-Key` | Canonical idempotency |
| `X-Allagma-Run-Id` | Allagma run spine (via `AdditionalHeaders`) |

Legacy inbound: `Idempotency-Key`, `X-Ontogony-Roles`. See [`header-compatibility-matrix.md`](./header-compatibility-matrix.md) and [`TRACE_CORRELATION_CONTRACT.md`](../operators/TRACE_CORRELATION_CONTRACT.md).

Reusable proofs: `Ontogony.Testing.HeaderPropagationConformanceAssertions`.

## Run locally

```powershell
cd c:\dev\ontogony-platform
pwsh ./scripts/run-system-compatibility-gate.ps1 -DevRoot C:\dev
```

Requires sibling checkouts: `allagma-dotnet`, `kanon-dotnet`, `conexus-dotnet`, `ontogony-frontend`, `ontogony-ui`.

## CI

Full platform CI materializes registry siblings, then runs this gate with `-DevRoot` pointing at the materialized workspace. Failures block merge when any check status is `Fail` (skipped checks are allowed for optional repos in fixture-only runs).

## Header propagation conformance (PLATFORM-9-003) — done

See [`HEADER_PROPAGATION_CONTRACT.md`](./HEADER_PROPAGATION_CONTRACT.md). Checks: matrix, frozen constants, operator docs, sibling integration docs, `Ontogony.Testing` helpers.

**Consumer tests (done):** ALLAGMA-PROP-001, KANON-PROP-001, CONEXUS-PROP-001 — [`PHASE_TIGHT_CLOSEOUT_2026-05-22.md`](../planning/PHASE_TIGHT_CLOSEOUT_2026-05-22.md).

Standalone validation: [`scripts/validate-header-propagation-contract.ps1`](../scripts/validate-header-propagation-contract.ps1).

## Error envelope conformance (PLATFORM-9-002)

Included in the same gate run. See [`CROSS_SERVICE_ERROR_ENVELOPE_GATE.md`](./CROSS_SERVICE_ERROR_ENVELOPE_GATE.md).

Checks: conformance matrix, wire samples, `OperatorFailureTaxonomyAdapter` vs taxonomy matrix, Allagma/frontend OpenAPI `CrossServiceErrorEnvelope`, sibling integration docs, frontend taxonomy module.

## Non-goals

- No live stack smoke (see Allagma `scripts/system/run-system-cohesion-acceptance.ps1` or `run-system-cohesion-smoke.ps1`).
- No product semantics — mechanical alignment only.
- Not a replacement for per-repo contract tests (Kanon route inventory, Conexus DTO snapshots, Allagma OpenAPI snapshots).
