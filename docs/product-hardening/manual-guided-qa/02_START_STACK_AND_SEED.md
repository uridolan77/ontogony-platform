# 02 — Start stack and seed

Start Docker-local services and establish deterministic IDs for route validation.

## Start and wait

From `ontogony-platform`:

```powershell
.\docker\local-working-system\scripts\start-local-working-system.ps1
.\docker\local-working-system\scripts\wait-local-working-system.ps1
```

Checks:

- [ ] Stack start script completed without fatal errors
- [ ] Wait script reports all required services healthy
- [ ] Frontend responds on `http://localhost:5175/`
- [ ] Allagma responds on `http://localhost:5083/health`
- [ ] Kanon responds on `http://localhost:5081/health`
- [ ] Conexus liveness responds on `http://localhost:5082/health/live`

## Seed/validate guided flow

From `ontogony-platform`:

```powershell
.\docker\local-working-system\scripts\validate-docker-guided-main-flow.ps1
```

Expected artifact:

`docker/local-working-system/artifacts/docker-guided-main-flow-report.json`

Checks:

- [ ] Guided flow script returns pass verdict
- [ ] Artifact file exists and is fresh for this run
- [ ] `subjectRunId` present
- [ ] `subjectEvaluationRunId` present
- [ ] `baselineComparisonId` present (or explicit expected omission recorded)
- [ ] Trace ID and decision IDs are present for journey checks

## Capture working IDs

Record into your run notes:

- [ ] `subjectRunId`
- [ ] `subjectEvaluationRunId`
- [ ] `baselineComparisonId`
- [ ] `traceId`
- [ ] `planningDecisionId` (if present)

## Exit criteria for this step

- [ ] Stack is healthy enough for operator UI navigation
- [ ] Guided report has IDs required by steps `03` to `12`
