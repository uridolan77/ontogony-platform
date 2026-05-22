# Runtime Orchestration Correctness Matrix Contract

## Owner

`allagma-dotnet`

## Purpose

Define the minimum set of orchestration scenarios required to score runtime orchestration above 9.

## Matrix schema

Each row must include:

```text
id
run_path
preconditions
expected_status
expected_events
expected_downstream_calls
expected_failure_class
test_filter
script_or_artifact
release_gate_required
```

## Required rows

| ID | Expected proof |
|---|---|
| `start_completed_non_streaming` | Run reaches Completed with Kanon plan and Conexus model call |
| `start_completed_streaming` | Run reaches Completed with stream lifecycle events |
| `idempotent_start_replay` | Same client key/payload returns prior run response |
| `idempotent_start_conflict` | Same key/different payload returns conflict |
| `kanon_planning_failure` | Run failed with KanonPlanning failure class |
| `kanon_topology_denied` | Run does not continue to model call |
| `tool_registry_blocked` | Unregistered tool blocked before Kanon/execute |
| `tool_policy_denied` | Kanon deny blocks execution |
| `human_gate_waiting` | Run pauses and emits human gate event |
| `human_gate_approved` | Resume completes run |
| `human_gate_denied` | Resume terminates/denies without model continuation |
| `conexus_model_failure` | Run failed or retryable with ConexusModel failure class |
| `conexus_fallback_success` | Fallback route produces successful completion |
| `retry_after_model_failure` | Retry creates expected linkage and no duplicate original effect |
| `cancel_waiting_run` | Waiting run cancels with event |
| `replay_from_audit` | Replay uses stored decisions/responses where available |
| `restart_waiting_run` | Postgres restart preserves waiting run |
| `restart_completed_run` | Postgres restart preserves completed run and events |

## Release gate

Rows marked `release_gate_required=true` must pass before lock promotion.
