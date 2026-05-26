# Migration targets

## `ontogony-ui`

Expected additions:

- status taxonomy types;
- label helpers;
- severity helpers;
- badges/cards/summary components if appropriate;
- export guards.

Possible file families to inspect:

- `src/status/*`
- `src/system/*`
- `src/operator/*`
- `src/semantic/*`
- `src/execution/*`
- `src/settings/*`
- package export definitions

## `ontogony-frontend`

Expected migrations:

- Home service health cards;
- protocol truth cockpit;
- system compatibility;
- topology readiness;
- Evidence Spine summary/source attempts;
- Agent Interaction session mode;
- Kanon actor/ontology/source-binding/domain-pack pages;
- Allagma overview/run list/runtime posture;
- Conexus provider/routing/model-call pages;
- Settings credential/source labels;
- Evaluation dashboard.

## Backend repos

Do not make broad backend changes in this sprint. Only inspect backend DTOs/contracts where frontend adapters need to map existing fields into the taxonomy.

If a missing backend field blocks truthful UI, document it as a follow-up, do not fake it.
