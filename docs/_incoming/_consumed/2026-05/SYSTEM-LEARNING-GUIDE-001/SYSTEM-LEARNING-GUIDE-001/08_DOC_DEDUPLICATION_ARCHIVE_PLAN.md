# 08 — Doc deduplication and archive plan

## Rules

- Keep repo-specific implementation details in the owning repo.
- Keep cross-system learning docs in `ontogony-platform/docs/learn`.
- Keep generated reports in `docs/generated` and link to them.
- Mark stale docs at top before archive/delete.
- Do not delete in SYSTEM-LEARNING-GUIDE-001 implementation unless explicitly approved later.

## Stale marker template

```md
> Status: superseded candidate  
> Canonical replacement: `ontogony-platform/docs/learn/...`  
> Do not use this file as current system truth until reconciled.
```

## Archive sequence

1. Identify duplicate/stale doc.
2. Add stale marker.
3. Add link from canonical guide if historical context remains useful.
4. Move to archive only in a later cleanup PR.
5. Delete only after references are removed and reviewed.
