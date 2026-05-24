# 01 — Current Diagnosis

## What works today

The local alpha system is already capable of:

- running the service trio locally;
- showing operator Home with service reachability;
- listing Conexus model calls;
- invoking Conexus fake provider;
- recording Conexus execution run and provider attempt;
- resolving some evidence by trace id;
- exporting evidence-spine bundles;
- showing Allagma model purposes:
  - `summarize-player-risk` -> `risk-summary-v0`
  - `summarize-player-risk-stream` -> `risk-summary-stream-v0`

## What the recent direct fake Conexus test proved

A direct Conexus fake-provider chat produced:

```text
Trace
Model call
Conexus execution run
Provider attempt fake / fake.chat
Correlation
```

This means Conexus model-call telemetry is real enough for local inspection.

## What the direct fake Conexus test did not prove

A direct Conexus chat does not prove:

```text
Allagma governed run
Kanon semantic planning decision
Kanon policy/gate decision
Allagma -> Kanon -> Conexus causal chain
```

Therefore the missing Kanon decision in that graph was mostly expected.

## Actual gap exposed

The direct fake test also showed:

```text
conexusRouteDecisionId exists
/admin/v0/route-decisions/{routeDecisionId} failed
```

That is a real Conexus/evidence completeness gap. If route ids are emitted, they must resolve.

## Operator-console trust gap

The console currently says things like:

- trace bridge ready to test;
- Agent Interaction fixture replay;
- partial graph;
- unresolved downstream evidence;
- no live identifiers configured.

This package must make at least one live governed fake chain resolvable so the console can say:

```text
Live governed fake run resolved end-to-end.
```

without relying on fixture/demo data.
