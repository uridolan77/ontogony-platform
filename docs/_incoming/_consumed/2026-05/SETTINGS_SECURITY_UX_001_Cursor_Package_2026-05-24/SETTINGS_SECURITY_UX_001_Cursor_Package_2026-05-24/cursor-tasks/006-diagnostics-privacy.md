# Cursor Task 006 — Diagnostics Privacy

## Goal

Make diagnostics export privacy explicit and testable.

## Steps

1. Add privacy notice near export controls.
2. Add `privacy` metadata to export.
3. Assert raw secrets absent in tests.
4. List client diagnostics fields.

## Acceptance

- Export contains `containsRawSecrets: false`.
- Export contains `redactionApplied: true`.
- UI notice is visible.
