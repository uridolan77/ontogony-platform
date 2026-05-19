# KANON-DEEPEN-000 — Current state audit

**Date:** 2026-05-20  
**Scope:** Kanon semantic authority — backend contracts, operator frontend, actor/role behavior, cross-service identifiers  
**Repos reviewed:** `kanon-dotnet`, `ontogony-frontend`, `ontogony-ui` (shared primitives only), `ontogony-platform` (local stack), `allagma-dotnet`, `conexus-dotnet` (contracts/metadata)  
**Mode:** Audit only — no product code changes in this step.

## Executive summary

Kanon’s **backend is deep and Phase 3–complete** for v0: 38 OpenAPI paths under `/ontology/v0` plus `/health`, `/ready`, and service root. The **operator problem is legibility and authorization clarity**, not missing core semantics. The browser can reach Kanon; domain-pack read is correctly gated (`Admin | System | Auditor`) while many other read routes (ontology versions, source bindings, health) do not use the domain-pack role matrix.

The frontend already has **seven Kanon routes**, typed client wrappers for most read paths, a **decisions/provenance workbench** with trace/entity lookup, and **recent 403 UX** (`KanonActorAuthorizationCard`) on overview and domain-packs. Gaps cluster into:

1. **Local operator roles** — stale or empty `defaultActorRoles` in browser local settings produces real 403s that look like “Kanon is broken.”
2. **Shallow explorers** — POST-only facts/plans, no Conexus assistance UI, no actions/topology evaluate UI, incomplete domain-pack active/lifecycle surfacing.
3. **Cross-service links** — Allagma → Kanon links exist; Kanon pages rarely deep-link back to Allagma/Conexus; Conexus observability list/detail is still shallow on the frontend.

### KANON-DEEPEN-001 recommendation: **GO**

Proceed with **KANON-DEEPEN-001** (local operator auth + read workbench) as specified in the enhancement package. Prefer **Option A** from `07_AUTH_AND_LOCAL_OPERATOR_PLAN.md`: ensure Docker-local `local-operator` reliably has a **read role (`Auditor`)** while keeping mutation gates on `Admin | System`. Pair with frontend overview improvements (actor id, roles, read authority status) and retain the existing forbidden-state card when roles are still wrong.

---

## 1. Backend contract inventory

**Sources:** `kanon-dotnet/docs/CURRENT_IMPLEMENTATION_STATUS.md`, `kanon-dotnet/docs/api/kanon-openapi-v1.json` (38 paths), `kanon-dotnet/src/Kanon.Api/Program.cs` (authoritative).

### 1.1 Platform / host

| Family | Routes | Auth notes |
|---|---|---|
| Service root | `GET /` | Public |
| Health | `GET /health`, `GET /ready` | Public; Postgres/domain-pack checks on `/ready` |
| OpenAPI doc | `GET /openapi/v1.json` | Public (document name v1; **ontology prefix remains v0**) |

### 1.2 `/ontology/v0` by family

| Family | Methods | Role / auth (where enforced) |
|---|---|---|
| **Source bindings** | `POST` create, `GET` list, `POST` `{id}/review`, `POST` `{id}/test` | Service token on group in `ServiceToken` mode; no domain-pack role gate |
| **Ontology versions** | `POST` validate, `GET` `{ontologyId}/versions`, `POST` publish, `DELETE` draft | Same; list/publish/delete are operator-facing without domain-pack matrix |
| **Semantic plans** | `POST` compile | Emits decision record; typed 400 DTO on validation failure |
| **Canonical facts** | `POST` resolve | Emits decision record |
| **Actions / gates** | `POST` evaluate, `POST` human-gates/check, `POST` human-gates/{id}/resolve | Evaluate emits decisions; human-gate resolve used from Allagma via frontend |
| **Execution topologies** | `POST` evaluate | Present in OpenAPI/Program; **not listed** in `CURRENT_IMPLEMENTATION_STATUS.md` API block — doc drift only |
| **Decision records** | `POST` create, `GET` by id | Provenance sub-routes role-gated |
| **Provenance / replay** | `GET` provenance, `POST` verify, `POST` prepare-replay, `GET` replay-bundles (+ `{bundleId}`, `/export`), `GET` by-trace, `GET` by-entity | `ProvenanceReader \| Auditor \| Admin \| System` |
| **Domain packs** | `GET` list/detail/active, `GET` `{packId}/lifecycle`, `POST` validate/load/promote/deprecate | Read: `Admin \| System \| Auditor`; mutate: `Admin \| System`; HTTP load also `AllowHttpLoad` |
| **Conexus assistance** | Five `POST` `/conexus/assistance/*` | Feature-flagged; `AllowedRoles` (default includes Admin/System/Auditor/Reviewer); **draft_only**, non-authoritative |

