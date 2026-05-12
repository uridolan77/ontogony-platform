# Dependency baseline alignment

## Goal

Align platform/consumer dependency policy.

## Acceptance criteria

Baseline document updated; drift documented.

## Implementation (this repo)

- [`docs/planning/robustness/DEPENDENCY_BASELINE.md`](../DEPENDENCY_BASELINE.md) — policy, pin table, documented drift / exceptions.
- [`scripts/validate-dependency-baseline.ps1`](../../../../scripts/validate-dependency-baseline.ps1) — CI/release: one `Microsoft.Extensions.*` + `Microsoft.AspNetCore.TestHost` version; duplicate `PackageVersion` guard; `global.json` SDK `9.0.*`.
- [`docs/FRAMEWORK_BASELINE.md`](../../../FRAMEWORK_BASELINE.md) — links to dependency baseline from the central baseline table and starter guidance.

## Boundary checklist

- [ ] Reusable platform mechanics only.
- [ ] No Conexus routing/provider/model semantics.
- [ ] CI/build/package validation updated where relevant.
