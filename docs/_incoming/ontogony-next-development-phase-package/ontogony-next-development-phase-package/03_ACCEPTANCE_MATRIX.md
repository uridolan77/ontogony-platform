# 03 — Acceptance matrix

| ID | Priority | Repo(s) | Acceptance criteria | Non-claims |
| --- | --- | --- | --- | --- |
| `SYSTEM-SPRINT4-STATUS-RECON-001` | P0 | platform + service refs | Platform sprint plan and acceptance matrix mark Sprint 4 source/evidence closed; each Sprint 4 item links to service evidence; runtime promotion remains pending | Does not update runtime lock |
| `SYSTEM-ALPHA-004-PREP` | P0 | all | Prep evidence created; validation commands listed; current repo refs captured; blockers listed | Does not cut lock |
| `SYSTEM-ALPHA-004-CUT` | P0 | all | Docker-local stack rebuilt; system E2E, restart survival, observability, evidence spine, frontend live smoke pass or documented exceptions; lock updated to `SYSTEM-ALPHA-004` | Does not claim production readiness |
| `SYSTEM-BASELINE-001` | P1 | platform | `CURRENT_SYSTEM_BASELINE.md` exists with repo refs, package versions, compose entrypoints, smoke commands, evidence links, known deferrals | Not a lock replacement unless linked to lock |
| `REPO-DOCS-ARCHIVE-001` | P1 | all | `_incoming` and stale planning packages are archived or marked non-actionable; active docs index is clear | Does not delete historical evidence |
| `ROUTE-INVENTORY-001` | P1 | Conexus/Kanon/Allagma/platform | Generated inventories list route, method, auth, scope, response/error contract, classification; tests fail on unclassified route | Does not redesign APIs |
| `FE-LIVE-SMOKE-002` | P1 | frontend/platform | Docker-live Playwright covers at least one read-only page per domain plus evidence/correlation path; no real external execution | Not full live E2E |
| `SYSTEM-OBS-METERS-001` | P1 | all | `/ready` SLI, Kanon plan compile meters, Conexus cost OTEL counters, Allagma runtime/evidence meters documented and dashboard-linked | Not production SLO governance |
| `CONEXUS-RETENTION-002` | P1 | Conexus | Postgres maintenance-run table; status reads durable last runs; multi-instance story documented; tests cover restart survival | Not project erasure redesign |
| `UI-BUNDLE-001` | P2 | UI | Per-subpath bundle/dependency report; heavy deps isolated; CI guard for accidental heavy root exports | Not semver-major cleanup |
| `UI-VISUAL-001` | P2 | UI/frontend | Primitive visual/a11y matrix exists; Storybook/a11y/responsive coverage documented | Not full visual regression platform |
| `PROD-AUTH-ROADMAP-001` | P2 | all | Roadmap clarifies local alpha auth vs production identity; JWT/mTLS/RBAC migration options documented | Not immediate auth implementation |
