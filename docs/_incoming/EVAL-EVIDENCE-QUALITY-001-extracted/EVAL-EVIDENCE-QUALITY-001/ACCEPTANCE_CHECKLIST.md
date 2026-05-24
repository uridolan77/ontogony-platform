# Acceptance Checklist — EVAL-EVIDENCE-QUALITY-001

## General

- [ ] The Evaluations dashboard does not overstate current-page list data as durable analytics.
- [ ] Every aggregate clearly states its basis.
- [ ] Rows with missing metadata show explicit missing-data labels.
- [ ] Evidence Spine links show whether evidence is complete, partial, unresolved, or unavailable.
- [ ] Baseline comparison absence is explained and actionable.
- [ ] No fake provider/dataset/comparison/token data is invented.
- [ ] Fixture/demo data is visibly labeled if used.

## Summary cards

- [ ] Pass/fail/inconclusive counts identify the source scope.
- [ ] Average quality score states whether it is current-page-only.
- [ ] Scenario count is visible.
- [ ] Provider metadata coverage is visible.
- [ ] Dataset metadata coverage is visible.
- [ ] Baseline comparison coverage is visible.
- [ ] Run/evidence linkage coverage is visible.

## Evaluation table

- [ ] Provider `unknown` is styled as missing metadata, not normal data.
- [ ] Token `Not applicable` is not used as a catch-all for missing token data.
- [ ] Dataset metadata absence is explicit.
- [ ] Comparison absence is explicit.
- [ ] Run/evidence links are disabled or annotated when identifiers are absent.
- [ ] Evidence Spine links prefer the best available identifier.
- [ ] Row actions remain keyboard accessible and screen-reader understandable.

## Baseline comparisons

- [ ] If no comparison IDs are present, the page explains why.
- [ ] The comparison workbench remains reachable.
- [ ] No comparison-backed readiness is claimed without comparison metadata.

## Quality preview/trend

- [ ] Section is labeled `current page` or equivalent if no durable analytics API exists.
- [ ] It does not call itself a durable trend unless backed by backend time-series data.
- [ ] It warns when only one scenario is represented.
- [ ] It shows timestamp/order basis clearly.

## Empty/error states

- [ ] Backend failure is distinct from zero results.
- [ ] Zero results displays active filters.
- [ ] Clear filters action is visible.
- [ ] Fixture loading is clearly labeled as fixture/demo.

## Tests

- [ ] Targeted tests cover metadata completeness.
- [ ] Tests cover missing provider/dataset/comparison values.
- [ ] Tests cover current-page-only summary copy.
- [ ] Tests cover evidence link enabled/disabled behavior.
- [ ] Typecheck/build/lint are run where available.

## Non-goals preserved

- [ ] No backend routes were invented in frontend code.
- [ ] No broad rewrite was performed.
- [ ] No score semantics were changed without current repo support.
- [ ] Backend follow-ups are documented separately.
