# Documentation intake (`docs/_incoming`)

Cursor packages and cross-repo handoff specs land here **temporarily**. They are not canonical platform documentation.

## Policy

| Rule | Detail |
| --- | --- |
| Allowed direct children | `_active/`, `_consumed/`, this `README.md` only |
| No zips | Unpack into a package folder, then delete the archive |
| No loose package folders | Every package lives under `_active` or `_consumed` |
| Active work | `docs/_incoming/_active/<PACKAGE-NAME>/` |
| Completed / superseded | `docs/_incoming/_consumed/<YYYY-MM>/<PACKAGE-NAME>/` |
| Canonical docs | Promote durable decisions into `docs/contracts/`, `docs/operators/`, `docs/packages/`, etc., then **move the package to `_consumed`** |

---

## When to move a package to `_consumed` (required)

Move a package out of `_active` when **any** of the following is true:

| Trigger | Example |
| --- | --- |
| **Implemented** | Acceptance checklist passed; evidence or closeout recorded in `docs/evidence/` or `docs/reviews/` |
| **Promoted** | Durable rules live in canonical `docs/contracts/`, `docs/operators/`, or `docs/packages/` |
| **Superseded** | A newer package or ADR replaces this handoff |
| **Abandoned** | Scope dropped; keep folder only as historical context |
| **Wrong repo** | Package belongs in a product repo — move or delete here after copying to the owner |

**Do not** leave completed packages in `_active`. Historical intake is **read-only** under `_consumed/`.

### Archive procedure (do in order)

1. **Promote** — Copy any still-useful decisions into canonical docs (contracts, migrations, package pages). Link from canonical docs to the archive path if history matters.
2. **Record closure** — Add or update `docs/evidence/*` or `docs/reviews/*` when the program had a gate.
3. **Move the folder:**
   ```powershell
   $pkg = "MY-PACKAGE-001"
   $month = "2026-05"   # YYYY-MM of archive
   Move-Item "docs\_incoming\_active\$pkg" "docs\_incoming\_consumed\$month\$pkg"
   ```
4. **Mark consumed** — Add `CONSUMED.md` in the package root (date, reason, link to canonical docs).
5. **Update manifests** — Remove the row from [`_active/MANIFEST.md`](./_active/MANIFEST.md); add a row to [`_consumed/MANIFEST.md`](./_consumed/MANIFEST.md).
6. **Fix links** — Search repo for `docs/_incoming/_active/<PACKAGE>` and point to `_consumed/<YYYY-MM>/` or canonical paths.
7. **Delete zips** — Never leave `.zip` under `_incoming`.

Cross-repo packages: archive in **this** repo when **this repo’s slice** is done, even if sister repos still hold their own copy. Note sibling ownership in `CONSUMED.md`.

---

## Where to look

| Need | Path |
| --- | --- |
| Work in progress | [`_active/MANIFEST.md`](./_active/MANIFEST.md) |
| Historical packages | [`_consumed/MANIFEST.md`](./_consumed/MANIFEST.md) |
| Platform docs index | [`../INDEX.md`](../INDEX.md) |
| Current platform truth | [`../CURRENT_STATE.md`](../CURRENT_STATE.md) |
| Six-repo doc standard | [`../operators/ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md`](../operators/ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md) §8.4 |

## Boundary

Ontogony.Platform owns **mechanics** (tracing, errors, HTTP, hashing, idempotency, observability contracts, packaging). Intake may mention Kanon, Allagma, Conexus, or the operator console only as **consumers** or examples — not as product specs owned by this repo.
