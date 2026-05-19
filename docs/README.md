# Ontogony platform — documentation index

**Repo:** `uridolan77/ontogony-platform`  
**Purpose:** Shared infrastructure (`Ontogony.*` packages), cross-repo governance, environment programs, and platform evidence.

**Documentation standard (all six repos):** [`operators/ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md`](operators/ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md) (`DOCS-STANDARD-001`).

**Boundary:** Closed Docker-local and product-hardening milestones documented here are **not production readiness**.

---

## Current operator entry points

| Need | Document |
| --- | --- |
| Unified docs structure (six repos) | [`operators/ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md`](operators/ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md) |
| Terminology (Allagma, Kanon, Conexus, …) | [`operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md`](operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md) |
| Trace / correlation contract | [`operators/TRACE_CORRELATION_CONTRACT.md`](operators/TRACE_CORRELATION_CONTRACT.md) |
| CI cost / branch protection | [`operators/CI_COST_CONTROL.md`](operators/CI_COST_CONTROL.md) |
| Operator docs index | [`operators/README.md`](operators/README.md) |
| Docker-local compose stack (incl. real-provider scripts) | `docker/local-working-system/README.md` (repo root) |
| Environment programs index | [`environments/README.md`](environments/README.md) |
| Real provider local validation policy | [`operators/REAL_PROVIDER_LOCAL_VALIDATION_POLICY.md`](operators/REAL_PROVIDER_LOCAL_VALIDATION_POLICY.md) |

---

## Development

| Need | Document |
| --- | --- |
| Platform contributor start | [`00_START_HERE.md`](00_START_HERE.md) |
| Package boundaries | [`02_PACKAGE_BOUNDARIES.md`](02_PACKAGE_BOUNDARIES.md) |
| Per-package contracts | [`packages/`](packages/) |
| Consumer blueprints | [`consumer-blueprints/README.md`](consumer-blueprints/README.md) |
| Version matrix | [`VERSION_COMPATIBILITY_MATRIX.md`](VERSION_COMPATIBILITY_MATRIX.md) |

---

## Architecture / contracts

| Area | Path |
| --- | --- |
| ADRs | [`adr/`](adr/) |
| Architecture notes | [`architecture/`](architecture/) |
| Contracts / protocol | [`contracts/`](contracts/), [`protocol-ingress/`](protocol-ingress/) |
| Alignment snapshots | [`alignment/`](alignment/) |
| AGENTS rules | [`../AGENTS.md`](../AGENTS.md) |

---

## Evidence

Verification records: [`evidence/`](evidence/) — full index: [`evidence/README.md`](evidence/README.md).

Recent / program evidence:

| Item | File |
| --- | --- |
| RCQ-CODE-001 platform code/script hygiene | [`evidence/RCQ_CODE_001_ONTOGONY_PLATFORM_EVIDENCE.md`](evidence/RCQ_CODE_001_ONTOGONY_PLATFORM_EVIDENCE.md) |
| RCQ-DOCS-001 platform docs sweep | [`evidence/RCQ_DOCS_001_ONTOGONY_PLATFORM_EVIDENCE.md`](evidence/RCQ_DOCS_001_ONTOGONY_PLATFORM_EVIDENCE.md) |
| RCQ-DOCS-FINAL-001 repo-cleaning closeout | [`evidence/RCQ_DOCS_FINAL_001_REPO_CLEANING_CLOSEOUT_EVIDENCE.md`](evidence/RCQ_DOCS_FINAL_001_REPO_CLEANING_CLOSEOUT_EVIDENCE.md) |
| DOCS-STANDARD-001 | [`evidence/DOCS_STANDARD_001_UNIFIED_DOCUMENTATION_STRUCTURE_EVIDENCE.md`](evidence/DOCS_STANDARD_001_UNIFIED_DOCUMENTATION_STRUCTURE_EVIDENCE.md) |
| RCQ-000 package setup | [`evidence/RCQ_000_PACKAGE_SETUP_EVIDENCE.md`](evidence/RCQ_000_PACKAGE_SETUP_EVIDENCE.md) |
| PFH closeout | [`evidence/FE_PRODUCT_CLOSEOUT_001_EVIDENCE.md`](evidence/FE_PRODUCT_CLOSEOUT_001_EVIDENCE.md) |
| RP closeout | [`evidence/RP_CLOSEOUT_001_REAL_PROVIDER_VALIDATION_EVIDENCE.md`](evidence/RP_CLOSEOUT_001_REAL_PROVIDER_VALIDATION_EVIDENCE.md) |
| RP-005 manual QA | [`evidence/RP_005_REAL_PROVIDER_MANUAL_QA_EXECUTION_EVIDENCE.md`](evidence/RP_005_REAL_PROVIDER_MANUAL_QA_EXECUTION_EVIDENCE.md) |
| Post-Docker hardening closeout | [`evidence/POST_DOCKER_HARDENING_CLOSEOUT_001_EVIDENCE.md`](evidence/POST_DOCKER_HARDENING_CLOSEOUT_001_EVIDENCE.md) |

