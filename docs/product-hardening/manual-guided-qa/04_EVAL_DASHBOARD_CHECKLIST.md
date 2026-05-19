# 04 — Eval dashboard checklist

Primary route: `/allagma/evaluations`

Fixture route: `/allagma/evaluations?dashboardFixture=ci-suite`

## Live dashboard checks

- [ ] Route loads without crash
- [ ] Live sample banner appears, or honest empty/degraded state appears
- [ ] Filter controls render and can be toggled
- [ ] URL updates when filters are changed
- [ ] Pagination/cursor behavior is visible where data volume requires it
- [ ] Quality/judge metadata fields appear when available

## Fixture dashboard checks

- [ ] Fixture banner clearly identifies demo data
- [ ] Fixture values are deterministic across refresh
- [ ] No live/backend dependency is implied while fixture is active
- [ ] Fixture mode does not show contradictory live-state banner

## Data contract checks

- [ ] Results correspond to `GET /allagma/v0/evaluations` shape
- [ ] Dataset/baseline filter behavior follows known sparse-metadata limitation
- [ ] No unsupported UI actions imply backend capabilities that do not exist

## Error/degraded checks

- [ ] Unauthorized behavior is explicit when token missing/invalid
- [ ] Network error state provides clear retry or remediation guidance
- [ ] Empty list state is explicit and non-failing

## Evidence

- [ ] Screenshot: live list state
- [ ] Screenshot: fixture banner state
- [ ] Screenshot: filter URL sync
- [ ] Note: any observed mismatch between UI labels and backend contract
