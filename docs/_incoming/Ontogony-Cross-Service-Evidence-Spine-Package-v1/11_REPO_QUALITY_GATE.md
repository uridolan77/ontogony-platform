# Repo quality gate

## Correct placement

| Change | Repo |
|---|---|
| frontend resolver, graph UI, export UI | ontogony-frontend |
| shared graph/card primitive | ontogony-ui |
| backend lookup route if missing | owning backend service |
| cross-repo evidence/docs | ontogony-platform |
| schema for cross-service export | ontogony-platform, with frontend implementation |

## Avoid

- adding page-local bespoke link logic instead of shared graph model
- fake edges without source evidence
- resolving by fragile string search where direct API exists
- silent missing links
- huge raw DTO export
- backend aggregator before knowing actual frontend resolver gaps

## Required evidence

Every implementation slice should record:
- IDs tested
- services called
- nodes resolved
- missing edges
- tests run
- browser verification result
