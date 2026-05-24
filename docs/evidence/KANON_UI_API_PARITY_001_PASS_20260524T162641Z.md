# KANON-UI-API-PARITY-001 — cross-repo closure (PASS)

**Status:** CLOSED  
**Recorded:** 2026-05-24T16:26:41Z (Docker-local Playwright)  
**Package:** Phase 10 in [`docs/_incoming/NEXT.md`](../_incoming/NEXT.md)

---

## Environment

| Service | URL |
| --- | --- |
| Kanon | `http://localhost:5081` |
| Conexus | `http://localhost:5082` |
| Allagma | `http://localhost:5083` |
| Frontend (Docker) | `http://localhost:5175` |

Operator run from `C:\dev\ontogony-frontend` — `npm run docker:smoke:domain-switcher` (exit 0).

---

## Scope (Phase 10 slices)

| Slice | Repo | Deliverable |
| --- | --- | --- |
| 10.2 OpenAPI resync | `ontogony-frontend` | `openapi/kanon.v0.json`, generated `schema.ts` |
| 10.3 DTO shim removal | `ontogony-frontend` | Provenance + source-binding contracts from generated schema |
| 10.4 Schema coverage test | `ontogony-frontend` | `kanonClientSchemaCoverage.test.ts` |
| 10.1 Route usage taxonomy | `ontogony-frontend`, `kanon-dotnet` | 4-axis coverage JSON + fragment + policy doc |
| 10.5 Domain Switcher Docker smoke | `ontogony-frontend` | `domain-switcher-docker-live.spec.ts` |

---

## Commands

```powershell
cd c:\dev\ontogony-frontend
npm run docker:smoke:domain-switcher
```

Test-only (stack + image already current):

```powershell
cd c:\dev\ontogony-frontend
npm run test:e2e:docker-live:domain-switcher
```

Frontend unit/contract gates (reference):

```powershell
cd c:\dev\ontogony-frontend
npm run openapi:check
npm run kanon:operator-ui-coverage:check
npx vitest run src/kanon/contracts/kanonClientSchemaCoverage.test.ts
npx vitest run src/kanon/contracts/kanonOperatorUiRouteCoverage.test.ts
```

Kanon taxonomy gate (reference):

```powershell
cd c:\dev\kanon-dotnet
dotnet test tests/Kanon.Tests --filter "FullyQualifiedName~OntologyV0Route"
```

---

## Docker-live smoke identifiers (2026-05-24T16:26:41Z)

| Field | Value |
| --- | --- |
| `baselineOntologyVersionId` | `gaming-core@0.1.0` |
| `alternateOntologyVersionId` | `gaming-core@0.2.0` |
| Conexus alias (unchanged) | `gpt-4o-mini` |
| Domain packs on disk | 2 (`gaming-core`, `gaming-core-bump`) |
| Active pack rows | 5 |

---

## Verdict

| Gate | Result |
| --- | --- |
| Kanon domain-pack API preflight | PASS |
| Domain switcher UI + settings patch | PASS |
| Start Run ontology sync | PASS |
| Source Bindings context card sync | PASS |
| Evidence Spine banner sync | PASS |
| Playwright (`domain-switcher-docker-live`) | **1 passed** |

Smoke steps (`domain-switcher-docker-live-smoke-report-v1`):

1. `kanon-domain-pack-api-preflight` — PASS  
2. `domain-switch-ui` — switched to `gaming-core@0.2.0`; Conexus alias unchanged  
3. `downstream-domain-sync` — Start Run, Source Bindings context, Evidence Spine aligned  

---

## Preserved artifacts

- Committed baseline: [`artifacts/kanon-ui-api-parity-001/20260524T162641Z/domain-switcher-docker-live-smoke-report.json`](./artifacts/kanon-ui-api-parity-001/20260524T162641Z/domain-switcher-docker-live-smoke-report.json)
- Ephemeral mirror: `docker/local-working-system/artifacts/domain-switcher-docker-live-smoke-report.json`
- Frontend evidence: [ontogony-frontend `KANON_UI_API_PARITY_001_EVIDENCE.md`](https://github.com/uridolan77/ontogony-frontend/blob/main/docs/evidence/KANON_UI_API_PARITY_001_EVIDENCE.md)
- Kanon taxonomy evidence: [kanon-dotnet `KANON_UI_API_PARITY_001_ROUTE_TAXONOMY_EVIDENCE.md`](https://github.com/uridolan77/kanon-dotnet/blob/main/docs/evidence/KANON_UI_API_PARITY_001_ROUTE_TAXONOMY_EVIDENCE.md)

---

## Known caveats

- `gaming-core@0.2.0` is a **disk pack** (`gaming-core-bump`), not a published ontology version; Source Bindings `<select>` may only list `0.1.0 (active)` while the operator context card still reflects the switched settings version.
- `seedOperatorSettings({ force: true, once: true })` is required for cross-page domain-switch smoke so init script does not reset settings on every navigation.
- Local Docker stack only; not production IAM.

---

## Next workstream

Pick next package from [`docs/_incoming/NEXT.md`](../_incoming/NEXT.md) or [`docs/_CURRENT_PLAN.md`](../_CURRENT_PLAN.md).
