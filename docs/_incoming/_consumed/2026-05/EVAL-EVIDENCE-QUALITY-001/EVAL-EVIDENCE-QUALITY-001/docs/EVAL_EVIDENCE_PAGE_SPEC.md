# Evaluation Evidence Page Spec

## Page purpose

The Evaluations dashboard is an operator workbench for inspecting evaluation results, not a global quality-certification report.

It should help the operator answer:

- What evaluations are visible under the current filters?
- What scenarios/suites/datasets do they represent?
- How complete is the metadata?
- Which rows can be traced to runs and evidence?
- Are there baseline comparisons?
- Is the apparent quality result broad, narrow, or under-evidenced?

## Required page regions

### 1. Provider posture

Shows gateway/provider posture, but must avoid implying that evaluation rows actually used that provider unless row metadata confirms it.

### 2. Filters

Filters should map to backend query parameters. Active filters must be visible after applying.

### 3. Current-page summary

Must include:

- number of rows on current page
- pass/fail/inconclusive counts
- average quality score if scores exist
- current-page-only label unless backend analytics exist
- scenario count
- suite count
- provider metadata coverage
- dataset metadata coverage
- comparison coverage
- evidence linkage coverage

### 4. Suite/dataset dimensions

Must say whether dimensions come from:

- current page only
- backend analytics endpoint
- fixture data

### 5. Baseline comparisons

Must show whether comparison IDs are present in rows. Absence must be explained.

### 6. Evaluation result rows

Each row should show:

- evaluation ID
- completed timestamp
- scenario ID/name
- provider/provider status
- verdict
- quality score
- token/cost status
- suite/dataset metadata
- run/evidence links
- comparison link when present
- row evidence quality

### 7. Scenario matrix

Must be labeled as latest evaluation per scenario in current scope. If only one scenario exists, show narrow-coverage callout.

### 8. Quality preview

If based only on list page, label as current-page preview, not durable trend.

## Evidence quality states

```text
complete    Enough identifiers exist to resolve run/evidence/model/decision chain.
partial     Some identifiers exist, but important links are missing.
unresolved  Identifiers exist but lookup failed or is unsupported.
unavailable No usable evidence identifiers are present.
```

## Metadata completeness

Suggested dimensions:

```text
providerKnown
tokensKnown
datasetKnown
comparisonKnown
runLinked
traceLinked
modelCallLinked
decisionLinked
```

## Copy rule

Any aggregate number must answer:

```text
What rows is this based on?
Is this current page only?
What metadata is missing?
```
