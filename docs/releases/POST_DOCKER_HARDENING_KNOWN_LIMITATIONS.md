# Post-Docker-local hardening — known limitations

**Date:** 2026-05-19  
**Scope:** `POST-DOCKER-HARDENING-CLOSEOUT-001` (after twelve hardening PRs)

**This is post-Docker-local hardening, not production readiness.**

These items are **accepted** for closing the hardening package. They are not blockers unless you are targeting production deploy.

## Inherited from Docker-local program

The first Dockerized local working system limitations still apply. See [FIRST_DOCKER_LOCAL_WORKING_SYSTEM_KNOWN_LIMITATIONS.md](./FIRST_DOCKER_LOCAL_WORKING_SYSTEM_KNOWN_LIMITATIONS.md), including:

- Fake/local Conexus provider by default; no real external execution unless opted in
- Development credentials only; no TLS or production identity
- `VITE_*` compile-time in frontend image; runtime nginx env injection **deferred**
- Baseline `topologyAuthorizationDecisionId` null by design on `single_workflow`
- Conexus `/ready` 503 pre-bootstrap is expected

## Hardening-specific limitations

### Frontend check sprawl

`ontogony-frontend` `npm run check` runs many sequential gates (`fixtures:check`, `replay:check`, `config:check`, OpenAPI, routes, build, tests, etc.). Valuable for local/CI quality but **runtime cost** grows with each gate. `CI-COST-001` remains optional if Actions minutes become painful.

### Replay and live API gaps

- Replay **catalog and E2E** exist; **live replay trigger** may be absent from OpenAPI — UI shows limitation banner rather than pretending live replay exists (`FE-TEST-REPLAY-001`).
- Eval list remains **live sample** from recent runs, not a global catalog API.

### UI package consumption

- `@ontogony/ui` requires **build before frontend**; `file:../ontogony-ui` is not an npm workspace.
- **No npm registry publish** assumed; `npm pack` validates packaging only.
- Full `ontogony-ui` `npm run check` (Storybook, size limits) is heavier than the packaging subset used in integration evidence.

### Operator automation gaps

- Browser walkthrough for secret hygiene and some banners remains **partially manual** beyond scripted HTTP/Playwright gates.
- Evidence scripts write **local redacted JSON** under `docker/local-working-system/artifacts/` — not a centralized observability backend.

### Documentation scope

- Historical archives (`_donors/`, old planning) may still mention **Agentor** / **Athanor** — active operator docs use [ONTOGONY_TERMINOLOGY_GLOSSARY.md](../operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md).

## Safety (non-limitations — do not waive)

| Rule | Status |
| --- | --- |
| Real external execution | Disabled by default |
| Secrets in `VITE_*` or committed reports | Forbidden |
| Production readiness claim | Not made |
| `@ontogony/ui/src/*` imports in product apps | Forbidden |

## Source alignment

| Topic | Canonical doc |
| --- | --- |
| Terminology | `docs/operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md` |
| Fixture vs live | `ontogony-frontend/docs/operators/FRONTEND_FIXTURE_LIVE_BOUNDARY.md` |
| Trace/correlation | `docs/operators/TRACE_CORRELATION_CONTRACT.md` |
| UI packaging | `ontogony-ui/docs/development/PACKAGING_STATUS.md` |
| Docker-local limitations | `docs/releases/FIRST_DOCKER_LOCAL_WORKING_SYSTEM_KNOWN_LIMITATIONS.md` |
