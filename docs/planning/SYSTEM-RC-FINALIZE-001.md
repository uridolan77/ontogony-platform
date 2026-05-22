# SYSTEM-RC-FINALIZE-001 — Current stage closure and manual QA entry

**Stage:** Alpha RC / operator-console validation  
**Not in scope:** New backend features unless manual QA proves a route is genuinely missing.

## Freeze (six repos)

During this closure pass, changes are limited to:

- docs reconciliation
- OpenAPI snapshot sync
- failing-test fixes
- manual-QA blocker fixes
- stale backend-waiting cleanup
- evidence capture

Repos:

```text
ontogony-platform
conexus-dotnet
kanon-dotnet
allagma-dotnet
ontogony-ui
ontogony-frontend
```

## Definition of done

1. Backend and frontend docs agree with committed OpenAPI snapshots.
2. No stale backend-waiting doc contradicts an implemented backend route.
3. Runtime lock (`allagma-dotnet/docs/system/ontogony-runtime.lock.json`) and protocol registry (`ontogony-platform/docs/system/system-protocol-registry.json`) are current.
4. Platform package inventory docs match `scripts/validate-shipping-inventory.ps1` (27 shipping packages).
5. All six repos pass their automated gates.
6. Allagma system cohesion smoke passes.
7. Frontend `npm run check:full` passes (sibling `ontogony-ui` + `packages/ontogony-agent-interaction` built; pinned `ONTOGONY_UI_REF` for release).
8. Manual QA sheet below is executed with recorded results.
9. Outcome is one of: green signoff, blocker list, or deferred limitation list.

## Reconciliation status (docs)

| Capability | Backend | Frontend | Doc status |
| --- | --- | --- | --- |
| Allagma start run | `POST /allagma/v0/runs` | `/allagma/runs/start` | Activated |
| Allagma retry/cancel/replay | POST mutations on run | Run ops + replay | Activated |
| Conexus model-call list | `GET /admin/v0/model-calls` | Observability + evidence spine | Activated |
| Conexus project list | **Absent** | Single-project mode | Backend-waiting |
| Kanon semantic plan detail | `GET /ontology/v0/semantic-query-plans/{planId}` | Plan history panel | Activated |
| `/ready` capability metadata | Partial per service | OpenAPI + health resolver | Backend-waiting |

Frontend reconciliation notes: [`ontogony-frontend/docs/phase-j-frontend-ui-tightening/backend-waiting/`](../../ontogony-frontend/docs/phase-j-frontend-ui-tightening/backend-waiting/).

## Platform public API snapshots (resolved)

Clean rebuild (`git clean -xdf`, Release restore/build/test on `main`) reproduced two `Ontogony.PublicApi.Tests` Verify failures. Inspection showed **intentional additive surface** from PLATFORM-9-003 already documented in `CHANGELOG.md`, not tooling drift:

| Assembly | Delta | Verdict |
| --- | --- | --- |
| `Ontogony.Http` | `OntogonyPropagationHeaderContract` (+ frozen header constants) | Accept snapshot |
| `Ontogony.Testing` | `HeaderPropagationConformanceAssertions`, `PropagationHeaderScenario` | Accept snapshot |

Snapshots were accepted on branch; full `dotnet test Ontogony.Platform.sln -c Release` must be green before RC sign-off. Not a manual-QA blocker for frontend/allagma pushes.

## Automated gates (order)

