# Runbook — full locked-runtime release gate

## Prerequisites

- GitHub token with access to private repos.
- .NET 9 SDK.
- PowerShell 7.
- Docker available if restart/capacity/Postgres sub-gates are enabled.
- Repos accessible:
  - `ontogony-platform`
  - `kanon-dotnet`
  - `conexus-dotnet`
  - `allagma-dotnet`

## Local command

```powershell
cd C:\dev\ontogony-platform
./scripts/release/run-locked-runtime-release-gate.ps1 `
  -ReleaseId "SYSTEM-ALPHA-007" `
  -Mode Locked `
  -WorkspaceRoot "C:\dev\ontogony-release"
```

## Expected output

```text
artifacts/releases/SYSTEM-ALPHA-007/<timestamp>/
```

## Operator checks

1. Confirm `release-evidence-bundle.json` exists.
2. Run validator:
   ```powershell
   ./scripts/release/validate-runtime-release-evidence.ps1 -BundlePath artifacts/releases/SYSTEM-ALPHA-007/<timestamp>/release-evidence-bundle.json -ReleaseMode
   ```
3. Confirm verdict is `PASS`.
4. Confirm no secret-like values.
5. Attach JSON + Markdown bundle to closeout PR.

## Failure handling

- `lock.invalid`: fix lock before running release.
- `checkout.failed`: pinned commit missing or token lacks access.
- `package_mode.failed`: run package train directly.
- `cohesion.failed`: inspect scenario summary.
- `capacity.failed`: inspect Conexus capacity Markdown report.
