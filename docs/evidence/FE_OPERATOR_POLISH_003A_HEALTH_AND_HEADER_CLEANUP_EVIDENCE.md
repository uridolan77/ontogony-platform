# FE-OPERATOR-POLISH-003A — Operator health and header cleanup (platform evidence)

**Date:** 2026-05-20  
**Frontend PR/commit:** ontogony-frontend — health probe contracts, Kanon actor roles in settings, shell theme label, observability guidance.

## Backend health contracts (unchanged; frontend now aligned)

| Service | Liveness | Readiness | Notes |
|---------|----------|-----------|-------|
| Conexus (5082) | `/health`, `/health/live`, `/live` | `/ready` | Strict readiness may 503 while liveness OK |
| Kanon (5081) | `/health` | `/ready` | No `/live` or `/health/live` |
| Allagma (5083) | `/health` | `/ready` | No `/live` or `/health/live` |

## Docker-local Kanon domain packs

Frontend sends `X-Ontogony-Actor-Id` from operator settings (`local-operator` default) and `X-Ontogony-Roles` from **Allagma default actor roles** (default `Admin`). Kanon `DevelopmentTrustedHeaders` trusts these headers; domain-pack read requires Admin, System, or Auditor.

No Kanon image change required for Option A (frontend role header).

## Rebuild

Use the orchestrator so `DOCKER_EXTRA_CA_CERT_BASE64` is injected when local TLS interception breaks `npm ci` in the UI build stage (see `PMQA002_002`):

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\start-local-working-system.ps1 -Build
# or: docker compose build ontogony-frontend && docker compose up -d ontogony-frontend
```

**RCQ note:** A bare `docker compose build --no-cache ontogony-frontend` without CA injection can fail at `ontogony-ui` with `tsc: not found`; the running container may then still serve a **pre-003A** image until a successful rebuild.

## Acceptance checklist

- [ ] Command Center refresh: no Kanon/Allagma `/live` or `/health/live` 404s in browser network tab.
- [ ] Conexus badge: live when `/health` OK even if `/ready` is 503.
- [ ] Kanon domain packs load or show actor-role card (not generic load failure).
- [ ] Theme label matches UI.
- [ ] Observability guidance visible without request ID.
