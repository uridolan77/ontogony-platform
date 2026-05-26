# Evaluation Evidence Status Taxonomy

Use consistent wording across the evaluations page.

## Data source

| Status | Meaning |
|---|---|
| `live` | Data came from backend API. |
| `live_with_fallback` | Backend data was used, but some fields/components came from fallback/defaults. |
| `fixture` | Demo/static fixture data. |
| `imported` | User-imported JSON/JSONL data. |
| `generated_artifact` | Static generated artifact, not live backend. |

## Summary basis

| Status | Meaning |
|---|---|
| `current_page` | Computed only from rows loaded on the current page. |
| `filtered_result` | Computed from all rows matching current filters. Use only if backend guarantees it. |
| `durable_analytics` | Computed from a dedicated analytics/time-series API. |
| `fixture_summary` | Computed from fixture/demo data. |

## Evidence quality

| Status | Meaning |
|---|---|
| `complete` | Row can resolve the expected evidence chain. |
| `partial` | Row has some evidence identifiers but missing important links. |
| `unresolved` | Row has identifiers, but resolution failed or is unsupported. |
| `unavailable` | No usable evidence identifiers are present. |

## Metadata status

| Status | Meaning |
|---|---|
| `known` | Field is present and meaningful. |
| `missing` | Field is absent/null/empty. |
| `not_applicable` | Field truly does not apply to this evaluation. |
| `not_recorded` | Field should exist but was not recorded. |
| `unknown` | Backend returned unknown or frontend cannot classify. |

Prefer `not_recorded` over `not_applicable` when the row likely should have carried the data.
