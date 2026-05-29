# P05 — ALLAGMA-CONEXUS-MODEL-ALIAS-001

Implement slice 5 across `allagma-dotnet` and `conexus-dotnet`.

## Read

- `slices/ALLAGMA-CONEXUS-MODEL-ALIAS-001/README.md`
- `allagma-dotnet/src/Allagma.Application/AllagmaModelPurposeRoute.cs`
- `conexus-dotnet/docs/MODEL_ROUTING.md`

## Task

1. Audit `allagma-dotnet/src/` for `gpt-` strings — replace with aliases.
2. Audit tests — use `test-alias` or manifest aliases only.
3. Refresh `conexus-model-alias-manifest.snapshot.json`.
4. Add `CONEXUS_MODEL_ALIAS_CONSUMER_CONTRACT_V0.md` in Conexus.
5. Update Allagma `appsettings` examples.
6. Add CI grep or architecture test blocking provider model IDs in Allagma src.

## Done when

`ALIAS-001`–`ALIAS-005` PASS.
