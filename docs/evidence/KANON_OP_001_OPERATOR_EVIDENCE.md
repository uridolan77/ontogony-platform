# KANON-OP-001 — Operator topology decision evidence

**Recorded at (UTC):** 2026-05-19  
**Verdict:** **PASS** (documentation + script contract)  
**Statement:** Post-Docker-local hardening operator visibility for Kanon topology authorization; **not production readiness**.

## Scope

Document how operators trace Allagma runs → Kanon topology authorization decisions in Docker-local flows. Add `inspect-kanon-topology-evidence.ps1` and report validator. No service `src/` changes unless an evidence gap is proven (none required for this slice).

## Delivered

```text
docker/local-working-system/scripts/inspect-kanon-topology-evidence.ps1
docker/local-working-system/scripts/validate-kanon-topology-evidence-report.ps1
docker/local-working-system/README.md
docs/environments/compose-to-docker-closeout-package-v2/post-closeout-hardening/KANON-OP-001.md
docs/environments/compose-to-docker-closeout-package-v2/01_PR_SEQUENCE.md
docs/environments/compose-to-docker-closeout-package-v2/04_STATUS_BOARD.md
docs/releases/FIRST_DOCKER_LOCAL_WORKING_SYSTEM_NEXT_STEPS.md
docs/evidence/KANON_OP_001_OPERATOR_EVIDENCE.md
kanon-dotnet/docs/operators/TOPOLOGY_DECISION_EVIDENCE.md
kanon-dotnet/docs/development/DOCKER_LOCAL.md
```

## Operator questions answered

| Question | Answer |
| --- | --- |
| Why is baseline `topologyAuthorizationDecisionId` null? | Baseline uses `single_workflow` with `requiresKanonAuthorization=false`. Allagma does not call Kanon `POST /ontology/v0/execution-topologies/evaluate` for that path. |
| Why does subject override have a Kanon decision? | `context.topologyOverride=centralized_orchestrator` selects a topology that requires Kanon authorization. Allagma records `TopologyAuthorizationRequested` / `Completed` and stores the Kanon `decisionId` on the run spine. |
| How do I fetch the Kanon record? | `GET /ontology/v0/decision-records/{topologyAuthorizationDecisionId}` with dev service bearer + `ProvenanceReader` role headers (see script). |
| How does this relate to planning? | `planningDecisionId` is a separate Kanon decision from run planning; topology authorization is evaluated **before** planning when required. |

## Report fields (guided flow)

`docker-guided-main-flow-report.json` includes:

| Field | Role |
| --- | --- |
| `baselineRunId` | Low-risk baseline run |
| `subjectRunId` | Topology override subject run |
| `subjectTopologyAuthorizationDecisionId` | Kanon decision id for subject authorization |
| `baselineRouteDecisionId` / `subjectRouteDecisionId` | Conexus routing evidence (separate concern) |

`planningDecisionId` is **not** in the guided report; the inspect script fetches it from `GET /allagma/v0/runs/{runId}`.

## Commands (operator)

```powershell
cd C:\dev\ontogony-platform

# Prerequisite: stack + guided flow PASS
.\docker\local-working-system\scripts\wait-local-working-system.ps1 -SkipFrontend
.\docker\local-working-system\scripts\run-docker-guided-main-flow.ps1 -SkipFrontend

# Inspect topology evidence (requires live Allagma + Kanon on 5083 / 5081)
.\docker\local-working-system\scripts\inspect-kanon-topology-evidence.ps1
.\docker\local-working-system\scripts\validate-kanon-topology-evidence-report.ps1

# Confirm report contains no raw secrets
Select-String -Path .\docker\local-working-system\artifacts\kanon-op-001-topology-evidence-report.json `
  -Pattern 'allagma-dev-service-token|kanon-dev-service-token|_local_pw|cx-dev-key'
# Expected: no matches
```

## Validation checks (repo diff)

| Check | Result |
| --- | --- |
| No Allagma/Kanon `src/` changes | **yes** |
| No workflow changes | **yes** |
| No secrets committed | **yes** |
| JSON report local only (not committed) | **yes** |
| Script rejects raw secret patterns in output | **yes** |
| Production readiness claimed | **no** |

## Live validation (2026-05-19)

Stack: `local-working-system-kanon-api-1`, `local-working-system-allagma-api-1` healthy on host ports 5081 / 5083.

| Check | Result |
| --- | --- |
| `inspect-kanon-topology-evidence.ps1` exit code | **0** |
| `validate-kanon-topology-evidence-report.ps1` exit code | **0** |
| Report verdict | **PASS** |
| Baseline `topologyAuthorizationDecisionId` | **null** |
| Subject `topologyAuthorizationDecisionId` | `decision_c1bea0d2f88044deabc2c42015f0d8b9` |
| Kanon `decisionType` | `topology_policy_evaluation` |
| Kanon `outcome.status` | `allow` |
| Raw secrets in report | **none** (grep confirmed) |

Sample linkage (IDs from local run; not committed in repo):

```text
baselineRunId:     run_926c1317fbe94ce9ac2fd7832840552e
subjectRunId:      run_33c73fabdbba46e980a0b9315e6a14c8
subjectTopologyAuthorizationDecisionId: decision_c1bea0d2f88044deabc2c42015f0d8b9
```

## Live stack note

Re-run inspect after stack reset or new guided flow to refresh `kanon-op-001-topology-evidence-report.json`.

## Cross-references

- Allagma E2E: `allagma-dotnet/docs/evidence/AGM-TOPO-003_TOPOLOGY_AUTHORIZATION_E2E_EVIDENCE.md`
- Kanon operator doc: `kanon-dotnet/docs/operators/TOPOLOGY_DECISION_EVIDENCE.md`
- Frontend walkthrough: `ontogony-frontend/docs/development/DOCKER_LOCAL_OPERATOR_WALKTHROUGH.md`
