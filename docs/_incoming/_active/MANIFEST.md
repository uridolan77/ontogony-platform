# Active intake manifest

Last updated: **2026-05-26**

| Package | Status | Reason active | Next action | Date |
| --- | --- | --- | --- | --- |
| [ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001](./ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001/) | **in_progress** | Cross-repo decision reconstructability spine; platform protocol docs + optional validation script pending | Complete Task 5 (platform/docs) after Kanon/Allagma/Conexus/frontend slices; **then move to `_consumed`** per [`../README.md`](../README.md) | 2026-05-26 |
| [ONTOGONY-BACKEND-ABOVE9-PACKAGES](./ONTOGONY-BACKEND-ABOVE9-PACKAGES/) | **planned** | Six-repo score lift plans (platform/conexus/kanon/allagma above 9) | Pick next PLAT-9 slice; **move to `_consumed`** when promoted or superseded | 2026-05-26 |

## Notes

- Unpacked trees may use a nested `<PACKAGE-NAME>/<PACKAGE-NAME>/` layout from Cursor exports — paths in prompts refer to the inner folder.
- Do not add zip files or loose markdown stubs in `_active/`; update this manifest instead.
- **Archive rule:** completed packages **must** leave `_active` — see [`../README.md#when-to-move-a-package-to-_consumed-required`](../README.md#when-to-move-a-package-to-_consumed-required).
