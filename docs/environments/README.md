# Environment planning docs

This folder holds planning packages and **canonical operator documentation** for local Ontogony environments. Planning packages are specs and stubs; `local-operator-sanity/` is the operator-facing doc set for the active program.

## Active program — First working local operator environment

**Canonical docs (operator-facing):**

- `docs/environments/local-operator-sanity/`

Start with:

- `local-operator-sanity/00_MANIFEST.json`
- `local-operator-sanity/01_WORKSPACE_LAYOUT.md`
- `local-operator-sanity/02_EXACT_SETTINGS.md`
- `local-operator-sanity/03_MAIN_USE_FLOW.md`

**Planning package (PR specs, prompts, script stubs):**

- `docs/environments/local-operator-sanity-package/`

Incoming ZIP preserved at:

- `docs/_incoming/ontogony-first-working-environment-package.zip`

Expected workspace layout (all repos under `C:\dev\`):

```text
C:\dev\
  ontogony-platform\
  allagma-dotnet\
  kanon-dotnet\
  conexus-dotnet\
  ontogony-frontend\
  ontogony-ui\
```

## PR status

| PR | Status | Repo |
| --- | --- | --- |
| ENV-SETUP-001 | **Done** — workspace/settings docs | `ontogony-platform` |
| ENV-SETUP-002 | Next — stack start/check scripts | `allagma-dotnet` |
| ENV-RUN-001 … ENV-CLOSEOUT-001 | Planned | See `local-operator-sanity-package/03_PR_SEQUENCE.md` |

Evidence:

- `docs/evidence/ENV_SETUP_001_LOCAL_OPERATOR_SANITY_DOCS.md`

## Boundary

This program targets a **first working local environment**, not production readiness. Use fake/local Conexus provider first; optional real-provider mode is a later, explicit PR. Do not treat acceptance here as deploy or hardening sign-off.

## Other planning (unchanged)

Eval and platform planning packages remain in their own trees:

- `docs/planning/eval-durability-to-first-sanity-current/` — active eval durability → first sanity program
- `docs/eval-basing/ontogony-eval-based-cross-repo-development-package/` — completed eval-basing baseline
- `docs/planning/` — other non-environment planning packages (see `docs/planning/README.md`)

Notes:

- Implementation PRs land in the repos named in each spec under `local-operator-sanity-package/04_PR_SPECS/`.
- Script stubs under `local-operator-sanity-package/06_SCRIPT_STUBS/` are not executed until copied into `allagma-dotnet` (ENV-SETUP-002 onward).
