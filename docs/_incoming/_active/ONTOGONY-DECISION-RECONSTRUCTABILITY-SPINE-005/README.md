# ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-005

**Title:** Canonical golden fixtures (five Allagma decision kinds)  
**Status:** active  
**Depends on:** SPINE-004 smoke harness

## Canonical fixture set

```text
ontogony-platform/fixtures/decision-reconstructability/
  allagma-run-operation.pass.json
  allagma-human-gate.warn.json
  allagma-workflow-transition.pass.json
  allagma-agent-tool-call.warn.json
  allagma-policy-evaluation.fail.json
```

Each file is an **Allagma-exported** `DecisionEventContract` JSON body. Filename suffix encodes expected Kanon governance: `.pass`, `.warn`, `.fail`.

## Contract chain under test

```text
Allagma projector shape (or enriched export)
  → deserialize as Kanon DecisionEventContract
  → DecisionReconstructabilityClassifier.Classify
  → stable ontogonyGovernanceStatus
```

## Regenerate

```powershell
cd C:\dev\allagma-dotnet
$env:ALLAGMA_EMIT_DEC_RECON_GOLDEN = "1"
dotnet test tests\Allagma.Tests\Allagma.Tests.csproj --filter "Emit_golden_fixtures"
```

Copies to platform, `allagma-dotnet/tests/.../Fixtures/decision-reconstructability`, and `kanon-dotnet/tests/.../Fixtures/decision-reconstructability`.

## Tests

| Repo | Test |
| --- | --- |
| `kanon-dotnet` | `DecisionReconstructabilityGoldenFixtureTests` |
| `allagma-dotnet` | `DecisionReconstructabilityGoldenFixtureTests` (existence + deserialize) |
| `ontogony-frontend` | `decisionReconstructabilityGolden.test.ts` |
