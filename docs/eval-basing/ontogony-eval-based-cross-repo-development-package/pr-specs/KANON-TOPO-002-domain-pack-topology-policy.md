# PR Spec — KANON-TOPO-002 — Domain-Pack Topology Policy

## Repo

`kanon-dotnet`

## Goal

Allow domain packs to define topology policy.

## Example

```json
{
  "topologyPolicies": [
    {
      "policyId": "gaming-core.topology.approve-withdrawal.v0",
      "actionName": "ApproveWithdrawal",
      "allowedTopologies": ["centralized_orchestrator", "deny_or_human_gate_first"],
      "forbiddenTopologies": ["decentralized_research", "parallel_review"],
      "requiresFinalValidation": true,
      "requiresHumanGate": true
    }
  ]
}
```

## Tests

- domain pack validation rejects unknown topology names.
- lifecycle governance includes topology policy hash.
- topology policy is visible in active pack detail.
- action-policy and topology-policy mismatch is detected.

## Acceptance

- migration docs updated if persistence changes.
- domain pack examples updated.
