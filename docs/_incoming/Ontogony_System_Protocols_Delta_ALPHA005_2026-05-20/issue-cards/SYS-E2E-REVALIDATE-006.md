# SYS-E2E-REVALIDATE-006 — Re-run full cohesion suite against current moving-main before next lock

**Priority:** P0  
**Repo:** allagma-dotnet primary  
**Theme:** System E2E

## Problem

SYSTEM-ALPHA-005 evidence is strong, but current main includes additional work in platform, Conexus, Kanon, and Allagma after the cut. Those changes need one integrated proof pass.

## Scope

Run local stack, system cohesion smoke with assistance/fallback/streaming evidence, restart survival, package-mode build, persistence smoke, Kanon route parity, frontend live smoke, and evidence spine Docker-live checks.

## Acceptance criteria

A single evidence bundle records PASS/FAIL for each required scenario, with exact repo SHAs and artifact paths. Any failure becomes an explicit quarantine, not hidden under Alpha-005 evidence.

## Source anchors

- `allagma-dotnet/docs/e2e/SYSTEM_COHESION_SMOKE.md`
- `allagma-dotnet/docs/system/SYSTEM_TEST_MATRIX.md`

## Implementation notes

- Keep changes additive unless correcting stale docs.
- Do not make production-readiness claims.
- Do not enable real external tool execution.
- Prefer generated inventories and validator scripts over handwritten assertions.
