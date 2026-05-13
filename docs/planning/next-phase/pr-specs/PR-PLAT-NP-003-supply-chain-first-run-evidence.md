# PR-PLAT-NP-003 — Supply-chain first-run evidence

## Goal

Convert security workflow definitions into proven operational checks.

## Scope

- Record successful run URLs for CodeQL, Supply chain, Dependency submission.
- Test Dependency review on a real PR.
- If a workflow fails due to environment/permissions/action behavior, fix the workflow and document why.
- Ensure SBOM artifact is uploaded and retained.

## Acceptance

- CodeQL: green on main.
- Supply chain: green on main.
- Dependency submission: green on main once **Dependency graph** is enabled (repo setting); until then this criterion is intentionally unmet — not a workflow-code gap.
- Dependency review: executes on PR and comments/fails as configured (`pull_request` only; see evidence doc).
- Evidence doc added under `docs/security/`.

## Non-goals

- Do not weaken gates merely to get green unless the gate is clearly noisy or invalid.

## Status (repo)

**Partially complete.** Evidence lives in [`docs/security/PLAT-NP-003-supply-chain-first-run-evidence.md`](../../../security/PLAT-NP-003-supply-chain-first-run-evidence.md): CodeQL, supply chain (including SBOM artifact), and dependency-review wiring are documented with green or appropriate run links. **Dependency submission** remains **blocked** until the repository enables **Dependency graph** in GitHub settings; the evidence doc records the failure mode and owner follow-up. Full NP-003 acceptance (submission green on `main`) stays **open** in [`backlog.json`](../backlog.json) until that run exists.
