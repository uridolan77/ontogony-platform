# KANON-OP-002 — Kanon operational diagnostics (Docker-local topology)

## Purpose

Add operator diagnostics and troubleshooting for Docker-local Kanon topology authorization evidence after `KANON-OP-001`.

Operators must distinguish:

- baseline null `topologyAuthorizationDecisionId` **by design**
- Allagma never calling Kanon (`requiresKanonAuthorization=false`)
- subject missing authorization ID when auth is required
- Kanon decision-record lookup failure (404, auth, unavailable)
- Kanon deny / human_gate outcomes

## Timing

Post-`KANON-OP-001`. Does not block the closed Docker-local milestone.

## Boundary

- Diagnostics and documentation only.
- No new topology policy behavior.
- No real provider keys, no real external execution.
- **Not production readiness.**

## Repos

| Repo | Deliverables |
| --- | --- |
| `ontogony-platform` | `_docker-local-env.ps1`, `diagnose-kanon-topology-ops.ps1`, validator, README, spec, evidence; env-aware `inspect-kanon-topology-evidence.ps1` |
| `kanon-dotnet` | `docs/operators/TOPOLOGY_DIAGNOSTICS.md`, links from existing operator docs |

## Token/config handling

Scripts read from `docker/local-working-system/.env` or `.env.example`:

- `ALLAGMA_SERVICE_TOKEN`, `KANON_SERVICE_TOKEN`
- `ALLAGMA_HOST_PORT`, `KANON_HOST_PORT`

Parameter overrides remain. Reports redact values and reject known secret patterns (including tokens loaded from env).

## Acceptance criteria

| # | Topic | Expected |
| --- | --- | --- |
| 1 | Baseline null semantics | `BASELINE_NULL_BY_DESIGN` when `requiresKanonAuthorization=false` |
| 2 | Subject happy path | `SUBJECT_AUTH_REQUIRED` + `KANON_ALLOW` when override authorized |
| 3 | No-call vs missing ID | `ALLAGMA_NO_KANON_CALL` vs `SUBJECT_MISSING_AUTH_ID` |
| 4 | Kanon lookup failures | `KANON_DECISION_NOT_FOUND`, `KANON_AUTH_FAILURE`, `KANON_UNAVAILABLE` |
| 5 | Policy outcomes | `KANON_DENY`, `KANON_HUMAN_GATE` surfaced without new policy code |
| 6 | Report safety | No raw service tokens in JSON artifacts |
| 7 | No unjustified runtime changes | Docs + scripts only |

## Operator verification

```powershell
cd C:\dev\ontogony-platform

# Happy path (after guided flow)
.\docker\local-working-system\scripts\inspect-kanon-topology-evidence.ps1
.\docker\local-working-system\scripts\validate-kanon-topology-evidence-report.ps1
.\docker\local-working-system\scripts\diagnose-kanon-topology-ops.ps1
.\docker\local-working-system\scripts\validate-kanon-topology-diagnostics-report.ps1
```

## Evidence

| Repo | Path |
| --- | --- |
| `ontogony-platform` | `docs/evidence/KANON_OP_002_OPERATIONAL_DIAGNOSTICS_EVIDENCE.md` |
| `kanon-dotnet` | `docs/operators/TOPOLOGY_DIAGNOSTICS.md` |

## Follow-up

| PR | Focus |
| --- | --- |
| `TRACE-CONTRACT-001` | Cross-service trace/correlation contract |
