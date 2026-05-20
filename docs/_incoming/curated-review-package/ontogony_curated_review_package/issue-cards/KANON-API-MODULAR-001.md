# KANON-API-MODULAR-001 — Split large API Program.cs into feature endpoint modules

**Priority:** P1  
**Repo:** kanon-dotnet  
**Theme:** Maintainability

## Problem

The API surface is broad enough that monolithic endpoint registration is now a review and maintenance risk.

## Scope

Extract endpoint registration modules for ontology, source bindings, semantic plans, canonical facts, actions/human gates, provenance/replay, domain packs, and Conexus assistance.

## Acceptance criteria

No route behavior changes; tests/OpenAPI route inventory remain stable.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
