# Required-edge matrix v2 implementation notes

## Principle

Matrix v1 proves the minimal system spine. Matrix v2 proves operational reconstructability.

## Edge profile model

Add profile-aware evaluation:

```text
core-run-profile
metabole-first-profile
human-gated-run-profile
tool-side-effect-profile
model-fallback-profile
streaming-model-profile
retention-erasure-profile
```

Every rule should define:

```yaml
requiredEdgeId:
fromProducer:
fromNativeId:
toProducer:
toNativeId:
relations:
requiredWhen:
profile:
severityIfMissing:
suggestedProducerFix:
notApplicableReasonField:
```

## Do not over-require

A Metabole-first trace should not fail because it lacks a human-gate edge unless the trace actually includes a human gate.

A non-streaming model call should not fail because it lacks a streaming session edge.

A simulation-only tool trace should not fail because it lacks real side-effect evidence, but it should declare `sideEffectMode = simulation`.
