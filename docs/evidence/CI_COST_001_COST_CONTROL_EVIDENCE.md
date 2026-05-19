# CI-COST-001 — cost control evidence

**Status:** Implemented (workflow + docs). **Not production readiness.**

## Workflows inspected

### ontogony-platform

| Workflow | Trigger (before) | Expensive jobs | Secrets |
|----------|------------------|----------------|---------|
| `ci.yml` | push/PR `main`, dispatch | Postgres + dotnet test + pack + 10+ PS scripts | none |
| `dependency-review.yml` | PR `main` | dependency graph diff | `GITHUB_TOKEN` |
| `dependency-submission.yml` | push `main` | restore + submit | `GITHUB_TOKEN` |
| `supply-chain.yml` | push/PR `main`, dispatch | vuln audit, Trivy SBOM, gitleaks | `GITHUB_TOKEN` |
| `codeql.yml` | push/PR `main`, schedule, dispatch | build + CodeQL | `GITHUB_TOKEN` |
| `release-packages.yml` | tags, dispatch | full release pipeline + Postgres | `GITHUB_TOKEN` |

Samples (`multi-checkout`, `consume-internal-feed`) are `workflow_dispatch` only — unchanged.

### ontogony-frontend

| Workflow | Trigger (before) | Expensive jobs | Secrets |
|----------|------------------|----------------|---------|
| `ci.yml` | push/PR, dispatch | UI checkout + build + `npm run check` + Playwright | `ONTOGONY_CROSS_REPO_CHECKOUT_TOKEN` |
| `dependency-review.yml` | PR | license/severity review | `GITHUB_TOKEN` |

### ontogony-ui

| Workflow | Trigger (before) | Expensive jobs | Secrets |
|----------|------------------|----------------|---------|
| `ci.yml` | push/PR | `npm run check:full` (Storybook, knip, size, consumers, e2e) | none |

### allagma-dotnet

| Workflow | Trigger (before) | Expensive jobs | Secrets |
|----------|------------------|----------------|---------|
| `ci.yml` | push/PR `main` | 3 jobs: build-test (4-repo checkout), persistence-smoke, package-mode | `ONTOGONY_CROSS_REPO_CHECKOUT_TOKEN` |
| `system-cohesion.yml` | **push `main`**, dispatch | 4-repo + headless stack + cohesion smoke | token |
| `restart-e2e.yml` | dispatch only | Docker restart proof | token |

### kanon-dotnet

| Workflow | Trigger (before) | Expensive jobs | Secrets |
|----------|------------------|----------------|---------|
| `ci.yml` | push/PR `main` | build, persistence-smoke, cross-repo-arch (4 repos), package-mode | none (public siblings) |

### conexus-dotnet

| Workflow | Trigger (before) | Expensive jobs | Secrets |
|----------|------------------|----------------|---------|
| `ci.yml` | push/PR `main` | build, persistence-smoke, ontogony package-mode | none |

## Changes made

| Repo | File | Change |
|------|------|--------|
| `ontogony-platform` | `.github/workflows/ci.yml` | Concurrency; `changes` job; split `docs-gates` / `dotnet`; docker PS parse job; skip notices |
| `ontogony-platform` | `docs/operators/CI_COST_CONTROL.md` | Operator guide (canonical) |
| `ontogony-frontend` | `.github/workflows/ci.yml` | Path-scoped `check` / `e2e`; `run-e2e` label; skip notices |
| `ontogony-frontend` | `docs/development/CI_COST_CONTROL.md` | Repo-local summary |
| `ontogony-ui` | `.github/workflows/ci.yml` | Concurrency; `check-core` / `check-heavy`; path + label gates |
| `ontogony-ui` | `docs/development/CI_COST_CONTROL.md` | Package gate tiers |
| `allagma-dotnet` | `.github/workflows/ci.yml` | Concurrency; path-scoped jobs |
| `allagma-dotnet` | `.github/workflows/system-cohesion.yml` | **Removed** auto `push: main` (dispatch only) |
| `kanon-dotnet` | `.github/workflows/ci.yml` | Concurrency; path-scoped jobs |
| `conexus-dotnet` | `.github/workflows/ci.yml` | Concurrency; path-scoped jobs |

## Before / after trigger behavior

| Scenario | Before | After |
|----------|--------|-------|
| Docs-only PR (platform) | Full Postgres + dotnet + pack | Docs validation scripts only |
| Docs-only PR (frontend) | check + Playwright | Skipped with notice; optional `run-e2e` label |
| Docs-only PR (UI) | `check:full` | Skipped with notice |
| Docs-only PR (backend) | All CI jobs | Skipped with notice |
| `main` push | Full CI per repo | Full CI preserved (path `run_full` true on push) |
| Allagma merge to `main` | + system-cohesion auto | Cohesion **manual** only |
| UI Storybook change PR | `check:full` always | `check-core` + `check-heavy` |
| New PR commits | Stacked runs | Superseded runs cancelled |

## Jobs remaining automatic

- Platform: dotnet pipeline on code/script/workflow/docker path changes and all `main` pushes.
- Frontend: `check` on src/config/scripts; `e2e` when e2e/src/playwright paths or `main` or `run-e2e` label.
- UI: `check-core` on package/src changes; `check-heavy` when heavy paths or `main` or `run-full-ci`.
- Backends: `build-test` on code paths + `main`; persistence/package-mode scoped (see workflow `if:`).

## Jobs manual / conditional

- `allagma-dotnet` / `system-cohesion.yml` — **workflow_dispatch** only.
- `allagma-dotnet` / `restart-e2e.yml` — unchanged manual.
- Expensive backend jobs on PR — skipped unless paths, `run-full-ci` label, or `workflow_dispatch`.

## How to run full validation manually

```bash
# Platform
dotnet test Ontogony.Platform.sln -c Release
./scripts/pack-all.ps1

# Frontend (with ontogony-ui sibling built)
npm run check -- --e2e

# UI
npm run check:full

# Allagma cohesion (Actions UI or gh)
gh workflow run system-cohesion.yml -R uridolan77/allagma-dotnet
```

## Safety statement

- No required security workflows removed (CodeQL, supply-chain, dependency-review, gitleaks policy unchanged).
- No secrets committed.
- Core unit/API tests and export smoke gates preserved on relevant path changes.
- `main` push still runs full per-repo CI where configured.

## Repo-local evidence links

- [ontogony-frontend/docs/development/CI_COST_CONTROL.md](https://github.com/uridolan77/ontogony-frontend/blob/main/docs/development/CI_COST_CONTROL.md)
- [ontogony-ui/docs/development/CI_COST_CONTROL.md](https://github.com/uridolan77/ontogony-ui/blob/main/docs/development/CI_COST_CONTROL.md)

## Validation performed

- YAML/workflow edits reviewed in-repo.
- Local commands recommended in operator guide (not repeated here to avoid burning Actions minutes).

**No production-readiness claim.**
