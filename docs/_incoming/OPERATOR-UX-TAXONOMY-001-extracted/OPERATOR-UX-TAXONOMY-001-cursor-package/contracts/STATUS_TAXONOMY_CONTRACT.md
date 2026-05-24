# Status taxonomy contract

## Principle

Status is multi-dimensional. Never collapse these dimensions into one ambiguous label.

## Dimensions

### Connectivity

Can the frontend reach the service/API endpoint?

Allowed values:

- `live` — endpoint reachable and returned a response.
- `degraded` — endpoint reachable but response is incomplete, slow, warning, or partially failed.
- `offline` — endpoint unreachable or explicitly down.
- `unknown` — connectivity has not been checked or cannot be determined.

### Readiness

Can the service perform the requested workflow now?

Allowed values:

- `ready`
- `not_ready`
- `unknown`

Readiness must include dependency reasons where possible.

### Contract health

Did the payload match the expected UI/backend contract?

Allowed values:

- `valid`
- `warning`
- `invalid`
- `unknown`

### Operator usability

Can the operator safely use this surface for the intended task?

Allowed values:

- `usable`
- `degraded`
- `blocked`
- `unknown`

### Evidence completeness

How complete is the evidence graph/session/record?

Allowed values:

- `resolved`
- `partial`
- `unresolved`
- `not_applicable`
- `unknown`

### Data source

Where did the displayed data come from?

Allowed values:

- `live`
- `live_with_fallback`
- `fixture`
- `generated`
- `imported`
- `mock`
- `unknown`

### Authority

What authority level does the displayed result have?

Allowed values:

- `authoritative`
- `advisory`
- `demo`
- `inferred`
- `historical`
- `unknown`

### Topology edge state

What is the current truth-state of a system edge?

Allowed values:

- `validated`
- `degraded`
- `missing`
- `planned`
- `blocked`
- `unknown`

## Required metadata

Every status view model should support:

```ts
{
  dimension: string;
  state: string;
  label: string;
  severity: 'positive' | 'neutral' | 'warning' | 'critical' | 'info';
  reason?: string;
  details?: string[];
  source?: string;
  checkedAt?: string;
  nextAction?: string;
}
```

## Unknown rule

If state is `unknown`, `label` must not be only `Unknown`. It must name the subject:

```text
Compatibility unknown
Provider unknown
Task type unknown
Readiness unknown
```

and should include a reason.
