# 08 — Operator evidence spine plan

## Goal

Make the operator console answer: **Why did this run do what it did?**

## Entry identifiers

The evidence spine resolver should accept:

| Identifier | Owner | Primary route |
|---|---|---|
| `runId` | Allagma | `/allagma/v0/runs/{runId}` |
| `traceId` | Platform / all services | service-specific trace discovery |
| `correlationId` | Platform / all services | service-specific correlation discovery |
| `decisionId` | Kanon | `/ontology/v0/decision-records/{decisionId}` |
| `humanGateId` | Kanon | `/ontology/v0/semantic-graph?humanGateId=...` |
| `domainPackId` | Kanon | `/ontology/v0/semantic-graph?domainPackId=...` |
| `modelCallId` | Conexus | `/conexus/v0/model-calls/{id}` or admin evidence routes |
| `routeDecisionId` | Conexus | `/admin/v0/route-decisions/{routeDecisionId}` |
| `evaluationRunId` | Allagma | `/allagma/v0/evaluations/{evaluationRunId}` |

## Resolver graph model

Each node should contain:

```json
{
  "id": "string",
  "kind": "allagma_run | kanon_decision | kanon_provenance | conexus_model_call | route_decision | human_gate | trace | correlation | unresolved",
  "ownerService": "allagma | kanon | conexus | platform | frontend",
  "label": "string",
  "href": "string|null",
  "status": "resolved | unresolved | forbidden | unavailable",
  "summary": {},
  "evidenceRefs": []
}
```

Edges:

```json
{
  "from": "node-id",
  "to": "node-id",
  "kind": "planned_by | governed_by | gated_by | completed_by_model_call | routed_by | derived_from | approved_by | rejected_by | unresolved_reference"
}
```

## Operator screens

### 1. Run Audit Overview

- Run status.
- Timeline.
- Actor.
- Model purpose and alias.
- Decision ids.
- Model call ids.
- Failure summary.

### 2. Semantic Authority Panel

- Planning decision.
- Action decisions.
- Human gate decisions.
- Provenance verify result.
- Semantic graph.
- Replay bundle links.

### 3. Model Gateway Panel

- Model call detail.
- Route decision.
- Provider attempts.
- Fallback chain.
- Usage/cost/quota.
- Evidence bundle.

### 4. Stream Lifecycle Panel

- Started time.
- Chunk count.
- Completion/interruption status.
- Payload retention status.

### 5. Failure Diagnosis Panel

- Normalized error class.
- Downstream service.
- Retryable flag.
- Operator next step.

## Acceptance

- A completed run with model call shows Allagma + Kanon + Conexus nodes.
- A human-gated run shows human gate graph and resolve decision.
- A Conexus fallback run shows provider attempt chain.
- A Kanon assistance review shows `draft_only` assistance and accept/reject review decision.
- Missing evidence is represented explicitly as unresolved, not silently omitted.
