# 05 — Risk register

| Risk | Severity | Description | Mitigation |
| --- | --- | --- | --- |
| Stale runtime lock | Critical | `SYSTEM-ALPHA-003` lock remains while main has advanced | `SYSTEM-ALPHA-004-PREP/CUT` |
| Platform Sprint 4 lag | High | Service evidence says Sprint 4 closed, platform plan may still say deferred | `SYSTEM-SPRINT4-STATUS-RECON-001` |
| Closed ≠ runtime-certified | High | Source/evidence closeout may be read as full live/system proof | Status vocabulary and baseline lock rules |
| Process-local retention lastRun | Medium | Conexus `lastRun` clears on restart and is per-instance | `CONEXUS-RETENTION-002` |
| Live browser confidence shallow | Medium | Existing Docker-live frontend smoke is read-only/overview focused | `FE-LIVE-SMOKE-002` |
| Route/security drift | High | New endpoints can land without auth/classification review | `ROUTE-INVENTORY-001` |
| Docs search noise | Medium | `_incoming` and stale planning packages pollute repo search | `REPO-DOCS-ARCHIVE-001` |
| Observability panel without meters | Medium | Dashboard exists but underlying SLI/meter depth may be insufficient | `SYSTEM-OBS-METERS-001` |
| Product logic creeping into UI package | Medium | Shared UI could absorb product DTOs/adapters over time | `UI-BUNDLE-001`, API surface guards |
| Production auth ambiguity | High | Local alpha auth might be mistaken for production auth | `PROD-AUTH-ROADMAP-001` |
