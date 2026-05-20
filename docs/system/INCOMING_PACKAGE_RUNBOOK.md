# Incoming package runbook (SYS-STALE-PACKAGE-GUARD-001)

Use this runbook before copying content from `docs/_incoming/` into canonical docs (`docs/evidence/`, `docs/system/`, service repos).

## Non-claims

- Passing the stale-package validator does **not** mean production readiness.
- Incoming packages are **source material** until reconciled against the runtime lock and current evidence.

## Required files for new reconciliation packages

Adoptable planning packages must include these at the package root (numbered aliases are accepted):

| Canonical name | Accepted aliases |
| --- | --- |
| `CURRENT_BASELINE.md` | `01_CURRENT_STATE_ASSESSMENT.md` |
| `MOVING_MAIN_DELTA.md` | `02_MOVING_MAIN_DELTA.md` |
| `SUPERSEDED_ITEMS.md` | `08_SUPERSEDED_ITEMS.md` |

Also add `INTAKE_STATUS.md` documenting extraction date, non-claims, and a stale-term scan.

## Legacy / stale packages

Do **not** delete historical packages without archive policy (`REPO-DOCS-ARCHIVE-001`). Instead:

1. Add [`STALE_PACKAGE_QUARANTINE.json`](../_incoming/curated-review-package/STALE_PACKAGE_QUARANTINE.json) at the package wrapper root.
2. Set `staleFindingsExpected: true` and list why the package is quarantined.
3. Keep content under `docs/_incoming/` — do not merge into canonical docs without explicit reconciliation.

## Validate locally

```powershell
cd C:\dev\ontogony-platform
.\scripts\validate-stale-incoming-package.ps1
```

Patterns inventory: [`stale-incoming-package-patterns.json`](./stale-incoming-package-patterns.json)

## Reconcile before adoption

1. Read [`allagma-dotnet/docs/system/ontogony-runtime.lock.json`](../../../allagma-dotnet/docs/system/ontogony-runtime.lock.json) and [`system-protocol-registry.json`](./system-protocol-registry.json).
2. Compare route inventories / OpenAPI to package claims.
3. Cross-check evidence index cleared items (B-010, B-011, B-012, B-013).
4. Run `.\scripts\validate-stale-incoming-package.ps1` — reconciliation packages must report **zero** non-exempt findings.
5. Implement from `issue-cards/` only via focused PRs with new evidence docs.

## CI

`scripts/validate-stale-incoming-package.ps1` runs in platform `ci.yml` (docs-gates and full dotnet job).

Evidence: [`docs/evidence/SYS_STALE_PACKAGE_GUARD_001_EVIDENCE.md`](../evidence/SYS_STALE_PACKAGE_GUARD_001_EVIDENCE.md).
