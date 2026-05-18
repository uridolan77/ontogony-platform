# 02 — Target Architecture

## Runtime loop

```text
1. Request enters Allagma
2. Allagma classifies task
3. Allagma proposes topology
4. Kanon authorizes topology
5. Allagma executes selected topology
6. Conexus records route/model decision for each model call
7. Allagma records run/audit/eval evidence
8. Eval harness compares against baseline
9. Operator/UI surfaces evidence and scores
```

## Topology modes

Initial topology vocabulary:

| Mode | Meaning | Allowed now? |
|---|---|---|
| `single_workflow` | One governed sequential run path | Yes |
| `centralized_orchestrator` | One orchestrator with validation bottleneck | Yes, minimal |
| `parallel_review` | Multiple independent analyses, centralized final validation | Later |
| `decentralized_research` | Looser exploratory agent collaboration | Later, restricted |
| `hybrid_validation` | Parallel work plus centralized policy/evidence gate | Later |
| `deny_or_human_gate_first` | No autonomous execution before human/policy check | Yes, for high-risk |

## Task classifications

Initial vocabulary:

| Classification | Meaning |
|---|---|
| `sequential` | Later steps depend on previous outputs |
| `parallelizable` | Subtasks can run independently |
| `exploratory` | Open-ended research/analysis |
| `tool_heavy` | Tool intents likely dominate |
| `high_risk` | Consequential actions, irreversible changes, or regulated workflows |
| `human_gate_likely` | Human approval likely required |
| `model_only` | No tool or policy action expected |
| `policy_only` | Decision/action evaluation without model need |

## Decision ownership

| Decision | Owner |
|---|---|
| classify task | Allagma |
| select candidate topology | Allagma |
| authorize topology under ontology/policy | Kanon |
| choose model/provider route | Conexus |
| record mechanics | Ontogony.Platform |
| evaluate outcome | Allagma eval harness using neutral Ontogony contracts |
| display and operate | future UI / operator docs |

## Evidence graph

```text
AllagmaRun
  ├── TaskClassification
  ├── TopologySelection
  ├── KanonPlanningDecision
  ├── KanonTopologyPolicyDecision
  ├── ToolIntentEvaluationDecision(s)
  ├── HumanGateDecision(s)
  ├── ConexusRouteDecision(s)
  ├── ConexusModelCall(s)
  ├── ExecutionEvents
  ├── ReplayBundles
  └── EvaluationRun
        ├── BaselineComparison
        ├── MetricScores
        └── Verdict
```

## Defaulting rule

```text
If classification confidence is low, select `single_workflow`.
If risk is high, require Kanon topology authorization before execution.
If topology authorization fails, fail closed or pause for human gate.
If eval evidence does not show improvement over baseline, do not promote the topology.
```
