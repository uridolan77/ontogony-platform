# DOCKER-LOCAL-VERIFY-001 — Frontend browser provenance and freshness

**Date:** 2026-05-20  
**Scope:** Close the “source fixed, browser still stale” loop for Docker-local SPA verification.

## Problem

RCQ-VERIFY-001 notes that code review does not prove browser behavior unless the frontend image
rebuild succeeded. Operators need a mechanical check that the running container serves the
expected `ontogony-frontend` commit.

## Solution

| Layer | Change |
| --- | --- |
| Build | Docker `ontogony-frontend` image receives `VITE_GIT_SHA` / `VITE_BUILD_TIME` from repo HEAD via `start-local-working-system.ps1 -Build` |
| Runtime artifact | `dist/provenance.json` served at `/provenance.json` |
| HTML probe | Vite injects `meta name="ontogony:git-sha"` into `index.html` |
| UI | Shell header **Build {short-sha}**; Settings environment panel; diagnostics export `frontend.gitSha` |
| Scripts | `verify-frontend-browser-provenance.ps1 -Build` — rebuild + HTTP probe + validate |

## Operator command

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\verify-frontend-browser-provenance.ps1 -Build
```

## Acceptance

- [ ] `verify-frontend-browser-provenance.ps1 -Build` exits 0 after a frontend change and rebuild
- [ ] Shell header build label matches `git rev-parse HEAD` in `ontogony-frontend` (7-char prefix)
- [ ] Diagnostics JSON includes `frontend.gitSha` equal to repo HEAD
- [ ] Report `artifacts/docker-local-verify-001-report.json` has `verdict: PASS`
- [ ] Stale image (skip rebuild after commit) produces `verdict: FAIL` with explicit issues
