# Environment planning docs

This folder holds planning packages for local Ontogony operator environments. Packages here are documentation and script stubs only—not runtime code.

## Active package — First working local operator environment

Path:

- `docs/environments/local-operator-sanity-package/`

Purpose:

Plan and implement the **first working local environment** for the cross-repo stack: exact settings, PR sequence, guided operator flow, script stubs, acceptance checklist, and evidence template.

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

Start with:

- `local-operator-sanity-package/00_MANIFEST.json`
- `local-operator-sanity-package/01_EXACT_SETTINGS.md`
- `local-operator-sanity-package/03_PR_SEQUENCE.md`
- `local-operator-sanity-package/04_PR_SPECS/`

Next implementation PR (after unpack-only):

- **ENV-SETUP-001** — Workspace layout, settings, and environment docs (`ontogony-platform`)

## Boundary

This program targets a **first working local environment**, not production readiness. Use fake/local Conexus provider first; optional real-provider mode is a later, explicit PR. Do not treat acceptance here as deploy or hardening sign-off.

## Other planning (unchanged)

Eval and platform planning packages remain in their own trees and are not replaced by this package:

- `docs/planning/eval-durability-to-first-sanity-current/` — active eval durability → first sanity program
- `docs/eval-basing/ontogony-eval-based-cross-repo-development-package/` — completed eval-basing baseline
- `docs/planning/` — other non-environment planning packages (see `docs/planning/README.md`)

Notes:

- Unpack PRs add or refresh planning docs only; implementation PRs land in the repos named in each spec.
- Do not implement `06_SCRIPT_STUBS/` or start ENV-SETUP-001 from an unpack-only PR unless explicitly requested.
