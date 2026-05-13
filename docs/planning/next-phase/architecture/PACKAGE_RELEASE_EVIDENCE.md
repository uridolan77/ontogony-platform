# Package release evidence model

A package release is not accepted merely because `release-packages.yml` exists.

Track **PLAT-NP-001** in two parts: **001A** (workflow parity + cleanup + evidence scaffold on `main`) vs **001B** (real tag publish + filled evidence table). **001B is complete** for tag **`v0.3.0-alpha.1`** (`docs/releases/PR-PLAT-NP-001-release-parity-evidence.md`).

## Required evidence

For each first proof tag:

- tag name;
- workflow run URL;
- package version;
- generated `PACKAGE_MANIFEST.json`;
- list of pushed packages;
- GitHub Packages feed screenshot or CLI listing;
- GitHub Release URL;
- artifact hash check result;
- consumer smoke result.

## Release workflow parity

The release workflow must run all validation that protects the package line, including:

- dependency baseline;
- package levels;
- shipping inventory;
- AI runtime docs/boundaries;
- Conexus consumer baseline alignment;
- changelog strict validation;
- package manifest generation;
- Conexus package smoke;
- coordination path scan on packed `.nupkg` archives ([`validate-nupkg-coordination-path-hygiene.ps1`](../../../../scripts/validate-nupkg-coordination-path-hygiene.ps1); PLAT-NP-004).

## Symbols

`.snupkg` push may remain best-effort. `.nupkg` push must be hard-fail.
