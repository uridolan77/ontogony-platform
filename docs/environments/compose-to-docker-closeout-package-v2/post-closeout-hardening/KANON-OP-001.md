# KANON-OP-001 — Operator-readable Kanon topology/decision evidence

## Purpose

Make Kanon topology authorization decision evidence **operator-readable** for Docker-local flows after `CONEXUS-PERSIST-003`.

Operators must be able to trace:

```text
Allagma subject run
→ topology override (context.topologyOverride)
→ Kanon topology policy evaluation (decision record)
→ authorization result on the run spine
→ frontend / run audit evidence
```

## Timing

Post-`ENV-DOCKER-CLOSEOUT-001` and post-`CONEXUS-PERSIST-003`. Does not block the closed Docker-local milestone.

## Boundary

- Operator visibility and documentation **first**.
- No new topology policy behavior unless a direct evidence gap requires a small fix (not expected for this slice).
- No real provider keys, no real external execution.
- **Not production readiness.**

## Repos

| Repo | Deliverables |
| --- | --- |
| `ontogony-platform` | `docker/local-working-system/README.md`, `inspect-kanon-topology-evidence.ps1`, spec, next-steps, evidence |
| `kanon-dotnet` | `docs/operators/TOPOLOGY_DECISION_EVIDENCE.md`, `docs/development/DOCKER_LOCAL.md` link |

## What reports already contain

| Artifact | Key fields |
| --- | --- |
| `env-seed-001-report.json` | `runs.baselineRunId`, `runs.subjectRunId`, `topology.baselineTopologyAuthorizationDecisionId` (null), `topology.subjectTopologyAuthorizationDecisionId`, `topology.*SelectedTopology` |
| `docker-guided-main-flow-report.json` | `baselineRunId`, `subjectRunId`, `subjectTopologyAuthorizationDecisionId`, eval/route IDs, restart proof |

`planningDecisionId` is **not** in machine reports; fetch from Allagma run detail or audit bundle when needed.

## Where decisions are produced

| Layer | Location |
| --- | --- |
| Gate (Allagma) | `TopologyAuthorizationGate.ShouldRequestKanonAuthorization` — requires `topologySelection.requiresKanonAuthorization` and no `topologyAuthorizationExempt` |
| Coordinator (Allagma) | `KanonTopologyAuthorizationCoordinator` — events `TopologyAuthorizationRequested` / `Completed`, sets `topologyAuthorizationDecisionId` |
| Policy (Kanon) | `POST /ontology/v0/execution-topologies/evaluate` → persisted `DecisionRecord` with `decisionType = topology_policy_evaluation` |
| Operator read (Kanon) | `GET /ontology/v0/decision-records/{decisionId}` (+ optional `/provenance`) |
| Run spine (Allagma) | `GET /allagma/v0/runs/{runId}/events?includeTopologySummary=true` → `topology.topologyAuthorizationDecisionId` |

Cross-repo E2E reference: `allagma-dotnet/docs/evidence/AGM-TOPO-003_TOPOLOGY_AUTHORIZATION_E2E_EVIDENCE.md`.

## Acceptance criteria

| # | Topic | Expected understanding |
| --- | --- | --- |
| 1 | Baseline null `topologyAuthorizationDecisionId` | `single_workflow` + low-risk path: `requiresKanonAuthorization=false`; Kanon topology evaluate **not invoked** |
| 2 | Subject non-null ID | `topologyOverride=centralized_orchestrator` → selection requires Kanon authorization → decision record created |
| 3 | Trace subject run → Kanon | Use `subjectTopologyAuthorizationDecisionId` from guided report → `GET /ontology/v0/decision-records/{id}` on Kanon **5081** |
| 4 | Link planning | `planningDecisionId` on run start response / audit bundle; separate from topology authorization decision |
| 5 | Frontend | Walkthrough URLs use `subjectRunId` from guided report; topology ID visible in run topology summary when present |
| 6 | Script | `inspect-kanon-topology-evidence.ps1` writes redacted JSON; no raw dev tokens/passwords |
| 7 | No unjustified runtime changes | Docs + script only unless evidence gap proven |

## Operator verification

```powershell
cd C:\dev\ontogony-platform

# Prerequisite: guided flow PASS (produces report with subjectTopologyAuthorizationDecisionId)
.\docker\local-working-system\scripts\run-docker-guided-main-flow.ps1 -SkipFrontend
.\docker\local-working-system\scripts\validate-docker-guided-main-flow.ps1

# Inspect + redacted report
.\docker\local-working-system\scripts\inspect-kanon-topology-evidence.ps1
.\docker\local-working-system\scripts\validate-kanon-topology-evidence-report.ps1

# Manual spot-check (dev bearer tokens — do not commit)
$report = Get-Content .\docker\local-working-system\artifacts\docker-guided-main-flow-report.json | ConvertFrom-Json
$kanonHeaders = @{
  Authorization = "Bearer kanon-dev-service-token-change-in-production"
  "X-Ontogony-Actor-Id" = "operator"
  "X-Ontogony-Actor-Type" = "service"
  "X-Ontogony-Roles" = "ProvenanceReader"
}
Invoke-RestMethod -Uri "http://localhost:5081/ontology/v0/decision-records/$($report.subjectTopologyAuthorizationDecisionId)" -Headers $kanonHeaders
```

## Evidence

| Repo | Path |
| --- | --- |
| `ontogony-platform` | `docs/evidence/KANON_OP_001_OPERATOR_EVIDENCE.md` |
| `kanon-dotnet` | `docs/operators/TOPOLOGY_DECISION_EVIDENCE.md` |

## Follow-up

| PR | Focus |
| --- | --- |
| `KANON-OP-002` | Kanon operational diagnostics |
| `TRACE-CONTRACT-001` | Cross-service trace/correlation contract |
