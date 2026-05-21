# Locked-runtime drift policy

## Definitions

| Term | Meaning |
|---|---|
| `locked commit` | Exact commit SHA recorded in runtime lock |
| `expected ref` | Branch intent recorded in runtime lock, usually `main` |
| `drift` | Current expected ref head differs from locked commit |
| `release evidence` | Validated bundle produced in `Locked` mode |

## Policy

1. Locked release runs must checkout `lockedCommits`.
2. Scheduled moving-main runs may checkout `expectedRefs`.
3. Drift is informational unless it breaks scheduled cohesion.
4. Runtime lock must not be updated automatically by scheduled jobs.
5. Updating runtime lock requires a PR with evidence from a passing locked or candidate run.

## Lock update criteria

A lock bump PR should include:

- old lock and new lock diff;
- four repo commit SHAs;
- package-mode summary;
- full-system cohesion summary;
- Conexus capacity summary;
- restart survival summary;
- known post-lock deltas if any.
