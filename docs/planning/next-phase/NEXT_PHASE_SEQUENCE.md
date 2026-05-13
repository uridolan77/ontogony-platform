# Ontogony.Platform next-phase sequence

## Phase A — prove the platform can be consumed and shipped

1. **PLAT-NP-001 — Release workflow parity + first tag publish proof** — **closed**
   - **001A (done):** Conexus baseline alignment validation in `release-packages.yml`; pre-pack cleanup of `artifacts/packages`; evidence doc + manifest script fixes on `main`.
   - **001B (done):** Tag `v0.3.0-alpha.1` published; evidence table filled in [`docs/releases/PR-PLAT-NP-001-release-parity-evidence.md`](../../releases/PR-PLAT-NP-001-release-parity-evidence.md) (workflow run, Packages feed, Release, manifest vs artifacts, Conexus smoke).

2. **PLAT-NP-002 — Real Conexus package-mode compatibility** — **closed**
   - **Conexus CI:** `conexus-ontogony-package-mode` job packs Ontogony from a non-sibling path, adds a local feed, then restore/build/test with `UseOntogonyPackages=true` (see `conexus-dotnet` repo).
   - **Contract + proof:** [`docs/consumer-blueprints/CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md`](../../consumer-blueprints/CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md) (green job URL in doc).

3. **PLAT-NP-003 — Security workflow first-run evidence** — **partially complete**
   - CodeQL, supply chain (incl. SBOM upload), and action pins are proven in [`docs/security/PLAT-NP-003-supply-chain-first-run-evidence.md`](../../security/PLAT-NP-003-supply-chain-first-run-evidence.md).
   - **Owner follow-up:** enable **Dependency graph** on the GitHub repo, then re-run Dependency submission and replace the evidence link with a green run (see evidence doc).

## Phase B — repo hygiene and documentation accuracy

4. **PLAT-NP-004 — Donor and incoming-package hygiene** — **closed**
   - **`scripts/validate-nupkg-coordination-path-hygiene.ps1`** runs in CI and release after pack; fails if `.nupkg` entries contain coordination path fragments.
   - **Docs:** [`docs/packages/index.md`](../../packages/index.md) states planning vs shipped separation.

5. **PLAT-NP-005 — README/docs final accuracy pass** — **closed**
   - Top-level docs aligned with current package inventory and consumer docs (including `docs/start-here.md` vs 23-package catalog).

6. **PLAT-NP-006 — Deferred item register** — **closed**
   - [`docs/planning/robustness/DEFERRED_ITEMS.md`](../robustness/DEFERRED_ITEMS.md) and status table in [`PLAT_ROBUSTNESS_SEQUENCE.md`](../robustness/PLAT_ROBUSTNESS_SEQUENCE.md).

## Phase C — measured capability additions

7. **PLAT-NP-007 — Secret resolver parsing and diagnostics** — **closed**
   - `SecretValueReferenceParser` in `Ontogony.Secrets` (see [`pr-specs/PR-PLAT-NP-007-secret-reference-parser.md`](./pr-specs/PR-PLAT-NP-007-secret-reference-parser.md)); vault resolution remains external.

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
