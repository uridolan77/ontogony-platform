# Acceptance checklist

## Shared taxonomy

- [ ] `@ontogony/ui` exports a shared operator status taxonomy.
- [ ] Taxonomy has separate dimensions for connectivity, readiness, contract health, operator usability, evidence completeness, data source, authority, and topology edge state.
- [ ] No single mega-status enum replaces the multi-dimensional model.
- [ ] Taxonomy labels are human-readable and operator-grade.
- [ ] Unknown states require a subject and reason.
- [ ] Public exports are covered by export/import tests or package boundary tests.

## Frontend adoption

- [ ] Home health cards use shared taxonomy.
- [ ] System compatibility uses shared taxonomy.
- [ ] Topology readiness uses shared topology edge state.
- [ ] Evidence Spine uses shared evidence completeness and data-source badges.
- [ ] Agent Interaction uses shared mode/data-source/authority language.
- [ ] Kanon pages use shared actor/authorization/status vocabulary where relevant.
- [ ] Allagma run list and runtime posture use shared labels.
- [ ] Conexus provider/routing posture uses shared labels.
- [ ] Settings credential source labels use shared secret-source/data-source language.
- [ ] Evaluation dashboard does not overstate evidence quality.

## Truth rules

- [ ] No page says “healthy” when readiness is `not_ready` or contract health is `warning`/`invalid`.
- [ ] Fixture/generated/demo/imported states never count as live readiness.
- [ ] `Live with fixture fallback` is never a page title/headline.
- [ ] Planned/future topology links are never rendered as implemented links.
- [ ] Partial/degraded/unknown states include route-level or field-level reasons.
- [ ] Evidence `not_applicable` is distinct from `unresolved`.

## Copy hygiene

- [ ] Bare `unknown` removed or converted to labeled unknown.
- [ ] Raw backend/API route notes moved to developer details.
- [ ] Refresh buttons are scoped when ambiguous.
- [ ] `Failed runs (sample)` and similar ambiguous copy removed.
- [ ] Raw provider/debug output remains in detail views, not primary cards.

## Tests

- [ ] Taxonomy unit tests pass.
- [ ] Component tests cover common status combinations.
- [ ] Console page tests cover at least Home, Evidence Spine, Agent Interaction, Kanon, Allagma, and Conexus status displays where feasible.
- [ ] String scan catches forbidden headline/status terms.
- [ ] Build/lint/test pass in touched repos.
