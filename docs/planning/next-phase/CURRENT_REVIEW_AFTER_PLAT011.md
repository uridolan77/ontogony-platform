# Ontogony.Platform review after PR-PLAT-011

## Current judgment

Ontogony.Platform is now a credible mechanical substrate for Conexus.NET:

- package publishing workflow exists;
- Conexus package-consumer smoke exists;
- public API approval tests exist;
- dependency baseline validation exists;
- XML docs are enforced for the Conexus consumer surface;
- supply-chain workflows exist;
- in-memory non-durable warnings are implemented;
- generic secret-value resolution exists.

## Status by robustness PR

| PR | Status | Judgment |
| --- | --- | --- |
| PR-PLAT-001 | Implemented | Package release workflow exists. **PLAT-NP-001B** first tag publish proof is recorded ([`docs/releases/PR-PLAT-NP-001-release-parity-evidence.md`](../../releases/PR-PLAT-NP-001-release-parity-evidence.md), `v0.3.0-alpha.1`). |
| PR-PLAT-002 | Implemented | Package-consumer smoke exists. Full real Conexus repo package-mode test remains open. |
| PR-PLAT-003 | Deferred / not implemented | Typed HTTP client support was not needed by Conexus because existing integration HTTP client pipeline was sufficient. Keep deferred. |
| PR-PLAT-004 | Implemented | Public API approval snapshots are in place. |
| PR-PLAT-005 | Implemented | Dependency baseline script and policy are in place. |
| PR-PLAT-006 | Implemented as docs expansion | XML docs were expanded across consumer-facing APIs. |
| PR-PLAT-006.1 | Implemented | `src/Directory.Build.targets` strips CS1591 suppression for the 16 Conexus consumer packages only. |
| PR-PLAT-007 | Implemented | CodeQL, dependency review, dependency submission, and supply-chain workflows exist. |
| PR-PLAT-007.1 | Implemented as workflow definition | Stabilization changes are present. Green run proof should still be recorded. |
| PR-PLAT-008 | Not implemented | Donor folder hygiene remains open if donor material is still present. |
| PR-PLAT-009 | Implemented | Non-durable in-memory store startup warnings are wired for key in-memory stores. |
| PR-PLAT-010 | Not separately verified | README resilience contradiction appears largely superseded by prior documentation cleanup, but no explicit PR-PLAT-010 closeout exists. |
| PR-PLAT-011 | Implemented | `ISecretValueResolver`, env resolver, composite resolver, DI extension, docs, tests, and public API updates exist. |
| PR-PLAT-012 | Deferred / not implemented | Durable quota ledger design spike remains deferred; Conexus currently owns its EF quota ledger. |

## Important caveat

The platform is structurally strong, but at least one item is still operational rather than purely code-level:

1. First green run proof for CodeQL / Supply chain / Dependency submission after the latest workflow fixes (see PLAT-NP-003).

**PLAT-NP-001B** (first package-tag publish to GitHub Packages with filled evidence) is **closed** — see [`docs/releases/PR-PLAT-NP-001-release-parity-evidence.md`](../../releases/PR-PLAT-NP-001-release-parity-evidence.md).

## Top risks now

1. Release workflow: Conexus baseline alignment and pre-pack `artifacts/packages` cleanup are in `release-packages.yml` on `main`; tag publish proof for **`v0.3.0-alpha.1`** is recorded in the NP-001 evidence doc.
2. Package adoption gap: Conexus still compiles sibling source; real package-mode consumption belongs in Conexus CI or a multi-checkout compatibility workflow.
3. Over-platforming risk: do not add provider, model, routing, price catalog, or gateway semantics to Ontogony.
4. Security workflow fragility: Trivy/Gitleaks/dependency submission can fail because of environment/permissions/action behavior, not design.
5. Donor/incoming package hygiene: `_agent_prompts`, `_issue_bodies`, and any donor/source overlay material should not pollute the distributable package surface.

## Immediate recommendation

Proceed in this order:

1. PLAT-NP-002 — real Conexus package-mode compatibility (NP-001 is closed).
2. PLAT-NP-003 — supply-chain first-run evidence and stabilization.
3. PLAT-NP-004 — donor/incoming package hygiene.
4. PLAT-NP-005 — README/docs final accuracy pass.
5. PLAT-NP-006 — explicit deferred-items register for PR-PLAT-003 and PR-PLAT-012.
