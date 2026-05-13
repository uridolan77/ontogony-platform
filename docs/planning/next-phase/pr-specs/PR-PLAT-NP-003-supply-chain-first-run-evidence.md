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
- Dependency submission: green on main.
- Dependency review: executes on PR and comments/fails as configured.
- Evidence doc added under `docs/security/`.

## Non-goals

- Do not weaken gates merely to get green unless the gate is clearly noisy or invalid.
