# Master Cursor prompt — OPERATOR-UX-TAXONOMY-001

You are working on the Ontogony operator console and shared UI package.

Implement **OPERATOR-UX-TAXONOMY-001**: a shared operator state taxonomy that eliminates misleading, inconsistent, and local status wording across Ontogony UI surfaces.

## Repos to inspect first

Inspect the actual repo state before editing:

- `ontogony-ui`
- `ontogony-frontend`
- any local packages/import aliases used between them
- optionally inspect `conexus-dotnet`, `kanon-dotnet`, `allagma-dotnet`, and `ontogony-platform` only where frontend adapters consume their health/readiness/evidence shapes

Do not assume file paths. Locate the actual status components, badges, cards, health adapters, topology components, evidence state renderers, and shell status summaries.

## Implementation goals

1. Add a shared taxonomy to `@ontogony/ui`.
2. Export typed primitives and helpers from stable public subpaths.
3. Update `ontogony-frontend` to map page/backend state into this taxonomy.
4. Remove misleading or ambiguous copy from normal operator views.
5. Add tests that prove pages cannot regress to local ad-hoc status language.

## Required dimensions

Implement separate dimensions for:

- connectivity: `live`, `degraded`, `offline`, `unknown`
- readiness: `ready`, `not_ready`, `unknown`
- contract health: `valid`, `warning`, `invalid`, `unknown`
- operator usability: `usable`, `degraded`, `blocked`, `unknown`
- evidence completeness: `resolved`, `partial`, `unresolved`, `not_applicable`, `unknown`
- data source: `live`, `live_with_fallback`, `fixture`, `generated`, `imported`, `mock`, `unknown`
- authority: `authoritative`, `advisory`, `demo`, `inferred`, `historical`, `unknown`
- topology edge state: `validated`, `degraded`, `missing`, `planned`, `blocked`, `unknown`

Do not collapse these into a single “status” enum. A service may be reachable but not ready; live but contract-warning; fixture-backed but usable for demo only; resolved but advisory; etc.

## Required UI behavior

- Never show bare `unknown`.
- Never use `healthy` as a summary if readiness is not ready or contract health has warning/invalid.
- Never show `Live with fixture fallback` as a page title/headline.
- Never let fixture/generated/demo/imported states count as release readiness or live evidence.
- Route/API implementation details belong behind developer details, not normal operator cards.
- Planned/future topology links must be visibly distinct from current validated links.

## Required migration targets

Migrate at least these surfaces where present:

- Home / operator home health cards
- System compatibility / topology readiness
- Evidence Spine resolution summary and graph completeness
- Agent Interaction mode/session state
- Kanon overview, ontology, domain packs, source bindings pages
- Allagma overview/run list/runtime posture
- Conexus routing/observability/provider posture
- Settings credential/source labels
- Evaluation dashboard quality/evidence state

## Tests

Add unit and/or component tests covering:

- taxonomy label helpers;
- no bare unknown labels;
- service summary behavior when liveness is live but readiness not ready;
- fixture fallback as badge, not headline;
- planned topology edge not displayed as implemented;
- evidence not-applicable distinct from unresolved;
- developer route details are collapsible/secondary.

## Acceptance

This work is accepted only when status semantics are unified and the console no longer uses ambiguous status language in the major operator surfaces.
