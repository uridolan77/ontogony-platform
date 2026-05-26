# Active intake manifest

Last updated: **2026-05-26**

| Package | Status | Reason active | Next action | Date |
| --- | --- | --- | --- | --- |
| [ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001](./ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001/) | **in_progress** | Cross-repo decision reconstructability spine; platform protocol docs + optional validation script pending | Complete Task 5 (platform/docs) after Kanon/Allagma/Conexus/frontend slices; **then move to `_consumed`** per [`../README.md`](../README.md) | 2026-05-26 |

## Above-9 platform work (not a separate active package)

Six-repo score-lift and remaining **PLAT-9-003 / 004 / 005** items live in the consumed archive:

[`../_consumed/2026-05/SIX-REPO-SCORE-PLANS/`](../_consumed/2026-05/SIX-REPO-SCORE-PLANS/) — see [`README.md`](../_consumed/2026-05/SIX-REPO-SCORE-PLANS/README.md) for platform status vs plan.

The former active stub `ONTOGONY-BACKEND-ABOVE9-PACKAGES` was empty (zip removed during intake cleanup) and was removed.

## Notes

- Unpacked trees may use a nested `<PACKAGE-NAME>/<PACKAGE-NAME>/` layout from Cursor exports — paths in prompts refer to the inner folder.
- Do not add zip files or loose markdown stubs in `_active/`; update this manifest instead.
- **Archive rule:** completed packages **must** leave `_active` — see [`../README.md#when-to-move-a-package-to-_consumed-required`](../README.md#when-to-move-a-package-to-_consumed-required).
