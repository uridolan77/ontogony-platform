# PLATFORM_CLEANUP_TIGHTENING_REPORT

## What was changed

- Tightened top-level positioning and boundaries in `README.md` and `AGENTS.md`:
  - active alpha consumers are now described as Conexus.NET and Allagma.NET
  - product-boundary wording now points at Kanon semantics, Allagma execution, and Conexus gateway ownership
  - Agentor references are kept only as explicit historical donor context
- Tightened docs index and onboarding consistency:
  - updated `docs/start-here.md` and `docs/00_START_HERE.md` to current active-consumer reality
  - removed/neutralized stale Agentor-centric adoption wording in core index guides
  - refreshed `docs/adoption/index.md`, `docs/examples/index.md`, and `docs/operations/index.md` with neutral service IDs and current trace/correlation wording
- Clarified trace ID vs correlation ID semantics:
  - `docs/contracts/header-compatibility-matrix.md` now distinguishes canonical trace (`X-Ontogony-Trace-Id`) from canonical correlation (`X-Ontogony-Correlation-Id`)
  - documented legacy `X-Correlation-ID` as inbound compatibility alias
  - aligned `docs/03_TRACE_CORRELATION_STANDARD.md` and `docs/adoption/service-to-service-integration.md` to this split
- Clarified error-contract boundaries:
  - `src/Ontogony.Errors/README.md` and `docs/packages/Ontogony.Errors.md` now explicitly separate:
    - endpoint API payload mechanics (`ApiError`, middleware JSON shape, mapped JSON results)
    - `CrossServiceErrorEnvelope` as internal cross-service envelope mechanics
    - product-owned public HTTP contract variants
- Tightened public API governance docs:
  - `docs/public-api-review.md` now references current consumers (Allagma/Kanon/Conexus) and generic migration-note conventions under `docs/migrations/`
- Recorded this cleanup in `CHANGELOG.md` under `Unreleased` as docs-only tightening (no runtime behavior change)
- Follow-up failure remediation:
  - added repo-level `nuget.config` with package source mapping to remove local `NU1507` restore/build failures caused by external user-level feeds
  - refreshed public API verified snapshots for `Ontogony.Contracts`, `Ontogony.Errors`, `Ontogony.Http`, and `Ontogony.Observability`

## What was intentionally not changed

- No runtime code behavior was changed in:
  - `OntogonyErrorJsonPayloadBuilder`
  - `OntogonyMappedJsonResults.ApiError`
  - exception middleware implementation
  - `CrossServiceErrorEnvelope` type shape
  - trace/correlation implementation in `OntogonyIntegrationHeaders`, `IntegrationHeaderPropagation`, or `OntogonyCorrelationContext`
- Historical sprint/planning/migration archives were not broadly rewritten; cleanup focused on active root/index/package/governance/adoption docs.
- No new abstractions, packages, or public API surface additions were introduced.

## Remaining risks

- Legacy-header deprecation timeline remains partly planned (documented as phased); runtime behavior remains unchanged.

## Validation results

### Requested commands

- `dotnet restore Ontogony.Platform.sln`  
  - **Passed**
- `dotnet build Ontogony.Platform.sln -c Release --no-restore`  
  - **Passed**
- `dotnet test Ontogony.Platform.sln -c Release --no-build`  
  - **Passed**
- `./scripts/validate-docs-links.ps1`  
  - **Passed**
- `./scripts/validate-public-api-governance.ps1`  
  - **Passed** (`No public API snapshot changes detected`)

### Additional checks

- ReadLints on edited files: **no linter errors**
