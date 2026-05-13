# PR-PLAT-NP-001 — Release workflow parity + first tag publish proof

## Goal

Close the remaining gap between release automation existing and release automation being proven.

## Scope

- Add `scripts/validate-conexus-consumer-baseline-alignment.ps1` to `release-packages.yml`.
- Clear `artifacts/packages` before pack so manifest validation never sees stale `.nupkg` files (explicit in `release-packages.yml`; also helps self-hosted runners).
- Confirm release workflow still runs all CI package gates.
- Run one tag publish, e.g. `v0.3.0-alpha.1` or next appropriate alpha.
- Record evidence in a new doc:
  - workflow run URL;
  - GitHub Packages feed entries;
  - GitHub Release URL;
  - manifest hash list;
  - Conexus package smoke result.

## Acceptance

- Manual dispatch still builds artifacts without publishing.
- Tag push publishes `.nupkg` files.
- `.snupkg` push remains non-fatal.
- `PACKAGE_MANIFEST.json` matches artifact files.
- Evidence document added under `docs/releases/`.

## Acceptance tracking (001A vs 001B)

Treat **PLAT-NP-001** as two slices so the backlog does not look “closed” when only code merged:

| ID | Done when |
| --- | --- |
| **PLAT-NP-001A** | Baseline script runs in `release-packages.yml`, **Clear package output** before pack, manifest script/evidence doc as needed; `workflow_dispatch` still does not publish. |
| **PLAT-NP-001B** | A tag-triggered run completed; evidence table in `docs/releases/PR-PLAT-NP-001-release-parity-evidence.md` filled (run URL, feed, Release, manifest vs hashes, smoke). **Do not mark full NP-001 complete until 001B is done.** |

## Status (repo)

**Closed.** 001A and 001B are done; operational proof for tag **`v0.3.0-alpha.1`** is in [`docs/releases/PR-PLAT-NP-001-release-parity-evidence.md`](../../../releases/PR-PLAT-NP-001-release-parity-evidence.md).

## Non-goals

- Do not add new platform runtime APIs.
- Do not change package inventory unless required by an unrelated approved PR.
