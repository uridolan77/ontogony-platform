# OPERATOR-UX-TAXONOMY-001 Cursor package

## Purpose

Make the Ontogony operator console use one shared vocabulary for state, truth, evidence, data source, and authority.

This package targets the UI taxonomy problems surfaced by the console review backlog:

- health/readiness/contract/operator-usability are currently blurred;
- `unknown` appears without saying unknown *what*;
- `healthy`, `live`, `ready`, and `warning` are mixed inconsistently;
- fixture/demo/generated surfaces can look equivalent to live evidence;
- planned topology links can look implemented;
- backend/API/debug language leaks into normal operator views;
- product pages reinvent local status wording instead of using a shared system.

## Primary implementation target

1. `ontogony-ui` should own the taxonomy types, labels, descriptions, severity mapping, and UI primitives.
2. `ontogony-frontend` should adapt live product/backend data into the shared taxonomy and remove local one-off wording.

## Expected outcome

After this work, an operator can look at any page and understand:

- Is the service reachable?
- Is it ready for the requested workflow?
- Is the payload contract valid?
- Is this evidence live, fixture, generated, imported, or unknown?
- Is this result authoritative, advisory, demo, inferred, or unresolved?
- If something is partial/degraded/unknown, what exactly is missing?

## How to use this package in Cursor

Start with:

1. `01_MASTER_CURSOR_PROMPT.md`
2. `prompts/00_REPO_AUDIT.md`
3. `02_IMPLEMENTATION_PLAN.md`
4. repo-specific prompts under `prompts/`
5. contracts under `contracts/`
6. schemas under `schemas/`
7. scripts under `scripts/`

Do not implement blindly. First inspect the current repos and adapt file paths/names to the actual state.
