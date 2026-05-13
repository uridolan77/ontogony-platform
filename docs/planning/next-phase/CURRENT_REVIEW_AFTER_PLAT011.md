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
| PR-PLAT-002 | Implemented | Package-consumer smoke exists. **PLAT-NP-002** real Conexus package-mode job + contract documented ([`CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md`](../../consumer-blueprints/CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md)). |
| PR-PLAT-003 | Deferred / not implemented | Typed HTTP client support was not needed by Conexus because existing integration HTTP client pipeline was sufficient. Keep deferred. |
| PR-PLAT-004 | Implemented | Public API approval snapshots are in place. |
| PR-PLAT-005 | Implemented | Dependency baseline script and policy are in place. |
| PR-PLAT-006 | Implemented as docs expansion | XML docs were expanded across consumer-facing APIs. |
| PR-PLAT-006.1 | Implemented | `src/Directory.Build.targets` strips CS1591 suppression for the 16 Conexus consumer packages only. |
| PR-PLAT-007 | Implemented | CodeQL, dependency review, dependency submission, and supply-chain workflows exist. |
| PR-PLAT-007.1 | Implemented as workflow definition | Stabilization changes are present. Green run proof should still be recorded. |
| PR-PLAT-008 | Partially addressed | **PLAT-NP-004** adds CI scanning of packed `.nupkg` for coordination path fragments; donor tree hygiene in-repo remains a docs/process concern. |
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
2. Package adoption gap: default Conexus dev layout still uses sibling `ProjectReference`; **package-mode** is proven in CI (`conexus-ontogony-package-mode`) per [`CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md`](../../consumer-blueprints/CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md).
3. Over-platforming risk: do not add provider, model, routing, price catalog, or gateway semantics to Ontogony.
4. Security workflow fragility: Trivy/Gitleaks/dependency submission can fail because of environment/permissions/action behavior, not design.
5. **PLAT-NP-004** packs are scanned in CI/release for forbidden coordination paths; keep planning overlays out of `src/` pack patterns.

## Immediate recommendation

Proceed in this order:

1. PLAT-NP-003 — finish **Dependency submission** after enabling Dependency graph (see evidence doc); keep other security runs green.
2. ~~PLAT-NP-005 — README/docs final accuracy pass.~~ **Done** (ongoing: keep docs aligned when packages or baselines change).
3. ~~PLAT-NP-006 — explicit deferred-items register for PR-PLAT-003 and PR-PLAT-012.~~ **Done** — see [`docs/planning/robustness/DEFERRED_ITEMS.md`](../robustness/DEFERRED_ITEMS.md) and [`PLAT_ROBUSTNESS_SEQUENCE.md`](../robustness/PLAT_ROBUSTNESS_SEQUENCE.md).
4. ~~PLAT-NP-007 — secret reference parser.~~ **Done** — `SecretValueReferenceParser` in `Ontogony.Secrets`.

## PLAT-NP next-phase status (backlog-aligned)

| ID | Status | Notes |
| --- | --- | --- |
| PLAT-NP-001 | Closed | Tag `v0.3.0-alpha.1` release proof |
| PLAT-NP-002 | Closed | Conexus `conexus-ontogony-package-mode` + contract |
| PLAT-NP-003 | Open (partial) | Submission blocked by Dependency graph setting; see evidence doc |
| PLAT-NP-004 | Closed | Packed `.nupkg` coordination-path scan |
| PLAT-NP-005 | Closed | Docs accuracy baseline; ongoing maintenance |
| PLAT-NP-006 | Closed | `DEFERRED_ITEMS.md` register |
| PLAT-NP-007 | Closed | `SecretValueReferenceParser` |
| PLAT-NP-008 | Open | In-memory warning coverage expansion |
| PLAT-NP-009 | Open | Public API change checklist hardening |

Source of truth: [`backlog.json`](./backlog.json) and [`NEXT_PHASE_SEQUENCE.md`](./NEXT_PHASE_SEQUENCE.md).
