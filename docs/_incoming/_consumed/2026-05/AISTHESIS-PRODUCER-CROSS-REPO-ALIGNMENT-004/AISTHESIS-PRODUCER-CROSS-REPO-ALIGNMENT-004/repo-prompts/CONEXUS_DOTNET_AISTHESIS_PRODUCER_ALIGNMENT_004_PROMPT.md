# Cursor prompt — Conexus Aisthesis producer alignment 004

You are working in `conexus-dotnet`.

## Context

Aisthesis 003A is green for fixture/harness scope. This repo must now emit producer-native evidence and edges for a future real five-service live proof.

Read:

- `contracts/AISTHESIS_PRODUCER_EMISSION_CONTRACT_V1.md`
- `contracts/PRODUCER_NATIVE_ID_FIELD_MATRIX.md`
- `07_REQUIRED_EDGE_ALIGNMENT_MATRIX.md`
- `workstreams/C_CONEXUS_PRODUCER_ALIGNMENT.md`

## Tasks

1. Review existing Aisthesis integration.
2. Ensure required native IDs are emitted.
3. Ensure required edges owned by this producer are emitted.
4. Ensure edges resolve to real Aisthesis evidence IDs.
5. Add/update tests.
6. Add/update producer smoke script.
7. Write `docs/evidence/CONEXUS_DOTNET_AISTHESIS_PRODUCER_ALIGNMENT_004_CLOSEOUT.md`.

## Validation

```powershell
dotnet restore
dotnet build *.sln -c Release
dotnet test *.sln -c Release --no-build
```

Then run the producer Aisthesis smoke script.

Do not claim live five-service proof unless Aisthesis `-Mode Live` returns `status: PASS`.
