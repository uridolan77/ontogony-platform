# Decision Reconstruction API V0

This file describes candidate API surfaces. Implement using each repo's existing route conventions. Do not add these exact routes if they conflict with current inventories.

## Kanon preferred read APIs

Kanon is the natural home for classification and semantic governance.

```http
GET /ontology/v0/decision-events/{decisionEventId}/reconstructability
GET /ontology/v0/decision-events/{decisionEventId}/reconstruction
GET /ontology/v0/decision-events/{decisionEventId}/missing-evidence
```

Candidate response:

```ts
DecisionReconstructionResponseV0 {
  decisionEvent: DecisionEventV0;
  reconstructabilityReport: ReconstructabilityReportV0;
  evidenceGraph?: EvidenceGraphSummary;
}
```

## Cross-service lookup APIs

Add convenience lookup only if consistent with existing backend patterns.

```http
GET /ontology/v0/reconstructability/by-run/{runId}
GET /ontology/v0/reconstructability/by-trace/{traceId}
GET /ontology/v0/reconstructability/by-model-call/{modelCallId}
GET /ontology/v0/reconstructability/by-decision/{kanonDecisionId}
```

## Allagma exposure

Allagma may expose reconstruction links from run/operation endpoints, or directly proxy assembled events.

```http
GET /runs/{runId}/decision-events
GET /runs/{runId}/operations/{operationId}/decision-event
```

If Allagma already has an operations or evidence endpoint, extend that instead of adding duplicates.

## Conexus exposure

Conexus should expose fragment-level details for routing/model decisions.

```http
GET /admin/model-calls/{modelCallId}/decision-event-fragment
GET /admin/route-decisions/{routeDecisionId}/decision-event-fragment
```

If Conexus already has model-call or route-decision details, add fields to those DTOs rather than adding new endpoints.

## Frontend data fetching

The frontend should prefer a single report endpoint when possible:

```ts
getDecisionReconstruction({ decisionEventId })
getDecisionReconstructionByRun({ runId })
getDecisionReconstructionByTrace({ traceId })
getDecisionReconstructionByModelCall({ modelCallId })
```

## Error semantics

- `404` — no such decision event or source id.
- `422` — source exists but cannot be assembled into a decision event.
- `200` with `FAIL` — event exists and classification ran, but governance failed.
- `200` with `WARN` — event reconstructable enough for non-critical workflows but missing non-blocking evidence.
