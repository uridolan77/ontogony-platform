# Master implementation sequence

## Phase 0 — Baseline and branch

1. Create branch: `feature/ONTOGONY-PLATFORM-MECHANICS-ONLY-CONFORMANCE-001`.
2. Run existing baseline:
   ```powershell
   dotnet restore Ontogony.Platform.sln
   dotnet build Ontogony.Platform.sln --no-restore
   dotnet test Ontogony.Platform.sln --no-build
   .\scripts\validate-package-levels.ps1
   .\scripts\validate-shipping-inventory.ps1
   .\scripts\check-no-product-semantics.ps1
   ```
3. Save baseline evidence under:
   `artifacts/platform-mechanics-conformance/<stamp>/baseline/`.

## Phase 1 — Mechanics-only proposal gate

Add:

- `docs/governance/PLATFORM_MECHANICS_ONLY_PROPOSAL_GATE.md`
- `schemas/mechanics/v1/platform-proposal-gate.schema.json`
- `scripts/governance/check-platform-mechanics-only.ps1`
- tests proving allowed/forbidden proposals.

Acceptance: `PLAT-MECH-001` to `PLAT-MECH-003`.

## Phase 2 — Mechanical schema registry

Add:

- `docs/contracts/MECHANICAL_SCHEMA_REGISTRY_V1.md`
- versioned schemas under `schemas/mechanics/v1/`
- schema examples under `fixtures/mechanics/`
- validation script `scripts/conformance/Test-MechanicalSchemaRegistry.ps1`.

Acceptance: `PLAT-MECH-004` to `PLAT-MECH-006`.

## Phase 3 — Consumer conformance suite

Add:

- `docs/conformance/CONSUMER_CONFORMANCE_SUITE_V1.md`
- `scripts/conformance/run-consumer-conformance-suite.ps1`
- one focused script per conformance dimension;
- machine-readable summary schema;
- repo-local instructions for each consumer.

Acceptance: `PLAT-MECH-007` to `PLAT-MECH-011`.

## Phase 4 — Observability and no-product-semantics hardening

Add:

- observability meter naming contract;
- forbidden semantic token lists;
- allow-list for neutral words;
- cross-repo scan mode;
- evidence report output.

Acceptance: `PLAT-MECH-012` to `PLAT-MECH-014`.

## Phase 5 — Consumer adoption task cards

Add or update docs for:

- Conexus;
- Kanon;
- Allagma;
- Metabole;
- Aisthesis.

Each consumer must know which scripts to run and where to commit its conformance summary.

Acceptance: `PLAT-MECH-015`.

## Phase 6 — Closeout

1. Run full Platform validation.
2. Run conformance suite in fixture mode.
3. If local sibling repos exist, run read-only or non-mutating consumer checks.
4. Commit:
   - closeout evidence;
   - generated conformance summary;
   - deferral index.
5. Do not claim production readiness.

Acceptance: `PLAT-MECH-016`.
