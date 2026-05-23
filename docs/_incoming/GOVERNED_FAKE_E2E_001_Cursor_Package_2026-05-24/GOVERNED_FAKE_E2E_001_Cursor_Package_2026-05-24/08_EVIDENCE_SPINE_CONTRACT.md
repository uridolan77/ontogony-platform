# 08 — Evidence Spine Contract for Governed Fake E2E

## Root identifiers

The resolver must support the following roots for this flow:

| Root kind | Example |
| --- | --- |
| Allagma run id | `run_...` or repo-specific run id |
| Trace id | hex/string trace id |
| Correlation id | correlation id |
| Kanon decision id | `decision_...` |
| Conexus model call id | `chatcmpl-...` |
| Conexus route decision id | `rd-...` |

## Required nodes

```json
[
  "allagma.run",
  "platform.trace",
  "platform.correlation",
  "kanon.decision",
  "conexus.modelCall",
  "conexus.routeDecision",
  "conexus.providerAttempt"
]
```

## Required edge names

```json
[
  "has_trace",
  "has_correlation",
  "used_kanon_decision",
  "used_model_call",
  "used_route_decision",
  "has_provider_attempt",
  "derived_from"
]
```

## Missing reason codes

Use these reason codes:

```text
not_applicable
not_recorded
not_resolved
not_found
backend_missing
authorization_failed
source_failure
fixture_only
not_implemented
id_mismatch
store_mismatch
```

## Applicability logic

### Direct Conexus chat

Expected:

```text
Conexus model call
Route decision
Provider attempt
Trace/correlation
```

Not applicable:

```text
Kanon decision
Allagma run
Human gate
Tool call
```

### Governed fake run

Expected:

```text
Allagma run
Kanon decision
Conexus model call
Route decision
Provider attempt
Trace/correlation
```

Optional:

```text
Human gate
Tool call
Action decision
Replay bundle
```

## Source attempt contract

Each source attempt must include:

```json
{
  "system": "allagma|kanon|conexus|platform",
  "endpoint": "...",
  "identifierKind": "...",
  "identifierValue": "...",
  "status": "success|not_found|error|skipped",
  "latencyMs": 0,
  "reasonCode": null,
  "message": null
}
```
