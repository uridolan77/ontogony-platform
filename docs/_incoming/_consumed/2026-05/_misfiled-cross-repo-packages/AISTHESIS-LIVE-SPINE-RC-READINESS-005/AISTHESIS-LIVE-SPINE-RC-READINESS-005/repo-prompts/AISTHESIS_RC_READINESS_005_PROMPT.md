# Cursor prompt — Aisthesis RC readiness 005

Apply this inside `C:\Dev\aisthesis-dotnet`.

Goal: implement Aisthesis RC-readiness gates, not new Aisthesis features.

Tasks:

1. Add `scripts/system/run-aisthesis-rc-readiness.ps1`.
2. Add `scripts/system/run-five-service-ci-smoke.ps1`.
3. Add docs:
   - `docs/evidence/AISTHESIS_LIVE_SPINE_RC_READINESS_005_CLOSEOUT.md`
   - `docs/evidence/AISTHESIS_LIVE_SPINE_RC_READINESS_005_RELEASE_GATES.md`
   - `docs/evidence/AISTHESIS_LIVE_SPINE_RC_READINESS_005_LIVE_PROOF.md`
   - `docs/evidence/AISTHESIS_LIVE_SPINE_RC_READINESS_005_LOCK_DECISION.md`
   - `docs/operations/AISTHESIS_FIVE_SERVICE_CI_SMOKE_RUNBOOK.md`
   - `docs/operations/AISTHESIS_RELEASE_MODE_RUNBOOK.md`
   - `docs/contracts/AISTHESIS_LIVE_SPINE_SUMMARY_V2.md`
   - `docs/contracts/AISTHESIS_RC_READINESS_GATE_V0.md`
4. Run full Release build/test with APIs stopped.
5. Run fixture smoke.
6. Record LES-001 and LES-002 evidence.
7. Analyze LES-002 partial grade.
8. Produce lock recommendation.

Do not change routes unless a failing gate requires it.
