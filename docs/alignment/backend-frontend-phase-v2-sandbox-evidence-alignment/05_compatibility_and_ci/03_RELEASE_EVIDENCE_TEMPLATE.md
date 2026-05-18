# Release Evidence Template

**Filled instance:** `07_evidence/FBA2-007_RELEASE_SCORECARD.md` (FBA2-007, 2026-05-18)

## Backend

| Service | Repo | Commit | OpenAPI hash | Tests |
|---|---|---|---|---|
| Allagma | `allagma-dotnet` | `8cab40bbcd2abe744b81ba29214d56fc10b23e74` | `b6d6e6e5…50964` | Capabilities + OpenAPI: 8 passed |
| Kanon | — | — | — | Not in FBA2 scope |
| Conexus | — | — | — | Not in FBA2 scope |
| Ontogony.Platform | `ontogony-platform` | (evidence docs) | — | Alignment package only |

## Frontend

| Repo | Commit | OpenAPI snapshot hash | Checks |
|---|---|---|---|
| ontogony-frontend | `955cc868672d6577aad48aedf57963875ac790ca` | `330782f3…47d63` | vitest 17 passed; `openapi:check` passed |

## Required proof

| Item | Status |
|---|---|
| Allagma P3-SB-006 closeout recorded | Yes — `allagma-dotnet/docs/evidence/PHASE3_MINIMAL_SANDBOX_RC_EVIDENCE.md` |
| Sandbox evidence rendered | Yes — panel + mocked E2E |
| Sandbox timeline rendered | Yes — five execute events |
| Real external execution blocked shown | Yes — capabilities + panel |
| Local sandbox execute from backend capabilities | Yes — `/capabilities` hook |
| No raw content exposed | Yes — redaction + panel note |
| Limitation banners accurate | Yes — OpenAPI + capabilities |
| Contract gate | Yes — `npm run openapi:check` |
