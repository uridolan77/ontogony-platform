# Cursor unpack prompt — ONTOGONY-RECONSTRUCTABILITY-CLOSURE-OPTION1

You are working in the Ontogony local multi-repo layout:

```text
C:\dev\ontogony-platform
C:\dev\kanon-dotnet
C:\dev\conexus-dotnet
C:\dev\allagma-dotnet
C:\dev\ontogony-frontend
C:\dev\ontogony-ui
```

Unpack this package under:

```text
C:\dev\ontogony-platform\docs\_incoming\packages\ONTOGONY-RECONSTRUCTABILITY-CLOSURE-OPTION1
```

Also copy or reference the package from the other involved repos if needed, but keep this package as the source-of-truth intake.

## Mission

Close the cross-service reconstructability spine so Ontogony can prove that consequential decisions are reconstructable across Kanon, Allagma, Conexus, and shared Platform mechanics.

This package is explicitly designed to raise the backend repos above strong-alpha status by adding proof, not feature sprawl.

## Current-state assumptions to verify before coding

Verify against the actual local repos before modifying anything:

1. `kanon-dotnet`
   - Reconstructability classifier core exists.
   - Classifier supports F/P/S/O grades and PASS/WARN/FAIL.
   - Endpoints exist:
     - `POST /ontology/v0/reconstructability/classify`
     - `POST /ontology/v0/reconstructability/classify-batch`
     - `GET /ontology/v0/decision-events/{decisionEventId}/reconstructability`
     - `GET /ontology/v0/reconstructability/by-trace/{traceId}`

2. `allagma-dotnet`
   - `GET /allagma/v0/runs/{runId}/decision-events` exists.
   - `AllagmaDecisionEventProjector` exists.
   - `RunDecisionEventsTests` pass locally.
   - High/critical events have verified post-condition states where backed by domain status.
   - Every fragment ref resolves to an evidence fragment.

3. `conexus-dotnet`
   - Model-call evidence/admin APIs exist.
   - Route decisions, model calls, streaming evidence, provider capability, quota/cost telemetry are available or can be projected from existing journal/telemetry/admin records.
   - Conexus owns model routing and provider decision semantics.

4. `ontogony-platform`
   - Shared mechanics only.
   - Existing packages include tracing/correlation, error envelope, idempotency, headers, HTTP resilience, observability, compatibility gates, and testing utilities.
   - Platform must not absorb Kanon semantics, Allagma run semantics, or Conexus routing semantics.

## Working rules

1. Do not start with Conexus emitters until Allagma → Kanon classifier closure is green.
2. Do not build frontend panels until backend classification is proven.
3. Do not put product semantics into Ontogony.Platform.
4. Do not put model routing into Allagma.
5. Do not put governed execution into Kanon.
6. Do not put provider SDKs or raw provider policy into Allagma.
7. Every cross-service classification fixture must preserve traceId, correlationId, runId when applicable, Kanon decision ids, Conexus modelCallId / routeDecisionId when applicable.
8. High/critical decision events must not classify `FAIL`.
9. Any failure should improve emitted evidence, not weaken classifier rules.
10. Keep production-readiness/security hardening separate unless a missing safety contract directly blocks reconstructability.

## First implementation target

Start with:

```text
pr-specs/PR-001-ALLAGMA-KANON-CLASSIFIER-CLOSURE.md
```

Do not implement Conexus emitters before PR-001 is complete.

## Initial validation commands

Run these before coding and record results in:

```text
docs/_incoming_active/ONTOGONY-RECONSTRUCTABILITY-CLOSURE-OPTION1/IMPLEMENTATION_NOTES.md
```

```powershell
cd C:\dev\kanon-dotnet
dotnet build Kanon.sln -c Release
dotnet test Kanon.sln -c Release --filter "FullyQualifiedName~Reconstructability"

cd C:\dev\allagma-dotnet
dotnet build Allagma.sln -c Release --no-incremental
dotnet test tests/Allagma.Tests/Allagma.Tests.csproj --filter "RunDecisionEvents"
dotnet test tests/Allagma.Tests/Allagma.Tests.csproj --filter "AllagmaV0RouteInventoryTests|FeatureConnectionMatrixAuditTests"

cd C:\dev\conexus-dotnet
dotnet build Conexus.sln -c Release -p:NoWarn=CS1591
dotnet test Conexus.sln -c Release --filter "Category!=ExternalProviderSmoke&Category!=LoadSoak&Category!=PersistenceSmoke&Category!=CapacityBaseline" -p:NoWarn=CS1591

cd C:\dev\ontogony-platform
dotnet build Ontogony.Platform.sln -c Release
dotnet test Ontogony.Platform.sln -c Release
```

If any command fails due to pre-existing state, document it honestly before code changes.
