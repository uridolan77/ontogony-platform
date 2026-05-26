# Cursor Task 002 — Warning Consolidation

## Goal

Replace repeated local credential warnings with one warning group.

## Steps

1. Find all local-storage/browser credential warning copy.
2. Replace repeated warnings with one `LocalCredentialStorageNotice`.
3. Add compact per-row tags.
4. Add component test.

## Acceptance

- Settings page has one local credential warning.
- Credential rows still show per-row risk/source.
