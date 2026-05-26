# Platform evidence index

Verification records for **platform-owned mechanical gates** only. Filename pattern: `<ITEM>_EVIDENCE.md`.

**Boundary:** Evidence here validates platform substrate and documentation governance. Cross-repo program evidence (PFH, Kanon deepen, UI harden, etc.) lives in the **owning repos**.

**System runtime baseline:** [`allagma-dotnet/docs/evidence/README.md`](https://github.com/uridolan77/allagma-dotnet/blob/main/docs/evidence/README.md) — Platform does not duplicate runtime lock tables.

---

## Cross-repo operator proofs

| Item | File |
| --- | --- |
| GOVERNED-FAKE-REPLAY-E2E-001 (2026-05-25 closure) | [GOVERNED_FAKE_REPLAY_E2E_001_PASS_20260525T081956Z.md](./GOVERNED_FAKE_REPLAY_E2E_001_PASS_20260525T081956Z.md) |
| GOVERNED-FAKE-E2E-001 (2026-05-24 closure) | [GOVERNED_FAKE_E2E_001_PASS_20260524T102932Z.md](./GOVERNED_FAKE_E2E_001_PASS_20260524T102932Z.md) |
| GOVERNED-FAKE-E2E-001 (2026-05-23 smoke) | [GOVERNED_FAKE_E2E_001_PASS_20260523T232255Z.md](./GOVERNED_FAKE_E2E_001_PASS_20260523T232255Z.md) |
| KANON-UI-API-PARITY-001 (2026-05-24 closure) | [KANON_UI_API_PARITY_001_PASS_20260524T162641Z.md](./KANON_UI_API_PARITY_001_PASS_20260524T162641Z.md) |

Artifacts:

- [`artifacts/governed-fake-e2e/`](./artifacts/governed-fake-e2e/) — committed baselines for `-RequireGovernedFakeE2eEvidence` ([`README`](./artifacts/governed-fake-e2e/README.md)).
- [`artifacts/governed-fake-replay-e2e/`](./artifacts/governed-fake-replay-e2e/) — committed baselines for `-RequireGovernedFakeReplayEvidence` ([`README`](./artifacts/governed-fake-replay-e2e/README.md)).

Ephemeral runs use repo-root `/artifacts/` in Allagma.

KANON-UI-API-PARITY-001: [`artifacts/kanon-ui-api-parity-001/`](./artifacts/kanon-ui-api-parity-001/) — Domain Switcher Docker-live smoke JSON (see [`artifacts/kanon-ui-api-parity-001/README.md`](./artifacts/kanon-ui-api-parity-001/README.md)).

---

## PLAT-9 score lift (`ontogony-platform`, done 2026-05-26)

| Item | File |
| --- | --- |
| PLAT-9-001 Six-repo compatibility gate | [PLAT_9_001_SIX_REPO_COMPATIBILITY_GATE_EVIDENCE.md](./PLAT_9_001_SIX_REPO_COMPATIBILITY_GATE_EVIDENCE.md) |
| PLAT-9-003 Consumer / reconstructability conformance | [PLATFORM_RECONSTRUCTABILITY_CONFORMANCE_EVIDENCE.md](./PLATFORM_RECONSTRUCTABILITY_CONFORMANCE_EVIDENCE.md) |
| PLAT-9-004 Public API hardening (Tier A XML) | [PLAT_9_004_PUBLIC_API_HARDENING_EVIDENCE.md](./PLAT_9_004_PUBLIC_API_HARDENING_EVIDENCE.md) |
| PLAT-9-005 Observability mechanics (phase 1) | [PLAT_9_005_OBSERVABILITY_MECHANICS_EVIDENCE.md](./PLAT_9_005_OBSERVABILITY_MECHANICS_EVIDENCE.md) |
| PLAT-9-005 Observability mechanics (phase 2) | [PLAT_9_005_OBSERVABILITY_MECHANICS_PHASE2_EVIDENCE.md](./PLAT_9_005_OBSERVABILITY_MECHANICS_PHASE2_EVIDENCE.md) |
| Program index | [SIX-REPO-SCORE-PLANS README](../_incoming/_consumed/2026-05/SIX-REPO-SCORE-PLANS/README.md) |

---

## Implementation depth (PLAT-DEPTH)

| Item | File |
| --- | --- |
| PLAT-DEPTH-001 HTTP resilience | [PLAT_DEPTH_001_HTTP_RESILIENCE_EVIDENCE.md](./PLAT_DEPTH_001_HTTP_RESILIENCE_EVIDENCE.md) |
| PLAT-DEPTH-002 Conformance harnesses | [PLAT_DEPTH_002_CONFORMANCE_HARNESSES_EVIDENCE.md](./PLAT_DEPTH_002_CONFORMANCE_HARNESSES_EVIDENCE.md) |
| PLAT-DEPTH-003 Compatibility gate | [PLAT_DEPTH_003_COMPATIBILITY_GATE_EVIDENCE.md](./PLAT_DEPTH_003_COMPATIBILITY_GATE_EVIDENCE.md) |
| PLAT-DEPTH-004 Tier A XML (baseline slice) | [PLAT_DEPTH_004_TIER_A_DOCS_EVIDENCE.md](./PLAT_DEPTH_004_TIER_A_DOCS_EVIDENCE.md) |
| Closeout report | [IMPLEMENTATION_DEPTH_OVER9_CLOSEOUT_REPORT.md](../reviews/IMPLEMENTATION_DEPTH_OVER9_CLOSEOUT_REPORT.md) |

---

## Platform gates

| Item | File |
| --- | --- |
| Phase Tight (PLATFORM-9-001/002/003) | [PLATFORM_PHASE_TIGHT_2026_05_22_EVIDENCE.md](./PLATFORM_PHASE_TIGHT_2026_05_22_EVIDENCE.md) |
| Substrate RC freeze (PLATFORM-RC-001) | [PLATFORM_RC_001_SUBSTRATE_CONTRACT_FREEZE_EVIDENCE.md](./PLATFORM_RC_001_SUBSTRATE_CONTRACT_FREEZE_EVIDENCE.md) |
| Protocol registry | [SYSTEM_PROTOCOL_REGISTRY_001_EVIDENCE.md](./SYSTEM_PROTOCOL_REGISTRY_001_EVIDENCE.md) |
| Trace contract | [TRACE_CONTRACT_001_EVIDENCE.md](./TRACE_CONTRACT_001_EVIDENCE.md) |
| Post-lock delta register | [SYS_POSTLOCK_DELTA_REGISTER_001_EVIDENCE.md](./SYS_POSTLOCK_DELTA_REGISTER_001_EVIDENCE.md) |
| Real tools block verify | [SYS_REAL_TOOLS_BLOCK_VERIFY_001_EVIDENCE.md](./SYS_REAL_TOOLS_BLOCK_VERIFY_001_EVIDENCE.md) |

---

## Documentation governance (RCQ)

| Item | File |
| --- | --- |
| DOCS-STANDARD-001 | [DOCS_STANDARD_001_UNIFIED_DOCUMENTATION_STRUCTURE_EVIDENCE.md](./DOCS_STANDARD_001_UNIFIED_DOCUMENTATION_STRUCTURE_EVIDENCE.md) |
| RCQ-DOCS-001 platform sweep | [RCQ_DOCS_001_ONTOGONY_PLATFORM_EVIDENCE.md](./RCQ_DOCS_001_ONTOGONY_PLATFORM_EVIDENCE.md) |
| RCQ-DOCS-FINAL-001 closeout | [RCQ_DOCS_FINAL_001_REPO_CLEANING_CLOSEOUT_EVIDENCE.md](./RCQ_DOCS_FINAL_001_REPO_CLEANING_CLOSEOUT_EVIDENCE.md) |
| RCQ-CODE-001 | [RCQ_CODE_001_ONTOGONY_PLATFORM_EVIDENCE.md](./RCQ_CODE_001_ONTOGONY_PLATFORM_EVIDENCE.md) |
| RCQ-000 package setup | [RCQ_000_PACKAGE_SETUP_EVIDENCE.md](./RCQ_000_PACKAGE_SETUP_EVIDENCE.md) |
| Documentation cleanup (2026-05-26) | [DOCS_CLEANUP_REPORT.md](../DOCS_CLEANUP_REPORT.md) |
| AG-UI spine contract index (PLAT-AGUI-000) | [PLAT_AGUI_000_EVIDENCE.md](./PLAT_AGUI_000_EVIDENCE.md) |
| Evidence spine contract (SYS-TIGHT-002) | [SYS_TIGHT_002_SYSTEM_EVIDENCE_SPINE_CONTRACT_EVIDENCE.md](./SYS_TIGHT_002_SYSTEM_EVIDENCE_SPINE_CONTRACT_EVIDENCE.md) |
| Operator V1 RC (FIRST-VERSION-RC-001) | [FIRST_VERSION_RC_001_EVIDENCE.md](./FIRST_VERSION_RC_001_EVIDENCE.md) |
| Protocol runtime metadata (SYS-PROTOCOL-RUNTIME-001) | [SYS_PROTOCOL_RUNTIME_001_EVIDENCE.md](./SYS_PROTOCOL_RUNTIME_001_EVIDENCE.md) |