```powershell
# ontogony-ui
npm run check:full

# @ontogony/agent-interaction (before frontend typecheck — see ontogony-frontend integration doc)
cd ontogony-platform/packages/ontogony-agent-interaction
npm ci
npm run build

# ontogony-frontend (requires ontogony-ui + agent-interaction dist/)
npm run check:full

# ontogony-platform
dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln -c Release --no-restore
dotnet test Ontogony.Platform.sln -c Release --no-build

# Conexus
dotnet restore Conexus.sln
dotnet build Conexus.sln -c Release --no-restore -p:NoWarn=CS1591
dotnet test Conexus.sln -c Release --no-build -p:NoWarn=CS1591 --filter "Category!=ExternalProviderSmoke&Category!=LoadSoak&Category!=PersistenceSmoke&Category!=CapacityBaseline"

# Kanon
dotnet restore Kanon.sln
dotnet build Kanon.sln -c Release --no-restore
dotnet test Kanon.sln -c Release --no-build

# Allagma
dotnet restore Allagma.sln
dotnet build Allagma.sln -c Release --no-restore
dotnet test Allagma.sln -c Release --no-build --filter "Category!=CrossRepo&Category!=PersistenceSmoke"
```

System-level (after per-repo green):

```powershell
# Allagma — from allagma-dotnet repo root
./scripts/run-system-cohesion-smoke.ps1 -UseExistingServices

# Allagma restart survival (when Postgres/testcontainers available)
./scripts/restart-e2e-first-real-system.ps1

# Platform system compatibility
dotnet test tests/Ontogony.SystemCompatibility.Tests/Ontogony.SystemCompatibility.Tests.csproj `
  -c Release --filter "Category=SystemCompatGate"
```

Frontend release gate reference: [`ontogony-frontend/docs/RELEASE_CANDIDATE.md`](../../ontogony-frontend/docs/RELEASE_CANDIDATE.md).

## Manual QA — backend

Record: **PASS** | **FAIL** | **SKIP** | **Notes**

| # | Flow | Result | Notes |
| --- | --- | --- | --- |
| B1 | Health/readiness honest for Kanon, Conexus, Allagma | | |
| B2 | Conexus project API key → `/v1/chat/completions` | | |
| B3 | Conexus admin key → admin routes | | |
| B4 | Allagma service token → `/allagma/v0` | | |
| B5 | Kanon auth as configured | | |
| B6 | Conexus list models, chat, streaming, idempotency | | |
| B7 | Conexus model-call list/detail/evidence bundle | | |
| B8 | Conexus route decision + fallback fake provider | | |
| B9 | Kanon ontology versions, plan compile/detail | | |
| B10 | Kanon canonical facts, decisions, provenance | | |
| B11 | Kanon human gate check/resolve | | |
| B12 | Kanon domain-pack validate/load/promote | | |
| B13 | Kanon Conexus assistance (draft/redaction) | | |
| B14 | Allagma start → completed run (Kanon + Conexus) | | |
| B15 | Allagma run detail/events/audit/operations | | |
| B16 | Allagma human-gate pause/resume/deny | | |
| B17 | Allagma retry / cancel / replay | | |
| B18 | Allagma streaming model-purpose path | | |
| B19 | Allagma restart survival (Postgres if available) | | |
| B20 | Cross-service trace links run ↔ decision ↔ model-call | | |

## Manual QA — frontend (operator console)

Stack: build `ontogony-ui` → start backends → `ontogony-frontend` on port 5175 → configure `/settings`.

| # | Flow | Result | Notes |
| --- | --- | --- | --- |
| F1 | Boot, settings, service badges | | |
| F2 | `/system`, topology, evidence-spine, agent-interaction | | |
| F3 | Conexus overview, routing, projects, chat, observability | | |
| F4 | Conexus model-call list wired; project list limitation honest | | |
| F5 | Kanon ontologies, bindings, facts, plans, decisions | | |
| F6 | Kanon domain-packs, assistance, review-queue, policies | | |
| F7 | Allagma runs, start, detail, audit, gates, replay | | |
| F8 | Allagma evaluations + baseline comparisons | | |
| F9 | Empty/loading/error, bad token, downstream down | | |
| F10 | Redaction in diagnostics/export; keyboard destructive dialogs | | |
| F11 | Responsive/mobile; no console errors on primary routes | | |

## Signoff

| Role | Name | Date | Outcome |
| --- | --- | --- | --- |
| Operator QA | | | |
| Backend | | | |
| Frontend | | | |
