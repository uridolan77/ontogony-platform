# Platform release governance

Mechanical governance for Ontogony.Platform **0.3.0-alpha.1** and Phase 1 system consumers. These documents encode **how to consume and change** platform packages — not product semantics (canonization, agent plans, provider routing, or business rules belong in Kanon, Allagma, or Conexus).

| Document | Purpose |
| --- | --- |
| [`kanon-ontogony-package-union.txt`](./kanon-ontogony-package-union.txt) | Sorted union of Ontogony packages referenced by Kanon.NET (`eng/Ontogony.References.props`); validated by `validate-kanon-ontogony-package-union.ps1` |
| [`PHASE1_CONSUMER_COMPATIBILITY.md`](./PHASE1_CONSUMER_COMPATIBILITY.md) | Phase 1 consumer matrix: Allagma, Kanon, Conexus — package lines, validation scripts, upgrade expectations |
| [`PACKAGE_COMPATIBILITY_CHECKLIST_0.3.0-alpha.1.md`](./PACKAGE_COMPATIBILITY_CHECKLIST_0.3.0-alpha.1.md) | Pre-merge / pre-tag checklist for the current alpha package line |
| [`PACKAGE_LEVEL_GOVERNANCE.md`](./PACKAGE_LEVEL_GOVERNANCE.md) | Golden map, forbidden edges, and change workflow (`validate-package-levels.ps1` + `package-levels.md`) |
| [`NUGET_SOURCE_MAPPING.md`](./NUGET_SOURCE_MAPPING.md) | Repo-level `nuget.config`, package source mapping, and consumer feed patterns |

## Related governance (outside this folder)

- [`TESTING.md`](../TESTING.md) — public API snapshot tests and CI gates
- [`PACKAGE_COMPATIBILITY_CHECKLIST_0.3.0-alpha.1.md`](./PACKAGE_COMPATIBILITY_CHECKLIST_0.3.0-alpha.1.md) — breaking-change checklist and snapshot refresh
- [`contracts/compatibility-policy.md`](../contracts/compatibility-policy.md) — wire contract compatibility policy
- [`docs/VERSION_COMPATIBILITY_MATRIX.md`](../VERSION_COMPATIBILITY_MATRIX.md) — proven baseline across packages and consumers
- [`docs/consumer-blueprints/README.md`](../consumer-blueprints/README.md) — per-consumer readiness and package-mode contracts
