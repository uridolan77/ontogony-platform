# SYS-LOCK-006 — Refresh runtime lock only after moving-main validation passes

**Priority:** P0  
**Repo:** allagma-dotnet primary; all repos referenced  
**Theme:** Runtime lock / baseline governance

## Problem

Current main branches are ahead of the SYSTEM-ALPHA-005 lock: platform +8, Conexus +5, Kanon +3, Allagma +7 at review time. The lock is valid as a cut baseline, but it is no longer a full description of moving-main.

## Scope

Run current-main validation across platform, Conexus, Kanon, Allagma, frontend, and UI. If green, cut SYSTEM-ALPHA-006 or equivalent by updating ontogony-runtime.lock.json, closeout evidence, companion repo SHAs, package versions where changed, and required scenario artifacts.

## Acceptance criteria

validate-runtime-lock.ps1 -RequireEvidence -ReleaseMode PASS for new lock; system cohesion summary PASS; restart summary PASS; observability B-012 either PASS or explicitly quarantined with rationale; package/evidence indexes agree on the baseline label.

## Source anchors

- `allagma-dotnet/docs/system/ontogony-runtime.lock.json`
- `compare lock commits to main results from GitHub review`

## Implementation notes

- Keep changes additive unless correcting stale docs.
- Do not make production-readiness claims.
- Do not enable real external tool execution.
- Prefer generated inventories and validator scripts over handwritten assertions.
