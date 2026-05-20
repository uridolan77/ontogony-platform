# SYS-REAL-TOOLS-BLOCK-VERIFY-001 — Add explicit recurring verification that real external tool execution is still blocked

**Priority:** P1  
**Repo:** allagma-dotnet and ontogony-platform  
**Theme:** Safety

## Problem

The system is growing execution surfaces. Alpha-005 explicitly says real external tool execution remains blocked; future packages should keep proving that invariant.

## Scope

Add or centralize tests/docs verifying RealExecutionEnabled=false by default, kill switch behavior, sandbox/local marker labeling, no provider SDKs in Allagma core, and no real_external execution mode unless SANDBOX-003 is complete.

## Acceptance criteria

Default CI has a named safety test that fails if real execution becomes enabled by default or if docs overclaim real side-effect readiness.

## Source anchors

- `allagma-dotnet/docs/evidence/SYSTEM_ALPHA_005_CLOSEOUT.md`
- `allagma-dotnet/docs/system/SYSTEM_COMPATIBILITY_MATRIX.md`

## Implementation notes

- Keep changes additive unless correcting stale docs.
- Do not make production-readiness claims.
- Do not enable real external tool execution.
- Prefer generated inventories and validator scripts over handwritten assertions.
