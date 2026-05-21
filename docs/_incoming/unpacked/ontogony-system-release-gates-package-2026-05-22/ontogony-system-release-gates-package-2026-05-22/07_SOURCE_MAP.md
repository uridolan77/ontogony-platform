# Source map

This package was designed against the current repo conventions below.

## Allagma

Important current files:

```text
docs/system/SYSTEM_COMPATIBILITY_MATRIX.md
docs/system/ontogony-runtime.lock.json
.github/workflows/ci.yml
scripts/pack-cross-repo-packages.ps1
scripts/run-system-cohesion-smoke.ps1
scripts/restart-e2e-first-real-system.ps1
```

Allagma currently owns runtime lock and package-mode proof.

## Conexus

Important current files:

```text
docs/reviews/CONEXUS_NEXT_009_CAPACITY_AND_RESILIENCE_BASELINE.md
docs/testing/CAPACITY_BASELINE.md
scripts/capacity/run-capacity-baseline.ps1
.github/workflows/capacity-baseline.yml
```

Conexus owns gateway capacity thresholds and reports.

## Kanon

Important current files:

```text
docs/generated/KANON_COMPATIBILITY_MANIFEST.json
docs/contracts/KANON_V0_CONTRACT_FREEZE.md
docs/api/kanon-openapi-v1.json
scripts/update-kanon-compatibility-manifest.ps1
scripts/update-kanon-openapi-baseline.ps1
```

Kanon owns semantic contract gates and compatibility manifest.

## Platform

Important current files:

```text
docs/governance/
docs/consumer-blueprints/
docs/VERSION_COMPATIBILITY_MATRIX.md
scripts/pack-all.ps1
```

Platform owns release mechanics and shared package governance.
