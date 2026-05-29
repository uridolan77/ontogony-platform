# Current state baseline — ONTOGONY-BACKEND-COORDINATION-002

**Date:** 2026-05-29  
**Source:** `BACKEND-REPO-CLEANUP-ORGANIZATION-001` audits and validation runs.

Verify these artifacts exist before implementing. If present, **update** — do not recreate.

---

## Cross-repo anchors (already exist)

| Artifact | Location | Notes |
| --- | --- | --- |
| Cleanup summary | `allagma-dotnet/docs/system/BACKEND_REPO_CLEANUP_ORGANIZATION_001_SUMMARY.md` | Readiness scores, open gaps |
| Boundary matrix | `allagma-dotnet/docs/system/BACKEND_REPO_BOUNDARY_MATRIX.md` | Ownership table |
| Docs index | `allagma-dotnet/docs/system/BACKEND_REPO_DOCS_INDEX.md` | Links to per-repo docs |
| Runtime lock | `allagma-dotnet/docs/system/ontogony-runtime.lock.json` | Pinned commits + package versions |
| Compatibility matrix | `allagma-dotnet/docs/system/SYSTEM_COMPATIBILITY_MATRIX.md` | Repos, ports, prefixes |
| Trace context matrix | `allagma-dotnet/docs/system/SYSTEM_TRACE_CONTEXT_MATRIX.md` | Header propagation |
| Error compatibility matrix | `allagma-dotnet/docs/system/SYSTEM_ERROR_COMPATIBILITY_MATRIX.md` | Partial — needs slice 3 |
| System E2E scenarios | `allagma-dotnet/docs/testing/SYSTEM_E2E_SCENARIOS.md` | Scenario catalog |

---

## Platform mechanics (partial adoption)

| Mechanic | Platform location | Adoption status |
| --- | --- | --- |
| `CrossServiceErrorEnvelope` | `Ontogony.Errors` + conformance tests | Matrix exists; **not uniform** across services |
| Propagation headers | `Ontogony.Http`, `Ontogony.Contracts` | Documented; **E2E gaps** |
| Idempotency | `Ontogony.Idempotency` | Per-service adoption varies |
| System compat gate | `Ontogony.SystemCompatibility` | CI gate when siblings present |
| Actor context | `Ontogony.Security` | Not propagated E2E |

---

## Per-repo baseline

### ontogony-platform (score 8.2)

- 27 packages, strong CI; `_incoming` hygiene restored.
- **Open:** Public API snapshot drift (`Ontogony.Observability`).
- **Deferrals:** `docs/DEFERRALS.md`

### conexus-dotnet (score 8.3)

- Clean intake; provider adapters isolated.
- **Open:** Release build failures on some dev machines; alias manifest for consumers.
- Model routing docs: `docs/MODEL_ROUTING.md`

### kanon-dotnet (score 8.3)

- 115 `/ontology/v0` routes; 1 active intake package (source-binding HTTP wiring).
- **Open:** OpenAPI baseline drift (9 tests); error contract normalization.
- Boundary: `docs/SEMANTIC_AUTHORITY_BOUNDARY.md`

### allagma-dotnet (score 8.2)

- System matrices mature; real tools blocked.
- **Open:** ~59 broken doc links; 14 event-vocabulary test failures; hard-coded `gpt-4o-mini` in some tests.
- Model purpose → alias: `AllagmaModelPurposeRoute.ConexusModelAlias` (config-driven; verify no provider IDs in production paths).

### metabole-dotnet (score 7.5)

- 393 tests passing; SLOD spine implemented.
- **Open:** Legacy `_incoming_active/` archive; live certify-promote in progress.
- Boundary: `docs/architecture/METABOLE_SYSTEM_BOUNDARY.md`

### aisthesis-dotnet (score 6.5)

- RC-certification **partial**; live five-service PASS **NOT_RUN**.
- **Open:** SDK pin `9.0.312`; no `docs/system/` matrices.
- Active: `AISTHESIS-LIVE-FIVE-SERVICE-PASS-009` (may merge into slice 7).

---

## Five-service port matrix (canonical)

| Service | Port |
| --- | ---: |
| Kanon | 5081 |
| Conexus | 5082 |
| Allagma | 5083 |
| Metabole | 5084 |
| Aisthesis | 5085 |

Source: `allagma-dotnet/docs/system/SYSTEM_COMPATIBILITY_MATRIX.md`, `metabole-dotnet/docs/system/ONTOGONY_FIVE_SERVICE_PORT_MATRIX.md`

---

## Known pre-existing failures (do not hide)

| Repo | Failure | Category |
| --- | --- | --- |
| Platform | `PublicApi.Tests` snapshot | Pre-existing |
| Conexus | Release build (52 errors on some hosts) | Investigate before slice 2 |
| Kanon | OpenAPI baseline (9 tests) | In-flight routes |
| Allagma | `FirstWorkingSystemApiTests` vocabulary (14) | Event stream drift |
| Aisthesis | SDK 9.0.312 not installed | Environment |

---

## Starting assumption for this sprint

The boundary triangle is **coherent**. This sprint **tightens contracts and proof**, not ownership. Any slice that would blur boundaries must be rejected or deferred with reason.
