# Current-state baseline

## Aisthesis 003A verified state

```text
dotnet build Aisthesis.sln -c Release                    PASS
dotnet test Aisthesis.sln -c Release                     74 passed / 0 failed
run-aisthesis-live-spine-smoke.ps1                       PASS, grade complete
run-five-service-aisthesis-live-smoke.ps1 -Mode Fixture  PASS
requiredEdges.present                                    10
requiredEdges.missing                                    0
reconstructabilityGrade                                  complete
```

## Still unproven

```text
Native five-service live proof: not yet
Frontend live backing: handoff only
Production IAM: deferred
Retention/erasure APIs: deferred
Distributed trace export: deferred
```

## Why producer alignment is required

Aisthesis cannot manufacture live proof. Each producer must emit native envelopes, native IDs, and edges that satisfy the 10 required-edge matrix.
