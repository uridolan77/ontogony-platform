# P02 — SYSTEM-COMPATIBILITY-MATRIX-001

Implement slice 2. Primary workspace: `allagma-dotnet`.

## Read

- `slices/SYSTEM-COMPATIBILITY-MATRIX-001/README.md`
- `allagma-dotnet/docs/system/SYSTEM_COMPATIBILITY_MATRIX.md`
- `allagma-dotnet/docs/system/ontogony-runtime.lock.json`

## Task

1. Fix Release builds in Conexus, Kanon, Allagma if blocking conformance (document root cause).
2. Regenerate route inventories; update matrix route counts.
3. Verify ports: 5081–5085 (Kanon, Conexus, Allagma, Metabole, Aisthesis).
4. Refresh compatibility manifest snapshots in each product repo.
5. Run:
   ```powershell
   ./scripts/validate-runtime-lock.ps1 -DevRoot C:\dev -ReleaseMode
   ./scripts/architecture-conformance/run-cross-repo-conformance.ps1 -DevRoot C:\dev
   ```
6. Bump runtime lock **only** with evidence commit message.
7. Write `docs/evidence/SYSTEM_COMPATIBILITY_MATRIX_001_CLOSEOUT.md`.

## Done when

`COMPAT-001`–`COMPAT-005` PASS or deferred with reason.
