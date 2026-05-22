# SYSTEM-RC-001C — Package-Mode Certification

## Owner

`allagma-dotnet`, with dependency on package-producing upstream repos.

## Problem

Sibling-source builds can hide package-boundary drift. Above-9 integration requires Allagma to build and test against packed Ontogony, Kanon, and Conexus packages at the runtime lock versions.

## Goal

Certify package-mode parity for the locked backend runtime.

## Required command

```powershell
cd C:\dev\allagma-dotnet
pwsh ./scripts/run-package-mode-build.ps1 -WriteCiEvidenceSummary
```

## Required artifacts

```text
artifacts/ci-evidence/package-mode/summary.json
docs/evidence/SYSTEM_RC_001C_PACKAGE_MODE_CERTIFICATION.md
```

## Acceptance criteria

- Allagma restores against packed packages, not sibling project references.
- Allagma builds in Release.
- Allagma tests pass under package mode.
- Conexus streaming typed-client contract compiles in package mode.
- Kanon client/contract package versions match lock.
- Ontogony package version matches lock.
- Any package version drift fails.

## Non-goals

- No public package publishing requirement.
- No version bump unless required for package compatibility.
- No feature implementation.
