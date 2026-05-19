# FE-LOCAL-OPERATOR-UX-001 — Local operator health clarity and navigation

**Status:** Implementation complete (operator browser verification pending rebuild)  
**Scope:** Docker-local / Development operator UX only — **not production readiness**

## Goals

1. Fix remaining Kanon Ontogony actor-header CORS for browser calls from `http://localhost:5175`.
2. Clarify service health/status semantics (live vs readiness strict vs browser blocked vs not configured).
3. Improve sidebar navigation with collapsible groups, scrollable primary nav, and pinned System/Settings.

## Part A — Kanon CORS (FE-LOCAL-CORS-001A)

Delivered in `kanon-dotnet` commit `82a70b1`:

- Development CORS allows `Authorization`, `Content-Type`, Ontogony actor/context headers.
- Integration test: OPTIONS preflight for `/ontology/v0/domain-packs` with `authorization,x-ontogony-actor-id,x-ontogony-actor-type`.

See also `docs/evidence/FE_LOCAL_CORS_001_DOCKER_LOCAL_BROWSER_API_EVIDENCE.md` addendum.

## Part B — Health/status clarity

| Signal | Operator-facing label | Notes |
|--------|----------------------|-------|
| `/health` OK, `/ready` not green | Live · readiness strict (not ready) | Does not mark service down; product routes may still work |
| Missing `/health` version | Live · version metadata missing | Compatibility warning only |
| Browser CORS / fetch failure | Browser blocked | Distinct from generic degraded |
| Empty base URL | Not configured | Settings copy, not service down |
| `/health` down / 5xx | Unreachable | Service error detail shown |

**Repos:** `ontogony-frontend` (presentation + probes), `@ontogony/ui` (card labels, settings `healthMessage`).

**Diagnostics export:** per-service `operatorStatus` + `note` (redacted; no secrets).

## Part C — Sidebar/navigation

| Change | Detail |
|--------|--------|
| Collapsible groups | Command center, Conexus, Kanon, Allagma, System |
| Scroll | Primary nav scrolls; brand header sticky |
| Utility section | Settings + Topology pinned below scroll area |
| Persistence | `ontogony-ui.shell.nav.collapsed.v1` in localStorage |
| Accessibility | `aria-expanded` on group toggles; active route expands group |

## Operator verification (after rebuild)

From `docker/local-working-system`:

```powershell
cd C:\dev\ontogony-platform\docker\local-working-system
docker compose build kanon-api ontogony-frontend
docker compose up -d
.\scripts\wait-local-working-system.ps1
```

### Kanon CORS (DevTools on `http://localhost:5175`)

```javascript
const s = JSON.parse(localStorage.getItem("ontogony.frontend.operator-settings.v1"));

await fetch("http://localhost:5081/ontology/v0/domain-packs", {
  headers: {
    Authorization: "Bearer " + s.kanon.serviceToken,
    "X-Ontogony-Actor-Id": s.allagma.defaultActorId || "local-operator",
    "X-Ontogony-Actor-Type": "human"
  }
}).then(async r => ({
  status: r.status,
  contentType: r.headers.get("content-type"),
  bodyStart: (await r.text()).slice(0, 200)
})).catch(e => ({ error: String(e) }));
```

Expected: no CORS failure; HTTP 200 (or normal API error, not browser block).

### UI checks

| Route | Expected |
|-------|----------|
| `/allagma/evaluations` | Live evaluation rows |
| `/kanon/domain-packs` | No actor-header CORS errors |
| Header badges | Explain live vs readiness strict vs compatibility warning |
| Sidebar | Groups collapse/expand; Settings reachable in pinned System section |
| Settings | No OpenAI/provider key field; diagnostics contain no secrets |

## Automated tests

- `kanon-dotnet`: `CorsIntegrationTests` (4 tests)
- `ontogony-frontend`: `serviceHealthPresentation.test.ts`, `nav.test.ts`, existing health adapter tests
- `ontogony-ui`: built with layout/system prop updates

## Boundaries preserved

- No production CORS wildcard.
- No provider keys in UI.
- No real-provider behavior changes.
- Fake provider remains default.
- No secrets in git diff.
