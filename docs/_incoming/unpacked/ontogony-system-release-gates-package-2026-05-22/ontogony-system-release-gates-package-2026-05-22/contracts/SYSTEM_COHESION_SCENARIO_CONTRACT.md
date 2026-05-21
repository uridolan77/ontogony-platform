# System cohesion scenario contract

## Purpose

Defines the scheduled/manual system cohesion scenario summary for SYSTEM-ALPHA-008.

## Scenario result

Each scenario must emit:

```json
{
  "id": "completed_run",
  "name": "Completed governed run",
  "verdict": "PASS",
  "startedAtUtc": "...",
  "completedAtUtc": "...",
  "durationMs": 1234,
  "artifacts": [],
  "failure": null
}
```

## Scenario ids

```text
completed_run
idempotent_run_start
human_gate_waiting
human_gate_approved
human_gate_denied
kanon_conexus_assistance
conexus_fallback
restart_survival
streaming_smoke
conexus_capacity_baseline
```

## Verdict values

| Value | Meaning |
|---|---|
| `PASS` | Scenario completed and validator accepted output |
| `FAIL` | Scenario ran and failed |
| `SKIPPED` | Scenario intentionally skipped by operator flag |
| `INCONCLUSIVE` | Environment or prerequisite unavailable |

Release mode treats `SKIPPED` and `INCONCLUSIVE` as failing for required scenarios.
