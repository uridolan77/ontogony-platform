# Active intake manifest

Last updated: **2026-05-26**

| Package | Status | Reason active | Next action | Date |
| --- | --- | --- | --- | --- |
| [ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001](./ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001/) | **in_progress** | Parent spine program; slices 004/005 consumed | Complete Task 5 — promote protocol docs to `docs/contracts/`; **then move to `_consumed`** | 2026-05-26 |
| [ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-006](./ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-006/) | **done** | DEC-RECON-006 Evidence Spine graph | Archive to `_consumed/2026-05/` when operator confirms | 2026-05-26 |
| [ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-007](./ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-007/) | **in_progress** | DEC-RECON-007 persisted report artifacts | Implement Kanon → frontend → smoke; see package `00_IMPLEMENTATION_PROMPT.md` | 2026-05-26 |

**Consumed follow-on slices (not separate active packages):**

| Slice | Status | Archive |
| --- | --- | --- |
| DEC-RECON-004 smoke | **PASS** | [`../_consumed/2026-05/ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-004/`](../_consumed/2026-05/ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-004/) |
| DEC-RECON-005 golden fixtures | **Done** | [`../_consumed/2026-05/ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-005/`](../_consumed/2026-05/ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-005/) |

## Above-9 platform work (not a separate active package)

Six-repo score-lift and remaining **PLAT-9-003 / 004 / 005** items live in the consumed archive:

[`../_consumed/2026-05/SIX-REPO-SCORE-PLANS/`](../_consumed/2026-05/SIX-REPO-SCORE-PLANS/) — see [`README.md`](../_consumed/2026-05/SIX-REPO-SCORE-PLANS/README.md) for platform status vs plan.

The former active stub `ONTOGONY-BACKEND-ABOVE9-PACKAGES` was empty (zip removed during intake cleanup) and was removed.

## Notes

- Unpacked trees may use a nested `<PACKAGE-NAME>/<PACKAGE-NAME>/` layout from Cursor exports — paths in prompts refer to the inner folder.
- Do not add zip files or loose markdown stubs in `_active/`; update this manifest instead.
- **Archive rule:** completed packages **must** leave `_active` — see [`../README.md#when-to-move-a-package-to-_consumed-required`](../README.md#when-to-move-a-package-to-_consumed-required).
