# FE-LOCAL-CORS-001 — Docker-local browser API CORS evidence

**Status:** Implementation complete (operator verification pending rebuild)  
**Scope:** Docker-local / Development only — **not production readiness**

## Problem

- Docker-local stack and fake/real-provider backend flows **PASS**.
- Operator settings in browser `localStorage` were **valid** (`allagma.baseUrl`, `allagma.serviceToken`, Conexus admin key, Kanon token).
- Browser `fetch` from `http://localhost:5175` to backend APIs failed with CORS (`No Access-Control-Allow-Origin`).
- UI copy implied **“Allagma not configured”** when credentials were present but the browser could not reach APIs.

## Root cause

Allagma and Kanon APIs had **no CORS middleware**. Conexus had Development CORS but browser calls still needed explicit Docker-local origin wiring for port **5175**.

## Fix summary

| Repo | Change |
|------|--------|
| `allagma-dotnet` | `Allagma:Cors` options; Development allows `localhost:5175` / `127.0.0.1:5175`; Production disabled |
| `kanon-dotnet` | `Kanon:Cors` options; same Development origins |
| `conexus-dotnet` | Explicit CORS methods/headers (`Authorization`, `X-Conexus-Admin-Key`, `X-API-Key`, `Content-Type`) |
| `ontogony-frontend` | Classify `Failed to fetch` as browser reachability; stop “not configured” wording on CORS failures |
| `ontogony-platform` | Docker-compose CORS env; README browser API section |

## Operator settings (no secrets)

Verified pattern (keys redacted):

```text
localStorage key: ontogony.frontend.operator-settings.v1
allagma.baseUrl     → http://localhost:5083
allagma.serviceToken → [present]
conexus.baseUrl     → http://localhost:5082
conexus.adminApiKey → [present]
kanon.baseUrl       → http://localhost:5081
kanon.serviceToken  → [present]
```

## Original CORS failure (before fix)

DevTools from `http://localhost:5175`:

```text
Access to fetch at 'http://localhost:5083/allagma/v0/evaluations' from origin
'http://localhost:5175' has been blocked by CORS policy:
No 'Access-Control-Allow-Origin' header is present on the requested resource.
```

## Browser fetch checks (after fix — operator rerun)

Rebuild API images, restart compose, then from DevTools Console:

```javascript
const s = JSON.parse(localStorage.getItem("ontogony.frontend.operator-settings.v1"));

// Allagma — expect status 200, no CORS error
await fetch("http://localhost:5083/allagma/v0/evaluations", {
  headers: { Authorization: "Bearer " + s.allagma.serviceToken }
}).then(async r => ({
  status: r.status,
  contentType: r.headers.get("content-type"),
  bodyStart: (await r.text()).slice(0, 200)
}));

// Conexus — expect normal HTTP status (200 when admin key valid)
await fetch("http://localhost:5082/admin/v0/provider-inventory", {
  headers: { "X-Conexus-Admin-Key": s.conexus.adminApiKey }
}).then(async r => ({
  status: r.status,
  contentType: r.headers.get("content-type"),
  bodyStart: (await r.text()).slice(0, 200)
}));

// Kanon — expect status 200
await fetch("http://localhost:5081/health").then(async r => ({
  status: r.status,
  contentType: r.headers.get("content-type"),
  bodyStart: (await r.text()).slice(0, 200)
}));
```

## UI verification (after fix — operator rerun)

| Route | Expected |
|-------|----------|
| `/allagma/evaluations` | Live evaluation data |
| `/allagma/evaluations/baseline-comparisons` | Live data |
| `/allagma/evaluations/datasets` | Live data |
| `/conexus/observability` | Provider/route evidence |
| `/settings` | Connection health degraded/unreachable — **not** “not configured” when URLs/tokens present |
| Settings form | **No** OpenAI/provider API key field |

## Automated tests

- `allagma-dotnet`: `CorsIntegrationTests`
- `kanon-dotnet`: `CorsIntegrationTests`
- `conexus-dotnet`: `CorsIntegrationTests` (5175 origin)
- `ontogony-frontend`: `operatorQueryErrorDisplay.test.ts`, `ProductLiveQueryState.test.tsx`

## Boundaries preserved

- No production CORS wildcard.
- No provider keys in UI.
- No real-provider behavior changes.
- Fake provider remains default.
- No secrets in git diff.

---

## FE-LOCAL-CORS-001A addendum — Kanon Ontogony actor/header CORS follow-up

**Status:** Implementation complete (operator browser verification pending rebuild)  
**Scope:** Docker-local / Development only — **not production readiness**

### Context after FE-LOCAL-CORS-001

- Allagma evaluations load live in the browser from `http://localhost:5175`.
- Operator `localStorage` settings remain valid.
- Remaining browser CORS failure on Kanon operator routes that send Ontogony actor headers.

### Remaining failure (before 001A)

DevTools from `http://localhost:5175`:

```text
Access to fetch at 'http://localhost:5081/ontology/v0/domain-packs' from origin
'http://localhost:5175' has been blocked by CORS policy: Request header field
x-ontogony-actor-id is not allowed by Access-Control-Allow-Headers.
```

Frontend Kanon client sends `X-Ontogony-Actor-Id` and `X-Ontogony-Actor-Type` when `defaultActorId` is set.

### Fix (001A)

| Repo | Change |
|------|--------|
| `kanon-dotnet` | Extend `Kanon:Cors` allowed request headers: `Authorization`, `Content-Type`, `X-Ontogony-Actor-Id`, `X-Ontogony-Actor-Type`, plus standard Ontogony context headers (`X-Ontogony-Trace-Id`, `X-Ontogony-Correlation-Id`, `X-Ontogony-Tenant-Id`, `X-Ontogony-Idempotency-Key`); add OPTIONS preflight test for `/ontology/v0/domain-packs` |
| `ontogony-platform` | This addendum; README browser API header list |

Allagma and Conexus CORS behavior unchanged.

### Browser fetch check (after 001A — operator rerun)

Rebuild Kanon API container, restart compose, then from DevTools Console:

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

Expected: no CORS failure; normal HTTP status (ideally 200).

### UI verification (after 001A — operator rerun)

| Route | Expected |
|-------|----------|
| `/kanon/domain-packs` (if routed) | Live domain pack data |
| `/allagma/evaluations` | Continues to load live data |
| `/conexus/observability` | Continues to load |

### Automated tests (001A)

- `kanon-dotnet`: `CorsIntegrationTests.OPTIONS_preflight_for_domain_packs_allows_ontogony_actor_headers`
