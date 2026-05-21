# SYS-TIGHT-002 — Cross-service evidence spine resolver contract

**Repo:** ontogony-platform, ontogony-frontend, allagma-dotnet  
**Type:** contract + adapter tests  
**Priority:** P0

## Goal

Define and consume a unified resolver contract for system evidence objects.

## Scope

- Contract for runId, decisionId, modelCallId, routeDecisionId, traceId, correlationId, humanGateId, domainPackId.
- Adapter tests for Allagma, Kanon, Conexus evidence sources.
- Unresolved edge handling.

## Acceptance

- Starting from runId, resolver creates Allagma → Kanon → Conexus graph when ids exist.
- Missing downstream evidence is explicit and non-fatal.
- Resolver uses Kanon evidence-spine handoff and Conexus evidence routes.
