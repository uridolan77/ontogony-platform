# Allagma Implementation Plan

## Role

Allagma should provide orchestration evidence: run, workflow, operation, gate, actor, status transition, and state-before/state-after fragments.

## Slice A — Decision event fragments for operations

Add a mapper/service that converts existing run-operation records into `DecisionEvent` fragments.

Candidate names, adapt to existing repo conventions:

```text
RunOperationDecisionEventMapper
AllagmaDecisionEventFragmentBuilder
RunOperationReconstructabilityProjection
```

For each operation, expose:

- run id;
- operation id;
- operation type;
- run status before;
- run status after;
- actor id and roles from operator context;
- correlation id;
- trace id;
- authorization source, if operation required a role/human gate;
- state hash/snapshot if available;
- linked Kanon decision id, Conexus model call id, or route decision id if present.

## Slice B — Human gate fragments

Human gates are excellent governance anchors. Capture:

- gate id;
- approver actor id;
- requested action;
- approval/denial result;
- required roles;
- satisfied roles;
- timestamp;
- note/comment if available;
- related run operation;
- state transition caused by the gate.

## Slice C — Explicit not-applicable state

For read-only actions, do not leave post-condition state blank. Emit:

```json
{
  "stateKind": "not_applicable",
  "verificationStatus": "not_applicable"
}
```

This allows `postConditionState` to classify as `S` rather than `O` for non-mutating actions.

## Slice D — Endpoint / projection

Prefer extending existing run detail or operation endpoints. If no suitable endpoint exists, add:

```http
GET /runs/{runId}/decision-events
GET /runs/{runId}/operations/{operationId}/decision-event-fragment
```

## Required tests

Add tests for:

- operation fragment contains actor context;
- human gate approval produces authorization envelope fragment;
- denied gate produces output action `blocked`;
- non-mutating operation emits explicit `not_applicable` post-condition;
- mutating operation missing post-condition yields a diagnostic through the classifier fixture.

## Documentation

Update Allagma docs around run lifecycle and operations to state that critical workflow transitions are reconstructable decision events.
