# SYSTEM-TESTING-BASELINE-001

Goal:
Establish a clean alpha integration baseline for running and manually testing
the Ontogony local system.

This is not:
- a release candidate
- production readiness
- package publishing certification
- security hardening
- real provider certification

Repos:
- ontogony-platform
- allagma-dotnet
- kanon-dotnet
- conexus-dotnet
- ontogony-frontend
- ontogony-ui

## Baseline Policy

For this milestone, RC/process paperwork is non-blocking. Do not block system
testing on:

- six-repo lock freshness
- post-lock delta registers
- release readiness scorecards
- strict RC promote scripts
- per-SHA release certification
- full bureaucratic artifact archival

`main` is allowed to be an alpha integration baseline while this milestone is
active.

## Required Success

1. All repos build.
2. Default non-expensive tests pass.
3. Docker-local stack starts.
4. Frontend opens on `http://localhost:5175`.
5. Health is green for Kanon, Conexus, Allagma.
6. Operator can start one Allagma run.
7. Operator can inspect:
   - Allagma run/events/audit
   - Kanon decision/provenance
   - Conexus model call/evidence
   - Evidence Spine graph
8. No real tools enabled.
9. No real provider required.
10. Known limitations are documented, not hidden.

## Minimal Execution Order

1. Pull latest `main` in all six repos.
2. Build/test backends with default non-expensive filters.
3. Build/check UI and frontend (`npm run check`, not `check:full`).
4. Start docker local working system.
5. Run one manual end-to-end operator journey and capture IDs.
6. Record results in `SYSTEM_TESTING_BASELINE_001_RESULTS.md`.

## Manual Journey (First Path)

Use this exact sequence:

1. Configure `/settings` endpoints and dev credentials.
2. Verify `/` health posture is green.
3. Send one request in `/conexus/chat`, capture `modelCallId`.
4. Start one run at `/allagma/runs/start`, capture `runId` and decision IDs.
5. Verify `/allagma/runs/{runId}` detail/events/audit load.
6. Verify `/kanon/decisions` resolves the run-linked decision/provenance.
7. Verify `/system/evidence-spine` resolves run/model/decision links.
8. Verify `/system/agent-interaction` timeline loads for the run.

## Known Constraints

- This baseline is for local alpha integration only.
- It does not claim production readiness.
- It does not certify provider parity or enterprise security posture.
- Any failing expensive suites should be logged as known gaps, not hidden.
