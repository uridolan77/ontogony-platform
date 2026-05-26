# IMPLEMENTATION_NOTES — ONTOGONY-RECONSTRUCTABILITY-CLOSURE-OPTION1

**Date:** 2026-05-26  
**Agent:** Cursor unpack + baseline validation per `00_UNPACK_PROMPT.md`

## Intake placement

| Item | Result |
| --- | --- |
| Zip unpacked | `ONTOGONY-RECONSTRUCTABILITY-CLOSURE-OPTION1.zip` → removed after unpack |
| Active location | `docs/_incoming/_active/ONTOGONY-RECONSTRUCTABILITY-CLOSURE-OPTION1/` |
| Note | `00_UNPACK_PROMPT.md` references `docs/_incoming/packages/…`; platform hygiene allows only `_active/`, `_consumed/`, `README.md` under `_incoming` — package placed under `_active` per `docs/_incoming/README.md` |
| Manifest | Row added to `docs/_incoming/_active/MANIFEST.md` (status: **in_progress**) |
| Hygiene script | `scripts/validate-docs-incoming-hygiene.ps1` — **PASS** (3 active packages) |

## Current-state assumptions (verified 2026-05-26)

### kanon-dotnet — **confirmed**

| Assumption | Status |
| --- | --- |
| Reconstructability classifier core | Present (`DecisionReconstructabilityClassifier`, F/P/S/O + PASS/WARN/FAIL) |
| `POST /ontology/v0/reconstructability/classify` | Present (`ReconstructabilityEndpoints.cs`) |
| `POST /ontology/v0/reconstructability/classify-batch` | Present |
| `GET /ontology/v0/decision-events/{decisionEventId}/reconstructability` | Present |
| `GET /ontology/v0/reconstructability/by-trace/{traceId}` | Present |

### allagma-dotnet — **confirmed**

| Assumption | Status |
| --- | --- |
| `GET /allagma/v0/runs/{runId}/decision-events` | Present (`DecisionEventsEndpoints.cs`) |
| `AllagmaDecisionEventProjector` | Present |
| `RunDecisionEventsTests` | 22 tests **PASS** (baseline) |
| Fragment-ref / post-condition coverage | Covered in `RunDecisionEventsTests` + `DecisionReconstructabilityGoldenCases` |
| PR-001 gap | No `AllagmaKanonReconstructabilityIntegrationTests` yet; golden fixture emitter exists but classifier batch closure not proven end-to-end in Allagma tests |

### conexus-dotnet — **partially confirmed**

| Assumption | Status |
| --- | --- |
| Model-call evidence / admin APIs | Present (journal, telemetry, admin surfaces) |
| Route / model-call decision semantics | Owned by Conexus (per package scope) |
| PR-002+ | Not started (emitters deferred until PR-001) |

### ontogony-platform — **confirmed**

| Assumption | Status |
| --- | --- |
| Shared mechanics only | AGENTS.md boundary unchanged |
| Tracing, errors, idempotency, HTTP, observability, compatibility, testing | Present in solution |

## Baseline validation commands

Recorded before any package implementation code changes.

### kanon-dotnet

```powershell
dotnet build Kanon.sln -c Release          # PASS (0 warnings, 0 errors)
dotnet test Kanon.sln -c Release --filter "FullyQualifiedName~Reconstructability"  # PASS — 75 passed
```

### allagma-dotnet

```powershell
dotnet build Allagma.sln -c Release --no-incremental   # PASS
dotnet test ... --filter "RunDecisionEvents"            # PASS — 22 passed
dotnet test ... --filter "AllagmaV0RouteInventoryTests|FeatureConnectionMatrixAuditTests"  # PASS — 6 passed
```

### conexus-dotnet

```powershell
dotnet build Conexus.sln -c Release -p:NoWarn=CS1591    # PASS
dotnet test Conexus.sln -c Release --filter "Category!=ExternalProviderSmoke&..."  # PARTIAL FAIL (pre-existing)
```

**Failures (not introduced by this unpack):**

1. `OpenApiSnapshotTests.Gateway_v1_openapi_snapshot_matches_committed_file` — live OpenAPI has `/v1/governance/budget`; snapshot still expects `quota`.
2. `OpenApiSnapshotTests.Admin_v0_openapi_snapshot_matches_committed_file` — `budget-policy` vs committed `cache-policy` / quota naming.
3. `ConexusRouteInventoryTests.Live_route_surface_matches_catalog_signatures` — catalog missing new `budget*` routes (e.g. `GET /admin/v0/budgets/events`).

**Remediation (out of PR-001 scope unless blocking):** refresh OpenAPI snapshots and `ConexusRouteInventoryTests` catalog for budget rename.

### ontogony-platform

```powershell
dotnet build Ontogony.Platform.sln -c Release   # PASS
dotnet test Ontogony.Platform.sln -c Release    # PARTIAL FAIL (pre-existing)
```

**Failure:**

- `ShippingAssemblyPublicApiTests.Public_api_matches_snapshot` for `Ontogony.Idempotency` — `InMemoryIdempotencyLedger.TryRemoveKeyAsync` added to API but Verify snapshot not updated.

**Remediation:** accept/update `ShippingAssemblyPublicApiTests` verified snapshot for `TryRemoveKeyAsync`.

## Next step (per package)

Implement **`pr-specs/PR-001-ALLAGMA-KANON-CLASSIFIER-CLOSURE.md`** in `allagma-dotnet` only. Do **not** start Conexus emitters (PR-002) until PR-001 acceptance criteria are met.
