# ALIGN-EVAL-001 — Backend/frontend eval alignment refresh

**Recorded at (UTC):** 2026-05-18  
**Verdict:** PASS

## Scope

Publish current eval contract alignment package after EVAL-DUR-001 through FE-POLISH-001. Documentation only — no runtime contract changes.

## Delivered

```text
docs/alignment/eval-full-sanity-alignment/
  00_MANIFEST.json
  01_BACKEND_CONTRACT_SNAPSHOT.md
  02_FRONTEND_CONSUMPTION_MATRIX.md
  03_EVAL_DATASET_MATRIX.md
  04_OPENAPI_PROVENANCE.md
  05_CAPABILITY_LIMITATION_MATRIX.md
  06_FULL_SANITY_TEST_PLAN.md
  07_KNOWN_LIMITATIONS.md
```

## Repo heads at alignment time

| Repo | HEAD |
| --- | --- |
| `allagma-dotnet` | `38e6b7c79aeac30d6db03d8fae02853f9354a8f6` |
| `ontogony-frontend` | `61b111c57e30cf9dd2921cda6f197bc159a5ed0e` |
| `kanon-dotnet` | `b4e1d34123534fcdd29dbfcb9aa294ab57ad9791` |
| `conexus-dotnet` | `cb7860694e53b0abe8fe607ec5532d6734de73ae` |
| `ontogony-platform` | `4754824e49d2fcd684cb408a930c30500d9cb8f8` |

## Commands run

```powershell
cd c:\dev\ontogony-frontend
npm run openapi:check

cd c:\dev\allagma-dotnet
dotnet test tests/Allagma.Tests/Allagma.Tests.csproj -c Release --filter "FullyQualifiedName~AllagmaOpenApiSnapshot|FullyQualifiedName~AllagmaEvaluationApi"

cd c:\dev\ontogony-frontend
npm run test -- src/allagma/adapters/allagmaEvaluationAdapters.test.ts src/allagma/adapters/allagmaEvaluationDashboardAdapters.test.ts
```

## Results

| Gate | Result |
| --- | --- |
| `openapi:check` | **pass** |
| Allagma eval OpenAPI + API tests | **13 passed** |
| Frontend eval adapter unit tests | **10 passed** |

## Alignment summary

- Five eval GET routes + two gated POST routes documented in backend snapshot.
- Frontend consumption mapped to four operator routes + run-detail eval section.
- `scenario-dataset-v0` matrix linked to CI suite and full sanity scenarios.
- OpenAPI SHA256 provenance recorded; content drift check passing.
- Capability matrix and known limitations consolidated for SYS-FULL-SANITY-001.

## Safety confirmation

- No contract or route changes in this PR.
- Limitation banners remain honest (no global eval list, sample trend).
- Fixture/live distinction preserved in consumption matrix.

## Next step

**SYS-FULL-SANITY-001** — execute `06_FULL_SANITY_TEST_PLAN.md` and export full sanity report.
