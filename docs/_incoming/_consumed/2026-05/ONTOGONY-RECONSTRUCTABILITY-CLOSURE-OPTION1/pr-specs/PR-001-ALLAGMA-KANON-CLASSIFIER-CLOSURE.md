# PR-001 — Allagma → Kanon classifier closure

## Repo focus

Primary:

```text
C:\dev\allagma-dotnet
```

Secondary only if contract mismatch is found:

```text
C:\dev\kanon-dotnet
```

## Goal

Prove that Allagma's normalized decision events can be classified by Kanon with no high/critical failures.

## Current state

Allagma already has:

```text
GET /allagma/v0/runs/{runId}/decision-events
AllagmaDecisionEventProjector
RunDecisionEventsTests
fragment-ref integrity tests
verified post-condition tests
```

Kanon already has:

```text
POST /ontology/v0/reconstructability/classify
POST /ontology/v0/reconstructability/classify-batch
DecisionReconstructabilityClassifier
```

## Implementation tasks

### 1. Inspect current Allagma decision-event contracts

Confirm `AllagmaDecisionEventContract` is structurally compatible with Kanon classifier input.

If not compatible, prefer a mapping adapter in Allagma tests or shared test fixture, not broad contract churn.

### 2. Add Allagma classifier fixture builder

Implement one of:

```text
tests/Allagma.Tests/Reconstructability/AllagmaDecisionEventFixtureBuilder.cs
```

or a test helper in `RunDecisionEventsTests`.

It must produce events for:

```text
run_operation
human_gate_decision
agent_tool_call
policy_evaluation
workflow_transition
```

At least one high/critical event must be included.

### 3. Add classifier integration test

Possible name:

```text
tests/Allagma.Tests/Reconstructability/AllagmaKanonReconstructabilityIntegrationTests.cs
```

Test behavior:

```text
1. Build Allagma decision events.
2. Map/serialize into Kanon classify-batch request.
3. Invoke Kanon classifier directly if package reference/client available; otherwise test against serialized contract fixture.
4. Assert no high/critical event has governanceStatus FAIL.
5. Persist fixture under tests/Allagma.Tests/Fixtures/reconstructability/.
```

### 4. Fix emitters, not classifier

If classifier fails:
- identify missing property;
- improve Allagma event projection;
- add fragment;
- add policy basis/output/post-condition/source route where real.

Do not weaken Kanon rules unless the rule is wrong for all services.

### 5. Update docs

Update:

```text
docs/CURRENT_STATE.md
docs/KNOWN_LIMITATIONS.md
docs/system/ALLAGMA_FEATURE_CONNECTION_MATRIX.md
docs/system/allagma-feature-connection.matrix.json
docs/evidence/ALLAGMA_KANON_RECONSTRUCTABILITY_CLOSURE.md
```

## Required tests

```powershell
cd C:\dev\allagma-dotnet

dotnet build Allagma.sln -c Release --no-incremental

dotnet test tests/Allagma.Tests/Allagma.Tests.csproj `
  --filter "RunDecisionEvents|Reconstructability" `
  -q

dotnet test tests/Allagma.Tests/Allagma.Tests.csproj `
  --filter "AllagmaV0RouteInventoryTests|FeatureConnectionMatrixAuditTests" `
  -q
```

If Kanon tests changed:

```powershell
cd C:\dev\kanon-dotnet
dotnet test Kanon.sln -c Release --filter "FullyQualifiedName~Reconstructability" -q
```

## Acceptance criteria

```text
Allagma decision-events fixture exists.
Kanon classifier compatibility is proven.
No high/critical Allagma event classifies FAIL.
Docs correctly say classifier integration is implemented.
Remaining warnings are documented with reason and remediation.
```

## Non-goals

- Conexus emitters.
- Frontend panel.
- Real tool execution.
- Production SLO/security hardening.
