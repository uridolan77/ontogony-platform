# Cursor Master Prompt — EVAL-EVIDENCE-QUALITY-001

You are working on the Ontogony operator console.

Implement **EVAL-EVIDENCE-QUALITY-001**: make the Allagma Evaluations dashboard and related evidence drilldown surfaces honest, evidence-rich, and operator-grade.

## Context

The current evaluations screen is useful but too optimistic. It lists live evaluation rows, but the evidence basis is sparse:

- Many rows show provider as `unknown`.
- Token usage is `Not applicable`.
- Dataset metadata is absent.
- Baseline comparison IDs are absent.
- Quality summaries and trend sections are based on the current list page, not a durable analytics API.
- The page can look like broad evaluation proof while really showing one scenario and repeated pass rows.
- Evidence Spine links exist, but the page does not clearly communicate whether each row has a resolved run, trace, model call, decision, or comparison.

## Objective

Make the evaluation dashboard truthful and actionable:

1. Every aggregate must disclose its basis.
2. Every row must show metadata completeness.
3. Every Evidence Spine link must be either clearly resolvable or clearly partial/unavailable.
4. Current-page summaries must be labeled as current-page summaries.
5. Baseline comparison entry points must be useful even when comparison IDs are absent.
6. Missing provider/dataset/token/run/correlation data must become visible evidence-quality signals, not quiet blanks.
7. The page should guide the operator toward the next useful action.

## Scope

Primary:
- Allagma Evaluations page/dashboard.
- Evaluation result rows.
- Scenario matrix.
- Quality trend/list-preview section.
- Baseline comparison entry points.
- Evidence Spine links from evaluations.
- Empty states, partial states, source badges, copy, tests/fixtures.

Secondary:
- Shared UI components only if existing patterns already support this.
- Backend follow-up docs if frontend discovers missing fields/routes.

## Non-goals

- Do not implement a new analytics backend.
- Do not invent backend data in the frontend.
- Do not change evaluation scoring semantics.
- Do not remove existing live list functionality.
- Do not make fixture data appear live.
- Do not broaden into Kanon assistance/domain-pack polish.
- Do not broaden into System topology/release-readiness polish.

## Implementation posture

Before editing:
1. Inspect the current route/component structure.
2. Locate evaluation API clients, adapters, fixtures, tests, and shared UI status components.
3. Identify whether rows already carry runId, traceId, comparisonId, modelCallId, provider, token, dataset, scenario metadata, and quality score.
4. Compare this package against real current repo state and discard stale instructions.

During editing:
- Prefer narrow view-model/adaptor changes over broad component rewrites.
- Add helper functions for evidence completeness rather than scattering conditionals.
- Keep copy direct, operational, and non-marketing.
- Preserve existing styling and public component APIs unless a small shared change is clearly warranted.

After editing:
- Run targeted tests.
- Run typecheck/build/lint where available.
- Document any missing backend fields as follow-ups, not fake frontend values.

## Definition of done

The Evaluations dashboard should answer, at a glance:

- Is this live, fixture, fallback, or current-page-only data?
- How complete is the metadata behind these evaluation rows?
- Which rows are evidence-complete enough to trust?
- Which rows lack provider, token, dataset, comparison, or correlation data?
- Which Evidence Spine links resolve and which are partial?
- Are quality/trend numbers durable analytics or only current-page summaries?
