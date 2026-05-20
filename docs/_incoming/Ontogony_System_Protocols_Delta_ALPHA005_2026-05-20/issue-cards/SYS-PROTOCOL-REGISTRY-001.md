# SYS-PROTOCOL-REGISTRY-001 — Create canonical machine-readable system protocol registry

**Priority:** P0  
**Repo:** ontogony-platform primary; all repos consumed  
**Theme:** Protocol registry

## Problem

The system now has matrices, route inventories, evidence ledgers, runtime lock, frontend route catalogs, and generated OpenAPI artifacts, but no single registry binds them together and detects drift.

## Scope

Add a JSON registry linking baseline, repos, commits, API prefixes, route inventories, OpenAPI snapshots, env/auth matrices, error contracts, trace headers, E2E artifacts, frontend route catalogs, and known quarantines.

## Acceptance criteria

Registry validates against schema; each referenced file exists; registry can answer: what route/auth/env/error/evidence artifacts define this protocol surface? CI or script fails on missing evidence, stale baseline labels, obsolete package references, or dead paths.

## Source anchors

- `allagma-dotnet/docs/system/SYSTEM_COMPATIBILITY_MATRIX.md`
- `allagma-dotnet/docs/system/SYSTEM_TEST_MATRIX.md`
- `kanon-dotnet/docs/generated/ONTOLOGY_V0_ROUTE_INVENTORY.json`
- `ontogony-frontend/src/app/route-workflow-catalog.json`

## Implementation notes

- Keep changes additive unless correcting stale docs.
- Do not make production-readiness claims.
- Do not enable real external tool execution.
- Prefer generated inventories and validator scripts over handwritten assertions.
