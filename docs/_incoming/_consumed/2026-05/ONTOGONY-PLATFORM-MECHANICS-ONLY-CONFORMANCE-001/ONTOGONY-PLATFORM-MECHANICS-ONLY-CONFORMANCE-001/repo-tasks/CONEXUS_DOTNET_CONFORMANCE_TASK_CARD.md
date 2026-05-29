# conexus-dotnet — Platform conformance task card

## Role

`conexus-dotnet` is the Ontogony model gateway.

## Required checks

- header propagation
- error envelope
- idempotency for model-call requests
- observability meter naming
- no provider policy in Platform

## Commands

```powershell
cd C:\dev\ontogony-platform
.\scripts\conformance\run-consumer-conformance-suite.ps1 `
  -PlatformRoot C:\dev\ontogony-platform `
  -ConsumerRoot C:\dev\conexus-dotnet `
  -ConsumerName conexus
```

## Evidence

Commit or attach:

```text
artifacts/platform-mechanics-conformance/conexus/<timestamp>/summary.json
```

## Non-claims

This conformance task does not certify product behavior. It only checks shared mechanics.
