# Agent interaction

> **Audience:** operator, frontend developer  
> **Applies to:** `allagma-dotnet`, `ontogony-frontend`, `ontogony-platform/docs/operators`  
> **Source of truth:** [`../operators/AGENT_INTERACTION_SPINE_CONTRACT.md`](../operators/AGENT_INTERACTION_SPINE_CONTRACT.md)  
> **Last verified:** 2026-05-25

## What it is

**Agent Interaction** is the operator workbench for inspecting a governed run as an **event stream**: tool intents, Kanon evaluations, human gates, model steps, and MAF workflow checkpoints. It complements Evidence Spine (cross-service graph) with **run-scoped narrative and actions** (resume, retry scopes).

## Backend (Allagma)

- HTTP: `/allagma/v0/runs/{runId}/…` events, workflow, operations
- MAF adapter isolated in `Allagma.MicrosoftAgentFramework` (composition only)
- Consequential tools: Kanon must evaluate before execution (`allagma-dotnet/AGENTS.md`)

Workflow depth docs (incoming/historical): `allagma-dotnet/docs/workflows/`

## Frontend

- Workbench route: Agent Interaction pages under `/allagma/…`
- Signal groups: `ontogony-frontend/src/operatorConsoleSignals.ts` (`buildAgentInteractionSignalGroups`, etc.)
- Canonical frame: [15_UI_CANONICALIZATION_AND_CONSOLE_UX.md](./15_UI_CANONICALIZATION_AND_CONSOLE_UX.md)

## Proof smokes

| Smoke | Command |
| --- | --- |
| First system (tool intent) | `allagma-dotnet/scripts/smoke-first-system.ps1` |
| Governed MAF | `allagma-dotnet/scripts/smoke/run-governed-maf-e2e.ps1` |
| Human gate | `allagma-dotnet/docs/HUMAN_GATES_AND_RESUME.md` |

## Platform contract artifacts

| Artifact | Path |
| --- | --- |
| Agent interaction contract | `ontogony-platform/docs/operators/AGENT_INTERACTION_SPINE_CONTRACT.md` |
| Event matrix | `ontogony-platform/docs/system/agent-interaction-event.matrix.json` |
| AG-UI resolver contract | `ontogony-platform/docs/operators/AG_UI_EVIDENCE_RESOLVER_CONTRACT.md` |

## Tests

```powershell
cd C:\dev\ontogony-frontend
npm run test -- src/allagma
npx playwright test e2e/high-value-operator-pages.spec.ts
```

## Next

- MAF workflow idempotency evidence: `allagma-dotnet/docs/evidence/AGM_MAF_WORKFLOW_IDEMPOTENCY_001.md`
- Debugging: [14_DEBUGGING_PLAYBOOK.md](./14_DEBUGGING_PLAYBOOK.md)
