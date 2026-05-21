# SYS-TIGHT-001 — Runtime lock and post-lock delta register

**Repo:** allagma-dotnet, ontogony-platform  
**Type:** docs + validation + CI  
**Priority:** P0

## Goal

Make runtime lock promotion reproducible and prevent moving-main confusion.

## Scope

- Add machine-readable post-lock delta register.
- Extend release-mode lock validator to require delta classification.
- Link delta register from system compatibility matrix and closeout docs.

## Acceptance

- Release-mode validation fails if any locked repo has unclassified delta.
- All four runtime repos are represented.
- Docs distinguish `expectedRefs` from `lockedCommits`.
- No runtime code changes.
