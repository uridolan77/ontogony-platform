# PR-PLAT-NP-001 — Release workflow parity + first tag publish proof

## Goal

Close the remaining gap between release automation existing and release automation being proven.

## Scope

- Add `scripts/validate-conexus-consumer-baseline-alignment.ps1` to `release-packages.yml`.
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

## Non-goals

- Do not add new platform runtime APIs.
- Do not change package inventory unless required by an unrelated approved PR.
