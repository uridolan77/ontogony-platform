# Cursor unpack prompt — SYSTEM-COH-001

You are implementing **SYSTEM-COH-001 — Ontogony Runtime Cohesion Baseline** across the Ontogony repos.

## Operating mode

Be conservative, evidence-driven, and current-state aware. Before editing anything:

1. Inspect these repos if present as sibling checkouts:
   - `ontogony-platform`
   - `conexus-dotnet`
   - `kanon-dotnet`
   - `allagma-dotnet`
   - `ontogony-frontend`
   - `ontogony-ui`
2. Identify existing SYSTEM-COH / runtime-lock / evidence-spine / trace-context / package-mode / observability artifacts.
3. Do not duplicate existing files. Update, consolidate, validate, or add missing pieces only.
4. Preserve ownership boundaries:
   - Allagma coordinates runtime and owns canonical system matrices/lock evidence.
   - Kanon owns semantic authority, decisions, policy, human gates, provenance, semantic graph.
   - Conexus owns model gateway, aliases, provider routing, fallback, model-call telemetry.
   - Platform owns neutral schemas/contracts/mechanics.
   - Frontend/UI own operator inspection and shared UI components.
5. Do not enable real tool execution. This package may improve the trust gate, but execution remains blocked.

## Current-state starting assumptions to verify

The latest repository state may already include:

- `allagma-dotnet/docs/system/SYSTEM_COMPATIBILITY_MATRIX.md`
- `allagma-dotnet/docs/system/SYSTEM_TRACE_CONTEXT_MATRIX.md`
- `allagma-dotnet/docs/system/ONTOGONY_RUNTIME_LOCK.md`
- `allagma-dotnet/docs/system/ontogony-runtime.lock.json`
- `allagma-dotnet/scripts/lib/system-cohesion-e2e.ps1`
- model purposes with `ConexusModelAlias` in Allagma config/docs
- streaming model-purpose support
- package-mode build path
- enriched Kanon Evidence Spine paths in frontend/platform/Kanon docs

If these exist, do not recreate them. Instead make SYSTEM-COH-001 a closure/consolidation baseline.

## Goal

Produce a coherent, testable baseline that answers:

> Which repo refs, route surfaces, auth modes, package versions, environment keys, context headers, error shapes, smoke scenarios, evidence artifacts, and known deferrals define the current alpha governed runtime?

## Required edits / additions

### Allagma-owned canonical system layer

Update or create:

- `docs/system/SYSTEM_COHESION_BASELINE.md`
- `docs/system/SYSTEM_COHESION_ACCEPTANCE_MATRIX.md`
- `docs/system/system-cohesion-acceptance.matrix.json`
- `docs/system/SYSTEM_ERROR_COMPATIBILITY_MATRIX.md`
- `docs/system/SYSTEM_OBSERVABILITY_EVIDENCE_GATE.md`
- `scripts/validate-system-coh-001.ps1`
- `scripts/run-system-coh-001-acceptance.ps1`

Do not conflict with existing `SYSTEM_COMPATIBILITY_MATRIX.md`; link to it and summarize closure state.

### Platform-owned schemas/contracts

Update or create:

- `docs/contracts/CROSS_SERVICE_CONTEXT_PROPAGATION_V1.md`
- `docs/schemas/ontogony-system-cohesion-summary-v1.schema.json`
- `docs/schemas/ontogony-cross-service-error-classification-v1.schema.json`

If equivalent schemas already exist, add compatibility notes and validation rather than creating duplicates.

### Service alignment docs

Update or create:

- `kanon-dotnet/docs/integrations/SYSTEM_COHESION_KANON_ALIGNMENT.md`
- `conexus-dotnet/docs/integrations/SYSTEM_COHESION_CONEXUS_ALIGNMENT.md`
- `ontogony-frontend/docs/system/SYSTEM_COHESION_OPERATOR_CONSOLE.md`

### Tests / validators

Add tests or extend existing tests to prove:

- runtime lock/matrix references are internally consistent;
- required files listed by `docs/system/README.md` exist;
- system acceptance matrix JSON is schema-valid;
- context propagation docs match actual helper/header constants;
- error compatibility matrix lists all known downstream failure families;
- real tool execution remains blocked by conformance tests;
- optional smoke flags are classified correctly and not silently skipped in release mode.

## Acceptance command target

At minimum provide a documented command sequence like:

```powershell
# Allagma repo
./scripts/validate-runtime-lock.ps1 -ReleaseMode
./scripts/validate-system-coh-001.ps1 -DevRoot C:\dev -ReleaseMode
./scripts/run-system-coh-001-acceptance.ps1 -DevRoot C:\dev -IncludeKanonAssistance -IncludeConexusFallback -IncludeCorrelationChain -RequireEvidence

dotnet test Allagma.sln -c Release
dotnet test tests/Allagma.ArchitectureConformance.Tests -c Release
```

Add frontend/platform commands when operator evidence/schema validation is included.

## Definition of done

SYSTEM-COH-001 is accepted only when the final report classifies all scenarios as:

- `PASS`
- `DEFERRED_WITH_REASON`
- `NOT_APPLICABLE_FOR_ALPHA`

No scenario may be silently omitted.

Write final closeout to:

- `allagma-dotnet/docs/evidence/SYSTEM_COH_001_CLOSEOUT.md`
- machine summary under `artifacts/system-coh-001/<timestamp>/summary.json`

Include exact commands run, repo refs, failed/skipped scenarios, and known deferrals.