### 1.3 Backend vs package snapshot drift

The incoming package `02_REPO_REVIEW_FINDINGS.md` route list is **missing** governance routes already shipped: `promote`, `deprecate`, `GET .../active`, `GET .../{packId}/lifecycle`, replay bundle list/export, `execution-topologies/evaluate`, ontology publish/delete, and all Conexus assistance routes. Treat **`Program.cs` + OpenAPI baseline** as ground truth.

### 1.4 Intentional POST-only “read” gaps

| Operator question | Existing contract | Gap |
|---|---|---|
| Browse resolved facts history | `POST .../canonical-facts/resolve` only | No list/history endpoint |
| Browse compiled plans | `POST .../semantic-query-plans/compile` only | Plans persisted in Postgres but no list API in v0 surface |
| Browse action evaluations | `POST .../actions/evaluate` only | No browse endpoint |

**Rule (from package):** Add backend list/history only when UI cannot answer the question without awkward client-side reconstruction.

---

## 2. Frontend surface inventory

**Sources:** `ontogony-frontend/src/app/routes.tsx`, `src/kanon/**`.

### 2.1 Route map

| Route | Page | Primary hooks / client | Backend calls |
|---|---|---|---|
| `/kanon` | `KanonOverviewPage` | `useKanonHealth`, `useKanonOntologyVersions`, `useKanonDomainPacks` | `GET /health`, `GET .../ontologies/{id}/versions`, `GET .../domain-packs` |
| `/kanon/ontologies` | `OntologyVersionsPage` | `useKanonOntologyVersions` | List versions; panels call validate/publish/delete |
| `/kanon/source-bindings` | `SourceBindingsPage` | `useKanonOntologyVersions`, `useKanonSourceBindings` | `GET` bindings; create/review/test mutations |
| `/kanon/facts` | `FactsAndPlansPage` | `useKanonResolveCanonicalFact` | `POST .../canonical-facts/resolve` (interactive, not history) |
| `/kanon/plans` | `KanonSemanticPlansPage` | compile mutation | `POST .../semantic-query-plans/compile` |
| `/kanon/decisions` | `DecisionsProvenancePage` | `KanonDecisionsProvenanceWorkbench` | decision lookup, provenance, replay bundles; `LiveTraceCorrelationPanel` |
| `/kanon/domain-packs` | `DomainPacksPage` | `useKanonDomainPacks`, lifecycle workbench | `GET .../domain-packs`; lifecycle mutations + `GET .../{packId}/lifecycle` |

**Not exposed in UI (backend exists):** Conexus assistance (all five), `POST .../actions/evaluate`, `POST .../execution-topologies/evaluate`, `GET .../domain-packs/active`, `GET .../domain-packs/{packId}/active`, decision `verify` / `prepare-replay` / export (partially via replay panel), `POST .../decision-records` create.

### 2.2 Client and actor headers

`kanonClient.ts` sends on every mutating/read call (when configured):

- `X-Ontogony-Actor-Id` ← `settings.allagma.defaultActorId`
- `X-Ontogony-Actor-Type` ← `human`
- `X-Ontogony-Roles` ← comma-split `settings.allagma.defaultActorRoles`
- `Authorization: Bearer` ← `settings.kanon.serviceToken` when set

