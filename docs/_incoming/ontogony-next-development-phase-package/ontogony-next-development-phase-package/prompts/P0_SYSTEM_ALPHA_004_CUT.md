# P0 — SYSTEM-ALPHA-004-CUT

## Objective

Cut the new reproducible alpha runtime baseline.

## Preconditions

- `SYSTEM-ALPHA-004-PREP` complete.
- Required validation gates pass or have documented, approved quarantines.

## Tasks

1. Update `allagma-dotnet/docs/system/ontogony-runtime.lock.json`.
2. Set baseline to `SYSTEM-ALPHA-004` with current locked commits, current evidence artifact paths, and current required scenarios.
3. Add `allagma-dotnet/docs/evidence/SYSTEM_ALPHA_004_CLOSEOUT.md`.
4. Update platform baseline docs to point to the new lock.
5. Mark `SYSTEM-ALPHA-003` historical.

## Acceptance

- Runtime lock points to current validated commits.
- Evidence artifacts exist.
- Closeout includes caveats and non-claims.
- Platform references the new lock.
