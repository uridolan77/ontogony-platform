# Source Attempt Contract

## Purpose

Source attempts are the operator audit trail for Evidence Spine resolution. They must be exact, structured, and non-generic.

## Shape

```ts
export type EvidenceSourceAttempt = {
  id: string;
  system: 'allagma' | 'kanon' | 'conexus' | 'platform' | 'frontend';
  endpoint: string;
  method: 'GET' | 'POST' | 'PUT' | 'PATCH' | 'DELETE' | 'LOCAL';
  identifierKind?: EvidenceIdentifierKind;
  identifierValue?: string;
  status: 'success' | 'error' | 'skipped' | 'not_applicable';
  httpStatus?: number;
  latencyMs?: number;
  reasonCode?: EvidenceMissingReasonCode;
  message?: string;
  retryable?: boolean;
  dataSource: EvidenceDataSource;
  startedAt?: string;
  completedAt?: string;
};
```

## Display rules

Each attempt should render:

```text
conexus · error · GET /admin/v0/route-decisions/{routeDecisionId}
routeDecisionId: rd-... · 10ms · backend_missing · 404
Route decision ID was emitted by model-call evidence links, but no route-decision detail record exists.
```

Do not render:

```text
An unexpected error occurred.
```

unless it is a secondary raw diagnostic hidden under details.

## Mapping rules

| Raw condition | Source attempt status | reasonCode |
|---|---|---|
| 200 + valid body | success | none |
| 200 + invalid body | error | contract_mismatch |
| 401/403 | error | authorization_failed |
| 404 for emitted routeDecisionId | error | backend_missing |
| 404 for optional lookup | error or skipped | not_resolved / not_recorded |
| endpoint unavailable | error | upstream_unavailable |
| timeout | error | timeout |
| direct Conexus root skipping Kanon | not_applicable | not_applicable |
| fixture fallback used | success/skipped | fixture_only |

## Contract route names

Use actual API templates:

```text
/allagma/v0/runs/{runId}
/allagma/v0/runs/{runId}/events
/allagma/v0/runs/{runId}/audit
/allagma/v0/runs/{runId}/evaluations
/allagma/v0/evaluations/{evaluationRunId}/evidence
/allagma/v0/evaluations/baseline-comparisons/{comparisonId}
/admin/v0/model-calls/{modelCallId}
/admin/v0/model-calls/{modelCallId}/evidence-links
/admin/v0/route-decisions/{routeDecisionId}
/admin/v0/diagnostics/execution-runs/by-request-id/{requestId}
/ontology/v0/decision-records/{decisionId}
/ontology/v0/decision-records/{decisionId}/provenance
/ontology/v0/semantic-graph
```
