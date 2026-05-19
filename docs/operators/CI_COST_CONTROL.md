# CI cost control (CI-COST-001)

Operator guide for GitHub Actions usage across Ontogony repos. This is **CI discipline**, not production readiness.

## Principles

1. **Local-first, CI-second** — run `dotnet test`, `npm run check`, and Docker-local validation on your machine before pushing.
2. **Cheap checks run automatically** — lint, unit tests, contract checks, docs link validation.
3. **Expensive checks run when relevant or manually** — Playwright E2E, cross-repo cohesion, package-mode matrix, Storybook/knip/size, full Docker stack.
4. **Docs-only PRs** skip build/test/e2e unless docs affect generated artifacts or validation scripts.
5. **Superseded PR runs cancel** via workflow `concurrency` (`cancel-in-progress: true`).
6. **`main` still gets full validation** on push (per-repo), so integration breakage is caught without running heavy jobs on every docs-only PR.

## Repos and workflows

| Repo | Primary CI | Expensive / manual |
|------|------------|-------------------|
| `ontogony-platform` | `ci.yml` (dotnet + docs gates) | Full stack: local Docker only; `release-packages.yml` on tags |
| `ontogony-frontend` | `ci.yml` → `check` | `ci.yml` → `e2e` (path-scoped); `dependency-review.yml` (PR advisory) |
| `ontogony-ui` | `ci.yml` → `check-core` | `ci.yml` → `check-heavy` (path / main / label) |
| `allagma-dotnet` | `ci.yml` (build-test, persistence-smoke, package-mode scoped) | `system-cohesion.yml`, `restart-e2e.yml` (**manual** `workflow_dispatch`) |
| `kanon-dotnet` | `ci.yml` (build, persistence, package-mode scoped) | `cross-repo-architecture` on `main` + relevant PRs |
| `conexus-dotnet` | `ci.yml` (build, persistence, package-mode scoped) | — |

See [CI_COST_001_COST_CONTROL_EVIDENCE.md](../evidence/CI_COST_001_COST_CONTROL_EVIDENCE.md) for the full inventory and before/after triggers.

## Automatic vs manual

### Always automatic (when paths match)

- Unit / API tests (`dotnet test`, Vitest)
- Required package export smoke (`check:exports`, `check:smoke-dist` on UI)
- OpenAPI / contract checks when API or spec paths change
- Docs link / API name validation when `docs/**` or `*.md` change
- Supply-chain / CodeQL on `ontogony-platform` `main` and relevant PRs (unchanged policy)

### Conditional on PR paths

- Frontend Playwright (`e2e` job): `src/`, `e2e/`, `playwright.config.ts`, `package.json`, scripts — **not** docs-only
- UI heavy gates: Storybook, knip, size, consumer matrix, Playwright demo E2E
- Backend `persistence-smoke`: persistence/migration paths
- Backend `package-mode` / `*-package-mode`: package refs, `nuget.config`, `Directory.Build.*`, or `main` push

### Manual (`workflow_dispatch`)

| Workflow | Repo | Purpose |
|----------|------|---------|
| `System cohesion` | `allagma-dotnet` | Four-repo stack + cohesion smoke (was auto on `main` push; now manual to save minutes) |
| `Restart E2E` | `allagma-dotnet` | Restart survival proof (Docker + Postgres on runner) |
| `ci` | all repos | Re-run full gates with `workflow_dispatch` |

### Label triggers (optional)

Add PR label **`run-full-ci`** to force expensive jobs when path filters would skip them (backend + platform). Add **`run-e2e`** on `ontogony-frontend` to force Playwright on a docs-only PR.

Labels are documented first; branch protection should **not** depend on label-only jobs unless you explicitly add them as optional checks.

## When to run full CI locally

| Change type | Local command |
|-------------|----------------|
| Platform packages | `dotnet test Ontogony.Platform.sln -c Release` + `./scripts/validate-docs-links.ps1` |
| Frontend | `npm run check` (from `ontogony-frontend` with sibling `ontogony-ui` built) |
| Frontend + E2E | `npm run check -- --e2e` or `npm run test:e2e` |
| UI package | `npm run build && npm run test:run && npm run check:exports && npm run check:smoke-dist` |
| UI before publish | `npm run check:full` |
| Allagma / Kanon / Conexus | `dotnet test` on the solution; persistence jobs need local Postgres per repo docs |
| Cross-repo cohesion | `allagma-dotnet`: `./scripts/run-system-cohesion-smoke.ps1` locally, or dispatch `System cohesion` |

## Docker-local stack

The first Dockerized local working system is **closed**. Do **not** expect CI to start the full compose stack on every PR.

- **Local:** `ontogony-platform/docker/local-working-system/` (see README there).
- **CI:** PowerShell parse/static checks on `docker/**` script changes only; full stack is manual/local.
- **Allagma:** `system-cohesion.yml` and `restart-e2e.yml` remain manual dispatch workflows.

## Frontend: E2E port and cross-repo token

### `E2E_PORT`

Default Playwright base URL port is **5175** (`playwright.config.ts`). When compose or another dev server uses 5175, set:

```powershell
$env:E2E_PORT = "5176"
npm run test:e2e
```

See `ontogony-frontend/docs/operators/FRONTEND_DOCKER_LOCAL_CONTRACT.md`.

### `ONTOGONY_CROSS_REPO_CHECKOUT_TOKEN`

Required **repository secret** on `ontogony-frontend` (and `allagma-dotnet` for private sibling checkouts). PAT with read access to `ontogony-ui` (and other private siblings). Never commit the token. Configure under **Settings → Secrets and variables → Actions**.

Pin UI ref via repository variable `ONTOGONY_UI_REF` or `workflow_dispatch` input `ontogony_ui_ref`.

## Dependency review

| Repo | Workflow | Blocking? |
|------|----------|-----------|
| `ontogony-platform` | `dependency-review.yml` | Fails PR on moderate+ severity (requires dependency graph from `dependency-submission` on `main`) |
| `ontogony-frontend` | `dependency-review.yml` | High severity + license deny list; confirm branch protection — often **advisory** unless required in GitHub settings |

## Reruns without wasting minutes

1. **Re-run failed jobs only** — GitHub Actions → workflow run → **Re-run failed jobs** (does not re-run successes).
2. **Re-run single job** — From the job log → **Re-run jobs** (dropdown).
3. **Avoid "Re-run all jobs"** on green runs unless you changed dependencies or need a fresh artifact.
4. **Cancel superseded pushes** — concurrency handles this on PRs; avoid pushing 10 commits in a row without waiting.
5. **Use `workflow_dispatch`** on `main` after merging doc-only PRs if you need a full platform pack smoke without a code PR.

## Inspecting expensive runs

- Actions tab → filter workflow (`System cohesion`, `ci` / `e2e`, `allagma-package-mode`).
- Compare duration before/after path filters (evidence doc).
- Download artifacts only when debugging (cohesion summaries, Playwright reports).

## Branch protection expectations

Document your required checks in each repo’s GitHub settings. After CI-COST-001:

- **Do not** require manual-only workflows (`system-cohesion`, `restart-e2e`) unless you accept that they will not run on every merge.
- **Do** require aggregate jobs that pass when scoped jobs skip (`check-full` on frontend, `ci-complete` where added).
- Skipped jobs with explicit `if:` should log `::notice::` explaining why.

## Not production readiness

Path filters and manual cohesion reduce cost; they do not replace staging validation, security review, or release discipline.
