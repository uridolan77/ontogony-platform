# Execution Sequence

**Bridge status (FBA2-007):** FBA2-000 through FBA2-007 **DONE**. Eval-basing **READY**. See `07_evidence/FBA2-007_RELEASE_SCORECARD.md`.

## FBA2-000 — Register this v2 package
**Status: DONE**
Repo: `ontogony-platform`

Add this package under `docs/alignment/backend-frontend-phase-v2-sandbox-evidence-alignment/` and cross-link the older package.

## FBA2-001 — Allagma Phase 3 RC contract closeout
**Status: DONE**

Repo: `allagma-dotnet`

Ensure the backend contract is clean before the frontend consumes it.

Required:

- P3-SB-006 validator/closeout/tag or explicit deferred status.
- Current OpenAPI snapshot exposing audit bundle shape including `SandboxEvidence`.
- Sandbox execute event vocabulary snapshot includes all five execute events.
- Evidence scripts validate current state.
- `RealExecutionEnabled=true` rejected.
- `SandboxExecutionEnabled=true` forbidden in production.

## FBA2-002 — Allagma OpenAPI/provenance refresh
**Status: DONE**

Repos: `allagma-dotnet`, `ontogony-frontend`

Produce and consume a stable Allagma contract snapshot.

Required:

- Backend OpenAPI snapshot from current Allagma main/tag.
- Contract provenance: repo, commit, generatedAt, snapshot hash.
- Frontend generated client or manual adapter contract update.
- Contract tests for `AgentRunAuditBundleContract.SandboxEvidence`.

## FBA2-003 — Frontend sandbox evidence adapter
**Status: DONE**

Repo: `ontogony-frontend`

Add neutral UI adapter types:

- `SandboxEvidenceViewModel`
- `SideEffectLedgerRowViewModel`
- `SandboxExecuteTimelineEventViewModel`
- `SandboxExecutionCapabilityViewModel`

Adapter must tolerate missing sandbox evidence and old audit bundles.

## FBA2-004 — Frontend audit/replay UI rendering
**Status: DONE**

Repo: `ontogony-frontend`

Make the evidence visible:

- Allagma run audit/replay panel.
- Sandbox evidence card.
- Side-effect ledger table.
- Sandbox execute timeline.
- Replay-safe status indicator.
- Local sandbox execution boundary note.
- Real external execution blocked note.

## FBA2-005 — Fixtures, fallbacks, and limitation cleanup
**Status: DONE**

Repo: `ontogony-frontend`

Add fixture audit bundle with `SandboxEvidence`, event timeline with all five sandbox execute events, remove/refine limitation banners only where backend support is real, and consume `GET /allagma/v0/capabilities` for sandbox execution boundaries.

## FBA2-006 — Cross-service trace and compatibility proof
**Status: DONE**

Repos: all relevant

Prove the UI can correlate evidence through local stack compatibility report. Evidence: `07_evidence/FBA2-006_COMPATIBILITY_REPORT.md`.

## FBA2-007 — Release evidence and scorecard
**Status: DONE**

Repos: all relevant

Record backend commits, frontend commit, OpenAPI hashes, adapter tests, UI tests, compatibility report, known limitations, and eval-basing handoff. Evidence: `07_evidence/FBA2-007_RELEASE_SCORECARD.md`.
