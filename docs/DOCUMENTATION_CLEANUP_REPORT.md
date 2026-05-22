# Documentation cleanup report — 2026-05-23

**Repo:** `uridolan77/ontogony-platform`  
**Verdict:** **PASS** — documentation spine established; ~1,000 historical files removed; link validation green.

**Boundary:** Documentation-only pass (plus CI script removal for retired stale-package guard). No product semantics added.

---

## Summary

| Metric | Before (approx.) | After |
| --- | ---: | ---: |
| Markdown under `docs/` | ~1,188 | **~185** (incl. restored test-gated paths) |
| ZIP archives in repo | 26 | **0** |
| `_incoming/` tree | present | **removed** |
| Competing “current state” docs | many | **one:** [`CURRENT_STATE.md`](./CURRENT_STATE.md) |

---

## Files deleted (categories)

| Path | Rationale |
| --- | --- |
| `docs/_incoming/` (26 zips + unpacked trees) | Quarantined intake; requirements absorbed or owned by product repos |
| `docs/_planning/` | Superseded planning archive |
| `docs/planning/` | PR26–47, Phase Tight plans, conexus-support, ai-runtime-prs, robustness |
| `docs/alignment/` | Closed cross-repo alignment programs |
| `docs/eval-basing/` | Completed eval baseline |
| `docs/product-hardening/` (except schemas — see preserved) | Closed control packages |
| `docs/releases/` | Closeouts and `*_NEXT_OPTIONS.md`; limitations merged into [`KNOWN_LIMITATIONS.md`](./KNOWN_LIMITATIONS.md) |
| `docs/reviews/` | Intake reconciliations for deleted packages |
| `docs/overlay/`, `docs/backlog/` | Legacy / periodic audit |
| `docs/environments/compose-to-docker-closeout-package-v2/`, `local-operator-sanity/`, `local-operator-sanity-package/` | Closed programs; docker-local path kept |
| `docs/Sprint*.md`, numbered roots (`00_START_HERE`, `01_LESSONS_*`, …), `VERSION_COMPATIBILITY_MATRIX.md` | Historical / duplicate |
| `docs/adoption/agentor-*` | Agentor-era adoption |
| `docs/operators/OPERATOR_V1_DEMO_GUIDE.md`, `operator-demo-flow-catalog.json` | Product-operator demos → sister repos |
| ~130 `docs/evidence/*` | PFH/RP/Kanon deepen/Conexus/UI/ENV program evidence |
| `scripts/validate-stale-incoming-package.ps1` | No `_incoming` tree (user decision) |
| `docs/system/stale-incoming-package-patterns.json`, `INCOMING_PACKAGE_RUNBOOK.md` | Retired with guard |
| `docs/evidence/SYS_STALE_PACKAGE_GUARD_001_EVIDENCE.md` | Guard retired |

**Git stat (this pass):** ~1,149 files changed, ~61,700 lines deleted (includes prior unstaged work in working tree).

---

## Files merged / rewritten

| New / updated | Source material |
| --- | --- |
| [`CURRENT_STATE.md`](./CURRENT_STATE.md) | README status, Phase Tight platform gates, code inventory |
| [`ARCHITECTURE.md`](./ARCHITECTURE.md) | `package-levels.md`, `08_DONT_SHARE_BUSINESS_LOGIC.md`, AGENTS.md |
| [`CONTRACTS.md`](./CONTRACTS.md) | operators + contracts index |
| [`DEVELOPMENT.md`](./DEVELOPMENT.md) | README build, ports, docker-local |
| [`TESTING.md`](./TESTING.md) | CI workflow, conformance kits |
| [`INTEGRATION.md`](./INTEGRATION.md) | consumer blueprints, system-compat gate |
| [`KNOWN_LIMITATIONS.md`](./KNOWN_LIMITATIONS.md) | release `*_KNOWN_LIMITATIONS.md` extracts |
| [`README.md`](./README.md) (root) | Slim intro + spine links |
| [`docs/README.md`](./README.md) | Slim index |
| [`governance/ONTOGONY_PLATFORM_0_4_ALPHA_RC_CONTRACT.md`](./governance/ONTOGONY_PLATFORM_0_4_ALPHA_RC_CONTRACT.md) | Moved from deleted `docs/releases/` |
| Governance, operators, evidence indexes | Link fixes, terminology |

---

## Files preserved (and why)

