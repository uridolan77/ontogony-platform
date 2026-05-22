# Six-repo post-lock delta register

**File:** `docs/system/ontogony-six-repo-post-lock-deltas.json`  
**Schema:** `ontogony-six-repo-post-lock-deltas-v1`

## Purpose

The six-repo lock pins specific commits. Between formal lock promotions, any of the six repos will advance. The delta register classifies those commits so that:

- Development mode: drift is visible but not blocking.
- Release/strict mode: unclassified drift blocks the gate.

## Entry format

```json
{
  "deltas": [
    {
      "repo": "ontogony-frontend",
      "commit": "abcdef1234567890...",
      "classification": "post-lock-dev",
      "summary": "Vitest upgrade — no API contract change",
      "addedAt": "2026-05-23T10:00:00Z",
      "addedBy": "uridolan77"
    }
  ]
}
```

### Classification values

| Value | Meaning |
|---|---|
| `post-lock-dev` | Development commit, safe to accept in dev mode. Blocks release gate until lock is promoted. |
| `post-lock-docs` | Documentation only. Does not affect contracts or hashes. |
| `post-lock-hotfix` | Emergency fix already reviewed; does not change the locked contract surface. |
| `lock-promotion-candidate` | This commit is the new lock candidate. Gate should promote the lock on the next release cycle. |

## Rules

1. Every commit beyond the pinned SHA in any repo must appear in this register before the gate runs in strict mode.
2. Only `lock-promotion-candidate` entries may appear when the lock is about to be promoted.
3. After a lock promotion, all prior delta entries are removed and `deltas` is reset to `[]`.
4. The delta register lives in `ontogony-platform` (the six-repo compatibility authority).

## Updating the lock (promotion workflow)

1. Verify all six repos' HEADs are the intended target commits.
2. Compute contract hashes:
   ```powershell
   ./scripts/run-six-repo-compatibility-gate.ps1 -DevRoot C:\dev -Update
   ```
3. Update `docs/system/ontogony-six-repo-lock.json` with new commits and hashes.
4. Reset `docs/system/ontogony-six-repo-post-lock-deltas.json` → `"deltas": []`.
5. Run gate in strict mode:
   ```powershell
   ./scripts/run-six-repo-compatibility-gate.ps1 -DevRoot C:\dev -ReleaseMode
   ```
