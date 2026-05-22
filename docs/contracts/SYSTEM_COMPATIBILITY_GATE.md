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
| Frontend route/client matrix | `ontogony-frontend` | `docs/system/ALLAGMA_FRONTEND_COVERAGE_MATRIX.md`, `docs/generated/ROUTE_WORKFLOW_INVENTORY.md`, `openapi/*.json` |
| Platform package version | `ontogony-platform` | `Directory.Build.props` `<Version>` |

## Outputs

```text
artifacts/system-compat/system-compatibility-summary.json
artifacts/system-compat/system-compatibility-summary.md
```

Schema: `ontogony-system-compatibility-summary-v1`.

## Frozen propagation headers

The gate documents and validates presence of these headers in operator contracts (implementation propagation is PLATFORM-9-003):

| Header | Notes |
| --- | --- |
| `traceparent` | W3C trace context |
| `X-Correlation-ID` | Legacy correlation alias (inbound interop) |
| `X-Ontogony-Correlation-Id` | Canonical correlation |
| `X-Ontogony-Actor-Id` | Actor identity |
| `X-Ontogony-Actor-Type` | Actor classifier |
| `X-Ontogony-Actor-Roles` | Comma-separated roles |
| `Idempotency-Key` | Legacy idempotency alias |
| `X-Ontogony-Idempotency-Key` | Canonical idempotency |
| `X-Allagma-Run-Id` | Allagma run spine (Conexus ingress) |

See also [`header-compatibility-matrix.md`](./header-compatibility-matrix.md) and [`TRACE_CORRELATION_CONTRACT.md`](../operators/TRACE_CORRELATION_CONTRACT.md).

## Run locally

```powershell
cd c:\dev\ontogony-platform
pwsh ./scripts/run-system-compatibility-gate.ps1 -DevRoot C:\dev
```

Requires sibling checkouts: `allagma-dotnet`, `kanon-dotnet`, `conexus-dotnet`, `ontogony-frontend`, `ontogony-ui`.

## CI

Full platform CI materializes registry siblings, then runs this gate with `-DevRoot` pointing at the materialized workspace. Failures block merge when any check status is `Fail` (skipped checks are allowed for optional repos in fixture-only runs).

## Error envelope conformance (PLATFORM-9-002)

Included in the same gate run. See [`CROSS_SERVICE_ERROR_ENVELOPE_GATE.md`](./CROSS_SERVICE_ERROR_ENVELOPE_GATE.md).

Checks: conformance matrix, wire samples, `OperatorFailureTaxonomyAdapter` vs taxonomy matrix, Allagma/frontend OpenAPI `CrossServiceErrorEnvelope`, sibling integration docs, frontend taxonomy module.

## Non-goals

- No live stack smoke (see Allagma `scripts/run-system-cohesion-smoke.ps1`).
- No product semantics — mechanical alignment only.
- Not a replacement for per-repo contract tests (Kanon route inventory, Conexus DTO snapshots, Allagma OpenAPI snapshots).
