# PLAT-QUALITY-001 — Public XML docs and coverage (staged policy)

This document is the **staged** policy for public API documentation completeness and test coverage reporting on Ontogony.Platform. It exists so we can improve quality **without** a single PR that turns on `CS1591` everywhere or a blocking coverage gate before baselines exist.

## Goals

1. **XML documentation (`CS1591`)** — Know exactly which shipped assemblies must have complete public XML comments today, which are deferred, and how we expand enforcement later.
2. **Coverage** — Always produce **human-readable** HTML from CI Cobertura output; keep numeric **thresholds advisory** until baselines stabilize, then introduce a blocking gate in a follow-up change.

## Shipped packages (27)

All `src/Ontogony.*` projects that `scripts/validate-shipping-inventory.ps1` counts are **shipping** NuGet packages (27 under `src/`). Each ships with `GenerateDocumentationFile=true` (see root `Directory.Build.props`).

## Tier A — `CS1591` enforced (complete XML required)

These projects match **`src/Directory.Build.targets`**: root `NoWarn` includes `CS1591`, and this file **strips** that suppression so missing public XML comments **fail the build** (`TreatWarningsAsErrors=true`).

The set is intentionally aligned with the **Conexus.NET baseline** in `docs/consumer-blueprints/conexus-dotnet-platform-readiness.md` (see `scripts/validate-conexus-consumer-baseline-alignment.ps1`).

| Package |
| --- |
| Ontogony.AI.Contracts |
| Ontogony.Artifacts |
| Ontogony.Contracts |
| Ontogony.Errors |
| Ontogony.Execution |
| Ontogony.Hashing |
| Ontogony.Hosting |
| Ontogony.Http |
| Ontogony.Idempotency |
| Ontogony.Logging |
| Ontogony.Observability |
| Ontogony.Primitives |
| Ontogony.Quotas |
| Ontogony.Redaction |
| Ontogony.Secrets |
| Ontogony.Security |

**Changing Tier A:** edit `src/Directory.Build.targets` and the readiness doc table together so the validation script stays green.

## Tier B — shipped; `CS1591` still suppressed (deferred doc completeness)

These **ship** as NuGet packages but remain on the repo-wide `NoWarn` for `CS1591` until doc work catches up **or** they are promoted into the Conexus (or other) consumer baseline.

| Package | Notes |
| --- | --- |
| Ontogony.Configuration | Mechanical configuration helpers |
| Ontogony.Messaging | Messaging abstractions |
| Ontogony.Persistence | Persistence ports/helpers |
| Ontogony.Persistence.Postgres | Postgres-backed implementations |
| Ontogony.ProtocolIngress | Ingress contracts |
| Ontogony.Replay.Contracts | Replay-oriented contracts |
| Ontogony.Testing | Test support (still shipped for reuse) |

**Promoting to Tier A:** add the project to `OntogonyConsumerSurfaceProject` in `src/Directory.Build.targets`, add the matching row to the Conexus readiness **Required packages** table (if Conexus is the driver), fix any new `CS1591` warnings, and merge as an intentional doc-hardening PR.

### Promotion checklist (Tier B → Tier A)

Use this as a **single PR** (or a tightly coupled PR pair) so enforcement and docs never drift.

1. **Targets** — Add the `MSBuildProjectName` clause to `OntogonyConsumerSurfaceProject` in `src/Directory.Build.targets`.
2. **Consumer baseline** — If Conexus is the driver for Tier A membership, add the matching table row for `Ontogony.<YourPackage>` under **Required packages** in `docs/consumer-blueprints/conexus-dotnet-platform-readiness.md` (before **Optional later**), using the same column shape as sibling rows. If another consumer owns the contract, document that alignment instead of silently diverging from Conexus.
3. **Validate alignment** — Run `./scripts/validate-conexus-consumer-baseline-alignment.ps1` when the readiness table changed.
4. **Build** — `dotnet build Ontogony.Platform.sln -c Release` and fix every new **`CS1591`** (and any knock-on warnings) in the promoted project.
5. **Policy tables** — Update the Tier A / Tier B tables in this file in the same change set.
6. **Public API** — If the package participates in `tests/Ontogony.PublicApi.Tests`, refresh verified snapshots and satisfy `scripts/validate-public-api-governance.ps1` (CHANGELOG note when required by that script).
7. **Shipping narrative** — If package README or `docs/packages/*.md` promised “docs relaxed,” adjust wording to match enforced XML.
8. **Consumer-visible shifts** — If the promotion is paired with contract or behavior changes (unusual for a pure doc gate), add `docs/migrations/` + `CHANGELOG.md` per Ontogony.Platform rules.

## Exempt from shipping rules (not Tier A/B)

- **`examples/**`** — sample/skeleton apps; not in the 23-package inventory.
- **`tests/**`** — test-only assemblies; XML doc completeness is not a product contract.

## Coverage reporting

### Cobertura (existing)

`dotnet test` in CI uses `--collect:"XPlat Code Coverage"`. Raw **`coverage.cobertura.xml`** (and `.trx`) are uploaded as the **`xplat-coverage`** artifact.

### HTML (PLAT-QUALITY-001)

CI runs [ReportGenerator](https://github.com/danielpalme/ReportGenerator) on those Cobertura files and uploads a second artifact, **`coverage-report-html`**, for reviewers and release triage. No job fails on line-rate thresholds yet.

### Future — blocking threshold (explicitly deferred)

When line/branch baselines are stable across `main`:

1. Record baseline percentages (per assembly or aggregate) from ReportGenerator summary or Cobertura.
2. Add a **small** CI step (script or `dotnet tool`) that fails PRs only when coverage **drops** below baseline (or below a conservative floor), with an escape hatch for mechanical refactors (documented exception process).
3. Keep HTML + Cobertura artifacts for drill-down.

Until that follow-up, **no** coverage percentage gate runs in CI.

## Related files

| Area | Location |
| --- | --- |
| Global doc file + `CS1591` suppress | `Directory.Build.props` |
| Tier A strip list | `src/Directory.Build.targets` |
| Conexus baseline alignment | `scripts/validate-conexus-consumer-baseline-alignment.ps1` |
| Shipping count / README per package | `scripts/validate-shipping-inventory.ps1` |
| CI coverage + HTML | `.github/workflows/ci.yml` |
