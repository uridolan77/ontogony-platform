# Agent Interaction mode contract

## Modes

```text
live_lookup
fixture_replay
imported_jsonl
```

## `live_lookup`

Meaning:

- Data was fetched from live backend APIs.
- It may still be partial, but it is real runtime evidence.

Required UI:

```text
Data source: live
```

Can contribute to readiness/system-connected claims only if the queried APIs succeed and the page explicitly records source attempts.

## `fixture_replay`

Meaning:

- Data is static or generated fixture/demo content.
- Useful only for UI demonstration and regression tests.

Required UI:

```text
Demo fixture — not live evidence
```

Forbidden:

- must not count as readiness evidence
- must not count as topology evidence
- must not prove cross-service trace correlation
- must not be used in “all services connected” claims

## `imported_jsonl`

Meaning:

- Data came from imported replay artifact.
- It may represent a real prior run, but is not live backend evidence at render time.

Required UI:

```text
Imported replay — offline artifact
```

## Defaulting rule

```text
if Allagma live lookup is reachable:
    default mode = live_lookup
else:
    show live unavailable reason and allow explicit fixture/import mode
```

## Export rule

Every exported interaction bundle must include:

```json
{
  "mode": "live_lookup",
  "dataSource": "live",
  "isLiveEvidence": true
}
```

or equivalent fixture/import values.
