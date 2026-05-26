# QA Checklist — EVAL-EVIDENCE-QUALITY-001

## Manual QA

### Live list with sparse metadata

Use the local Allagma evaluations list with rows where provider is unknown, tokens are absent/not applicable, dataset metadata is missing, and comparison IDs are absent.

Expected:

- Provider column says `Unknown provider`.
- Dataset area says `No dataset metadata`.
- Baseline area says `No baseline comparison`.
- Quality summary says `current page only`.
- Metadata coverage counts expose missing data.
- Evidence Spine links are not overconfident.

### Filter interaction

Test:

- verdict filter
- status filter
- suite ID
- dataset ID
- scenario ID
- comparison ID
- run ID
- trace ID
- date range

Expected:

- Active filter state is visible.
- Empty result state shows active filters.
- Clear filters works.
- Summary basis changes with filters.

### Evidence link behavior

Test rows with:

1. run ID only
2. trace ID only
3. evaluation ID only
4. no evidence identifiers
5. comparison ID present
6. comparison ID absent

Expected:

- strongest available evidence link is used.
- absent identifiers produce disabled state with reason.
- no broken navigation.

### Fixture/dashboard loading

If `Load CI-suite fixture dashboard` is available:

- Load it.
- Confirm fixture state is visibly labeled.
- Confirm fixture data does not look like live evaluation evidence.

## Automated checks

Run relevant commands discovered in package inspection, commonly one or more of:

```bash
npm test -- --run
npm run test
npm run typecheck
npm run lint
npm run build
```

Do not assume exact commands; inspect package.json.

## Regression risks

Watch for:

- status pills becoming too noisy
- table overflow due to longer missing-data labels
- accessibility regressions in table actions
- broken Evidence Spine query parameter names
- test fixtures that still imply provider/dataset data is present when it is not
- snapshot tests requiring intentional updates

## Final QA report format

```text
Changed:
- ...

Checks run:
- ...

Passed:
- ...

Failed / skipped:
- ...

Known follow-ups:
- ...
```
