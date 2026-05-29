# Package manifest — ONTOGONY-PLATFORM-MECHANICS-ONLY-CONFORMANCE-001

| Field | Value |
|---|---|
| Package ID | `ONTOGONY-PLATFORM-MECHANICS-ONLY-CONFORMANCE-001` |
| Repository | `uridolan77/ontogony-platform` |
| Package type | Dev implementation package |
| Target | Mechanical-only governance + consumer conformance + schema contracts |
| Acceptance prefix | `PLAT-MECH-*` |
| Main closeout | `docs/evidence/ONTOGONY_PLATFORM_MECHANICS_ONLY_CONFORMANCE_001_CLOSEOUT.md` |

## Options covered

| Option | Implementation spine |
|---|---|
| Option A — Keep Platform mechanical-only | Proposal gate, semantic-leak tests, ownership routing rules |
| Option B — Strengthen consumer conformance | Harnesses for headers, errors, idempotency, outbox/artifacts, observability meters, no-product-semantics |
| Option D — Cross-service contract schemas | Versioned schema registry for common mechanical contracts |

## Consumer repos

- `conexus-dotnet`
- `kanon-dotnet`
- `allagma-dotnet`
- `metabole-dotnet`
- `aisthesis-dotnet`

## Core output artifacts

- `docs/governance/PLATFORM_MECHANICS_ONLY_PROPOSAL_GATE.md`
- `docs/conformance/CONSUMER_CONFORMANCE_SUITE_V1.md`
- `docs/contracts/MECHANICAL_SCHEMA_REGISTRY_V1.md`
- `schemas/mechanics/v1/*.schema.json`
- `scripts/conformance/run-consumer-conformance-suite.ps1`
- `scripts/governance/check-platform-mechanics-only.ps1`
- `artifacts/platform-mechanics-conformance/<timestamp>/summary.json`
