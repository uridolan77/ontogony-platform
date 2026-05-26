# Golden Fixtures

Create deterministic fixtures so all repos agree on the same reconstructability semantics.

## Fixture 1 — Full critical action

A human-approved Allagma operation triggers a Conexus model route decision and a Kanon policy evaluation.

Expected:

```text
inputs                 F
policyBasis            F
operatorIdentity       F
authorizationEnvelope  F
reasoningEvidence      P or F
outputAction           F
postConditionState     F
ontogonyGovernance     PASS
```

## Fixture 2 — Missing policy basis

Action has run/operator/action records but no Kanon policy/ontology/domain-pack binding.

Expected:

```text
policyBasis            O
ontogonyGovernance     FAIL for high/critical
missing owner          kanon
```

## Fixture 3 — Opaque operator identity

Action has output and state but no actor or service principal.

Expected:

```text
operatorIdentity       O
ontogonyGovernance     FAIL for high/critical
missing owner          allagma or originating service
```

## Fixture 4 — Safe reasoning surrogate only

Conexus route decision has a deterministic reason code but no hidden reasoning.

Expected:

```text
reasoningEvidence      P or F depending on completeness
hiddenCoT              false
ontogonyGovernance     not failed solely due to absent hidden CoT
```

## Fixture 5 — Non-mutating read operation

Action is explicitly read-only.

Expected:

```text
postConditionState     S
stateKind              not_applicable
ontogonyGovernance     PASS if other required fields complete
```

## Fixture 6 — Mutating action with unknown state after

Action executed but no state delta/snapshot exists.

Expected:

```text
postConditionState     O or P
ontogonyGovernance     FAIL for critical, WARN for medium depending on policy
missing owner          service that executed mutation
```

## Fixture 7 — Blocked action

Policy or human gate denied action.

Expected:

```text
outputAction.status    blocked
policyBasis            F
authorizationEnvelope  F
postConditionState     S or F depending on whether attempted mutation changed anything
ontogonyGovernance     PASS as a reconstructed denial/block, not FAIL
```

## Fixture storage

Suggested placement:

```text
kanon-dotnet/tests/.../Fixtures/DecisionReconstructability/*.json
allagma-dotnet/tests/.../Fixtures/DecisionReconstructability/*.json
conexus-dotnet/tests/.../Fixtures/DecisionReconstructability/*.json
ontogony-frontend/src/features/decisionReconstruction/fixtures/*.ts
```

Prefer a single canonical JSON fixture copied or imported across repos if current tooling allows it.
