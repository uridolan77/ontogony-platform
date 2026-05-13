# Ontogony.Platform next-phase sequence

## Phase A — prove the platform can be consumed and shipped

1. **PLAT-NP-001 — Release workflow parity + first tag publish proof** — **closed**
   - **001A (done):** Conexus baseline alignment validation in `release-packages.yml`; pre-pack cleanup of `artifacts/packages`; evidence doc + manifest script fixes on `main`.
   - **001B (done):** Tag `v0.3.0-alpha.1` published; evidence table filled in [`docs/releases/PR-PLAT-NP-001-release-parity-evidence.md`](../../releases/PR-PLAT-NP-001-release-parity-evidence.md) (workflow run, Packages feed, Release, manifest vs artifacts, Conexus smoke).

2. **PLAT-NP-002 — Real Conexus package-mode compatibility**
   - **Conexus CI:** `conexus-ontogony-package-mode` job packs Ontogony from a non-sibling path, adds a local feed, then restore/build/test with `UseOntogonyPackages=true` (see `conexus-dotnet` repo).
   - **Contract:** `docs/consumer-blueprints/CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md` (this repo).

3. **PLAT-NP-003 — Security workflow first-run evidence**
   - Record successful CodeQL, Supply chain, Dependency submission, and PR-only Dependency Review behavior.
   - Stabilize permissions/action versions if needed.

## Phase B — repo hygiene and documentation accuracy

4. **PLAT-NP-004 — Donor and incoming-package hygiene**
   - Ensure planning overlays, donor folders, and agent prompts cannot enter shipped packages.
   - Keep coordination artifacts clearly separated from source/package artifacts.

5. **PLAT-NP-005 — README/docs final accuracy pass**
   - Remove stale statements about release mode, resilience, package counts, and Conexus readiness.
   - Keep docs aligned with `Directory.Build.targets`, `CHANGELOG.md`, and package inventory.

6. **PLAT-NP-006 — Deferred item register**
   - Explicitly mark PR-PLAT-003 and PR-PLAT-012 as deferred unless current Conexus pressure proves a platform need.
   - Avoid silent "not done but still listed" ambiguity.

## Phase C — measured capability additions

7. **PLAT-NP-007 — Secret resolver parsing and diagnostics**
   - Add a small parser/helper for strings like `env:NAME`, `vault:path`, etc., without adding any cloud SDK.
   - Keep actual vault implementations external.

8. **PLAT-NP-008 — In-memory warning coverage expansion**
   - Verify all platform-provided in-memory implementations either warn outside Development or are clearly harmless.
   - Add tests for every registered warning, not only artifacts.

9. **PLAT-NP-009 — Public API review gate hardening**
   - Add a checklist requiring changelog + migration note for intentional public API snapshot updates.

## Explicit non-goals

- No Conexus model/provider/route/pricing logic.
- No Anthropic/Gemini/OpenRouter semantics.
- No generic "AI gateway" product policy.
- No durable quota/platform store unless multiple consumers create the same pressure.
