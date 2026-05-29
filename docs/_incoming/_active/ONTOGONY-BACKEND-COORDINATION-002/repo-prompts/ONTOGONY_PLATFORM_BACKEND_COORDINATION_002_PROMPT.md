# Cursor prompt — ontogony-platform (BACKEND-COORDINATION-002)

Execute platform slices for `ONTOGONY-BACKEND-COORDINATION-002`.

**Package root:** `ontogony-platform/docs/_incoming/_active/ONTOGONY-BACKEND-COORDINATION-002/`

## Your slices

- **Slice 1:** Docs order (platform `docs/README.md`, deferrals)
- **Slice 3:** Shared error contract — promote `contracts/CROSS_SERVICE_ERROR_ENVELOPE_V1.md` to `docs/contracts/`
- **Slice 4:** Context propagation contract — promote to `docs/contracts/`

## Rules

- Mechanics only — no product semantics.
- Extend existing validators; do not duplicate `CrossServiceErrorEnvelope`.
- Run `validate-docs-incoming-hygiene.ps1` after package edits.

## Read

`repo-tasks/ontogony-platform.md` + prompts P01, P03, P04.
