# P01 — BACKEND-REPO-DOCS-ORDER-002

Implement slice 1 of `ONTOGONY-BACKEND-COORDINATION-002`.

## Read

- `slices/BACKEND-REPO-DOCS-ORDER-002/README.md`
- `06_TARGET_FILE_MAP.md` (docs order section)
- `allagma-dotnet/docs/system/BACKEND_REPO_DOCS_INDEX.md`

## Task

For **each** of the six backend repos:

1. Update `README.md` — 3–6 line purpose, boundary, test commands, link to `docs/README.md` and status doc.
2. Update `docs/README.md` — add sections if missing: Architecture, Contracts, API, Runbooks, Status, Reviews, Evidence, Incoming, Deferrals.
3. Add banner to stale planning docs: `> **Current status:** see [CURRENT_STATE.md](...)`
4. Run `validate-docs-incoming-hygiene.ps1` and `validate-docs-links.ps1`.
5. Fix broken links OR add row to `docs/DEFERRALS.md`.

Refresh `allagma-dotnet/docs/system/BACKEND_REPO_DOCS_INDEX.md` with coordination-002 status links.

## Do not

- Change API routes or code behavior.
- Delete historical evidence.

## Done when

Acceptance IDs `DOC-001`–`DOC-005` are PASS or `DEFERRED_WITH_REASON` with evidence.
