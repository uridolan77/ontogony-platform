# Active intake manifest

Last updated: **2026-05-26**

| Package | Status | Reason active | Next action | Date |
| --- | --- | --- | --- | --- |
| *(none)* | — | PLAT-9-004 public API hardening consumed | Add new intake when the next program starts | 2026-05-26 |

**Consumed follow-on slices (not separate active packages):**

| Slice | Status | Archive |
| --- | --- | --- |
| DEC-RECON-004 smoke | **PASS** | [`../_consumed/2026-05/ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-004/`](../_consumed/2026-05/ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-004/) |
| DEC-RECON-005 golden fixtures | **Done** | [`../_consumed/2026-05/ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-005/`](../_consumed/2026-05/ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-005/) |
| DEC-RECON-007 persisted artifacts | **PASS** | [`../_consumed/2026-05/ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-007/`](../_consumed/2026-05/ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-007/) |
| Parent spine program | **Done** | [`../_consumed/2026-05/ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001/`](../_consumed/2026-05/ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001/) |
| Evidence Spine graph (006) | **Done** | [`../_consumed/2026-05/ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-006/`](../_consumed/2026-05/ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-006/) |
| Closure Option 1 (PR-001–006) | **Done** | [`../_consumed/2026-05/ONTOGONY-RECONSTRUCTABILITY-CLOSURE-OPTION1/`](../_consumed/2026-05/ONTOGONY-RECONSTRUCTABILITY-CLOSURE-OPTION1/) |
| Observability mechanics (PLAT-9-005) | **Done** | [`../_consumed/2026-05/ONTOGONY-OBSERVABILITY-MECHANICS-PACK-001/`](../_consumed/2026-05/ONTOGONY-OBSERVABILITY-MECHANICS-PACK-001/) |
| Public API hardening (PLAT-9-004) | **Done** | [`../_consumed/2026-05/ONTOGONY-PUBLIC-API-HARDENING-001/`](../_consumed/2026-05/ONTOGONY-PUBLIC-API-HARDENING-001/) |

## Above-9 platform work (not a separate active package)

Six-repo score-lift and remaining **PLAT-9-003 / 004 / 005** items live in the consumed archive:

[`../_consumed/2026-05/SIX-REPO-SCORE-PLANS/`](../_consumed/2026-05/SIX-REPO-SCORE-PLANS/) — see [`README.md`](../_consumed/2026-05/SIX-REPO-SCORE-PLANS/README.md) for platform status vs plan.

## Notes

- Unpacked trees may use a nested `<PACKAGE-NAME>/<PACKAGE-NAME>/` layout from Cursor exports — paths in prompts refer to the inner folder.
- Do not add zip files or loose markdown stubs in `_active/`; update this manifest instead.
- **Archive rule:** completed packages **must** leave `_active` — see [`../README.md#when-to-move-a-package-to-_consumed-required`](../README.md#when-to-move-a-package-to-_consumed-required).
