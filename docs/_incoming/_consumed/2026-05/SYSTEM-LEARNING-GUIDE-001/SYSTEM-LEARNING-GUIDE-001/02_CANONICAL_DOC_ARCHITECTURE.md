# 02 — Canonical documentation architecture

## Principle

One learning index in `ontogony-platform/docs/learn/`. Repo-specific docs remain in their repos. Generated artifacts remain generated and are linked, not copied.

## Layers

1. **Learning layer** — `ontogony-platform/docs/learn/*` explains how to understand and operate the whole system.
2. **Reference layer** — owning repo docs explain implementation details.
3. **Evidence layer** — generated artifacts, inventories, reports, runtime-lock and E2E outputs.
4. **Archive layer** — stale docs marked first, archived/deleted only after review.

## Required frontmatter block for each guide

```md
> Audience: operator | frontend developer | backend developer | cross-repo integrator | maintainer  
> Applies to: repo/path list  
> Source of truth: generated artifacts and owning docs linked below  
> Last verified: YYYY-MM-DD by command/file checks
```
