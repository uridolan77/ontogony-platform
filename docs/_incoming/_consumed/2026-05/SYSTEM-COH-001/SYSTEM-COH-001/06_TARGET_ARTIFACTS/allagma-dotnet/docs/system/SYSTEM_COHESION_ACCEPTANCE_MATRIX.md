# SYSTEM-COH-001 acceptance matrix

Machine-readable source: `system-cohesion-acceptance.matrix.json`.

## Status values

| Status | Meaning | Release mode allowed? |
|---|---|---:|
| `PASS` | Scenario proved by command/test/evidence | yes |
| `FAIL` | Scenario failed | no |
| `DEFERRED_WITH_REASON` | Not accepted yet but explicitly deferred | yes, only with reason/owner/nextGate |
| `NOT_APPLICABLE_FOR_ALPHA` | Not part of alpha goal | yes, only with reason |
| `OPTIONAL_LOCAL_ONLY` | Helpful local proof, not release gate | no for required scenarios |

## Required scenarios

| ID | Name | Required | Evidence owner |
|---|---|---:|---|
| `compatibility_lock` | Compatibility lock/matrix validation | yes | Allagma |
| `service_health_readiness` | Service health/readiness | yes | Allagma |
| `governed_run_complete` | Allagma → Kanon → Conexus completed run | yes | Allagma |
| `idempotent_run_retry` | Idempotent retry without duplicate side effects | yes | Allagma |
| `human_gate_pause_resume` | Human gate pause/resume via Kanon | yes | Allagma/Kanon |
| `kanon_conexus_assistance` | Kanon assistance through Conexus with draft-only decision | yes | Kanon/Conexus |
| `conexus_fallback` | Conexus fallback chain evidence | yes | Conexus/Allagma |
| `correlation_chain` | Trace/correlation/run/model/decision linkage | yes | Allagma/Platform |
| `evidence_spine_operator_visibility` | Operator Evidence Spine can inspect graph | yes | Frontend |
| `restart_replay_survival` | Restart/replay survival | yes | Allagma |
| `package_mode_build` | Package-mode build proof | release proof | Allagma/Platform/Clients |
| `real_tools_blocked` | Real tools remain blocked | yes | Allagma |
| `observability_evidence_gate` | Metrics/dashboard evidence | advisory | Allagma/Platform |

## Closeout table

The closeout report must fill the table below with real values.

| ID | Status | Evidence artifact | Command/test | Notes |
|---|---|---|---|---|
| compatibility_lock | TBD | TBD | TBD | TBD |
| service_health_readiness | TBD | TBD | TBD | TBD |
| governed_run_complete | TBD | TBD | TBD | TBD |
| idempotent_run_retry | TBD | TBD | TBD | TBD |
| human_gate_pause_resume | TBD | TBD | TBD | TBD |
| kanon_conexus_assistance | TBD | TBD | TBD | TBD |
| conexus_fallback | TBD | TBD | TBD | TBD |
| correlation_chain | TBD | TBD | TBD | TBD |
| evidence_spine_operator_visibility | TBD | TBD | TBD | TBD |
| restart_replay_survival | TBD | TBD | TBD | TBD |
| package_mode_build | TBD | TBD | TBD | TBD |
| real_tools_blocked | TBD | TBD | TBD | TBD |
| observability_evidence_gate | TBD | TBD | TBD | TBD |
