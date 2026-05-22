# Ontogony six-repo compatibility lock

**Schema:** `ontogony-six-repo-lock-v1`  
**Owner:** `ontogony-platform/Ontogony.SystemCompatibility`  
**Lock file:** `docs/system/ontogony-six-repo-lock.json`  
**JSON schema:** `schemas/system/ontogony-six-repo-lock.schema.json`

## Purpose

The six-repo lock extends the four-runtime-repo `ontogony-runtime.lock.json` to include `ontogony-ui` and `ontogony-frontend` in the same reproducibility unit. It is the single source of truth for what combination of all six repositories has been tested and accepted together.

The `SixRepoCompatibilityGate` validates this lock as part of the platform system-compatibility gate suite.

## Repositories covered

| Repo | Locked fields |
|---|---|
| `ontogony-platform` | commit, packageVersion |
| `conexus-dotnet` | commit, clientVersion |
| `kanon-dotnet` | commit, clientVersion |
| `allagma-dotnet` | commit, apiPrefix |
| `ontogony-ui` | commit, packageVersion, tarballSha256 |
| `ontogony-frontend` | commit, buildProvenanceSha256 |

## Contract hashes

The `contracts` section locks the SHA256 fingerprints of key cross-repo artifacts so drift is detected immediately:

| Field | Source artifact |
|---|---|
| `openapiSnapshots.allagma` | `ontogony-frontend/openapi/allagma.v0.json` |
| `openapiSnapshots.conexus` | `ontogony-frontend/openapi/conexus.v0.json` |
| `openapiSnapshots.kanon` | `ontogony-frontend/openapi/kanon.v0.json` |
| `frontendRouteInventory` | `ontogony-frontend/docs/generated/ROUTE_WORKFLOW_INVENTORY.md` |
| `uiPublicSubpaths` | `ontogony-frontend/docs/generated/UI_PACK_CONSUMER_COMPATIBILITY_MANIFEST.json` |

## Post-lock delta register

Commits beyond the pinned SHA in any repo must appear in `allagma-dotnet/docs/system/post-lock-deltas.json` under the repo name. The gate treats an unclassified post-lock commit as a warning (not a hard failure) to allow forward development between formal lock promotions.

## Validation

```powershell
./scripts/run-six-repo-compatibility-gate.ps1 -DevRoot C:\dev
```

## Updating the lock

Run the sync script from the `ontogony-platform` root:

```powershell
./scripts/run-six-repo-compatibility-gate.ps1 -DevRoot C:\dev -Update
```

Or update `docs/system/ontogony-six-repo-lock.json` manually and re-run the gate to confirm all checks pass.

## Relationship to runtime lock

The four-repo runtime lock (`allagma-dotnet/docs/system/ontogony-runtime.lock.json`) remains the canonical source for backend package version and commit data. The six-repo lock reads the runtime lock for backend alignment and adds the frontend/UI layer on top.
