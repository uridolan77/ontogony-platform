# Slice 2 — SYSTEM-COMPATIBILITY-MATRIX-001

**Owner:** `allagma-dotnet`  
**Depends on:** Slice 1  
**Prompt:** [`../prompts/P02_SYSTEM_COMPATIBILITY_MATRIX_001.md`](../prompts/P02_SYSTEM_COMPATIBILITY_MATRIX_001.md)

## Goal

Refresh the **compatibility manifest** so runtime lock, route inventories, package pins, ports, and smoke expectations are internally consistent and CI-validated.

## Deliverables

1. `SYSTEM_COMPATIBILITY_MATRIX.md` refreshed with verified route counts and repo SHAs.
2. `ontogony-runtime.lock.json` updated only with evidence when commits/versions change.
3. Per-repo `*_COMPATIBILITY_MANIFEST.json` snapshots aligned.
4. `run-cross-repo-conformance.ps1` PASS or classified deferrals.
5. Fix blocking build/test failures in Conexus, Kanon, Allagma that prevent conformance.

## Key validations

```powershell
./scripts/validate-runtime-lock.ps1 -DevRoot C:\dev -ReleaseMode
./scripts/architecture-conformance/run-cross-repo-conformance.ps1 -DevRoot C:\dev
```

## Evidence

`allagma-dotnet/docs/evidence/SYSTEM_COMPATIBILITY_MATRIX_001_CLOSEOUT.md`

## Non-claims

- No new API routes unless required for manifest accuracy (document if so).
- No production deployment.
