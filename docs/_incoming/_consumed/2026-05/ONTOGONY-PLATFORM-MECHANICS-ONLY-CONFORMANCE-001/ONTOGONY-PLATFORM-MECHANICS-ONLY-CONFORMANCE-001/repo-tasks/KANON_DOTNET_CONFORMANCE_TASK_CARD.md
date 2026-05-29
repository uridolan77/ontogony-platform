# kanon-dotnet — Platform conformance task card

## Role

`kanon-dotnet` is the Ontogony semantic authority.

## Required checks

- header propagation
- error envelope
- actor context
- idempotency for action evaluation where applicable
- no ontology semantics in Platform

## Commands

```powershell
cd C:\dev\ontogony-platform
.\scripts\conformance\run-consumer-conformance-suite.ps1 `
  -PlatformRoot C:\dev\ontogony-platform `
  -ConsumerRoot C:\dev\kanon-dotnet `
  -ConsumerName kanon
```

## Evidence

Commit or attach:

```text
artifacts/platform-mechanics-conformance/kanon/<timestamp>/summary.json
```

## Non-claims

This conformance task does not certify product behavior. It only checks shared mechanics.
