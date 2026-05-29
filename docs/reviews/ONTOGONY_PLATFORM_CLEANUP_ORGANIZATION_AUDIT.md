# Ontogony Platform — cleanup and organization audit

**Date:** 2026-05-29  
**Program:** BACKEND-REPO-CLEANUP-ORGANIZATION-001  
**Branch:** `main` @ `e1bc35e`

---

## Current repo purpose

Ontogony.Platform is the **neutral mechanics layer** for Ontogony backends: tracing, correlation, error envelopes, HTTP resilience, idempotency, hashing, observability contracts, security/actor context, persistence/outbox ports, AI telemetry DTOs, replay contracts, and cross-service compatibility gates. It must remain free of product semantics (ontology, model routing, governed execution).

---

## Current structure assessment

| Area | Assessment |
| --- | --- |
| `src/` (27 packages) | Strong — clear package-per-concern layout with `Ontogony.*` naming |
| `tests/` (24 projects) | Strong — architecture, public API, system compatibility, per-package tests |
| `examples/` | Good — consumer skeleton smokes for Conexus/Allagma |
| `scripts/` | Extensive CI validators (31+ `validate*.ps1`) |
| `docs/` | Mature spine; `docs/status/` was missing (created in this pass) |
| `_donors/` | Quarantined historical reference — correctly isolated |

---

## Source-code organization issues

- **Low:** Product names appear in ~33 `src/` files — expected for `Ontogony.SystemCompatibility`, `Ontogony.Replay.Contracts`, and propagation contracts; guarded by `ProductSemanticLeakageTests`.
- **None blocking:** No product implementation assembly references detected.

---

## Documentation organization issues

- `docs/status/` folder did not exist — status now lives under `docs/status/ONTOGONY_PLATFORM_CLEANUP_ORGANIZATION_STATUS.md`.
- `docs/DEFERRALS.md` did not exist — created in this pass.
- Prior cleanup reports (`DOCS_CLEANUP_REPORT.md`, `CODE_ARTIFACT_CLEANUP_REPORT.md`) remain valid; `CODE_ARTIFACT_CLEANUP_REPORT.md` has stale note that `_incoming` was absent (fixed 2026-05-26).
- `docs/generated/` is empty placeholder.

---

## Stale / duplicate docs found

| Item | Disposition |
| --- | --- |
| `docs/_incoming/packages/` (Aisthesis RC trees) | Moved to `_consumed/2026-05/_misfiled-cross-repo-packages/` |
| `ONTOGONY-SAMPLING-POLICY-SPINE-001.zip` | Removed (package already consumed) |
| `_active/ONTOGONY-SAMPLING-POLICY-SPINE-001A.md` | Moved to consumed package folder |
| `METABOLE-*.md` loose files | Not present at audit time (prior cleanup) |

---

## Obsolete package / spec files

- **Active intake:** none (`_active/MANIFEST.md` empty).
- **Consumed:** 35+ packages under `_consumed/2026-05/` — well indexed.

---

## Test organization issues

- Tests mirror `src/` packages — good discoverability.
- System compatibility tests require sibling repos under `C:\dev` — documented in `docs/TESTING.md`.

---

## Boundary risks

| Risk | Severity | Notes |
| --- | --- | --- |
| Cross-repo packages dropped in platform `_incoming` | Medium | Fixed in this pass; add CI reminder |
| `Ontogony.SystemCompatibility` reads sibling repo filesystem | Low | Mechanical gate only — no product logic |
| Shared error envelope not yet uniform across all services | Medium | Defer to SHARED-ERROR-CONTRACT-001 |
| Identity/correlation propagation incomplete end-to-end | Medium | Defer to CROSS-REPO-IDENTITY-CORRELATION-001 |

---

## Cleanup actions completed

1. Restored `_incoming` hygiene (removed zip, archived misfiled `packages/`).
2. Created cleanup audit, status, evidence, and deferrals docs.
3. Updated `README.md` and `docs/README.md` with status/reviews links.

---

## Cleanup actions intentionally deferred

- Promote candidate primitives from product repos (non-trivial — separate packages).
- Full `docs/generated/` population.
- Resolve `PublicApi.Tests` snapshot drift (pre-existing).
- Add `_archived/` as first-class intake tier (would require validator change across repos).

---

## Validation commands run

```powershell
dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln -c Release
dotnet test Ontogony.Platform.sln -c Release --no-build
powershell -NoProfile -File .\scripts\validate-docs-incoming-hygiene.ps1
```

---

## Test results

| Suite | Result |
| --- | --- |
| Full solution test | **1 failed** (pre-existing): `Ontogony.PublicApi.Tests` — public API snapshot mismatch on `Ontogony.Observability.CorrelationState` |
| Build Release | **0 errors**, 0 warnings |
| `validate-docs-incoming-hygiene.ps1` | **Pass** after manifest update |

---

## Remaining open issues

1. Public API snapshot test failure — unrelated to cleanup; refresh snapshot in dedicated PR.
2. Cross-repo compatibility gate requires all six sibling checkouts.
3. Intake policy should explicitly document handling of misfiled cross-repo packages.
