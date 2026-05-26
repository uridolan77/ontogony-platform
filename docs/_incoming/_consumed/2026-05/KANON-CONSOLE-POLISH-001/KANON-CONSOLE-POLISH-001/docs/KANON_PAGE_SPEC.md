# Kanon page polish specification

## `/kanon/assistance`

### Required sections

1. Capability state
2. Trust boundary notice
3. Request form
4. Redaction preview
5. Draft result
6. Decision/evidence links

### Capability state

Display:

- enabled/disabled;
- configured/misconfigured;
- allowed roles;
- reason if unavailable.

### Trust boundary notice

Must include:

- draft-only;
- non-authoritative;
- cannot mutate ontology/bindings/packs/gates;
- acceptance requires governed human review.

### Redaction preview

Before submit, show:

- fields included;
- fields redacted;
- fields omitted because not allowed;
- whether the submitted context is empty after redaction.

## `/kanon/domain-packs`

### Required sections

1. Operator actor context
2. Inventory summary
3. Domain-pack list
4. Selected version detail
5. Lifecycle actions
6. Simulation workbench
7. Lifecycle timeline
8. Evidence links

### Inventory summary

Do not show one ambiguous `Active versions` card if it combines distinct concepts. Prefer:

- packs on disk;
- persisted lifecycle versions;
- active ontology versions;
- deprecated versions;
- generated/test-looking versions, if detectable.

### Lifecycle actions

Action controls:

- validate;
- load;
- promote;
- deprecate.

For each action:

- enabled state;
- required role;
- backend route availability if known;
- disabled reason.

### Simulation workbench

Diff, impact, and migration plan must be labeled simulation-only.

## `/kanon/review-queue`

Required state model:

- loading;
- live with items;
- live empty;
- unavailable due to role;
- unavailable due to backend/client gap;
- fallback/demo;
- error.

Each state needs specific copy.

## `/kanon/policies`

Required state model:

- policy list loaded;
- policy list empty;
- policy evaluation unavailable;
- role unavailable;
- backend/client gap;
- fallback/demo;
- error.

Policy rows should link to decisions/evidence when available.

## Settings — Kanon semantic settings health

Required state model:

- Kanon connection health;
- ontology defaults;
- actor defaults;
- role capability grants;
- provenance read grant;
- domain-pack read/mutation grant;
- health contract warning, if any.

Show role presets as guidance, not as hidden behavior.
