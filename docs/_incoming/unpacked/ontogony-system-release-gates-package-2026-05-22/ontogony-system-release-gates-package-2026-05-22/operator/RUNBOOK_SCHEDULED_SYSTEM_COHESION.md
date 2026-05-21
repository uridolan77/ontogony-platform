# Runbook — scheduled full-system cohesion

## Manual dispatch

Use the GitHub Actions workflow:

```text
system-cohesion-scheduled.yml
```

Inputs:

| Input | Default |
|---|---|
| `mode` | `MovingMain` |
| `include_streaming` | `true` |
| `include_capacity_baseline` | `true` |
| `include_restart_survival` | `true` |

## Local command

```powershell
cd C:\dev\allagma-dotnet
./scripts/release/run-scheduled-system-cohesion.ps1 `
  -Mode MovingMain `
  -IncludeStreaming `
  -IncludeCapacityBaseline `
  -IncludeRestartSurvival
```

## Expected artifacts

```text
artifacts/system-cohesion-scheduled/<timestamp>/
  system-cohesion-scheduled-summary.json
  system-cohesion-scheduled-summary.md
  scenarios/
  capacity/
  restart/
  streaming/
```

## Interpreting scheduled failures

Scheduled moving-main failures do not invalidate a prior release lock. They indicate drift or regression after the lock.

Recommended triage order:

1. identify failed scenario id;
2. inspect service logs;
3. check whether lock commits differ from moving-main heads;
4. reproduce in `CurrentWorkspace`;
5. classify as code regression, test flake, environment failure, or expected drift.
