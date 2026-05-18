# PR Spec — KANON-TOPO-001 — Topology Policy Evaluation

## Repo

`kanon-dotnet`

## Goal

Let Kanon authorize an execution topology under ontology/action/policy context.

## Add endpoint

```text
POST /ontology/v0/execution-topologies/evaluate
```

## Request

```json
{
  "ontologyVersionId": "gaming-core@0.1.0",
  "actor": {},
  "objectiveHash": "sha256:...",
  "taskClassification": "high_risk",
  "requestedTopologyMode": "centralized_orchestrator",
  "riskLevel": "high",
  "toolScope": ["Allagma.Workflow:ApproveWithdrawal"],
  "contextRefs": {}
}
```

## Response

```json
{
  "effect": "allow",
  "allowedTopologyModes": ["centralized_orchestrator"],
  "requiredValidationMode": "centralized_final_validation",
  "humanGate": { "required": false },
  "reason": "Topology allowed by ontology policy.",
  "decisionRecord": {}
}
```

## Policy rules v0

- Unknown topology -> deny.
- High-risk + decentralized topology -> deny.
- Consequential action -> require centralized validation.
- Human-gated action -> return `human_gate`.
- Low-risk + single_workflow -> allow.

## Decision record

Emit:

```text
DecisionRecordTypes.TopologyPolicyEvaluation
```

or equivalent additive constant.

## Tests

- low-risk single workflow allowed.
- high-risk decentralized denied.
- human gate topology returns human_gate.
- decision record emitted.
- provenance/verify still works.
- OpenAPI baseline updated.

## Acceptance

- no Allagma implementation dependency.
- no Conexus dependency.