Naming: `<ITEM>_EVIDENCE.md` — see the [documentation structure standard](operators/ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md#6-evidence-files).

---

## Releases / closeouts

Cross-repo milestone closeouts: [`releases/`](releases/).

| Program | Status | Entry |
| --- | --- | --- |
| First Dockerized local working system | **CLOSED / PASS** | [`releases/FIRST_DOCKER_LOCAL_WORKING_SYSTEM_CLOSEOUT.md`](releases/FIRST_DOCKER_LOCAL_WORKING_SYSTEM_CLOSEOUT.md) |
| Post-Docker-local hardening | **CLOSED / PASS** | [`releases/POST_DOCKER_LOCAL_HARDENING_CLOSEOUT.md`](releases/POST_DOCKER_LOCAL_HARDENING_CLOSEOUT.md) |
| Product feature hardening v1 (PFH) | **CLOSED / PASS** | [`releases/PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_CLOSEOUT.md`](releases/PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_CLOSEOUT.md) |
| Real provider validation v1 (RP) | **CLOSED / PASS** | [`releases/REAL_PROVIDER_VALIDATION_CLOSEOUT.md`](releases/REAL_PROVIDER_VALIDATION_CLOSEOUT.md) |
| Production readiness | **NOT STARTED** | — |

**Next options (strategic, not mandates):**

- [`releases/POST_DOCKER_LOCAL_HARDENING_NEXT_OPTIONS.md`](releases/POST_DOCKER_LOCAL_HARDENING_NEXT_OPTIONS.md)
- [`releases/PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_NEXT_OPTIONS.md`](releases/PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_NEXT_OPTIONS.md)
- [`releases/REAL_PROVIDER_VALIDATION_NEXT_OPTIONS.md`](releases/REAL_PROVIDER_VALIDATION_NEXT_OPTIONS.md)

---

## Product hardening control packages

| Package | Status | Path |
| --- | --- | --- |
| Eval / alignment / frontend depth v1 | **CLOSED / PASS** | [`product-hardening/eval-alignment-frontend-depth/`](product-hardening/eval-alignment-frontend-depth/) |
| Repo cleaning / docs / manual QA prep v1 | **CLOSED / PASS** | [`product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/`](product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/) |
| Real provider validation v1 | **CLOSED / PASS** | [`product-hardening/real-provider-validation-package-v1/`](product-hardening/real-provider-validation-package-v1/) |

Index: [`product-hardening/README.md`](product-hardening/README.md). RCQ closeout: [`evidence/RCQ_DOCS_FINAL_001_REPO_CLEANING_CLOSEOUT_EVIDENCE.md`](evidence/RCQ_DOCS_FINAL_001_REPO_CLEANING_CLOSEOUT_EVIDENCE.md). Fake-provider manual QA: [`evidence/PRODUCT_MANUAL_QA_002R1_EXECUTION_EVIDENCE.md`](evidence/PRODUCT_MANUAL_QA_002R1_EXECUTION_EVIDENCE.md). Real-provider manual QA: [`evidence/RP_005_REAL_PROVIDER_MANUAL_QA_EXECUTION_EVIDENCE.md`](evidence/RP_005_REAL_PROVIDER_MANUAL_QA_EXECUTION_EVIDENCE.md).

---

## Testing / troubleshooting

| Area | Path |
| --- | --- |
| Testing notes | [`testing/`](testing/) |
| Troubleshooting | [`troubleshooting/`](troubleshooting/) (create when needed) |
| Quality / checks | [`quality/`](quality/) |

---

## Known limitations

- Production readiness is **not started** — see PFH and post-Docker closeouts for scoped limitations.
- PFH v1: [`releases/PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_KNOWN_LIMITATIONS.md`](releases/PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_KNOWN_LIMITATIONS.md)
- Real provider validation v1: [`releases/REAL_PROVIDER_VALIDATION_KNOWN_LIMITATIONS.md`](releases/REAL_PROVIDER_VALIDATION_KNOWN_LIMITATIONS.md)
- Post-Docker: [`releases/POST_DOCKER_HARDENING_KNOWN_LIMITATIONS.md`](releases/POST_DOCKER_HARDENING_KNOWN_LIMITATIONS.md)

---

## Historical / archived docs

The following are **not current operator guidance** unless a living status board explicitly references them:

| Area | Notes |
| --- | --- |
| [`_incoming/`](_incoming/) | ZIP intake; preserved sources, not canonical tree |
| [`_planning/`](_planning/) | Superseded planning packages |
| [`planning/`](planning/) | Prior programs (e.g. eval durability); check dates |
| Numbered roots (`01_LESSONS_…`, Sprints, `start-here.md`) | Platform extraction history |
| [`governance/`](governance/), [`overlay/`](overlay/) | Legacy governance artifacts |

When editing historical docs, add a **Historical** banner per [`ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md`](operators/ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md#82-banner-for-superseded-docs).

---

## Sister repos (documentation)

| Repo | Docs index |
| --- | --- |
| allagma-dotnet | [`docs/README.md`](https://github.com/uridolan77/allagma-dotnet/blob/main/docs/README.md) — **current** (RCQ-DOCS-001, 2026-05-19) |
| kanon-dotnet | [`docs/README.md`](https://github.com/uridolan77/kanon-dotnet/blob/main/docs/README.md) — **current** |
| conexus-dotnet | [`docs/README.md`](https://github.com/uridolan77/conexus-dotnet/blob/main/docs/README.md) — **current** |
| ontogony-frontend | [`docs/README.md`](https://github.com/uridolan77/ontogony-frontend/blob/main/docs/README.md) — **current** |
| ontogony-ui | [`docs/README.md`](https://github.com/uridolan77/ontogony-ui/blob/main/docs/README.md) — **current** |
