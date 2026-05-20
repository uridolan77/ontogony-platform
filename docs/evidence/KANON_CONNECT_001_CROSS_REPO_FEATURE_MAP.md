# KANON-CONNECT-001 — Cross-repo Kanon feature map

**Recorded at (UTC):** 2026-05-20  
**Slice:** KANON-CONNECT-001 (Phase 1 — canonical connection inventory)  
**Verdict:** **PASS** (inventory complete; gaps named with follow-up items)  
**Statement:** Authoritative cross-repo inventory for Kanon deepening v2 (007–014) surfaces. **Not** a production-readiness claim. Real external tool execution remains blocked.

## Scope

| In scope | Out of scope |
| --- | --- |
| Map every Kanon feature to backend, frontend, settings, Allagma, Conexus, Evidence Spine, Docker-local | New Kanon product capability |
| Name explicit gaps and follow-up slice IDs | SYSTEM-ALPHA runtime lock promotion |
| Reference existing validation tests and evidence | Enterprise IAM expansion |

## Inventory sources (authoritative)

| Source | Repo | Path |
| --- | --- | --- |
| Route + auth matrix | kanon-dotnet | [`docs/architecture/ONTOLOGY_V0_ROUTE_AUTH_MATRIX.md`](https://github.com/uridolan77/kanon-dotnet/blob/main/docs/architecture/ONTOLOGY_V0_ROUTE_AUTH_MATRIX.md) |
| Machine-readable routes | kanon-dotnet | [`docs/generated/ONTOLOGY_V0_ROUTE_INVENTORY.json`](https://github.com/uridolan77/kanon-dotnet/blob/main/docs/generated/ONTOLOGY_V0_ROUTE_INVENTORY.json) |
| OpenAPI document | kanon-dotnet | [`docs/api/kanon-openapi-v1.json`](https://github.com/uridolan77/kanon-dotnet/blob/main/docs/api/kanon-openapi-v1.json) |
| Frontend route catalog | ontogony-frontend | [`src/app/route-workflow-catalog.json`](https://github.com/uridolan77/ontogony-frontend/blob/main/src/app/route-workflow-catalog.json) |
| OpenAPI snapshot catalog | ontogony-frontend | [`src/shared/capability/openApiSnapshotCatalog.ts`](https://github.com/uridolan77/ontogony-frontend/blob/main/src/shared/capability/openApiSnapshotCatalog.ts) |
| Operator settings | ontogony-frontend | [`src/app/settings/operatorSettingsTypes.ts`](https://github.com/uridolan77/ontogony-frontend/blob/main/src/app/settings/operatorSettingsTypes.ts) |
| Docker-local compose | ontogony-platform | [`docker/local-working-system/docker-compose.yml`](https://github.com/uridolan77/ontogony-platform/blob/main/docker/local-working-system/docker-compose.yml) |
| Seed anchors | ontogony-platform | [`docker/local-working-system/scripts/seed-and-verify-local-working-system.ps1`](https://github.com/uridolan77/ontogony-platform/blob/main/docker/local-working-system/scripts/seed-and-verify-local-working-system.ps1) |
| Browser QA (007–014) | ontogony-platform | [`docs/evidence/KANON_DEEPEN_014_BROWSER_QA_BASELINE_CANDIDATE_EVIDENCE.md`](./KANON_DEEPEN_014_BROWSER_QA_BASELINE_CANDIDATE_EVIDENCE.md) |

---

## Feature connection matrix

Columns: **Backend** = primary `/ontology/v0` routes; **FE page** = operator route; **Evidence Spine** = unified resolver path when wired.

| Feature | Kanon backend route(s) | Frontend page / component | Frontend client / hook | Settings required | Allagma source / consumer | Conexus source / consumer | Evidence Spine resolver | Docker-local env vars | Validation test | Known gaps |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| **Ontology versions** | `GET …/ontologies/{id}/versions`; `POST …/ontologies/validate`; `POST …/ontology-versions/{id}/publish`; `DELETE …/ontology-versions/{id}` | `/kanon/ontologies` — `OntologyVersionsPage` | `useKanonOntologyVersions`; `kanonClient.listKanonOntologyVersions`; `useValidateKanonOntology` / `usePublishKanonOntologyVersion` / `useDeleteKanonOntologyVersion` | `kanon.baseUrl`, `kanon.ontologyId`, `kanon.serviceToken`, `kanon.ontologyVersionId` | **Consumer:** run `ontologyVersionId` on start/resume. **Link:** `kanonOntologyVersionHref` in `buildAllagmaSemanticAuthorityLinkSlots` | — | Indirect via `ontologyVersionId` on run nodes in `resolveEvidenceSpine` | `VITE_KANON_ONTOLOGY_*` build args; seed `gaming-core@0.1.0` | `OntologyV0RouteInventoryTests`; FE `OntologyVersionsPage.test.tsx`; `validateOperatorSettingsDefaults.test.ts`; Playwright 014 step 2 | Resolved in **KANON-CONNECT-002** (`KanonSettingsHealthCard`, legacy migration) |
| **Domain packs** | `GET …/domain-packs`, `…/active`, `…/{packId}`, `…/lifecycle`; `POST …/validate`, `load`, `promote`, `deprecate` | `/kanon/domain-packs` — `DomainPacksPage`, `KanonDomainPackLifecycleWorkbench`, `KanonDomainPackActions` | `useKanonDomainPacks`, `useKanonActiveDomainPacks`; `kanonClient.listKanonDomainPacks`; lifecycle mutations | `kanon.baseUrl`, `kanon.serviceToken` (mutations); role gate `Admin`/`System` for mutate | **Link:** `kanonDomainPackHref` / `kanonDomainPackVersionHref` when run metadata includes pack id | — | Run graph may reference pack id when present on Allagma metadata | Same as Kanon service block | `DomainPackLifecycleApiTests`; FE `DomainPacksPage.test.tsx`; Playwright 014 step 7 | `domainPackId` not on Allagma run GET today — slot documents gap (**KANON-CONNECT-003**) |
| **Domain-pack evolution** | `POST …/domain-packs/diff`, `impact-analysis`, `migration-plan`, `simulate-promotion` | `/kanon/domain-packs` — `KanonDomainPackEvolutionPanel` | `useKanonDomainPackEvolution`; `kanonClient` diff/impact/migration/simulate | `kanon.baseUrl`, `kanon.serviceToken`, `DomainPackRead` roles | — | — | — | — | `kanonDeepeningHardeningContracts.test.ts` (route parity); Playwright 014 step 7 | No dedicated route; evolution is panel on domain-packs page (by design) |
| **Source bindings** | `GET/POST …/source-bindings`; `POST …/{id}/review`, `…/test` | `/kanon/source-bindings` — `SourceBindingsPage` | `useKanonSourceBindings`; `useCreateKanonSourceBinding`, `useReviewKanonSourceBinding`, `useTestKanonSourceBinding` | `kanon.baseUrl`, `kanon.ontologyVersionId`, `kanon.serviceToken` | Indirect via plan/fact context refs | Assistance: `suggest-source-bindings` (draft) | — | Conexus assistance enabled for draft flows | `SourceBindingApiTests`; Playwright 014 step 3 | — |
| **Source-binding quality** | `GET …/quality-summary`, `review-queue`, `coverage`, `contradictions` | `/kanon/source-bindings` — `KanonSourceBindingQualityPanel` | `useKanonSourceBindingQuality*` hooks | Same as source bindings | — | Assistance: `summarize-contradiction` | — | — | FE quality hooks + Playwright 014 step 3 | — |
| **Canonical facts** | `POST …/canonical-facts/resolve`; `GET …/canonical-facts`, `…/{factId}`, `…/entities/{entityRef}/facts` | `/kanon/facts` — `FactsAndPlansPage`, `KanonCanonicalFactResolveWorkbench`, `KanonCanonicalFactHistoryPanel` | `useKanonResolveCanonicalFact`; `kanonFactPlanHistoryClient` (list/history); `resolveKanonCanonicalFact` | `kanon.baseUrl`, `kanon.ontologyVersionId` | Planning/execution may reference fact entity refs in compile context | Assistance: `explain-canonical-facts` | `resolveKanonDecisionEvidenceGraph` when decision id known; no fact-id spine kind | — | `CanonicalFactApiTests`; `kanonCanonicalFactAdapters.test.ts`; Playwright 014 step 4 | No list workbench for facts (resolve-only UX documented in route catalog) |
| **Semantic plans** | `POST …/semantic-query-plans/compile`; `GET …/semantic-query-plans`, `…/{planId}`, `…/entities/{entityRef}/semantic-plans` | `/kanon/plans` — `KanonSemanticPlansPage`, `KanonSemanticPlanWorkbench`, `KanonSemanticPlanHistoryPanel` | `useCompileKanonSemanticQueryPlan`; `useKanonSemanticPlanHistory`; `compileKanonSemanticQueryPlan` | `kanon.baseUrl`, `kanon.ontologyId`, `kanon.ontologyVersionId` | **Consumer:** `KanonSemanticAuthorityClient.CompilePlanAsync` → `POST …/compile` | — | `planningDecisionId` / `kanonDecisionId` in `resolveEvidenceSpine`; deep graph via `resolveKanonTraceEvidenceGraph` | `Allagma__Downstream__Kanon__ServiceToken` | `SemanticQueryPlanApiTests`; Playwright 014 step 5 | — |
| **Decision provenance** | `POST …/decision-records`; `GET …/{id}`; `GET …/by-trace/{traceId}`, `…/by-entity/{entityRef}`; `GET …/provenance`; `POST …/verify`, `…/prepare-replay` | `/kanon/decisions` — `DecisionsProvenancePage`, `KanonDecisionsProvenanceWorkbench`, `KanonDecisionAuthorityPanel` | `useKanonDecisionLookup`, `useKanonDecisionProvenance`, `useKanonDecisionVerify`, `useKanonPrepareReplay`; `getKanonDecision*`, `getKanonDecisionProvenance` | `kanon.baseUrl`, `kanon.serviceToken`; provenance roles: `ProvenanceReader`/`Auditor`/`Admin`/`System` | **Emits:** planning/topology/action decision ids on runs. **Links:** `kanonDecisionHref`, `kanonDecisionsByTraceHref` | Assistance records decision per draft act | `resolveEvidenceSpine` kind `kanonDecisionId`; `resolveKanonDecisionEvidenceGraph` / `resolveKanonTraceEvidenceGraph` in `resolveEvidenceSpineDeepGraphs` | — | `ProvenanceReplayApiTests`; `resolveKanonEvidenceGraph.test.ts`; Playwright 014 step 6 | `POST …/decision-records` not exposed as FE mutation (internal/governed writes only) |
| **Replay bundles** | `GET …/replay-bundles`, `…/{bundleId}`, `…/export` | `/kanon/decisions` — replay panel + export via `useKanonReplayBundleExport`, `EvidenceExportPanel` | `listKanonReplayBundles`, `useKanonReplayBundleExport` | `kanon.baseUrl`, provenance read roles | Allagma replay evidence page links decisions; no direct bundle API from Allagma | — | Provenance nodes in Kanon evidence adapters; unified spine uses decision graph | — | `ReplayBundleApiTests`; provenance adapter tests | — |
| **Action policies** | `GET …/action-policies`, `…/{policyId}`, `…/decisions`; `POST …/explain`, `…/simulate` | `/kanon/policies` — `ActionPolicyPage`, `KanonActionPolicyWorkbench` | `kanonPolicyExplanationClient.*`; `useKanonActionPolicyWorkbench`, `useExplainKanonActionPolicy` | `kanon.baseUrl`, `kanon.serviceToken`, `allagma.defaultActorId`, `allagma.defaultActorRoles` | **Consumer:** `EvaluateToolIntentAsync` → `POST …/actions/evaluate` (not explain/simulate) | — | Policy explanation deep link `kanonPolicyExplanationHref` from Allagma gates | — | `ActionPolicyApiTests`; `kanonPolicyExplanationAdapters.test.ts`; Playwright 014 step 9 | Explain/simulate do not persist decisions (documented); live eval is Allagma/runtime only |
| **Human gates** | `POST …/actions/evaluate`; `POST …/actions/human-gates/check`; `POST …/actions/human-gates/{id}/resolve` | **Allagma:** `/allagma/gates` — `HumanGatesPage`, `HumanGateContextCard`. **Kanon:** `/kanon/policies?humanGateId=…` (explain context) | **Allagma:** `useResumeAllagmaRun` → `resolveKanonHumanGate`. **Kanon:** `kanonClient.resolveKanonHumanGate` | `allagma.*`, `kanon.*`, actor roles | **Consumer:** `CheckHumanGateAsync`, `EvaluateToolIntentAsync`; resume calls Kanon resolve | — | `humanGateId` node in `resolveEvidenceSpine`; gate links on run graph | — | `HumanGateApiTests`; `allagmaMutations.test.tsx`; Playwright 014 steps 9–10 | No standalone `/kanon/gates` page (policy + Allagma surfaces) |
| **Topology evaluation** | `POST …/execution-topologies/evaluate` | **Allagma:** run detail `AllagmaRunEvalTopologyPanel`; **System:** `/system/topology` (static readiness map, not live POST) | — (server-side only) | `kanon.ontologyVersionId` on start-run; Allagma `Kanon:BaseUrl` + service token | **Consumer:** `KanonSemanticAuthorityClient.EvaluateTopologyAsync` | — | `topologyAuthorizationDecisionId` in live spine test; `kanonSemanticGraphHref` for graph drilldown | `Kanon__BaseUrl` on `allagma-api` | `TopologyPolicyApiTests`; `resolveEvidenceSpine.live.test.ts` (topology decision) | No Kanon FE workbench for topology POST (Allagma-owned flow) |
| **Conexus assistance** | `GET …/conexus/assistance/capability`; `POST …/assistance/*` (5 draft endpoints) | `/kanon/assistance` — `ConexusAssistancePage`, `KanonConexusAssistanceWorkbench` | `kanonConexusAssistanceClient`; `useKanonConexusAssistanceCapability`, `useInvokeKanonConexusAssistance` | `kanon.baseUrl`, `kanon.serviceToken`, `kanon.ontologyVersionId`, `allagma.defaultActorId`, `allagma.defaultActorRoles` | — | **Consumer:** Kanon posts `v1/chat/completions` with project API key; **FE links:** `modelInvocationId` → Conexus observability + Evidence Spine | `kanonDecisionId` / `conexusModelCallId` from assistance `outputRef`; partial Conexus graph on 404 | `Kanon__ConexusAssistance__Enabled`, `__BaseUrl`, `__ProjectApiKey`; shares `CONEXUS_PROJECT_API_KEY` | `ConexusAssistanceApiTests`, `ConexusAssistanceNonAuthorityInvariantTests`; FE adapter + deep-graph tests; Playwright 014 steps 8, 11–12 | ✅ **KANON-CONNECT-004** |
| **Semantic graph** | `GET …/semantic-graph` | `/kanon/decisions?…&semanticGraph=1` — `KanonSemanticEvidenceGraphPanel` | `useKanonSemanticGraph`; `getKanonSemanticGraph` | `kanon.baseUrl`, `kanon.serviceToken` | **Link:** `kanonSemanticGraphHref` from Allagma semantic authority card | — | **Imported** in `resolveEvidenceSpineDeepGraphs` via `resolveKanonSemanticEvidenceGraph` | — | `SemanticGraphApiTests`; FE semantic-graph evidence adapter tests | ✅ **KANON-CONNECT-005** |
| **Auth / roles / settings** | ServiceToken / Development trusted headers; role gates per auth class (matrix) | `/settings` — `OperatorSettingsPage`; `KanonOperatorContextCard`, `KanonActorAuthorizationCard` | `kanonActorAuthorization.ts`; headers via `kanonClient` (`X-Ontogony-Actor-Id`, `X-Ontogony-Roles`, Bearer) | `kanon.baseUrl`, `kanon.serviceToken`, `kanon.ontologyId`, `kanon.ontologyVersionId`, `allagma.defaultActorId`, `allagma.defaultActorRoles`, `credentials.*` | Allagma propagates actor + idempotency via `AllagmaRunContextPropagation.ForKanon` | Conexus assistance `AllowedRoles` (default includes `Reviewer`) | Role-aware 403 warnings in `resolveEvidenceSpine` / `resolveKanonEvidenceGraph` | `KANON_SERVICE_TOKEN`, `Kanon__Cors__*`; `Allagma__Downstream__Kanon__ServiceToken` | `OntologyV0AuthorizationMatrixTests`; `kanonActorAuthorization.test.ts`; CORS fix in KANON-DEEPEN-014 | Compose may run stale `allagma-api`/`conexus-api` images (SDK pin) per 014 evidence |

---

## Allagma → Kanon runtime dependency summary

| Allagma stage | Kanon HTTP route | Client | Frontend link target |
| --- | --- | --- | --- |
| Plan compile | `POST /ontology/v0/semantic-query-plans/compile` | `IKanonSemanticPlanningClient` | `/kanon/plans`, `/kanon/decisions?decisionId=` |
| Topology authorization | `POST /ontology/v0/execution-topologies/evaluate` | `IKanonTopologyPolicyClient` | `/kanon/decisions`, `?semanticGraph=1` |
| Tool / action evaluation | `POST /ontology/v0/actions/evaluate` | `IKanonActionPolicyClient` | `/kanon/policies`, `/kanon/decisions` |
| Human gate check (resume) | `POST /ontology/v0/actions/human-gates/check` | `IKanonActionPolicyClient` | `/allagma/gates`, `/kanon/policies?humanGateId=` |
| Human gate resolve (approve/deny) | `POST /ontology/v0/actions/human-gates/{id}/resolve` | `kanonClient.resolveKanonHumanGate` (FE) | `/allagma/gates` |
| Run trace / decisions | `GET …/decision-records/by-trace/{traceId}` | `getKanonDecisionByTrace` | `/kanon/decisions?mode=trace` |

Configuration (Allagma): `Kanon:BaseUrl`, `Kanon:ServiceToken` (compose: `Kanon__BaseUrl`, `Allagma__Downstream__Kanon__ServiceToken`).

Implementation: `allagma-dotnet/src/Allagma.Infrastructure/Kanon/KanonSemanticAuthorityClient.cs`.

---

## Docker-local platform wiring

| Service | Host port (default) | Internal URL | Kanon-related env |
| --- | --- | --- | --- |
| `kanon-api` | `5081` | `http://kanon-api:8080` | `Kanon__Persistence__*`, `Kanon__Cors__*`, `Kanon__ConexusAssistance__*` |
| `conexus-api` | `5082` | `http://conexus-api:8080` | Project API key used by Kanon assistance |
| `allagma-api` | `5083` | — | `Kanon__BaseUrl`, `Allagma__Downstream__Kanon__ServiceToken` |
| `ontogony-frontend` | `5175` | — | Vite defaults → `localhost:5081/5082/5083` via `dockerLocalServiceDefaults` |

**Seed assumption (ENV-SEED-001):** `ontologyVersionId = gaming-core@0.1.0`, ontology id `gaming-core`, Kanon service token `kanon-dev-service-token-change-in-production`.

**Persistence:** Postgres (`Kanon__Persistence__Mode=Postgres`), migrations on startup.

**CORS:** Browser must send `X-Ontogony-Actor-Id`, `X-Ontogony-Roles`, trace/correlation headers — allowed on Kanon Development CORS after KANON-DEEPEN-014.

---

## Frontend navigation ↔ backend parity

| Route | OpenAPI catalog | Route workflow catalog | Playwright 014 |
| --- | --- | --- | --- |
| `/kanon` | health + versions + domain-packs | yes | step 1 |
| `/kanon/ontologies` | ontology lifecycle | yes | step 2 |
| `/kanon/source-bindings` | bindings + quality | yes | step 3 |
| `/kanon/facts` | resolve + history GETs | yes | step 4 |
| `/kanon/plans` | compile + history | yes | step 5 |
| `/kanon/decisions` | provenance + replay + semantic-graph | yes | step 6 |
| `/kanon/domain-packs` | lifecycle + evolution POSTs | yes | step 7 |
| `/kanon/assistance` | conexus assistance | yes | step 8 |
| `/kanon/policies` | action-policies explain/simulate | yes | step 9 |

Cross-service steps: Allagma run → Kanon link (10), Conexus model call (11), assistance safety (12).

---

## Evidence Spine touchpoints (current)

| Resolver entry | Kanon contribution | Semantic graph |
| --- | --- | --- |
| `resolveEvidenceSpine` | `kanonDecisionId`, `humanGateId`, provenance warnings | No |
| `resolveDeepEvidenceGraphContributions` | `resolveKanonDecisionEvidenceGraph`, `resolveKanonTraceEvidenceGraph` | **No** — follow-up **KANON-CONNECT-005** |
| Per-domain adapters | `src/kanon/adapters/resolveKanonEvidenceGraph.ts`, `kanonEvidenceAdapters.ts` | Panel on `/kanon/decisions` only |

Unified workbench: `/system/evidence-spine` — uses spine resolver; Kanon nodes appear when identifiers supplied.

---

## Gap table and follow-up issues

| Gap | Severity | Follow-up slice | Notes |
| --- | --- | --- | --- |
| ~~Frontend default `ontologyVersionId` drift~~ | — | **KANON-CONNECT-002** ✅ | Canonicalized to `gaming-core@0.1.0`; legacy `gaming-core-v0` migrates on read |
| Allagma run GET lacks `domainPackId`; semantic link slot documents unavailable | Low | — | Links work when metadata present; slot tests document gap |
| ~~No automated cross-link matrix test~~ | — | **KANON-CONNECT-003** ✅ | `buildAllagmaSemanticAuthorityLinkSlots.test.ts` + catalog contract |
| ~~Conexus assistance `modelInvocationId` observability drilldown not fully proven cross-service~~ | — | **KANON-CONNECT-004** ✅ | [004 evidence](./KANON_CONNECT_004_CONEXUS_ASSISTANCE_OBSERVABILITY_EVIDENCE.md) |
| ~~Evidence Spine does not import Kanon `GET /semantic-graph`~~ | — | **KANON-CONNECT-005** ✅ | [005 evidence](./KANON_CONNECT_005_EVIDENCE_SPINE_SEMANTIC_GRAPH_EVIDENCE.md) |
| ~~Route/catalog drift gate not enforced in CI for connect phase~~ | — | **KANON-CONNECT-006** ✅ | [006 evidence](./KANON_CONNECT_006_ROUTE_PARITY_EVIDENCE.md) — `kanon:route-parity:check` in `npm run check` |
| ~~Full cross-service Docker smoke after API image rebuild~~ | — | **KANON-CONNECT-007** ✅ | [007 evidence](./KANON_CONNECT_007_DOCKER_CROSS_SERVICE_SMOKE_EVIDENCE.md) — `npm run docker:smoke:kanon-connect-007` |
| `ontogony-runtime.lock.json` not updated | Process | **SYSTEM-ALPHA-004/005** | Kanon v2 baseline-owned promotion |
| Conexus OpenAPI catalog drift (non-Kanon) | Low | SYSTEM-ALPHA | Explicitly out of Kanon connect scope |
| No dedicated Kanon topology POST workbench | By design | — | Topology evaluation is Allagma-orchestrated |
| No `/kanon/gates` page | By design | — | Human gates via Allagma + policy explain |

---

## Acceptance checklist (KANON-CONNECT-001)

- [x] One row per feature (15 features: ontology through auth/settings)
- [x] Every row includes backend, frontend, settings, and test evidence columns
- [x] No “unknown” without a named follow-up issue
- [x] Every frontend Kanon page mapped to backend routes and settings
- [x] Every Allagma-Kanon runtime dependency listed
- [x] Cross-repo gaps explicitly named (no production-readiness claim)

---

## Related evidence

- Kanon deepening sequence: [KANON_DEEPEN_SEQUENCE_STATUS.md](./KANON_DEEPEN_SEQUENCE_STATUS.md)
- Cross-service semantic links (005): [KANON_DEEPEN_005_CROSS_SERVICE_SEMANTIC_LINKS_EVIDENCE.md](./KANON_DEEPEN_005_CROSS_SERVICE_SEMANTIC_LINKS_EVIDENCE.md)
- Browser baseline (014): [KANON_DEEPEN_014_BROWSER_QA_BASELINE_CANDIDATE_EVIDENCE.md](./KANON_DEEPEN_014_BROWSER_QA_BASELINE_CANDIDATE_EVIDENCE.md)
- Evidence Spine Kanon linking (005): `ontogony-frontend/docs/evidence/EVIDENCE_SPINE_005_KANON_DECISION_PROVENANCE_LINKING_EVIDENCE.md`

## Next slice

**KANON-CONNECT-002** — Kanon settings/env consistency (`gaming-core@0.1.0` canonicalization + settings health card).
