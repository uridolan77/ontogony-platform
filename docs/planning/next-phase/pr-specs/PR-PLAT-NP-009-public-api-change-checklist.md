# PR-PLAT-NP-009 — Public API change checklist

## Goal

Make public API snapshot updates safe and reviewable.

## Scope

- Add checklist to PR template or docs:
  - snapshot diff reviewed;
  - breaking/non-breaking classification;
  - changelog entry;
  - migration doc if breaking;
  - package README update if consumer-facing.
- Add docs link from `PUBLIC_API_COMPATIBILITY.md`.

## Acceptance

- PR template contains the checklist.
- Public API docs say snapshot updates alone are not sufficient.
