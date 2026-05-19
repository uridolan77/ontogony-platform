# 06 — Baseline comparison checklist

Routes:

- list: `/allagma/evaluations/baseline-comparisons`
- detail: `/allagma/evaluations/baseline-comparisons/{baselineComparisonId}`

## List checks

- [ ] List route loads without crash
- [ ] URL-synced filters work as expected
- [ ] Empty state is explicit when no comparisons exist
- [ ] Degraded state is explicit on backend failure
- [ ] Row selection/open-detail behavior is clear

## Detail checks

- [ ] Detail route resolves for guided `baselineComparisonId` when present
- [ ] Subject and baseline run linkage is visible
- [ ] Comparison summary/cards render (or honest unavailable state)
- [ ] Detail can be opened directly by URL without relying on prior list page state

## Fixture and limitation checks

- [ ] Fixture mode (if used) shows fixture banner
- [ ] No baseline create/operator authoring UI is implied (harness-only limitation respected)
- [ ] No rich side-by-side diff UI is falsely implied where not implemented

## Evidence

- [ ] Screenshot: baseline list route
- [ ] Screenshot: baseline detail route
- [ ] Note: guided-flow IDs consistency check
- [ ] Note: observed limitation text for baseline workflows
