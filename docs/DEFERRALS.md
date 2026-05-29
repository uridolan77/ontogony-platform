# Ontogony Platform — deferrals

Tracked items intentionally not addressed in BACKEND-REPO-CLEANUP-ORGANIZATION-001.

| ID | Area | Deferred item | Reason | Risk | Suggested future package |
| --- | --- | --- | --- | --- | --- |
| PLAT-DEF-001 | Public API | Refresh `Ontogony.Observability` public API snapshot | Pre-existing drift; out of cleanup scope | Low — CI noise | ONTOGONY-PUBLIC-API-SNAPSHOT-001 |
| PLAT-DEF-002 | Errors | Uniform cross-service error envelope enforcement | Product repos must align first | Medium — inconsistent operator UX | SHARED-ERROR-CONTRACT-001 — **closed** 2026-05-29 |
| PLAT-DEF-003 | Identity | End-to-end actor/correlation propagation | Requires all services | Medium — trace gaps | CROSS-REPO-IDENTITY-CORRELATION-001 |
| PLAT-DEF-004 | Intake | `_archived/` as first-class intake tier | Validator change across repos — not required for slice 1 | Low | Platform intake policy follow-on |
| PLAT-DEF-005 | Docs | Populate `docs/generated/` | No generator wired yet — placeholder remains | Low | Platform generated-docs intake |
| PLAT-DEF-006 | Compatibility | Full six-repo compatibility gate in local dev | Requires all siblings + Docker | Medium — integration blind spots | BACKEND-SYSTEM-E2E-001 |
