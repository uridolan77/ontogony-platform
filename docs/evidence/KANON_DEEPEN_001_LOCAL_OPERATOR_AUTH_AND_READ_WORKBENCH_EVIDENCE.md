# KANON-DEEPEN-001 — Local operator auth and read workbench

**Status:** Implementation complete (browser verification after frontend image rebuild)  
**Audit:** [`docs/reviews/KANON_DEEPEN_000_CURRENT_STATE_AUDIT.md`](../reviews/KANON_DEEPEN_000_CURRENT_STATE_AUDIT.md)  
**Scope:** Docker-local operator readability for Kanon semantic authority — **not production readiness**

## Problem

Browser calls to Kanon succeeded, but `GET /ontology/v0/domain-packs` returned **403** when `local-operator` had no `Admin`, `System`, or `Auditor` role in `X-Ontogony-Roles`. Stale or empty `defaultActorRoles` in browser localStorage overrode code defaults.

## Solution (Option A — preferred)

| Layer | Change |
|---|---|
| **Frontend defaults** | `local-operator` + `Auditor,ProvenanceReader` (read domain packs + provenance; no mutate roles) |
| **Settings normalization** | Backfill empty roles when `environmentLabel` is Local/docker-local |
| **Docker build args** | `VITE_KANON_DEFAULT_ACTOR_ID`, `VITE_KANON_DEFAULT_ACTOR_ROLES` in compose + Dockerfile |
| **Operator UX** | `KanonOperatorContextCard` on overview, domain-packs, ontologies — actor, roles, read authority badges |
| **Active packs summary** | `GET /ontology/v0/domain-packs/active` on overview when read is granted |
| **Backend** | Authorization unchanged; tests for Auditor read, no-role 403, mutate still Admin/System |

Backend authorization gates were **not** weakened.

## Repos touched

- `kanon-dotnet` — `DomainPackManagementApiTests` (local-operator + Auditor; AgentExecutor 403)
- `ontogony-frontend` — auth helpers, settings migration, overview/domain-packs/ontologies, active packs client
- `ontogony-platform` — docker compose env, `.env.example`, this evidence

## Validation

| Check | Command | Result |
|---|---|---|
| Kanon domain-pack auth tests | `dotnet test kanon-dotnet --filter DomainPackManagementApiTests` | Run after pull |
| Frontend unit tests | `npm test` in `ontogony-frontend` | Run after pull |
| Frontend typecheck | `npm run typecheck` in `ontogony-frontend` | Run after pull |

## Manual verification (after `docker compose build ontogony-frontend`)

From `docker/local-working-system`:

```powershell
docker compose build ontogony-frontend kanon-api
docker compose up -d
```

| Route / action | Expected |
|---|---|
| `/kanon` | Operator context card shows actor `local-operator`, roles `Auditor, ProvenanceReader`, domain-pack read **Granted** |
| `/kanon/domain-packs` | Pack list loads (or empty state), not generic service-down error |
| `/kanon/ontologies` | Versions load; docker-local hint visible |
| Settings → reset | Restores `Auditor,ProvenanceReader` defaults |

### DevTools spot-check (optional)

```javascript
const s = JSON.parse(localStorage.getItem("ontogony.frontend.operator-settings.v1"));
await fetch("http://localhost:5081/ontology/v0/domain-packs", {
  headers: {
    "X-Ontogony-Actor-Id": s.allagma.defaultActorId,
    "X-Ontogony-Actor-Type": "human",
    "X-Ontogony-Roles": s.allagma.defaultActorRoles,
  },
}).then((r) => ({ status: r.status }));
```

Expected: **200** (or **401** only if Kanon is in ServiceToken mode without bearer — DevelopmentTrustedHeaders does not require token).

### Negative case

Clear roles in settings (`defaultActorRoles` empty) and save — overview should show **Missing role** and remediation card; Kanon remains reachable (403 is authorization).

## Known limitations

- OpenAPI baseline does not yet schema-type `GET .../domain-packs/active` response; frontend uses a conservative DTO.
- Mutations (validate/load/promote) still require **Admin** or **System**; Auditor cannot mutate by design.
- Existing browsers with saved empty roles get backfill on read only for Local environment label.

## Follow-up

- **KANON-DEEPEN-002** — domain-pack lifecycle timeline and active-state workbench depth.
