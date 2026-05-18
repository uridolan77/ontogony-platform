# OpenAPI provenance — eval routes

## Snapshot locations

| Repo | Path | Role |
| --- | --- | --- |
| `allagma-dotnet` | `docs/api/allagma-openapi-v1.snapshot.json` | Backend canonical snapshot |
| `allagma-dotnet` | `docs/api/allagma-openapi-v1.provenance.json` | Backend provenance metadata |
| `ontogony-frontend` | `openapi/allagma.v0.json` | Frontend consumed snapshot |
| `ontogony-frontend` | `docs/openapi/ALLAGMA_SNAPSHOT_PROVENANCE.json` | Cross-repo sync record |
| `ontogony-frontend` | `src/allagma/api/generated/schema.ts` | Generated TypeScript types |

## Recorded provenance (2026-05-18)

From `allagma-dotnet/docs/api/allagma-openapi-v1.provenance.json`:

| Field | Value |
| --- | --- |
| `contractVersion` | `0.1.0` |
| `openapiVersion` | `3.1.0` |
| `snapshotSha256` | `18ce3f667643b1d650c5a36ae1d2d6e6b706bc3613784a7730cc8097f63e01a4` |
| `generatedAt` | `2026-05-18T15:38:53Z` |
| `sourceCommit` (snapshot generation) | `5d46ae4b1c23ebeef5c8db37380969a290cd76bf` |

From `ontogony-frontend/docs/openapi/ALLAGMA_SNAPSHOT_PROVENANCE.json`:

| Field | Value |
| --- | --- |
| `frontendSnapshotSha256` | `4d8d129ccd40c6ccf739d09baa3f991abff418dd2a2476ac257cb7831c0b0bc1` |
| `backendSnapshotSha256` | `18ce3f667643b1d650c5a36ae1d2d6e6b706bc3613784a7730cc8097f63e01a4` |
| `syncedAt` | `2026-05-18T15:39:16.041Z` |

**Note:** Local repo HEADs may advance after provenance was recorded; `openapi:check` validates snapshot **content** equality, not commit equality.

## Eval paths in snapshot (must stay paired)

```text
/allagma/v0/runs/{runId}/evaluations          GET, POST
/allagma/v0/evaluations/{evaluationRunId}     GET
/allagma/v0/evaluations/baseline-comparisons  POST
/allagma/v0/evaluations/baseline-comparisons/{comparisonId}  GET
```

Guarded by `AllagmaOpenApiSnapshotTests` in `allagma-dotnet`.

## Sync and verification commands

### Backend — regenerate snapshot

```powershell
cd c:\dev\allagma-dotnet
dotnet test tests/Allagma.Tests/Allagma.Tests.csproj -c Release --filter "FullyQualifiedName~AllagmaOpenApiSnapshot"
# Regenerate when routes change (project-specific exporter / test workflow)
```

### Frontend — verify drift

```powershell
cd c:\dev\ontogony-frontend
npm run openapi:check
npm run codegen:openapi   # after intentional backend snapshot update
```

### ALIGN-EVAL-001 verification (2026-05-18)

| Command | Result |
| --- | --- |
| `npm run openapi:check` | **pass** |
| `AllagmaOpenApiSnapshot` + `AllagmaEvaluationApi` tests | **13 passed** |

## When to update provenance

Update backend snapshot + provenance + frontend copy when:

1. Eval route paths or methods change
2. Eval DTO required fields change
3. New eval error codes are part of the public contract

Do **not** update snapshots for internal-only harness changes that do not affect OpenAPI.
