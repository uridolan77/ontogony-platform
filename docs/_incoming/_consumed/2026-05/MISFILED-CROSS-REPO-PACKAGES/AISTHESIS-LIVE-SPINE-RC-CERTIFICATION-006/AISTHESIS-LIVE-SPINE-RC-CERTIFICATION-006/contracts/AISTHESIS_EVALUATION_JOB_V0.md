# Contract — Aisthesis evaluation job v0

## Purpose

Make reconstructability/Krisis evaluation durable and auditable.

## Entity

```yaml
evaluationJobId: string
traceId: string
status: pending|running|succeeded|failed|retry_exhausted
attempt: int
maxAttempts: int
scheduledAtUtc: datetime
startedAtUtc: datetime|null
completedAtUtc: datetime|null
lastErrorCode: string|null
lastErrorMessage: string|null
evaluationRunId: string|null
trigger: envelope_ingested|batch_ingested|manual|scheduled
```

## Requirements

- Job creation is idempotent per trace/evaluation profile where appropriate.
- Failed jobs are queryable.
- Latest evaluation remains stable after restart in Postgres mode.
- Job failures do not disappear silently.
