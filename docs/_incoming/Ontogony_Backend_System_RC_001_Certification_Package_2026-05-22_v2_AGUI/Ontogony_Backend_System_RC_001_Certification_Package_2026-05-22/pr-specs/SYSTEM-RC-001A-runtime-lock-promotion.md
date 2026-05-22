# SYSTEM-RC-001A — Runtime Lock Promotion

## Owner

`allagma-dotnet`

## Problem

The system has a strong `SYSTEM-ALPHA-006` lock, but selected backend `main` heads have moved. The system cannot be scored above 9 while the lock and protocol registry describe an older certified baseline.

## Goal

Promote a fresh four-backend runtime lock for:

```text
ontogony-platform
conexus-dotnet
kanon-dotnet
allagma-dotnet
```

This PR should not add features.

## Required changes

### In `allagma-dotnet`

Update:

```text
docs/system/ontogony-runtime.lock.json
docs/system/post-lock-deltas.json
docs/system/POST_LOCK_DELTA_REGISTER.md
docs/system/SYSTEM_COMPATIBILITY_MATRIX.md
docs/releases/RELEASE_EVIDENCE_INDEX.md
```

Add:

```text
docs/evidence/SYSTEM_RC_001A_RUNTIME_LOCK_PROMOTION_EVIDENCE.md
```

### Optional Platform cross-link

If the Platform protocol registry still points to old SHAs, do not silently update it in this PR unless the package owner chooses to include Platform registry refresh here. Otherwise leave a clear follow-up for `PLATFORM-RC-001`.

## Required commands

```powershell
cd C:\dev\allagma-dotnet
pwsh ./scripts/validate-runtime-lock.ps1
pwsh ./scripts/validate-runtime-lock.ps1 -RequireEvidence
pwsh ./scripts/validate-runtime-lock.ps1 -ReleaseMode
pwsh ./scripts/validate-release-lock-crossref.ps1
```

## Acceptance criteria

- `lockedCommits` match selected repo heads.
- `packageVersions` match `Directory.Packages.props`.
- `post-lock-deltas.json` is empty or explains only intentional movement.
- `validate-runtime-lock.ps1 -ReleaseMode` passes.
- No feature implementation is included.
- Release evidence index references the promotion evidence doc.

## Non-goals

- No new API routes.
- No production IAM.
- No real external tool execution.
- No frontend feature work.
