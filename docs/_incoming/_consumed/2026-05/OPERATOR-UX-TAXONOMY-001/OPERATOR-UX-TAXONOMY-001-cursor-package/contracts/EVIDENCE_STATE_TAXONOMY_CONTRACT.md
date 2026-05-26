# Evidence state taxonomy contract

## States

- `resolved` — all expected links for this identifier/workflow are present.
- `partial` — some expected links are missing or some source attempts failed.
- `unresolved` — no useful evidence graph/session could be built.
- `not_applicable` — the link is not expected for this workflow.
- `unknown` — evidence state has not been computed.

## Important distinction

`not_applicable` is not a failure.

Example:

A direct Conexus chat call should not show missing Kanon decision as unresolved. It should say:

```text
Kanon decision: not applicable — direct Conexus calls do not create governed Kanon decisions.
```

## Partial state requirements

A partial evidence state must list:

- missing relationship;
- expected source system;
- reason code;
- suggested next action;
- source attempt if any.

## UI pattern

```text
Evidence: partial
Resolved: 42 nodes, 43 edges
Missing: route decision detail
Reason: route_decision_not_found
Next action: Open Conexus route-decision explorer or inspect model-call evidence links.
```
