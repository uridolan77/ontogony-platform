# 11 — Agent Authority Model

## Actor model

Every action comes from an Actor.

Actor has:

```text
actor_type
authority_level
```

## Actor types

```text
user
agent
system
external_tool
```

## Authority levels

```text
owner
editor
reviewer
proposer
extractor
viewer
system
```

## Canonization authority

Only:

```text
owner
editor
```

may set `ProjectObjectState.working_status = accepted`.

## Project-local authority source

Inside a project, authority comes from **active** `project_actor_role` rows.

`actor.default_role` is a fallback/capability hint only. It is not sufficient for project-local canonical actions.

## Contradiction resolution authority

Only:

```text
owner
editor
reviewer
```

may resolve a `ContradictionCase`.

## Snapshot authority

Only:

```text
owner
editor
```

may create a `CanonicalSnapshot`.

## Agent limitations

Agents may:

- propose objects
- propose relations
- flag contradictions
- open prediction runs (future metabolic slot)
- open surprise events (future metabolic slot)
- recommend merges
- recommend snapshots
- summarize evidence
- draft synthesis
- classify source material

Agents may not:

- accept objects
- resolve blocking contradictions
- create canonical snapshots
- destructively merge objects
- bypass review events
- silently rewrite project state
- auto-canonize via metabolic policies

### Runtime note (PR41)

The API enforces the above on canonical-risk commands: **only human (`actor_type=user`) actors may execute finalize paths** (accept object version, accept relation, create snapshot / snapshot synthesis, contradiction resolution, revision accept/close, etc.), regardless of whether a non-human actor has been granted `owner`/`editor` in `project_actor_role`. Policy rules are evaluated in addition to these service checks, not instead of them.

## System actor limitations

System actors may perform deterministic operations.

Examples:

- chunk source
- calculate content hash
- calculate embedding
- run validation rule
- create candidate contradiction
- create processing job

System actors may not canonize unless explicitly represented as an authorized human action.

System actors also do not automatically outrank project roles: they do not gain `owner/editor` powers inside a project unless explicitly granted an appropriate `project_actor_role`.

## Actor type alias

The schema stores `actor.actor_type` from the closed set:

```text
user, agent, system, external_tool
```

The API accepts `"human"` as a friendly alias for `"user"`, but `"user"` is what is persisted in PostgreSQL.

## LLM usage

LLMs are allowed as assistants, not authorities.

Allowed:

- extraction assistance
- relation suggestion
- contradiction candidate detection
- synthesis drafting
- critique
- crisis-time type invention

Forbidden:

- choosing canonical truth
- resolving contradictions without review
- changing accepted state directly
- hiding reasoning inside an untraceable generation

## ReviewEvent requirement

Any agent/system-generated candidate that becomes canonical must be associated with a human ReviewEvent.

## PR36 policy specification (design contract)
PR36 expands this authority model into a policy-engine specification without changing canonization rules.

See:
- `docs/PR36_AGENT_POLICY_ENGINE.md`
- `docs/AGENT_POLICY_CAPABILITY_MATRIX.md`
