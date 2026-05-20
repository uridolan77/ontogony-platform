# SYS-EVIDENCE-INDEX-006 — Reconcile evidence indexes to Alpha-005 plus moving-main deltas

**Priority:** P0  
**Repo:** ontogony-platform and allagma-dotnet  
**Theme:** Evidence governance

## Problem

Evidence indexes should identify the current cut baseline, cleared quarantines, remaining quarantine, post-cut companion commits, and moving-main deltas without ambiguity.

## Scope

Update platform and Allagma evidence indexes so Alpha-005, B-010/B-011/B-013 cleared, B-012 open, and post-cut Kanon Connect 001-007 evidence are visible and not mixed with older Alpha-004 wording.

## Acceptance criteria

A reader can determine from index alone: current cut baseline, current moving-main status, open quarantines, closed quarantines, companion FE/UI commits, and exact commands to validate.

## Source anchors

- `allagma-dotnet/docs/evidence/README.md`
- `ontogony-platform/docs/evidence/README.md`
- `ontogony-platform/docs/evidence/KANON_CONNECT_001_CROSS_REPO_FEATURE_MAP.md`

## Implementation notes

- Keep changes additive unless correcting stale docs.
- Do not make production-readiness claims.
- Do not enable real external tool execution.
- Prefer generated inventories and validator scripts over handwritten assertions.
