# Status Taxonomy for SYSTEM-TRUTH-001

## Connectivity

| State | Meaning |
|---|---|
| `live` | HTTP probe succeeded. |
| `degraded` | Probe succeeded but response was slow, partial, or non-standard. |
| `offline` | Probe failed. |
| `unknown` | Probe not attempted or not classified. |

## Readiness

| State | Meaning |
|---|---|
| `ready` | Required checks pass. |
| `degraded` | Required path is usable but optional/secondary checks warn. |
| `not_ready` | One or more required checks fail. |
| `unknown` | Readiness not checked or payload unusable. |
| `not_applicable` | Readiness concept does not apply. |

## Contract health

| State | Meaning |
|---|---|
| `valid` | Payload matches expected schema. |
| `warning` | Payload usable but missing optional/recommended fields, or legacy parser used. |
| `invalid` | Payload missing required fields or not JSON when JSON is expected. |
| `unknown` | Contract was not checked. |

## Data source

| State | Meaning |
|---|---|
| `live` | Value came from live backend response. |
| `live_with_fallback` | Backend was queried; fallback filled missing non-critical fields. |
| `generated` | Value came from generated artifact. |
| `fixture` | Value came from static/demo fixture. |
| `imported` | Value came from imported file/JSONL. |
| `unknown` | Source not known; must not be green. |

## Release posture

| State | Meaning |
|---|---|
| `not_assessed` | No live release validation was performed. |
| `blocked` | Critical release blocker. |
| `warning` | Candidate possible but unresolved warnings. |
| `candidate` | Live checks pass; formal release still pending. |
| `passed` | Release readiness explicitly assessed and passed. |
