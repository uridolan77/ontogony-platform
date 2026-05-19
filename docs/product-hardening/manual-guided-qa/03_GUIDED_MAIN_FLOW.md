# 03 — Guided main flow

Validate end-to-end route journey from dashboard to replay using guided-flow IDs.

## URL set

Use:

- `/allagma/evaluations`
- `/allagma/evaluations/{subjectEvaluationRunId}`
- `/allagma/evaluations/baseline-comparisons`
- `/allagma/evaluations/baseline-comparisons/{baselineComparisonId}`
- `/allagma/evaluations/datasets`
- `/allagma/runs/{subjectRunId}`
- `/allagma/replay?runId={subjectRunId}`

## Navigation checklist

- [ ] Dashboard opens and renders page shell
- [ ] Evaluation detail route resolves for guided `subjectEvaluationRunId`
- [ ] Baseline list route opens and shows list/empty/degraded state honestly
- [ ] Baseline detail route resolves for `baselineComparisonId` if present
- [ ] Dataset list route opens without crash
- [ ] Run detail route resolves for `subjectRunId`
- [ ] Replay route resolves by `runId` lookup

## Data integrity checks

- [ ] Subject IDs on detail pages match guided report values
- [ ] Cross-links from run detail to evaluation/replay do not alter IDs unexpectedly
- [ ] Any unsupported operations are shown as limitation states, not fake success
- [ ] No raw secrets, provider keys, or bearer tokens appear in visible UI data

## Degraded/empty honesty checks

- [ ] If data missing, UI shows explicit empty-state messaging
- [ ] If backend fails, UI shows explicit degraded/error state with retry path where applicable
- [ ] UI never silently drops content area to blank crash

## Evidence to capture

- [ ] Screenshot: dashboard live mode
- [ ] Screenshot: run detail showing subject ID and trace context
- [ ] Screenshot: replay lookup state by run ID
- [ ] Screenshot: one explicit limitation banner
- [ ] Short note with route + observed result per page

## Exit criteria for this step

- [ ] All target routes traversed at least once with guided IDs
- [ ] Any broken route is logged as fail with route and reproduction URL
