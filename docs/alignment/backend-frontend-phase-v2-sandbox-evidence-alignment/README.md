# Backend/Frontend Phase v2 — Sandbox Evidence Alignment Package

This package supersedes the older `cross-repo-backend-frontend-alignment` package as the current working plan for the immediate bridge after Allagma Phase 3 sandbox execution.

It does **not** discard the older plan. It absorbs its durable ideas: OpenAPI-backed backend truth, capability-driven frontend UI, shared correlation metadata, system compatibility evidence, and limitation banners removed only when backend support is real.

The new center of gravity is Allagma's Phase 3 sandbox evidence loop:

- `AgentRunAuditBundleContract.SandboxEvidence`
- side-effect ledger audit rows
- local sandbox execute events
- replay-safe skip event
- real external execution remaining blocked
- local sandbox execute capability state
- frontend audit/timeline rendering

## Bridge status

**FBA2-000 through FBA2-007: DONE** (2026-05-18). Eval-basing may proceed.

- Compatibility proof: `07_evidence/FBA2-006_COMPATIBILITY_REPORT.md`
- Release scorecard: `07_evidence/FBA2-007_RELEASE_SCORECARD.md`
- Eval handoff: `10_next_eval_basing/01_HANDOFF_TO_EVAL_BASING.md`

## Recommended execution (historical)

1. ~~Finish/confirm Allagma P3-SB-006 RC validator and closeout.~~
2. ~~Run this backend/frontend bridge.~~
3. **Start eval-basing** (EVAL-001+).

## Package layout

- `01_overview/` — strategy, sequence, status, boundaries.
- `02_contracts/` — contracts the frontend must consume.
- `03_repo_plans/` — per-repo PR plan.
- `04_frontend_bridge/` — UI/adapter/fixtures/test plan.
- `05_compatibility_and_ci/` — local stack and CI proof.
- `07_evidence/` — FBA2-006 compatibility and FBA2-007 release scorecard.
- `06_prompts/` — implementation prompts for coding agents.
- `07_schemas/` — JSON schemas for target contracts and evidence.
- `08_examples/` — sample payloads.
- `09_checklists/` — acceptance and safety checklists.
- `10_next_eval_basing/` — handoff criteria to eval-basing.
