# Source Findings — EVAL-EVIDENCE-QUALITY-001

These findings come from the raw Ontogony console text provided by the operator.

## Evaluations dashboard findings

Observed current screen behavior:

- Dashboard includes tabs/entry points:
  - Dashboard
  - Baseline comparisons
  - Scenario datasets
- Gateway provider posture is shown as fake-provider default.
- Filters include verdict, status, suite ID, dataset ID, scenario ID, comparison ID, run ID, trace ID, completed-from and completed-to.
- The page states that filters map to `GET /allagma/v0/evaluations` query parameters.
- The page states that dataset and comparison filters depend on evaluation metadata and may be sparse for manual writes.
- Live data is loaded from `GET /allagma/v0/evaluations`, limit 100 per page.
- Current list showed 25 rows.
- All listed rows passed.
- Average quality score was around 0.93.
- Scenario shown repeatedly: `scenario-risk-summary-v0`.
- Provider column showed `unknown`.
- Tokens column showed `Not applicable`.
- Suite showed `eval-profile-v0`.
- Dataset metadata was absent: `(no dataset metadata)`.
- Baseline comparison IDs were absent in the current list.
- The page explicitly says suite/dataset dimensions are counts from the current list page, not a separate analytics API.
- Quality trend is also based on current global list page and is not a durable time-series API.
- Evidence Spine links are present but the screen does not prove whether they resolve complete evidence.

## Interpretation

The dashboard is already live and useful, but it risks overstating confidence:

- "25 pass / 0 fail" reads like quality coverage, but the evidence basis is narrow.
- Unknown providers undermine evaluation interpretability.
- Missing dataset metadata prevents scenario-level or dataset-level confidence.
- Missing comparison IDs weakens regression/baseline analysis.
- Current-page summaries are acceptable only if clearly labeled as such.
- Repeated pass rows from one scenario should be presented as a narrow sample, not broad coverage.
- Evidence links need completeness indicators, not just navigation links.

## Product principle

Every quality number must say what it is based on.

Bad:
- `Average quality score 0.93`

Better:
- `Average quality score 0.93 — current page only, 25 rows, 1 scenario, 0 rows with provider metadata, 0 rows with dataset metadata`

## Evidence-quality dimensions to expose

For each evaluation row and summary:

- Provider known?
- Token/cost known?
- Dataset metadata present?
- Scenario metadata present?
- Baseline comparison linked?
- Run linked?
- Trace/correlation linked?
- Evidence Spine resolvable?
- Model call linked?
- Kanon decision linked?
- Source is live vs fallback vs fixture?
- Summary is current-page vs durable analytics?
