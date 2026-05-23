# 09 — Test Plan

## Backend unit tests

### Allagma

- start governed fake run stores trace id;
- start governed fake run stores correlation id;
- planning decision id is attached to run;
- model call id is attached to run;
- run events expose downstream ids.

### Kanon

- decision by trace returns the decision created during governed run;
- decision provenance includes trace/correlation;
- missing trace returns clean empty list, not error.

### Conexus

- fake model call records model call;
- fake model call records route decision;
- fake model call records provider attempt;
- emitted routeDecisionId resolves by admin route-decision endpoint;
- model-call evidence-links and route-decision endpoint agree on id.

## Integration tests

### GOV-FAKE-E2E-001.integration

Arrange:

```text
Kanon local/dev
Conexus fake provider
Allagma governed run
Postgres or test persistence as configured
```

Act:

```text
Start run with:
  ontologyVersionId = gaming-core@0.1.0
  modelPurpose = summarize-player-risk
  context.playerId = 123
```

Assert:

```text
run status completed
planningDecisionId present
modelCallId present
traceId present
correlationId present
Kanon by-trace returns planningDecisionId
Conexus model-call detail returns fake provider evidence
Conexus route-decision detail returns 200
Evidence Spine resolves required nodes
```

## Frontend tests

### Evidence Spine

- direct Conexus model call shows Kanon not applicable;
- governed fake run shows Kanon decision required and resolved;
- route decision node appears;
- model call/request/execution IDs are labeled distinctly;
- fixture IDs are marked demo.

### Agent Interaction

- live lookup mode default when backend available;
- fixture replay visibly marked demo;
- latest governed fake run shortcut populates graph.

### Start Run

- request preview includes ontologyVersionId, actor, objective, context, modelPurpose;
- start-and-open-evidence-spine navigates to resolved graph.

## Manual smoke scripts

Use scripts in:

```text
scripts/smoke/run-governed-fake-e2e.ps1
scripts/smoke/run-governed-fake-e2e.sh
```
