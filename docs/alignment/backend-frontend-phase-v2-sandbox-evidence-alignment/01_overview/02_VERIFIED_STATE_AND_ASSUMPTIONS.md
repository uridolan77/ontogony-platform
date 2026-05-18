# Verified State and Assumptions

## Verified state

Allagma `main` now shows:

- P3-SB-005 marked `DONE` in the Phase 3 dashboard.
- P3-SB-006 still marked `TODO`, acceptable if the final RC validator/tag is not complete.
- `PHASE3_RESTART_AUDIT_EVIDENCE.md` identifies commit `c87f1d8b03b07a4178843705ef7af589089e55ce`.
- Evidence wording is tightened to "Service-Reconstruction Replay Proof".
- `GetAgentRunAuditBundleService` accepts both marker external ref key shapes:
  - `sandbox.file:marker_relative_path`
  - `sandbox_file`
- `Phase3RestartAuditTests` contains a regression test for marker relative path mapping.

## Important assumption

The current frontend does not yet consume `SandboxEvidence` directly.

This package assumes the frontend already has broader Allagma run/audit/replay/correlation surfaces, generated-client or adapter infrastructure, fallback fixtures, and evidence/redaction testing. The bridge should extend those existing surfaces rather than create an unrelated new dashboard.

## Non-goals

This package does not enable real external execution, introduce arbitrary tools, create eval scoring, create topology selection, replace the eval-basing package, remove limitation banners without backend support, or expose raw side-effect payloads.
