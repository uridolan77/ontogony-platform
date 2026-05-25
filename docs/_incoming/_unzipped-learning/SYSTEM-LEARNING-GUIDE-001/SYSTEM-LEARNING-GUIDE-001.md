# SYSTEM-LEARNING-GUIDE-001 — canonical Ontogony learning guides

## Intent

Create a single navigable learning path for Ontogony developers/operators covering architecture, local execution, governed fake E2E, System Truth, Evidence Spine, Agent Interaction, contract discipline, extension workflows, debugging, and UI canonicalization.

This is a consolidation sprint, not a feature sprint.

## Target canonical files

Create under `ontogony-platform/docs/learn/`:

- `INDEX.md`
- `00_START_HERE.md`
- `01_ARCHITECTURE_MAP.md`
- `02_RUN_LOCAL_SYSTEM.md`
- `03_GOVERNED_FAKE_E2E.md`
- `04_SYSTEM_TRUTH_AND_RELEASE_READINESS.md`
- `05_EVIDENCE_SPINE.md`
- `06_AGENT_INTERACTION.md`
- `07_DOMAIN_MODEL_ROUTING_BOUNDARIES.md`
- `08_CONTRACT_DISCIPLINE.md`
- `09_ADD_A_DOMAIN.md`
- `10_ADD_A_PROVIDER_OR_MODEL_ALIAS.md`
- `11_ADD_OR_CHANGE_AN_API_ROUTE.md`
- `12_ADD_A_FRONTEND_PAGE.md`
- `13_ADD_AN_EVALUATION_OR_BASELINE.md`
- `14_DEBUGGING_PLAYBOOK.md`
- `15_UI_CANONICALIZATION_AND_CONSOLE_UX.md`
- `GLOSSARY.md`

## Canonical boundary story

- Ontogony Platform: neutral mechanics, shared contracts, local working system docs, runtime/system truth evidence, cross-system learning docs.
- Kanon: ontology, domain packs, source bindings, canonical facts, semantic plans, action policy, decisions, provenance/replay.
- Conexus: model gateway, model aliases, provider routing, provider keys, route decisions, model calls, usage/quota/cost.
- Allagma: governed execution runtime, runs/events, Kanon/Conexus coordination, human gates, evidence bundles, evaluations/baselines.
- ontogony-frontend: operator console, generated clients/schemas, route workflow catalog, Evidence Spine and Agent Interaction UI.
- ontogony-ui: reusable UI package, AppShell, canonical primitives, accessibility/export contracts.

## Implementation stages

1. Current docs/code/artifacts audit.
2. Create canonical docs structure and index.
3. Write Start Here, Architecture Map, Run Local System, Governed Fake E2E.
4. Write Evidence Spine, Domain/Model/Routing, Contract Discipline.
5. Write extension guides.
6. Mark stale docs and prepare archive plan.
7. Add docs validation scripts/checks where feasible.

## Non-goals

- No backend features.
- No architecture rewrite.
- No production IAM/security docs unless current and verified.
- No marketing docs.
- No manual duplication of generated contract tables.
