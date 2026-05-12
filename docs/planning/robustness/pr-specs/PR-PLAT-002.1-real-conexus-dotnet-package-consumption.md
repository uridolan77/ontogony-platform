# Real Conexus.NET package consumption compatibility

## Goal

Prove that the **current** Conexus.NET repository builds and tests against **released** Ontogony packages (not sibling `ProjectReference` paths to `ontogony-platform`).

## Relationship to PR-PLAT-002

[PR-PLAT-002](./PR-PLAT-002-consumer-compat-test-conexus.md) adds `examples/ConexusDotNetPackageSmoke`: a Conexus-shaped compile smoke that restores the frozen package set from local `.nupkg` files after `pack-all.ps1`. That validates **pack layout and API surface** for a minimal consumer.

This follow-up validates **full product codebase** compatibility (full solution build + test suite) when Conexus references the same versions published from this repo.

## Acceptance criteria

- Conexus CI (or an opt-in workflow) restores Ontogony from **GitHub Packages** (or another pinned internal feed), not from a checked-out `ontogony-platform` tree.
- `dotnet build` / `dotnet test` for the Conexus solution succeed on that configuration.

## Recommended placement (preferred)

**Option A (preferred):** Implement in the **Conexus.NET** repository: a `workflow_dispatch` or branch/PR job that adds the authenticated feed, pins `PackageReference` versions to the Ontogony line under test, and runs the full test matrix. Conexus owns “we consume released packages correctly.”

**Option B:** A `ontogony-platform` workflow that checks out `conexus-dotnet` alongside this repo, publishes packages to a transient feed or uses packed outputs + `nuget.config`, then builds/tests Conexus. Higher maintenance and couples repos in CI; use only if Conexus cannot host the job yet.

## Boundary checklist

- [ ] No Conexus routing/provider/model semantics added to `ontogony-platform`.
- [ ] Any workflow in this repo remains a thin mechanical checkout + restore + build, if Option B is used.
