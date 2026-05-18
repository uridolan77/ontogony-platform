# Master Implementation Prompt

We are implementing Backend/Frontend Phase v2 — Sandbox Evidence Alignment.

Repos: `uridolan77/ontogony-platform`, `uridolan77/allagma-dotnet`, `uridolan77/ontogony-frontend`, `uridolan77/conexus-dotnet`, `uridolan77/kanon-dotnet`.

Context: Allagma Phase 3 sandbox evidence is now available after P3-SB-005 and hygiene cleanup: `SandboxEvidence` in audit bundle, sandbox side-effect ledger rows, five sandbox execute events, replay-safe skip, real external execution still blocked, and local sandbox execute remains bounded/local-only.

Mission: Align the frontend with this backend evidence before eval-basing.

Rules: do not enable real external execution; do not create fake frontend functionality; do not remove limitation banners unless backend route + OpenAPI + tests exist; no raw marker content in UI; use typed adapters; keep frontend and backend checks green.

Execution: finish Allagma RC contract closeout, refresh OpenAPI and frontend client/adapter, add frontend sandbox evidence adapter, add UI rendering, add fixtures/tests, produce compatibility report, then hand off to eval-basing.
