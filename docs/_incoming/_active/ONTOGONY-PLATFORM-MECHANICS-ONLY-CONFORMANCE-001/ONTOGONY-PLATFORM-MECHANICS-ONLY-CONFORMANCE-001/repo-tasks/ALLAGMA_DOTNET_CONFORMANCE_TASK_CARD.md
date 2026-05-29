# allagma-dotnet — Platform conformance task card

## Role

`allagma-dotnet` is the Ontogony governed execution.

## Required checks

- header propagation
- error envelope
- idempotency for runs/tools
- outbox/artifact references
- runtime lock remains Allagma-owned

## Commands

```powershell
cd C:\dev\ontogony-platform
.\scripts\conformance\run-consumer-conformance-suite.ps1 `
  -PlatformRoot C:\dev\ontogony-platform `
  -ConsumerRoot C:\dev\allagma-dotnet `
  -ConsumerName allagma
```

## Evidence

Commit or attach:

```text
artifacts/platform-mechanics-conformance/allagma/<timestamp>/summary.json
```

## Non-claims

This conformance task does not certify product behavior. It only checks shared mechanics.
