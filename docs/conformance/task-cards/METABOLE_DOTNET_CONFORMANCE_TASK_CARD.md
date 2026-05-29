# metabole-dotnet — Platform conformance task card

## Role

`metabole-dotnet` is the Ontogony data transformation spine.

## Required checks

- header propagation
- error envelope
- idempotency for pipeline runs
- evidence references
- no SLOD acceptance semantics in Platform

## Commands

```powershell
cd C:\dev\ontogony-platform
.\scripts\conformance\run-consumer-conformance-suite.ps1 `
  -PlatformRoot C:\dev\ontogony-platform `
  -ConsumerRoot C:\dev\metabole-dotnet `
  -ConsumerName metabole
```

## Evidence

Commit or attach:

```text
artifacts/platform-mechanics-conformance/metabole/<timestamp>/summary.json
```

## Non-claims

This conformance task does not certify product behavior. It only checks shared mechanics.
