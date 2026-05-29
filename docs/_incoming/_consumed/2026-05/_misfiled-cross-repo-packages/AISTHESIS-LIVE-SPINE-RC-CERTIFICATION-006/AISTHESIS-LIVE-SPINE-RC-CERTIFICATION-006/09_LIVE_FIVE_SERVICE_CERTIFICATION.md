# Live five-service certification design

## Purpose

Aisthesis becomes the certification point for cross-repo reconstructability only when it can validate live producer evidence from:

```text
Allagma + Kanon + Conexus + Metabole -> Aisthesis
```

## Modes

### Preflight

Checks repo paths, script paths, config variables, port plan, and expected route availability. Does not require live APIs.

### Fixture

Starts/uses Aisthesis and ingests a complete golden fixture. Proves Aisthesis evaluator/bundle path.

### Live

Requires all five services ready and a configured live trigger. It must trigger a real workflow that causes all four producers to emit into Aisthesis.

### LiveOrExplain

Preferred for local/CI transitional use. It may return `NOT_RUN`, but only with precise reasons.

## Valid live PASS

A live PASS requires:

```text
services.aisthesis.ready = true
services.allagma.ready = true
services.kanon.ready = true
services.conexus.ready = true
services.metabole.ready = true
traceId != null
producersObserved includes allagma, kanon, conexus, metabole
requiredEdges.v1.missing = 0
requiredEdges.v2.missing = 0 for applicable profile OR acceptedPartial = true with reason
bundleFingerprintPresent = true
blockingFindings = 0
rawPayloadsIncluded = false
secretsIncluded = false
```

## Invalid PASS cases

Do not mark PASS when:

- services are down;
- trigger URL is missing;
- fixture was used but mode says live;
- producer evidence was manually injected and called live;
- required-edge missing count is unknown;
- redaction status is unknown.
