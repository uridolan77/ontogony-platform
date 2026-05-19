# 02 — Product Hardening Sequence

## Guiding principle

Backend semantics first, alignment second, frontend depth third.

Do not let the frontend invent backend product semantics. Do not let backend routes appear without frontend capability-state and tests.

| Order | Item | Primary repo(s) | Purpose |
|---:|---|---|---|
| 0 | `PFH-000` | platform | Unpack/register this package (**done**). |
| 1 | `PFH-001` | platform + all repos | Current-state audit and product gap matrix (**done**). |
| 2 | `EVAL-PRODUCT-001` | allagma + platform + frontend | Eval query/list contract and dashboard data model (**done**). |
| 3 | `ALIGN-PRODUCT-001` | platform + all repos | Contract matrix refresh (**done**). |
| 4 | `FE-PRODUCT-001` | frontend + platform | Eval dashboard v2 (**done**). |
| 5 | `EVAL-PRODUCT-002` | allagma + frontend | Baseline comparison workbench depth (**done**). |
| 6 | `EVAL-PRODUCT-003` | allagma + frontend | Scenario dataset management surfaces (**done**). |
| 7 | `EVAL-PRODUCT-004` | allagma + frontend | Quality scoring and judge calibration surfaces (**done**). |
| 8 | `FE-PRODUCT-002` | frontend | Run detail evidence depth (**done**). |
| 9 | `FE-PRODUCT-003` | frontend + allagma | Replay evidence workbench (**done**). |
| 10 | `EVAL-PRODUCT-005` | allagma + frontend + platform | Eval evidence export bundle (**next**). |
| 11 | `FE-PRODUCT-CLOSEOUT-001` | platform + frontend | Closeout, scorecard, known limitations. |
