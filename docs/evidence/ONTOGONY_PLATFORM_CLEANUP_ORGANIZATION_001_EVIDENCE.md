# Ontogony Platform — cleanup organization evidence

**Date:** 2026-05-29  
**Program:** BACKEND-REPO-CLEANUP-ORGANIZATION-001  
**Branch:** `main`  
**Commit before changes:** `e1bc35e44707e4666ed9eaf4b63ebf0597cc7592`

---

## Commands run

```powershell
cd C:\dev\ontogony-platform
git rev-parse HEAD
dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln -c Release --no-restore
dotnet test Ontogony.Platform.sln -c Release --no-build
powershell -NoProfile -File .\scripts\validate-docs-incoming-hygiene.ps1
```

---

## Test output summary

- **Build:** succeeded (0 errors, 0 warnings)
- **Test:** 1 failure in `Ontogony.PublicApi.Tests` — `Public_api_matches_snapshot` (pre-existing Observability API drift)
- **Hygiene validator:** pass after manifest update

---

## Files moved

| From | To |
| --- | --- |
| `docs/_incoming/packages/*` | `docs/_incoming/_consumed/2026-05/MISFILED-CROSS-REPO-PACKAGES/` |
| `docs/_incoming/_active/ONTOGONY-SAMPLING-POLICY-SPINE-001A.md` | `docs/_incoming/_consumed/2026-05/ONTOGONY-SAMPLING-POLICY-SPINE-001/` |

---

## Files removed

| File | Reason |
| --- | --- |
| `docs/_incoming/ONTOGONY-SAMPLING-POLICY-SPINE-001.zip` | Policy violation; package already consumed |

---

## Docs created / updated

- `docs/reviews/ONTOGONY_PLATFORM_CLEANUP_ORGANIZATION_AUDIT.md`
- `docs/status/ONTOGONY_PLATFORM_CLEANUP_ORGANIZATION_STATUS.md`
- `docs/evidence/ONTOGONY_PLATFORM_CLEANUP_ORGANIZATION_001_EVIDENCE.md`
- `docs/DEFERRALS.md`
- `docs/_incoming/_consumed/2026-05/_misfiled-cross-repo-packages/CONSUMED.md`
- `docs/_incoming/_consumed/MANIFEST.md` (row added)
- `README.md`, `docs/README.md` (status links)

---

## Files intentionally left untouched

- All `src/` and `tests/` code (no feature changes)
- `_donors/` quarantine
- `_consumed/2026-05/` historical packages (except misfiled archive)
- Public API snapshot baseline (failure documented, not refreshed)

---

## Known failures

| Failure | Category |
| --- | --- |
| `Ontogony.PublicApi.Tests` snapshot | Pre-existing |
| System compat gate not run | Requires full `C:\dev` sibling layout + `run-system-compatibility-gate.ps1` |
