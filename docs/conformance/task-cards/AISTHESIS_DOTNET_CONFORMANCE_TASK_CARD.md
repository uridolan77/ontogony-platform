# aisthesis-dotnet — Platform conformance task card

## Role

`aisthesis-dotnet` is the Ontogony evidence/reconstructability spine.

## Required checks

- header propagation
- error envelope
- evidence reference shape
- actor context
- no reconstructability scoring in Platform

## Commands

```powershell
cd C:\dev\ontogony-platform
.\scripts\conformance\run-consumer-conformance-suite.ps1 `
  -PlatformRoot C:\dev\ontogony-platform `
  -ConsumerRoot C:\dev\aisthesis-dotnet `
  -ConsumerName aisthesis
```

## Evidence

Commit or attach:

```text
artifacts/platform-mechanics-conformance/aisthesis/<timestamp>/summary.json
```

## Non-claims

This conformance task does not certify product behavior. It only checks shared mechanics.
