# Moving-main delta after SYSTEM-ALPHA-005

This package treats `SYSTEM-ALPHA-005` as the last cut baseline. Current `main` is ahead of the lock and must be revalidated before a new lock is cut.

## Observed post-lock movement

At review time:

| Repo | Movement after Alpha-005 lock | Planning implication |
|---|---:|---|
| `ontogony-platform` | +8 commits | Includes incoming old protocol ZIP and Kanon Connect evidence. Must reconcile into protocol delta. |
| `conexus-dotnet` | +5 commits | Includes route preview, quota status, streaming contract docs/tests, evidence-flow docs, and production exposure fixture changes. Must classify as lock-impacting or additive. |
| `kanon-dotnet` | +3 commits | Includes Kanon Connect planning package and Conexus assistance tests. Must classify against Kanon Connect evidence. |
| `allagma-dotnet` | +7 commits | Includes runtime posture contracts/service/tests, feature connection matrix, and model purpose/audit assertions. Must audit and then decide lock impact. |

## Required posture

Do not call current `main` a new baseline until:

1. All required repo tests pass.
2. System cohesion smoke passes with assistance and fallback.
3. Streaming evidence smoke passes or remains explicitly optional.
4. Restart survival passes.
5. Observability B-012 is closed or explicitly carried forward.
6. Protocol registry validates all canonical paths.
7. Runtime lock is updated with exact SHAs and evidence artifacts.
