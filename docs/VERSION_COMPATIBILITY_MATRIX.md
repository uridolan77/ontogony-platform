# Version Compatibility Matrix

This matrix captures the currently proven baseline for the Ontogony alpha package line.

## Current Baseline

| Area | Current proof / version | Evidence |
| --- | --- | --- |
| Ontogony package line | `0.3.0-alpha.1` | `CHANGELOG.md`, `PACKAGE_MANIFEST.json`, release pack flow |
| Shipping package count | 27 packages | `scripts/validate-shipping-inventory.ps1` |
| .NET SDK baseline | `9.0.100` | `global.json` |
| .NET runtime target | `net9.0` | package project files under `src/` |
| Release proof | `v0.3.0-alpha.1` tag publish | `docs/releases/PR-PLAT-NP-001-release-parity-evidence.md` |
| Conexus package-mode proof | real package-consumer CI job | `docs/consumer-blueprints/CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md` |

## Consumer Alignment Notes

| Consumer | Status | Notes |
| --- | --- | --- |
| `conexus-dotnet` | Proven against `0.3.0-alpha.1` package line | Package-mode proof in [`CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md`](consumer-blueprints/CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md); CI script `validate-conexus-consumer-baseline-alignment.ps1`. |
| `allagma-dotnet` | Proven against `0.3.0-alpha.1` package line | Readiness in [`allagma-dotnet-platform-readiness.md`](consumer-blueprints/allagma-dotnet-platform-readiness.md); CI script `validate-allagma-consumer-baseline-alignment.ps1`; skeleton `examples/AllagmaDotNetSkeleton`. |
| `kanon-dotnet` | Proven via sibling + package-mode CI | Package set in Kanon `eng/Ontogony.References.props`; no platform-owned Kanon blueprint — see [`PHASE1_CONSUMER_COMPATIBILITY.md`](governance/PHASE1_CONSUMER_COMPATIBILITY.md). |

## Operational Notes

- Security workflow proof for the current alpha line is tracked in `docs/security/PLAT-NP-003-supply-chain-first-run-evidence.md`.
- Public API governance for this line is documented in `docs/public-api-review.md` and `docs/planning/robustness/PUBLIC_API_COMPATIBILITY.md`.
- Phase 1 governance index: `docs/governance/README.md`.
- Coverage collection is enabled in CI and uploaded as an artifact; thresholds remain deferred until the new test projects mature.

## Scope Limits

This matrix is a practical proof matrix for the current alpha line. It is not yet a full multi-target or cross-runtime support statement.
