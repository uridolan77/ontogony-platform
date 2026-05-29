# Scope and boundary

## In scope

1. RC-readiness scripts and docs.
2. Full clean Release gate documentation.
3. CI-compatible five-service smoke harness.
4. ReleaseMode gate definition and checks.
5. Live proof summary v2 contract.
6. LES-001/LES-002 rerun discipline.
7. LES-002 completion or accepted partial-grade rationale.
8. Aisthesis lock decision record.
9. Frontend contract readiness handoff.
10. IAM/retention/OTel gates defined as promotion blockers.

## Out of scope unless a validation failure proves necessity

1. New Aisthesis domain concepts.
2. New public routes.
3. Changes to required-edge IDs.
4. Changes to producer ownership.
5. Moving orchestration into Aisthesis.
6. Rewriting producer emitters.
7. Frontend implementation.
8. Production IAM implementation.
9. Retention/erasure API implementation.
10. Distributed trace export implementation.

## Boundary statement

Aisthesis is a reconstructability service. It should ask:

```text
Can the system explain what happened, which producer emitted what, how the evidence is linked, and whether a decision/run/model/profile/artifact can be reconstructed?
```

It should not ask:

```text
Should this semantic decision be approved?
Which model/provider should be used?
Which workflow should execute next?
How should a SLOD mapping be transformed?
```
