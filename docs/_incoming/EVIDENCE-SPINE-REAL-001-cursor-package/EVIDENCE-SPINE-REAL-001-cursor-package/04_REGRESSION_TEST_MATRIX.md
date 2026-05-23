# 04 — Regression Test Matrix

## Test 1 — Direct Conexus model call

Root identifier:

```text
conexusModelCallId = chatcmpl-...
```

Expected:

- model call resolves;
- provider attempt resolves if available;
- route decision resolves or gives structured reason;
- Kanon decision relationship is `not_applicable` unless evidence explicitly links one;
- no hard missing `used_kanon_decision`.

## Test 2 — Governed Allagma run

Root identifier:

```text
allagmaRunId = run_...
```

Expected:

- Allagma run resolves;
- run events resolve;
- Kanon planning decision resolves;
- decision provenance resolves;
- Conexus model call resolves;
- provider attempt resolves;
- route decision resolves or gives structured reason;
- graph status is `resolved` or `partial` with exact reasons.

## Test 3 — Baseline comparison root

Root identifier:

```text
baselineComparisonId = cmp_...
```

Expected:

- comparison resolves;
- subject run resolves;
- evaluation run/evidence resolves if linked;
- governed run chain expands;
- no duplicate run nodes;
- missing route decision is structured.

## Test 4 — Kanon decision root

Root identifier:

```text
kanonDecisionId = decision_...
```

Expected:

- decision record resolves;
- provenance resolves;
- semantic graph resolves;
- linked Allagma run resolves via `/allagma/v0/runs/{runId}` if present;
- no fallback to malformed `/runs/{runId}` source attempt;
- placeholder run node merges with authoritative run node if both exist.

## Test 5 — Route decision root

Root identifier:

```text
conexusRouteDecisionId = rd-...
```

Expected:

- route detail resolves if present;
- if absent, typed reason is shown;
- related model call/request ID is linked if available;
- no generic `unexpected error`.

## Test 6 — Trace/correlation root

Root identifier:

```text
traceId = ...
correlationId = ...
```

Expected:

- resolver expands all known systems;
- governed subchains have required Kanon links;
- direct Conexus subchains mark Kanon as not applicable;
- summary separates required missing, optional missing, and not applicable.

## Test 7 — Fixture/imported evidence root

Expected:

- data source is labeled `fixture` or `imported`;
- no fixture-only success contributes to live completeness;
- export bundle records data source.
