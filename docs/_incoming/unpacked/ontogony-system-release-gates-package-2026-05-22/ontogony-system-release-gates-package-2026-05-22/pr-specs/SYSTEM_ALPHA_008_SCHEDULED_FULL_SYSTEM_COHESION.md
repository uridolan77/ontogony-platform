# SYSTEM-ALPHA-008 — Scheduled full-system cohesion

## Goal

Add a nightly/manual full-system cohesion workflow that continuously proves the Ontogony alpha governed runtime still works across moving repos.

## Owner

Primary: `allagma-dotnet`  
Sub-gate owners: `conexus-dotnet` for capacity baseline, `kanon-dotnet` for semantic gates.

## Deliverables

| Deliverable | Target path |
|---|---|
| Cohesion scenario contract | `allagma-dotnet/docs/system/SYSTEM_COHESION_SCENARIO_CONTRACT.md` |
| Scheduled summary schema | `allagma-dotnet/schemas/system-cohesion-scheduled-summary.schema.json` |
| Orchestrator script | `allagma-dotnet/scripts/release/run-scheduled-system-cohesion.ps1` |
| Workflow | `allagma-dotnet/.github/workflows/system-cohesion-scheduled.yml` |
| Runbook | `allagma-dotnet/docs/operators/RUNBOOK_SCHEDULED_SYSTEM_COHESION.md` |
| Evidence closeout | `allagma-dotnet/docs/evidence/SYSTEM_ALPHA_008_SCHEDULED_COHESION_EVIDENCE.md` |

## Scenario set

Required:

1. Completed governed run.
2. Idempotent run start.
3. Human gate waiting.
4. Human gate approved.
5. Human gate denied.
6. Kanon → Conexus assistance.
7. Conexus fallback.
8. Restart survival.
9. Streaming smoke.
10. Conexus capacity baseline.

## Command shape

```powershell
./scripts/release/run-scheduled-system-cohesion.ps1 `
  -Mode MovingMain `
  -IncludeStreaming `
  -IncludeCapacityBaseline `
  -IncludeRestartSurvival
```

## Modes

| Mode | Use |
|---|---|
| `MovingMain` | nightly drift detection |
| `Locked` | release evidence support |
| `CurrentWorkspace` | local debugging |

## Acceptance criteria

- Manual dispatch runs all required scenarios by default.
- Scheduled run uses clear drift label.
- Capacity baseline is included and summarized.
- Streaming smoke is included and summarized.
- Restart survival is included and summarized.
- Summary contains scenario-level PASS/FAIL/SKIPPED/INCONCLUSIVE.
- Artifacts are uploaded even on failure.

## Boundary

This workflow provides confidence and drift detection. It does not modify runtime lock automatically.
