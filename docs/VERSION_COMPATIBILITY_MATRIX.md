# Version Compatibility Matrix

This matrix captures the currently proven baseline for the Ontogony alpha package line.

## Current Baseline

| Area | Current proof / version | Evidence |
| --- | --- | --- |
| Ontogony package line | `0.3.0-alpha.1` | `CHANGELOG.md`, `PACKAGE_MANIFEST.json`, release pack flow |
| Shipping package count | 23 packages | `scripts/validate-shipping-inventory.ps1` |
| .NET SDK baseline | `9.0.100` | `global.json` |
| .NET runtime target | `net9.0` | package project files under `src/` |
| Release proof | `v0.3.0-alpha.1` tag publish | `docs/releases/PR-PLAT-NP-001-release-parity-evidence.md` |
| Conexus package-mode proof | real package-consumer CI job | `docs/consumer-blueprints/CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md` |

## Consumer Alignment Notes

| Consumer | Status | Notes |
| --- | --- | --- |
| `conexus-dotnet` | Proven against `0.3.0-alpha.1` package line | Package-mode proof is documented in the Conexus contract doc and validated by `validate-conexus-consumer-baseline-alignment.ps1`. |

## Operational Notes

- Security workflow proof for the current alpha line is tracked in `docs/security/PLAT-NP-003-supply-chain-first-run-evidence.md`.
- Public API governance for this line is documented in `docs/public-api-review.md`.
- Coverage collection is enabled in CI and uploaded as an artifact; thresholds remain deferred until the new test projects mature.

## Scope Limits

This matrix is a practical proof matrix for the current alpha line. It is not yet a full multi-target or cross-runtime support statement.