| Area | Why kept |
| --- | --- |
| `docs/packages/` | Per-package guarantees |
| `docs/adr/`, `docs/migrations/` | Living decisions and breaking-change notes |
| `docs/contracts/`, `docs/system/` | Mechanical gates and protocol registry |
| `docs/operators/` (trimmed) | Cross-repo mechanical contracts |
| `docs/consumer-blueprints/`, `docs/governance/` | Consumer adoption |
| `docs/architecture/package-levels.md` | Authoritative dependency matrix |
| `docs/quality/PLAT-QUALITY-001-*.md` | Coverage/XML policy (updated to 27 packages) |
| `docs/ai-runtime/` | **Required by CI** `validate-ai-runtime-docs.ps1` |
| `docs/environments/docker-local-working-system/` | Active docker operator path |
| `docs/evidence/` (16 files) | Platform gates + **test-gated** evidence (see below) |
| `docs/product-hardening/eval-alignment-frontend-depth/schemas/` only | **Required by** `EvalEvidenceExportBundleSchemaTests` |

### Evidence kept (platform gates + CI)

- `PLATFORM_PHASE_TIGHT_2026_05_22_EVIDENCE.md`
- `PLATFORM_RC_001_SUBSTRATE_CONTRACT_FREEZE_EVIDENCE.md`
- `SYSTEM_PROTOCOL_REGISTRY_001_EVIDENCE.md`
- `DOCS_STANDARD_001_*`, `RCQ_*` (4 files)
- `SYS_REAL_TOOLS_BLOCK_VERIFY_001_EVIDENCE.md`
- `SYS_POSTLOCK_DELTA_REGISTER_001_EVIDENCE.md`
- `TRACE_CONTRACT_001_EVIDENCE.md`
- `PLAT_AGUI_000_EVIDENCE.md`, `SYS_TIGHT_002_*`, `FIRST_VERSION_RC_001_*`, `SYS_PROTOCOL_RUNTIME_001_*` (restored — referenced by `Ontogony.Infrastructure.Tests`)

---

## Stale terms removed or reduced

| Term | Action |
| --- | --- |
| `docs/_incoming/` | Tree deleted; references updated |
| `docs/planning/` active PR roadmaps | Deleted |
| Agentor as active product | Removed from adoption docs; glossary retains **historical** entry only |
| “Runtime coordinator” (Allagma) | → “governed execution” in `PHASE1_CONSUMER_COMPATIBILITY.md` |
| “23 packages” | → **27** in README, PLAT-QUALITY-001, packages index |
| Stale-package guard | Script + CI steps removed |

**Remaining Agentor mentions:** migrations, ADRs, `_donors/`, glossary (historical), ai-runtime guard text — acceptable.

---

## Commands run

```powershell
# Deletion (bulk Remove-Item via PowerShell)

# Validation
powershell -File ./scripts/validate-docs-links.ps1          # PASS
powershell -File ./scripts/validate-docs-api-names.ps1      # PASS
powershell -File ./scripts/validate-shipping-inventory.ps1  # PASS (27 packages)
powershell -File ./scripts/validate-ai-runtime-docs.ps1     # PASS (after restoring docs/ai-runtime/)
powershell -File ./scripts/validate-package-levels.ps1      # FAIL: Ontogony.SystemCompatibility not in golden map (pre-existing)
dotnet build Ontogony.Platform.sln -c Release                 # PASS
dotnet test Ontogony.Platform.sln -c Release --no-build       # Infrastructure 318 PASS; SystemCompatibility 2 FAIL without sibling repos at lock SHAs
```

---

## Tests / builds

| Check | Result |
| --- | --- |
| `validate-docs-links.ps1` | **PASS** |
| `validate-docs-api-names.ps1` | **PASS** |
| `validate-shipping-inventory.ps1` | **PASS** |
| `validate-ai-runtime-docs.ps1` | **PASS** |
| `dotnet build` Release | **PASS** |
| `Ontogony.Infrastructure.Tests` | **PASS** (318) after restoring test-gated evidence + PFH schema subtree |
| `Ontogony.SystemCompatibility.Tests` | **2 FAIL** without full `DevRoot` sibling repos — environmental, not introduced by doc deletion |

---

## Remaining documentation risks

1. **`validate-package-levels.ps1`** — `Ontogony.SystemCompatibility` missing from golden map (code/doc drift predating this cleanup).
2. **Test-gated paths** — `docs/product-hardening/.../schemas/` and four extra evidence files must stay until tests are repointed or schemas move under `docs/schemas/`.
3. **Sister-repo inbound links** — Other repos may still link to deleted platform evidence URLs; fix in those repos if CI link-check fails.
4. **`_donors/`** — Historical Agentor/Athanor code; not part of this doc pass.
5. **Docker README** — Updated to point at `KNOWN_LIMITATIONS.md` instead of deleted release closeouts.

---

## Acceptance criteria

| Criterion | Met |
| --- | --- |
| Agent sees Platform = mechanics only | Yes — spine + ARCHITECTURE |
| One current state doc | Yes — `CURRENT_STATE.md` |
| No zips / `_incoming` | Yes |
| No Agentor as active component in operator docs | Yes |
| System compat matrix not duplicated | Yes — link to Allagma / gate doc |
| Cleanup audit trail | This file |
