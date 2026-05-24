# Cursor unpack prompt — REPLAY-RUNTIME-001

You are implementing `REPLAY-RUNTIME-001` across the Ontogony repos.

Read this package completely before changing code. Start with `REPLAY-RUNTIME-001.md`, then `01_CURRENT_STATE_AUDIT.md`, then the numbered implementation plans.

## Mission

Make replay/simulation a real cross-service operator workflow across Ontogony Platform, Allagma, Kanon, Conexus, and the unified frontend.

This is not a replay button. The goal is to define a coherent replay model, clarify exact replay vs simulation vs reconstruction, connect results to Evidence Spine, and give operators a trustworthy replay workbench without cluttering the console.

## Working directories

Use these sibling repos:

```text
C:\dev\ontogony-platform
C:\devllagma-dotnet
C:\dev\kanon-dotnet
C:\dev\conexus-dotnet
C:\dev\ontogony-frontend
C:\dev\ontogony-ui
```

Before changing anything, inspect the current local working tree, because this package was created from current GitHub refs and the local repos may include unpushed changes.

## First commands

```powershell
cd C:\dev\ontogony-platform; git status --short; git rev-parse HEAD
cd C:\devllagma-dotnet; git status --short; git rev-parse HEAD
cd C:\dev\kanon-dotnet; git status --short; git rev-parse HEAD
cd C:\dev\conexus-dotnet; git status --short; git rev-parse HEAD
cd C:\dev\ontogony-frontend; git status --short; git rev-parse HEAD
cd C:\dev\ontogony-ui; git status --short; git rev-parse HEAD
```

If local state differs from the refs in `README.md`, keep the package intent but trust local code over package assumptions.

## Implementation rules

1. Preserve service boundaries:
   - Allagma governs execution and owns replay orchestration records.
   - Kanon owns semantic decisions, semantic plans, provenance, and replay bundles.
   - Conexus owns model routing, model calls, provider attempts, usage/cost, and provider safety posture.
   - Platform owns shared mechanics/contracts/schemas/evidence taxonomy.
   - Frontend presents the operator workflow.
   - UI library provides canonical primitives.

2. Never silently execute real providers or real external tools.

3. Every replay result must declare one mode:
   - `exact_replay`
   - `deterministic_simulation`
   - `dry_run`
   - `reconstructed`
   - `evidence_only`
   - `unavailable`

4. Every eligibility result must explain why a target is exact, simulated, dry-run, reconstructed, evidence-only, or unavailable.

5. Keep existing Evidence Spine, Agent Interaction, governed fake E2E, runtime lock, and contract discipline intact.

6. Any route or DTO change must update:
   - backend route inventory;
   - backend OpenAPI snapshot;
   - frontend OpenAPI snapshot;
   - generated TypeScript schema;
   - service client;
   - route-workflow catalog;
   - `API_CLIENT_ROUTE_USAGE.json`;
   - manual DTO shim registry if needed;
   - service route parity checks;
   - `contracts:discipline`.

7. Do not add a dense page-local workbench. Use canonical UI primitives.

## Recommended stage order

1. Stage 1 — shared contracts and schemas only.
2. Stage 2 — Allagma replay record and normalized replay trigger.
3. Stage 3 — Kanon decision/provenance replay bundle integration.
4. Stage 4 — Conexus model-call/route-decision dry-run integration.
5. Stage 5 — cross-service replay evidence bundle.
6. Stage 6 — governed fake replay smoke evidence.

Stop after each stage and run the relevant tests from `11_TEST_AND_ACCEPTANCE_PLAN.md`.
