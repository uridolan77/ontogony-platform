# SYSTEM-COH-001 — Create canonical system compatibility, environment, auth, route, and test matrices

**Priority:** P0  
**Repo:** allagma-dotnet primary; all repos referenced  
**Theme:** Cross-repo cohesion

## Problem

The repos now expose real service boundaries, but compatibility is still discipline-driven rather than pinned and testable.

## Scope

Add docs/system/SYSTEM_COMPATIBILITY_MATRIX.md, SYSTEM_ENVIRONMENT_MATRIX.md, SYSTEM_AUTH_MATRIX.md, SYSTEM_ROUTE_MATRIX.md, SYSTEM_TEST_MATRIX.md. Pin repo refs, package/client versions, route prefixes, ports, auth modes, required env vars, and smoke scripts.

## Acceptance criteria

A new developer can start the local stack from the matrix; every cross-service client call in Allagma/Kanon/Conexus maps to one documented route/auth/env row; no obsolete naming appears.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
