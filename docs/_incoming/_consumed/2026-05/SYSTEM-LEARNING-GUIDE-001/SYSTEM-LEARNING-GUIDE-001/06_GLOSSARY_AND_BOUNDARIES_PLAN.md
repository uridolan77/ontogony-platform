# 06 — Glossary and boundaries plan

`GLOSSARY.md` must define at least:

Ontogony Platform, Kanon, Conexus, Allagma, ontogony-frontend, ontogony-ui, domain, ontology, ontology version, domain pack, active pack, source binding, canonical fact, semantic plan, semantic decision, model purpose, Conexus alias, provider key, provider model, route decision, model call, provider attempt, execution run, Allagma run, human gate, trace ID, correlation ID, Evidence Spine, Agent Interaction, runtime lock, governed fake E2E, System Truth, release readiness artifact, contract discipline, manual DTO shim, fixture/demo data, live data, generated artifact.

Boundary rule examples:

- Domain switch changes semantic context, not provider routing by itself.
- Allagma model purpose expresses execution intent.
- Conexus alias maps a purpose-like model name to provider/model/fallback routing.
- Provider key/model are execution-layer concerns owned by Conexus.
- Kanon decision is not a Conexus route decision.
- Evidence Spine graph is a resolution graph, not a new source of truth.
