# Validation matrix

## Local Aisthesis validation

| Command | Required |
|---|---|
| `dotnet restore` | PASS |
| `dotnet build Aisthesis.sln -c Release` | PASS |
| `dotnet test Aisthesis.sln -c Release --no-build` | PASS |
| `./scripts/run-aisthesis-live-spine-smoke.ps1 -StartApi` | PASS |
| `./scripts/system/run-five-service-aisthesis-live-smoke.ps1 -Mode Fixture -StartApi` | PASS |

## Cross-repo validation

| Repo | Gate |
|---|---|
| `allagma-dotnet` | full Release build/test when possible |
| `kanon-dotnet` | full Release build/test when possible |
| `conexus-dotnet` | full Release build/test when possible |
| `metabole-dotnet` | full Release build/test when possible |
| `aisthesis-dotnet` | full Release build/test |

If full gates are blocked by file locks, stop APIs and rerun. File locks are not acceptable as final evidence for RC-readiness.

## Live proof validation

| Proof | Required result |
|---|---|
| LES-001 | PASS, complete, 0 blockers |
| LES-002 | PASS, complete OR justified partial, 0 blockers |
| five-service CI smoke | PASS or explicitly not yet wired |

## Negative gates

The package must fail/partial-close if:

- Aisthesis tests fail;
- required-edge fixture has missing edges;
- bundle fingerprint becomes unstable;
- live proof records fabricated evidence;
- no live proof status is recorded;
- lock decision lacks evidence.
