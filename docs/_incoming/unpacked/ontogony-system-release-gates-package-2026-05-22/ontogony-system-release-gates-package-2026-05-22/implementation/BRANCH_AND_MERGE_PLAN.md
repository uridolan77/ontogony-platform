# Branch and merge plan

## Branches

```text
platform/system-alpha-007-release-gate
allagma/system-alpha-008-scheduled-cohesion
allagma/platform-rel-001-package-train
```

## Merge order

1. Platform evidence schemas and validators.
2. Allagma package-mode release summary.
3. Allagma scheduled cohesion workflow.
4. Platform locked release orchestrator.
5. Closeout evidence PR.

## Branch protection

Do not require scheduled cohesion on every PR. Require it only for release branches or manual release-candidate PRs.

Recommended required checks for normal PRs remain repo-local aggregate checks. Release branch protection may additionally require:

```text
runtime-release-gate
package-mode-release-train
system-cohesion-scheduled/manual
```
