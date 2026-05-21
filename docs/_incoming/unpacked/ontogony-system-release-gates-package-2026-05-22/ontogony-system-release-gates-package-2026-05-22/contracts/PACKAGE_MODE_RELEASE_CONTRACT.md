# Package-mode release contract

## Purpose

PLATFORM-REL-001 proves that Allagma can consume Platform, Kanon, and Conexus through NuGet packages instead of sibling project references.

## Required behavior

1. Pack Platform packages from pinned source.
2. Pack Kanon client/contract packages from pinned source.
3. Pack Conexus client/contract packages from pinned source.
4. Restore Allagma against the local feed.
5. Pass package-mode build/test.
6. Assert package versions match runtime lock.
7. Assert sibling project references are not used.

## Required summary

```json
{
  "schema": "ontogony-package-mode-release-summary-v1",
  "verdict": "PASS",
  "runtimeLockBaseline": "SYSTEM-ALPHA-006",
  "feedDirectory": "...",
  "packages": [
    { "id": "Ontogony.Primitives", "version": "0.3.0-alpha.1" },
    { "id": "Kanon.Client", "version": "0.1.0-alpha.0" },
    { "id": "Conexus.Client", "version": "0.1.0-alpha.1" }
  ],
  "restore": { "verdict": "PASS" },
  "build": { "verdict": "PASS" },
  "test": { "verdict": "PASS" },
  "noSiblingReferenceAssertion": { "verdict": "PASS" }
}
```
