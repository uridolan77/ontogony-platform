# PLAT-9-001 — Six-repo system compatibility gate — evidence

**Date:** 2026-05-22  
**Branch:** main (`47adabf`)  
**Baseline:** SYSTEM-RC-001A

## What was delivered

| Artifact | Location |
|---|---|
| Six-repo lock schema | `schemas/system/ontogony-six-repo-lock.schema.json` |
| Six-repo lock document | `docs/system/ontogony-six-repo-lock.json` |
| Post-lock delta register | `docs/system/ontogony-six-repo-post-lock-deltas.json` |
| Post-lock delta policy doc | `docs/system/ONTOGONY_SIX_REPO_POST_LOCK_DELTAS.md` |
| Contract doc | `docs/contracts/ONTOGONY_SIX_REPO_COMPATIBILITY_LOCK.md` |
| Gate class | `src/Ontogony.SystemCompatibility/SixRepoCompatibilityGate.cs` |
| `Warn` status on `SystemCompatibilityCheckStatus` | `src/Ontogony.SystemCompatibility/SystemCompatibilityCheckStatus.cs` |
| `StrictMode` + `Verdict` on gate result | `src/Ontogony.SystemCompatibility/SystemCompatibilityGateResult.cs` |
| Named artifact output in summary writer | `src/Ontogony.SystemCompatibility/SystemCompatibilitySummaryWriter.cs` |
| Gate tests (18 tests, 100% passing) | `tests/Ontogony.SystemCompatibility.Tests/SixRepoCompatibilityGateTests.cs` |
| Integration test with artifact output | `tests/Ontogony.SystemCompatibility.Tests/SixRepoCompatibilityGateIntegrationTests.cs` |
| Gate runner script with `-ReleaseMode`/`-Strict` | `scripts/run-six-repo-compatibility-gate.ps1` |

## Gate checks

| Check ID | What it verifies |
|---|---|
| `six-repo-lock` | Lock file present |
| `six-repo-schema` | Schema is `ontogony-six-repo-lock-v1` |
| `six-repo-presence` | All 6 repos have valid 40-char hex SHA commits |
| `six-repo-backend-align` | Backend commits/prefixes match runtime lock; warns (dev) or fails (strict) on drift |
| `six-repo-delta-register` | Delta register present, all deltas classified; warns (dev) or fails (strict) on unclassified |
| `six-repo-ui-version` | `@ontogony/ui` packageVersion matches lock |
| `six-repo-fe-provenance` | Frontend `dist/provenance.json` exists; hash matches if pinned |
| `six-repo-ui-manifest` | `UI_PACK_CONSUMER_COMPATIBILITY_MANIFEST.json` present; hash matches lock |
| `six-repo-openapi-hashes` | Pinned OpenAPI snapshot hashes match current files |
| `six-repo-route-inventory` | Frontend route inventory hash matches lock |

## Mode behavior

| Status | Dev mode | Strict/release mode |
|---|---|---|
| Pass | exit 0 | exit 0 |
| Warn | exit 0, print warning | exit 1 (treated as failure) |
| Fail | exit 1 | exit 1 |

## Test run output

```
dotnet test tests/Ontogony.SystemCompatibility.Tests/Ontogony.SystemCompatibility.Tests.csproj \
    -c Release --filter "FullyQualifiedName~SixRepo"

Passed!  - Failed: 0, Passed: 18, Skipped: 0, Total: 18
```

## Validation commands

```powershell
# Development mode (warns tolerated)
./scripts/run-six-repo-compatibility-gate.ps1 -DevRoot C:\dev

# Release/strict mode (warns fail)
./scripts/run-six-repo-compatibility-gate.ps1 -DevRoot C:\dev -ReleaseMode

# Show update instructions
./scripts/run-six-repo-compatibility-gate.ps1 -Update
```
