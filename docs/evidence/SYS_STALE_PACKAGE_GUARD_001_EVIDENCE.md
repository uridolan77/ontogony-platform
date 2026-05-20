# SYS-STALE-PACKAGE-GUARD-001 evidence

**Date:** 2026-05-20  
**Baseline:** `SYSTEM-ALPHA-006` (current lock; validator patterns reference same)  
**Owner repo:** `ontogony-platform`

**Verdict:** PASS — stale detector reports findings in quarantined legacy packages; reconciliation delta package has required files and zero non-exempt findings; CI wired.

**Non-claims:** Not production readiness; does not enable real external tool execution.

## Deliverables

| Item | Path |
| --- | --- |
| Patterns inventory | [`docs/system/stale-incoming-package-patterns.json`](../system/stale-incoming-package-patterns.json) |
| Validator | [`scripts/validate-stale-incoming-package.ps1`](../../scripts/validate-stale-incoming-package.ps1) |
| Runbook | [`docs/system/INCOMING_PACKAGE_RUNBOOK.md`](../system/INCOMING_PACKAGE_RUNBOOK.md) |
| Curated review quarantine | [`docs/_incoming/curated-review-package/STALE_PACKAGE_QUARANTINE.json`](../_incoming/curated-review-package/STALE_PACKAGE_QUARANTINE.json) |
| Next-phase quarantine | [`docs/_incoming/ontogony-next-development-phase-package/STALE_PACKAGE_QUARANTINE.json`](../_incoming/ontogony-next-development-phase-package/STALE_PACKAGE_QUARANTINE.json) |

## Acceptance mapping

| Criterion | Result |
| --- | --- |
| Validator reports stale items in old packages | PASS — curated-review (≥2), next-development-phase (≥3) |
| New packages include baseline / delta / superseded files | PASS — `Ontogony_System_Protocols_Delta_ALPHA005_2026-05-20` has `01_*`, `02_*`, `08_*` |
| CI / runbook prevents blind copy into canonical docs | PASS — `ci.yml` + `INCOMING_PACKAGE_RUNBOOK.md` |

## Validator (local)

```powershell
cd C:\dev\ontogony-platform
.\scripts\validate-stale-incoming-package.ps1
# expect: stale-incoming-package OK (SYS-STALE-PACKAGE-GUARD-001)
```

## Sample stale findings (quarantined legacy — informational)

| Package | Stale findings (2026-05-20 run) | Pattern ids (sample) |
| --- | --- | --- |
| `curated-review-package/ontogony_curated_review_package` | 5 | `stale-alpha004-pending-baseline`, `system-production-ready-claim` |
| `ontogony-next-development-phase-package/...` | 5 | `stale-alpha004-pending-baseline`, `stale-alpha004-cut-target`, `system-production-ready-claim` |
| `Ontogony_System_Protocols_Delta_ALPHA005_2026-05-20` | 0 (reconciliation) | — |

## Related

- Reconciliation package: [`docs/_incoming/Ontogony_System_Protocols_Delta_ALPHA005_2026-05-20/`](../_incoming/Ontogony_System_Protocols_Delta_ALPHA005_2026-05-20/)
- Protocol registry: [`SYSTEM_PROTOCOL_REGISTRY_001_EVIDENCE.md`](./SYSTEM_PROTOCOL_REGISTRY_001_EVIDENCE.md)