Defaults in `operatorSettingsTypes.ts`: `local-operator` + `Admin`. **Persisted settings** merge stored `allagma` over defaults (`operatorSettingsStorage.ts`), so a saved empty `defaultActorRoles` **overrides** the default and causes domain-pack 403.

### 2.3 Shared UI

- Semantic panels (`DecisionRecordPanel`, `ProvenanceTrailPanel`, `ReplayBundlePanel`, etc.) live in **`ontogony-ui`** — correct layer.
- Domain-pack forbidden UX: **`KanonActorAuthorizationCard`** + `isKanonActorRoleForbidden` — correct product layer.

### 2.4 Health probes

`serviceHealthContracts.ts` probes Kanon with `GET /health` and `GET /ready` only — **aligned** with Kanon (unlike Conexus multi-path liveness). No erroneous generic-path 404 noise for Kanon.

---

## 3. Actor / role audit

### 3.1 Backend policy (grounded)

| Surface | Required roles | Implementation |
|---|---|---|
| Domain-pack read | `Admin`, `System`, `Auditor` | `KanonDomainPackManagementAuthorization.EnsureReadAccess` |
| Domain-pack mutate | `Admin`, `System` | `EnsureMutateAccess` |
| Provenance / replay read | `ProvenanceReader`, `Auditor`, `Admin`, `System` | `KanonProvenanceAuthorization` |
| Other `/ontology/v0` routes | Trusted actor context; **no** domain-pack matrix | Headers + optional service token |

Documented in `docs/security/DOMAIN_PACK_MANAGEMENT_AUTHORIZATION.md`.

### 3.2 Local operator configuration

| Source | Actor | Roles |
|---|---|---|
| Frontend defaults | `local-operator` | `Admin` |
| Docker seed scripts (platform) | Various test calls | Often `ProvenanceReader` for provenance scripts — **not** the browser operator path |
| Browser localStorage | User-overridden | May be **empty** → 403 on domain-packs |

### 3.3 403 classification

| Symptom | Classification | Current UX |
|---|---|---|
| `Actor 'local-operator' lacks a role for domain-pack read (...)` | **Expected** if roles header empty/wrong | **Well surfaced** on `/kanon` and `/kanon/domain-packs` via `KanonActorAuthorizationCard` |
| Same 403 on routes without dedicated card | **Expected** but easy to misread as outage | Other pages show generic error from `ProductLiveQueryState` |
| Missing service token in `ServiceToken` mode | **401** transport auth | Settings copy mentions token |
| `AllowHttpLoad=false` on load | **403** `DomainPackHttpLoadDisabled` | Lifecycle workbench capability metadata; not always tied to role card |

**Not incorrect backend policy:** domain-pack 403 for an unroled actor is correct. The defect is **workflow/defaults** (local operator not reliably granted read role) and **inconsistent forbidden UX** across routes.

### 3.4 Provenance vs domain-pack roles

Default frontend role `Admin` satisfies **both** provenance and domain-pack read. Operators using only `ProvenanceReader` (as in platform shell scripts) can read provenance but **not** domain packs — by design. DEEPEN-001 should document that split.

---

## 4. Cross-service semantic links audit

**Canonical contract:** `allagma-dotnet/docs/contracts/CROSS_SYSTEM_IDENTIFIERS.md`.

### 4.1 Identifier ownership

| ID | Owner | Kanon persistence / API |
|---|---|---|
| `decisionId` | Kanon | `GET /decision-records/{id}`, by-trace, by-entity |
| `humanGateId` | Kanon | Human-gate routes; referenced in provenance entity index |
| `traceId`, `correlationId` | Platform | On decision records; `GET .../by-trace/{traceId}` |
| `runId`, `toolIntentId` | Allagma | Cited in Kanon metadata / entity index roles |
| `modelCallId` | Conexus | Surfaced in correlation view, not a Kanon API |

### 4.2 Allagma → Kanon (frontend)

- Run detail and triage build links to `/kanon/decisions?decisionId=...` and trace/entity query params.
- `AllagmaReplayEvidenceJourneySection` + `CrossServiceLinksCard` expose planning/action/human-gate/model-call slots.
- Allagma run metadata carries `kanonDecisionId`, `planningDecisionId`, `traceId` (generated schema).

