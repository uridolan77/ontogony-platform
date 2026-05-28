# Trace ID and correlation policy

All producers must propagate:

```text
traceId
correlationId
runId / semanticPlanId / decisionId / modelCallId / pipelineRunId as applicable
```

Suggested headers:

```text
x-ontogony-trace-id
x-ontogony-correlation-id
x-ontogony-run-id
```

Do not create separate trace IDs in each producer for the same cross-system operation.
