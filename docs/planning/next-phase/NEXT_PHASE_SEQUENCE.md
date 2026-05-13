# Ontogony.Platform next-phase sequence

## Phase A — prove the platform can be consumed and shipped

1. **PLAT-NP-001 — Release workflow parity + first tag publish proof**
   - Add Conexus baseline alignment validation to `release-packages.yml`.
   - Perform one tag publish.
   - Record feed entries, package manifest, artifact hashes, and GitHub Release attachments.

2. **PLAT-NP-002 — Real Conexus package-mode compatibility**
   - Prefer implementation in `conexus-dotnet` CI.
   - Build/test Conexus against packed or published Ontogony packages, not sibling source.
   - Keep the platform-owned smoke test, but do not confuse it with full Conexus compatibility.

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