### 4.3 Kanon → Allagma / Conexus

- `KanonDecisionsProvenanceWorkbench` uses `useTraceCorrelation` and `LiveTraceCorrelationPanel` when `runId` is present.
- `buildKanonProvenanceEvidence` can include `conexusModelCallId` from correlation.
- **Gap:** Kanon-native pages do not consistently offer “open Allagma run” / “open Conexus model call” chips comparable to the Allagma journey card.

### 4.4 Conexus → Kanon

- Conexus journals `allagma_run_id`, `correlation_id`; admin observability is deepening separately (Conexus DEEPEN package).
- Kanon Conexus assistance routes are **server-only** today; no operator UI.

### 4.5 Entity index (KANON-PROV-INDEX-001)

`GET /decision-records/by-entity/{entityRef}` uses explicit `decision_record_entity_ref` roles (`input`, `output`, `fact`, `policy_context`, `source_binding`, `human_gate`, `domain_pack`, `tool_intent`). Frontend supports entity lookup mode in decisions workbench.

---

## 5. Gap matrix

| Operator question | Current API | Current frontend | Data / seed | Gap type | Recommended next |
|---|---|---|---|---|---|
| Which actor am I using? | Actor from headers | Partially (forbidden card shows id/roles on 2 pages) | Default `local-operator` | **frontend** | DEEPEN-001 overview panel |
| Can this actor read domain packs? | `GET /domain-packs` | Auth card on overview + domain-packs | Roles may be empty in localStorage | **seed / auth / frontend** | DEEPEN-001: default `Auditor` for docker-local + settings migration |
| What packs exist / which is active? | `GET /domain-packs`, `GET .../active`, `GET .../{packId}/active` | List only; no active badge/timeline | `gaming-core` dev seed | **frontend** | DEEPEN-002 lifecycle workbench |
| What happened to this pack version? | `GET .../{packId}/lifecycle` | Lifecycle workbench (conservative JSON mapping) | Postgres lifecycle rows when enabled | **frontend** | DEEPEN-002 enrich timeline + decision ids |
| What ontology versions exist? | `GET .../versions` | Timeline + validate/publish/delete | Configured `ontologyId` | **frontend** (polish) | DEEPEN-001 read summary; later explorer depth |
| Approved source bindings? | `GET /source-bindings` | Filter by version; review/test | Seed bindings in dev | **frontend** | DEEPEN-004 filters/table |
| Facts used for a decision? | resolve POST + provenance | Facts page is POST playground | — | **frontend / docs** | DEEPEN-004 link facts ↔ decisions; list API only if needed |
| Semantic plans history? | compile POST | Plans page compile-only | — | **backend? / docs** | Defer list API unless persisted browse required |
| Why was this decision made? | provenance + verify + replay | Decisions workbench | — | **frontend** | DEEPEN-003 cross-links + copy/export polish |
| Decisions for this trace? | `by-trace/{traceId}` | Lookup mode + Allagma links | — | **frontend** | DEEPEN-005 prefill from Allagma/Conexus |
| Decisions for this entity? | `by-entity/{entityRef}` | Lookup mode | Entity index in Postgres | **frontend** | DEEPEN-003 entity workbench |
| Replay / verify evidence? | replay-bundles + export | `ReplayBundlePanel` | Signing key optional | **frontend** | DEEPEN-003 verify/export UX |
| Draft review notes from model? | 5× Conexus assistance POST | None | Feature flag off by default | **frontend** | Later slice; enable flag in local stack doc |
| Evaluate tool / topology policy? | evaluate routes | None (Allagma owns runtime) | — | **docs** | Keep in Allagma; link outcomes to Kanon decisions |
| Open Kanon from Conexus request? | trace/modelCall in journals | Conexus pages vary | — | **frontend** | DEEPEN-005 |

---

## 6. Implementation-layer correctness review

