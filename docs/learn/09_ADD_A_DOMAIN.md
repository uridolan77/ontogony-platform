# Add a domain (Kanon)

> **Audience:** backend developer (Kanon)  
> **Applies to:** `kanon-dotnet`  
> **Source of truth:** `kanon-dotnet/docs/migrations/2026-05-17-domain-pack-lifecycle-governance.md`, domain pack contracts  
> **Last verified:** 2026-05-25

After domain-pack or assistance workflow changes, refresh Kanon route inventory (`KANON_UPDATE_ROUTE_INVENTORY=1`) and run `npm run contracts:discipline` in `ontogony-frontend`. Canonical counts: **88** API routes, **78** client-covered (see `kanon-dotnet/docs/evidence/KANON_SEMANTIC_DEPTH_CLOSURE_001.md`).

## Overview

A **domain pack** is versioned ontology + policy material loaded into Kanon. Lifecycle: **validate → promote → load** (`accepted` ≠ `active`; one active version per `packId`).

## Checklist

1. **Model** — Extend domain model in `Kanon.Domain` (no EF/HTTP/SDK types).
2. **Pack artifact** — Add pack under repo conventions; record validate/promotion/load decision ids for replay.
3. **API** — Expose under `/ontology/v0` (stay on v0 until graduation gates in `docs/contracts/KANON-V1-001-v0-to-v1-graduation.md`).
4. **OpenAPI** — Refresh snapshot + provenance scripts in Kanon.
5. **Frontend** — Domain Switcher + domain pages; `npm run openapi:sync` if contracts changed.
6. **Allagma** — Runs reference `ontologyVersionId`; smoke with governed-fake or targeted eval.
7. **Tests** — Unit tests + forbidden dependency tests unchanged.
8. **Docs** — Migration note in `kanon-dotnet/docs/migrations/`.

## Console surfaces

- Domain Packs page
- Source bindings (automation suggestions need review before production)
- Semantic plans (must name ontology version)

## Do not

- Put provider routing in Kanon
- Mutate published ontology versions in place
- Skip provenance on canonical facts

## References

- [07_DOMAIN_MODEL_ROUTING_BOUNDARIES.md](./07_DOMAIN_MODEL_ROUTING_BOUNDARIES.md)
- `kanon-dotnet/AGENTS.md`
- [08_CONTRACT_DISCIPLINE.md](./08_CONTRACT_DISCIPLINE.md)
