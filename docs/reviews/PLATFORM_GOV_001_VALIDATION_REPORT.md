# PLATFORM-GOV-001 validation report

**Date:** 2026-05-17  
**Repo:** ontogony-platform  
**Scope:** Phase 1 release-governance tightening (docs + Conexus package-smoke `nuget.config` mapping fix)

## Summary

Documentation and governance artifacts now explicitly describe how **Conexus.NET**, **Kanon.NET**, and **Allagma.NET** consume Ontogony `0.3.0-alpha.1`, how to run the platform validation suite, and how to handle consumer-breaking public API changes. No product semantics or new packages were added.

## Validation results

| Command | Result |
| --- | --- |
| `dotnet restore Ontogony.Platform.sln` | **Passed** |
| `dotnet build Ontogony.Platform.sln -c Release --no-restore` | **Passed** |
| `dotnet test Ontogony.Platform.sln -c Release --no-build` | **Passed** (589 tests; includes 23 public API snapshot tests) |
| `./scripts/validate-docs-links.ps1` | **Passed** |
| `./scripts/validate-docs-api-names.ps1` | **Passed** |
| `./scripts/validate-ai-runtime-boundaries.ps1` | **Passed** |
| `./scripts/validate-shipping-inventory.ps1` | **Passed** (23 packages) |
| `./scripts/validate-ai-runtime-docs.ps1` | **Passed** |
| `./scripts/validate-package-levels.ps1` | **Passed** (after PLATFORM-GOV-001A — `Ontogony.Security` → `Ontogony.Http` in golden map) |
| `./scripts/validate-dependency-baseline.ps1` | **Passed** |
| `./scripts/validate-conexus-consumer-baseline-alignment.ps1` | **Passed** |
| `./scripts/validate-allagma-consumer-baseline-alignment.ps1` | **Passed** |
| `./scripts/validate-public-api-governance.ps1` | **Passed** |
| `PACKAGE_VERSION=0.3.0-alpha.1` `./scripts/pack-all.ps1 -NoBuild` | **Passed** (23 `.nupkg`) |
| `./scripts/validate-nupkg-coordination-path-hygiene.ps1` | **Passed** |
| ConexusDotNetPackageSmoke restore + Release build | **Passed** (after `packageSourceMapping` on local feed) |

## Deliverables

| Artifact | Path |
| --- | --- |
| Governance index | `docs/governance/README.md` |
| Phase 1 consumer compatibility | `docs/governance/PHASE1_CONSUMER_COMPATIBILITY.md` |
| Package checklist `0.3.0-alpha.1` | `docs/governance/PACKAGE_COMPATIBILITY_CHECKLIST_0.3.0-alpha.1.md` |
| NuGet source mapping | `docs/governance/NUGET_SOURCE_MAPPING.md` |
| Error/correlation examples | `docs/examples/error-correlation-mechanics.md` |
| Smoke feed mapping fix | `examples/ConexusDotNetPackageSmoke/nuget.config` |

## Remaining gaps / deferrals

| Item | Notes |
| --- | --- |
| Kanon platform validation script | Kanon proves package-mode in `kanon-dotnet` CI; no `validate-kanon-consumer-baseline-alignment.ps1` in platform (documented in Phase 1 compatibility doc) |
| PR template checklist | NP-009 optional PR-template item not added; checklist lives in governance + `PUBLIC_API_COMPATIBILITY.md` |
| External CI proof | This report is local; green GitHub Actions run not re-recorded for this docs PR |

## Reviewer checklist

- [x] No product semantics in new docs
- [x] Public API snapshots unchanged (tests green)
- [x] Agentor only as historical/legacy header context
- [x] Downstream consumption documented for all three Phase 1 consumers

## Ready for review

**Yes** — docs and smoke `nuget.config` fix are complete.

---

## PLATFORM-GOV-001A follow-up (2026-05-17)

Aligned golden map with intentional `Ontogony.Security` → `Ontogony.Http` (`CurrentActorOutboundPropagator`, PLAT-INT-001).

| Command | Result |
| --- | --- |
| `./scripts/validate-package-levels.ps1` | **Passed** |

**Files:** `scripts/validate-package-levels.ps1`, `docs/architecture/package-levels.md`, `CHANGELOG.md`.

Phase 1 package checklist item **B / validate-package-levels** is now satisfied locally.

---

## PLATFORM-GOV-002 closure (2026-05-17)

Finalized package-level governance after the PLATFORM-GOV-001A golden map fix (`Ontogony.Security` → `Ontogony.Http`). Docs-only; no product semantics and no package version bump.

| Deliverable | Path |
| --- | --- |
| Package-level governance index | `docs/governance/PACKAGE_LEVEL_GOVERNANCE.md` |
| Governance README link | `docs/governance/README.md` |
| Checklist cross-link | `docs/governance/PACKAGE_COMPATIBILITY_CHECKLIST_0.3.0-alpha.1.md` |
| Script header → governance doc | `scripts/validate-package-levels.ps1` |
| Architecture cross-link | `docs/architecture/package-levels.md` |

| Command | Result |
| --- | --- |
| `./scripts/validate-package-levels.ps1` | **Passed** |
| `./scripts/validate-docs-links.ps1` | **Passed** |
| `./scripts/validate-shipping-inventory.ps1` | **Passed** (23 packages) |

### Reviewer checklist (GOV-002)

- [x] Golden map and human matrix stay aligned (`Ontogony.Security` → `Ontogony.Http` documented)
- [x] Change workflow documented for future edge updates
- [x] No product semantics in new governance text
- [x] CI still runs `validate-package-levels.ps1` (unchanged workflow)

**Ready for review:** Yes — package-level governance is closed for Phase 1 alpha line.