| Change class | Expected repo | Observed | Verdict |
|---|---|---|---|
| CORS / exposed headers for operator browser | `kanon-dotnet` (and gateway) | Documented as handled pre-package | **Correct layer** |
| Shared dialog / layout primitives | `ontogony-ui` | Used by Kanon pages | **Correct layer** |
| Kanon route pages and API client | `ontogony-frontend` | `src/kanon/*` | **Correct layer** |
| Cross-service identifier contract | `allagma-dotnet` + platform | `CROSS_SYSTEM_IDENTIFIERS.md` | **Correct layer** |
| Domain-pack authorization rules | `kanon-dotnet` | Application + Infrastructure | **Correct layer** |
| Reusable HTTP/trace mechanics | `ontogony-platform` | Consumed by Kanon/Allagma | **Correct layer** |

### Evidence / overclaim risks

- Could not load `kanon-dotnet/docs/evidence/KANON_OP_001_OPERATOR_EVIDENCE.md` in this workspace path; treat any “browser verified” claims in older evidence as **stale until re-run** after DEEPEN-001.
- `CURRENT_IMPLEMENTATION_STATUS.md` API list omits `execution-topologies/evaluate` — minor **docs drift**, not a missing implementation.
- Frontend `getKanonDecisionByTrace` types a single `DecisionRecord`; API returns a **list** — verify adapter typing during DEEPEN-003 (potential client/schema mismatch).

### Frontend workarounds that should become contract

| Workaround | Recommendation |
|---|---|
| Entity lookup takes first decision only in `getKanonDecisionByEntity` | Use full list UI (workbench already does for entity mode); remove misleading “single” helper or align return type |
| Lifecycle `GET` returns `unknown` in client | Add OpenAPI schema for lifecycle response in Kanon (DEEPEN-002) |
| No `GET /domain-packs/active` client | Add thin client + overview badge (DEEPEN-002) |

---

## 7. KANON-DEEPEN-001 scope recommendation

**Go** with this minimal scope:

### Backend (`kanon-dotnet` + platform docker docs)

1. Confirm docker-local / dev documented actor roles include **`Auditor`** (or `Admin`) for `local-operator`.
2. Do **not** bypass `IDomainPackManagementAuthorization`.
3. Tests: roled actor reads packs; unroled actor 403; mutate still Admin/System only.

### Frontend (`ontogony-frontend`)

1. Overview: actor id, roles, read authority status, active ontology + pack summary.
2. Extend forbidden-state pattern to any page that calls domain-pack or provenance APIs without a card.
3. Settings hint: resetting operator settings restores `defaultActorRoles`.
4. Optional: on load, if `defaultActorRoles` is empty, merge default `Auditor` for local environment label.

### Platform docs

1. `docs/evidence/KANON_DEEPEN_001_LOCAL_OPERATOR_AUTH_AND_READ_WORKBENCH_EVIDENCE.md` per DEEPEN-001 prompt.
2. Manual routes: `/kanon`, `/kanon/domain-packs`, `/kanon/ontologies`.

### Explicitly out of scope for 001

- Domain-pack lifecycle timeline (DEEPEN-002).
- Full provenance explorer polish (DEEPEN-003).
- New list/history APIs for facts/plans (DEEPEN-004, only if still blocked).
- Conexus assistance UI.

---

## 8. Acceptance checklist (DEEPEN-000)

| Criterion | Status |
|---|---|
| No product code changes (audit only) | **Met** — this document only |
| Grounded backend route inventory | **Met** — Program.cs + OpenAPI |
| Frontend route → API map | **Met** |
| Actor/role 403 classification | **Met** |
| Cross-service link map | **Met** |
| Gap matrix with gap types | **Met** |
| Clear DEEPEN-001 go/no-go | **GO** |
| Backend vs frontend vs seed/auth gap list | **Met** — §5 and §7 |

---

## Related artifacts

- Package: `kanon-dotnet/docs/_incoming/Ontogony-Kanon-Deep-Enhancement-Package-v1/`
- Kanon status: `kanon-dotnet/docs/CURRENT_IMPLEMENTATION_STATUS.md`
- Next prompt: `.../prompts/KANON-DEEPEN-001_LOCAL_OPERATOR_AUTH_AND_READ_WORKBENCH.md`
