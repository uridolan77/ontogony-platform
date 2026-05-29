# Slice 1 — BACKEND-REPO-DOCS-ORDER-002

**Owner:** All six backend repos  
**Depends on:** `BACKEND-REPO-CLEANUP-ORGANIZATION-001` (complete)  
**Prompt:** [`../prompts/P01_BACKEND_REPO_DOCS_ORDER_002.md`](../prompts/P01_BACKEND_REPO_DOCS_ORDER_002.md)

## Goal

Make documentation **navigable, consistent, and non-contradictory** across all six backends without duplicating content in README files.

## Deliverables

1. Each repo `README.md` → points to `docs/README.md` + current status (cleanup or coordination sprint).
2. Each repo `docs/README.md` includes: Architecture, Contracts, API/Routes, Runbooks, Status, Reviews, Evidence, Incoming, Deferrals.
3. `allagma-dotnet/docs/system/BACKEND_REPO_DOCS_INDEX.md` updated with coordination-002 links.
4. Long-lived planning docs get **Current status** banner linking to `CURRENT_STATE.md` or `*_CURRENT_STATUS.md`.
5. `validate-docs-links.ps1` green OR failures in `DEFERRALS.md` with IDs.

## Acceptance gates

See parent matrix IDs `DOC-001` through `DOC-005`.

## Evidence

`docs/evidence/BACKEND_REPO_DOCS_ORDER_002_CLOSEOUT.md` per repo (or single platform index closeout + per-repo link table).

## Non-claims

- No API behavior changes.
- No route prefix changes.
