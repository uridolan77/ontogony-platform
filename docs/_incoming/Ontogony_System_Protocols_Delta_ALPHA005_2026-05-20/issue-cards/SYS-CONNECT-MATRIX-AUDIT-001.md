# SYS-CONNECT-MATRIX-AUDIT-001 — Audit Allagma feature connection matrix against actual route inventories and clients

**Priority:** P0  
**Repo:** allagma-dotnet primary; kanon-dotnet and ontogony-platform referenced  
**Theme:** Route/feature matrix truthfulness

## Problem

A new Allagma feature connection matrix exists and is valuable, but it must be proven against Program.cs, Kanon generated route inventory, Conexus route inventory, and frontend catalogs before becoming canonical.

## Scope

Compare every route string and dependency claim in ALLAGMA_FEATURE_CONNECTION_MATRIX.md with actual API route mappings, typed client calls, generated OpenAPI, and frontend hooks. Fix any route-name drift and mark indirect dependencies explicitly.

## Acceptance criteria

A generated test or script fails if a matrix route is absent from the source inventory. Matrix rows distinguish direct HTTP call, typed client abstraction, indirect data dependency, and frontend-only link. Known gaps remain explicit, not implied connected.

## Source anchors

- `allagma-dotnet/docs/system/ALLAGMA_FEATURE_CONNECTION_MATRIX.md`
- `kanon-dotnet/README.md route list`
- `allagma-dotnet/src/Allagma.Infrastructure/Kanon/KanonSemanticAuthorityClient.cs`

## Implementation notes

- Keep changes additive unless correcting stale docs.
- Do not make production-readiness claims.
- Do not enable real external tool execution.
- Prefer generated inventories and validator scripts over handwritten assertions.
