# Ontogony Next Development Phase Package

**Generated:** 2026-05-20  
**Scope:** `ontogony-platform`, `ontogony-frontend`, `ontogony-ui`, `conexus-dotnet`, `kanon-dotnet`, `allagma-dotnet`  
**Purpose:** Convert the current strong moving-main alpha state into a reproducible, locked, better-governed alpha baseline, then deepen operational verification and route/security discipline.

## Why this package exists

Recent reviews converge on three facts:

1. The repos are technically strong and well past prototype quality.
2. Sprint 4 implementation/evidence appears closed in service repos, including Conexus retention evidence, Allagma evidence alignment, Allagma streaming, Kanon assistance, and Kanon domain-pack governance.
3. The platform sprint plan / runtime lock still need reconciliation:
   - `ontogony-platform` Sprint plan may still list Sprint 4 as deferred.
   - `allagma-dotnet/docs/system/ontogony-runtime.lock.json` still points to `SYSTEM-ALPHA-003`.
   - The next phase should therefore start with governance/baseline reconciliation before adding new product depth.

## Package structure

```text
README.md
00_EXECUTIVE_NEXT_PHASE_PLAN.md
01_CURRENT_STATE_RECONCILIATION.md
02_PRIORITIZED_BACKLOG.md
03_ACCEPTANCE_MATRIX.md
04_VALIDATION_GATES.md
05_RISK_REGISTER.md
06_REPO_TOUCHPOINTS.md
07_EVIDENCE_TEMPLATE.md
08_CLOSEOUT_TEMPLATE.md
manifest.json
prompts/
  P0_SYSTEM_SPRINT4_STATUS_RECON_001.md
  P0_SYSTEM_ALPHA_004_PREP.md
  P0_SYSTEM_ALPHA_004_CUT.md
  P1_SYSTEM_BASELINE_001.md
  P1_REPO_DOCS_ARCHIVE_001.md
  P1_ROUTE_INVENTORY_001.md
  P1_FE_LIVE_SMOKE_002.md
  P1_CONEXUS_RETENTION_002.md
  P1_SYSTEM_OBS_METERS_001.md
  P2_UI_BUNDLE_001.md
  P2_UI_VISUAL_001.md
  P2_PROD_AUTH_ROADMAP_001.md
```

## Recommended implementation order

1. `SYSTEM-SPRINT4-STATUS-RECON-001`
2. `SYSTEM-ALPHA-004-PREP`
3. `SYSTEM-ALPHA-004-CUT`
4. `SYSTEM-BASELINE-001`
5. `REPO-DOCS-ARCHIVE-001`
6. `ROUTE-INVENTORY-001`
7. `FE-LIVE-SMOKE-002`
8. `SYSTEM-OBS-METERS-001`
9. `CONEXUS-RETENTION-002`
10. UI/prod-readiness refinements

## Non-claims

This package does not claim production readiness. It is an implementation plan for alpha baseline promotion, evidence-depth, live verification, and operational hardening.
