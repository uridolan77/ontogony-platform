# Cursor Task 001 — Credential Source Taxonomy

## Goal

Replace ambiguous credential source display with a typed taxonomy.

## Steps

1. Locate operator settings credential model.
2. Add `OperatorCredentialSource`.
3. Add display mapping.
4. Replace bare `unknown source`.
5. Add tests.

## Acceptance

- `unknown source` is absent from rendered Settings UI.
- Legacy unknown state displays as `source not classified`.
- Every credential row shows a known source.
