# Implementation Plan — EVAL-EVIDENCE-QUALITY-001

## Phase 0 — Inspect current implementation

Find the existing implementation for:

- Allagma Evaluations route/page.
- Evaluation API client/wrapper.
- Evaluation row/table component.
- Scenario matrix component.
- Quality trend component.
- Baseline comparison workbench link.
- Evidence Spine link builder.
- Shared status pill/badge/card components.
- Tests and fixtures.

Expected likely areas, but verify in repo:

```text
src/allagma/
src/allagma/evaluations/
src/system/evidence-spine/
src/components/
src/shared/
```

Do not assume paths.

## Phase 1 — Add evaluation evidence view model

Create or update a frontend view-model layer that computes:

- `dataSourceKind`
- `summaryBasis`
- `metadataCompleteness`
- `missingFields`
- `evidenceCompleteness`
- `evidenceTargets`
- `rowWarnings`
- `operatorNextActions`

This should be derived from actual API fields only.

Suggested helper names:

```ts
buildEvaluationEvidenceViewModel(...)
summarizeEvaluationListEvidence(...)
getEvaluationMissingMetadata(...)
getEvaluationEvidenceTargets(...)
```

## Phase 2 — Row-level metadata honesty

Update evaluation rows so missing data is explicit:

- Provider:
  - show provider when known
  - show `Unknown provider` with neutral/warning status when absent
- Tokens:
  - show token count/cost when known
  - show `Not recorded` or `Not applicable` only when semantically true
- Dataset:
  - show dataset ID/name when present
  - show `No dataset metadata`
- Comparison:
  - show comparison ID/link when present
  - show `No baseline comparison`
- Run/trace:
  - show whether row can open run/evidence
  - do not present a broken Evidence Spine link as a normal link

## Phase 3 — Summary cards with basis and coverage

Replace overconfident summary cards with coverage-aware summary cards.

Add:

- row count on current page
- filtered result scope
- scenario count
- provider metadata coverage
- dataset metadata coverage
- comparison coverage
- run/evidence linkage coverage
- pass/fail/inconclusive counts
- average score, if present

Example:

```text
Average quality score
0.93
Current page only · 25 rows · 1 scenario · 0/25 provider metadata · 0/25 dataset metadata
```

## Phase 4 — Quality trend honesty

If the current page uses only list-page data:

- Rename section from `Quality trend` to `Quality preview (current page)`.
- Add copy:
  - `This is not a durable time-series. It reflects the current filtered page only.`
- Sort direction should be obvious.
- Show a callout if only one scenario exists.
- Avoid chart-like language if there is no durable analytics API.

## Phase 5 — Baseline comparison entry points

When comparison IDs are absent:

- Keep the workbench link.
- Explain why there are no comparison rows:
  - no evaluations include comparison metadata
  - filters exclude comparison-backed rows
  - backend does not return comparison IDs
- Add operator next action:
  - `Open comparison workbench`
  - `Create comparison from a selected run`, only if supported
  - `Filter by comparison ID`, if a value is known/entered

Do not invent comparison IDs.

## Phase 6 — Evidence Spine link quality

For each row:

- If run ID or trace ID exists, link to Evidence Spine with the strongest identifier.
- If neither exists, show disabled state with reason.
- If only evaluation ID exists and Evidence Spine supports eval lookup, link by eval ID.
- If support is unknown, do not pretend it works; document backend follow-up.

Add a row-level evidence quality status:

```text
Complete | Partial | Unresolved | Not available
```

## Phase 7 — Empty states and filter feedback

For no results:

- Show active filters.
- Explain whether no rows were returned or the backend failed.
- Offer clear actions:
  - clear filters
  - load CI-suite fixture dashboard
  - open dataset workbench
  - open baseline comparison workbench

## Phase 8 — Tests

Add tests for:

- unknown provider handling
- no dataset metadata handling
- no baseline comparison handling
- current-page-only summary wording
- evidence link disabled when identifiers absent
- evidence link enabled when run/trace/eval identifiers exist
- metadata completeness counts
- filters empty state

## Phase 9 — Docs

Update or add operator docs if the repo has relevant docs structure:

- Explain current-page summaries vs durable analytics.
- Explain metadata completeness.
- Explain evidence-quality status.
- List backend follow-ups separately.

