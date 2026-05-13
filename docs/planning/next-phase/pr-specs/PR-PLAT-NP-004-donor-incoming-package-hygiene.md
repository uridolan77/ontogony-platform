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
