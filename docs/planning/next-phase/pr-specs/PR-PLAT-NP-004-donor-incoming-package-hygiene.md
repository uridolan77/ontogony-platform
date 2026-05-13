# PR-PLAT-NP-004 — Donor and incoming-package hygiene

## Goal

Ensure overlay/donor/agent-planning material does not become part of package, release, or consumer surfaces.

## Scope

- Audit `_agent_prompts`, `_issue_bodies`, `docs/_incoming_packages`, `.tmp`, donor folders, and copied overlay docs.
- Add or update `.gitignore` and package exclusion rules as needed.
- Add validation script or extend shipping inventory validation to reject donor/incoming paths inside packages.

## Acceptance

- Package artifacts do not contain donor/overlay paths.
- CI fails if future package contents include forbidden coordination paths.
- Repo docs clearly separate planning artifacts from shipped package docs.

## Non-goals

- Do not delete useful planning prompts unless they are obsolete; relocate if needed.

## Status (repo)

**Closed.** [`scripts/validate-nupkg-coordination-path-hygiene.ps1`](../../../../scripts/validate-nupkg-coordination-path-hygiene.ps1) runs in **`ci.yml`** and **`release-packages.yml`** immediately after `pack-all`, scanning every non-symbol `.nupkg` for forbidden coordination path fragments. [`docs/packages/index.md`](../../../packages/index.md) explains that planning-only paths must not ship inside packages.

## Manual / local verification

1. From repo root: `$env:PACKAGE_VERSION = "0.3.0-alpha.1"` (or your pack version), then `./scripts/pack-all.ps1 -NoBuild` after a Release build.
2. Run `./scripts/validate-nupkg-coordination-path-hygiene.ps1` — expect `OK: coordination-path hygiene`.
3. Optional negative check: add a disposable zip under `artifacts/packages/` whose entry names contain a forbidden fragment (for example `_agent_prompts/x.txt`); the script must throw naming the fragment; remove the test zip afterward.
