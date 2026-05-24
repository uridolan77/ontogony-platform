# Evaluation Copy Guide

## Core tone

Use operationally precise copy. Avoid marketing language.

## Preferred terms

Use:

- `Current page only`
- `No provider metadata`
- `No dataset metadata`
- `No baseline comparison`
- `Evidence partial`
- `Evidence unavailable`
- `Not recorded`
- `Fixture/demo data`

Avoid:

- `Trend` when there is no durable time-series
- `Quality proof`
- `Fully validated`
- `Production ready`
- `All clear`
- `Not applicable` for missing data unless actually semantically not applicable

## Examples

### Average quality score

Before:

```text
Average quality score
0.93
```

After:

```text
Average quality score
0.93
Current page only · 25 rows · 1 scenario · provider metadata 0/25
```

### Dataset dimensions

Before:

```text
Datasets (metadata)
(no dataset metadata) 25
```

After:

```text
Dataset metadata
0/25 rows include dataset metadata.
These evaluations can be filtered by dataset only when metadata is present.
```

### Baseline comparisons

Before:

```text
No baseline comparison IDs on evaluations in the current list.
```

After:

```text
No baseline comparisons in current page.
None of the 25 visible evaluations include a comparison ID. Open the comparison workbench to create or inspect comparisons.
```

### Quality trend

Before:

```text
Quality trend
Evaluations in the current global list page, newest first.
```

After:

```text
Quality preview — current page
This is not a durable trend. It reflects the 25 visible rows under the current filters, newest first.
```
