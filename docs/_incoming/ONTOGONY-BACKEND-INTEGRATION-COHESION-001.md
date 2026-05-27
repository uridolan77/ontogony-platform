## Executive verdict

The 4-backend system is now structurally coherent, but it is not yet fully tightened as a single runtime product.

The correct architecture is now clear:

```text
Conexus  = model gateway / provider routing / model-call evidence
Kanon    = semantic authority / ontology / policy / provenance / human gates
Allagma  = governed execution / runtime orchestration / run lifecycle
Metabole = data transformation plane / schema profiling / mapping candidates / lineage
```

The earlier three-repo cohesion review was basically right: the first governed loop was already connected, but production-grade full-system cohesion still depended on hardening compatibility, cross-repo E2E, error contracts, identity propagation, replay/idempotency, and readiness gates.  The important update is that Allagma, Kanon, and Conexus have since moved forward: Allagma now has model-purpose-to-Conexus-alias config and opt-in streaming; Kanon now has 104 `/ontology/v0` routes and a compatibility manifest; Conexus now has Postgres-backed idempotency ledger support, but snapshot drift and production-readiness gaps remain. Metabole is now the fourth backend plane, but it is not yet absorbed into Allagma’s canonical runtime lock.

My bottom line:

> **The Allagma–Kanon–Conexus triangle is alpha-cohesive. Metabole is foundation-alpha and domain-alpha, but still semi-attached. The next tightening phase should focus on integrating Metabole into the system lock, proving golden scenario G, normalizing evidence/read-model contracts, and eliminating documentation/status drift.**

---

## 1. Current state by repo

### Conexus.NET — model gateway

Conexus is the model-access authority. Its README states that routing, pricing, provider policy, and admin contracts live in Conexus, not semantic meaning or governed execution. 

Implemented strengths:

* OpenAI-compatible `POST /v1/chat/completions`, including JSON and SSE streaming. 
* Project API keys, optional Postgres for routing/replay/telemetry/journal/quotas/audit. 
* Fake, OpenAI, OpenAI-compatible HTTP, Anthropic, and Gemini providers, disabled by default. 
* Adaptive routing, fallback, exact response cache, quotas, usage/cost, admin APIs, model-call evidence, and Kanon reconstructability closure. 
* Postgres-backed chat replay, response cache, and `IIdempotencyLedger` when Postgres is configured. 

Main gaps:

1. **Current test truth is not fully green.**
   Conexus says its latest CI-equivalent local baseline has **822 passed, 1 skipped, 3 failed**, where the failures are OpenAPI/route-inventory snapshot drift, not logic regressions. Still, this must be fixed before any serious “release readiness” claim. 

2. **Streaming replay remains intentionally unsupported.**
   `Idempotency-Key` is rejected for streaming and there is no durable streaming replay. 

3. **Real-provider coverage remains deferred.**
   Provider parity is partial; Conexus is OpenAI-shaped and does not expose the full vendor surface such as vision/audio/batch/fine-tuning/Assistants. 

4. **Admin security is still alpha.**
   `AlphaSharedKey`, `ScopedKeys`, and alpha OIDC exist, but there is no enterprise IAM/RBAC policy engine or independent security review. 

5. **Observability packs are not operationalized.**
   Grafana/Prometheus artifacts are docs-only, OTLP export is opt-in, and some meters remain deferred. 

6. **Adaptive routing health is not durable.**
   Provider health and routing policies are in-memory unless future durable storage is added. 

**Conexus tightening priority:** fix snapshot drift, promote provider/fallback/capability evidence into the system lock, and make production-mode observability/security gaps explicit rather than scattered.

---

### Kanon.NET — semantic authority

Kanon’s boundary is very strong. The README states: Kanon owns meaning; Ontogony.Platform owns mechanics; Conexus owns model access; Allagma owns governed execution. 

Implemented strengths:

* 104 routes under `/ontology/v0`. 
* 86 `Kanon.Client` routes and 18 server-only routes. 
* Ontology kernel, source bindings, semantic plans, canonical facts, action policies, human gates, decision records, provenance, replay bundles, domain packs, Conexus assistance, operator review queue, semantic quality snapshots, reconstructability classifier, and Postgres persistence. 
* Domain pack lifecycle governance exists: `accepted` is distinct from `active`. 
* Test baseline is strong: **724 passed, 0 failed, 2 skipped**. 

Main gaps:

1. **Kanon is still `/ontology/v0`; v1 is intentionally deferred.**
   This is fine, but it means clients must not infer maturity from `/openapi/v1.json`; that is only the ASP.NET OpenAPI document name. 

2. **Some v0 error shapes remain intentionally typed DTOs.**
   Middleware and endpoint-local errors are normalized, but validation-style failures still return specific DTOs such as `ValidateOntologyResponse`, `CompileSemanticQueryPlanResponse`, and `PublishOntologyVersionResponse`. Clients must treat this as contract truth, not drift. 

3. **Kanon.Client still has server-only/admin gaps.**
   Source-binding mutations, decision record creation, ontology deletion, and Conexus assistance accept/reject are not fully typed-client surfaces. 

4. **Replay execution is not implemented.**
   Kanon can prepare/export replay bundles, but not execute replay. 

5. **Semantic planner execution is still shallow.**
   Multi-hop execution is not implemented; the planner warns diagnostically but does not retrieve data. Regex policy operator is also not implemented. 

6. **Binding governance and rollback governance need dedicated tests.**
   The platform manifest still lists open deferrals for rollback governance and binding governance evaluation. 

**Kanon tightening priority:** complete validation packages for rollback/binding governance, make server-only route policy consumable by operator tooling, and ensure Metabole-specific mapping validation routes exist and are contract-tested.

---

### Allagma.NET — governed execution runtime

Allagma is now the strongest cross-repo cohesion owner. Its README explicitly defines the boundary: Kanon semantic authority, Allagma governed execution, Conexus model access, Ontogony.Platform mechanics, and MAF isolated to the adapter project. 

Implemented strengths:

* First vertical slice: `POST /allagma/v0/runs` → Kanon → Conexus → completed run/events. 
* Model purpose → Conexus alias is now implemented, replacing the earlier hard-coded-model problem. 
* Opt-in Conexus streaming per model purpose exists. 
* Human gate pause/resume, Postgres persistence, restart/replay evidence, service-token auth, eval harness, package-mode build, system cohesion acceptance, MAF adapter isolation, tool registry simulation, durable execution engine, observability/SLO pack, and reconstructability golden trace are implemented. 
* Runtime lock exists for platform, Conexus, Kanon, and Allagma. 

Main gaps:

1. **Metabole is missing from Allagma’s canonical runtime lock.**
   The lock includes only `ontogony-platform`, `conexus-dotnet`, `kanon-dotnet`, and `allagma-dotnet`; no `metabole-dotnet` row, no port `5084`, no Metabole route prefix, no golden scenario G requirement. 

2. **System route matrix is still triangle-only.**
   It lists Allagma, Kanon, and Conexus flows, but no Metabole route family or Allagma → Metabole orchestration path. 

3. **Real external tool execution remains blocked.**
   This is correct, but still limits “fully connected runtime” claims. 

4. **Enterprise IAM is not implemented.**
   Allagma uses shared service token auth on `/allagma/v0/*`. 

5. **Production SLO certification is partial.**
   Metrics/catalog/alerts exist, but the SYS-OBS-002A live PASS artifact is not recorded. 

6. **Some live acceptance is still outside the baseline.**
   The system matrix lists open items: live governed-fake E2E, Playwright governed-fake live specs, full runtime lock promotion, deeper MAF work, production IAM, SLOs, and package-mode proof on demand. 

**Allagma tightening priority:** expand the runtime lock/matrices to include Metabole, add a live Allagma → Metabole orchestration scenario, and promote observability/replay/streaming evidence into one coherent release baseline.

---

### Metabole.NET — data transformation plane

Metabole is now correctly defined as the Ontogony data transformation plane. It owns governed ingestion, schema extraction, profiling, transformation planning, semantic mapping candidates, data-quality evidence, lineage, and replay-safe pipeline runs; it explicitly does not own ontology truth, model routing, or agent orchestration. 

Implemented strengths:

* Schema-profile vertical slice: pipeline run → schema extraction → profiling → mapping candidates → Conexus advisory → Kanon validation → evidence/events. 
* Boundary rules are explicit: no semantic authority, no provider/model details, no unrestricted execution engine, LLM output advisory unless validated by Kanon. 
* Routes exist for pipeline runs, events, evidence, SLOD candidates, operator review, gaming domain profile, schema profiles, and mapping candidates. 
* SQL Server metadata extraction is opt-in and metadata-only. 
* Postgres durability is opt-in. 
* Kanon/Conexus HTTP adapters are opt-in, with fake adapters as default. 
* SLOD domain alpha is implemented for ProgressPlay/gaming profiling. 
* Default gate: **216 tests passed, 0 failed**. 

Main gaps:

1. **Metabole is foundation-alpha + SLOD domain-alpha, not production-ready.**
   Its own status says this is not a production readiness claim. 

2. **No full cross-repo local-stack E2E includes Metabole by default.**
   The foundation status explicitly lists this as a gap. 

3. **Allagma orchestration is not live.**
   Metabole has a contract for Allagma → Metabole and a local Metabole-side proof, but system-level Allagma.Api automatic invocation is still pending. 

4. **Live Kanon/Conexus peer routes may still be missing.**
   Metabole sends to `POST /ontology/v0/metabole/mapping-candidates/validate` and `POST /conexus/v0/metabole/mapping-candidates/suggest`, but the audit notes peer repos may not expose all routes yet. 

5. **Replay bundle exists, replay execution route does not.**
   Replay bundle builder exists, but `POST .../replay` is not implemented. 

6. **OpenAPI is deferred.**
   Metabole now has generated route inventory JSON, but OpenAPI snapshot remains deferred because there is no Swagger/OpenAPI host map. 

7. **Only one client-covered route.**
   Metabole’s route inventory shows 12 live routes, but only one client route: `MetaboleClient.StartSchemaProfileRunAsync`. The rest are server-only/operator read routes. 

8. **Docs conflict with each other.**
   `METABOLE_SYSTEM_MATRIX.md` says HTTP adapters and Postgres durability are opt-in capabilities, while `METABOLE_SYSTEM_INTEGRATION_MATRIX.md` still says Kanon/Conexus are “fake adapter only” and Postgres is “not included.”  
   That is status drift and should be fixed immediately.

9. **Ontogony.Platform consumption is still transitional.**
   Metabole does not reference Ontogony.Platform yet and uses local error/idempotency/trace implementations. 

**Metabole tightening priority:** make golden scenario G executable, add live peer routes/proofs, add Allagma orchestration, align docs, and decide whether Metabole consumes Ontogony.Platform mechanics now or after one more hardening pass.

---

## 2. Cross-repo cohesion map

Current real graph:

```text
Client / Operator
  → Allagma
      → Kanon: semantic plan, policy, human gates, topology authorization
      → Conexus: model completion, fallback, streaming, model-call evidence

Kanon
  → Conexus: draft-only assistance, non-authoritative

Metabole
  → Kanon: mapping candidate validation, semantic approval path
  → Conexus: advisory mapping/classification/profiling summaries
  ← Allagma: contract exists, live orchestration pending
```

The platform manifest now recognizes the four backend runtime repos: Allagma as governed execution, Kanon as semantic authority, Conexus as model gateway, and Metabole as data transformation. 

But there is a split-brain:

* **Platform backend-cohesion manifest** already knows Metabole is part of the runtime backend set.
* **Allagma runtime lock** does not yet include Metabole.
* **Allagma system route matrix** does not yet include Metabole.
* **Metabole local system matrix** says cross-repo cohesion matrices remain owned by Allagma and should eventually add a Metabole row. 

That is the central integration issue.

---

## 3. Major gaps to collect and close

### A. Runtime lock and compatibility

**Gap:** Metabole is in the platform backend manifest, but not in Allagma’s `ontogony-runtime.lock.json`.

Current Allagma lock includes:

```text
ontogony-platform
conexus-dotnet
kanon-dotnet
allagma-dotnet
```

but not `metabole-dotnet`. 

**Fix:** create `SYSTEM-RC-003` or `SYSTEM-RC-002-METABOLE` with:

```json
"metabole-dotnet": "main"
```

plus locked commit, port `5084`, prefix `/metabole/v0`, required scenarios:

```text
metabole_schema_profile_golden
metabole_slod_profile_golden
allagma_metabole_orchestration
metabole_kanon_validation
metabole_conexus_advisory
metabole_replay_bundle_read
metabole_error_trace_correlation
```

---

### B. Allagma → Metabole orchestration

**Gap:** Metabole has an Allagma contract, but Allagma does not yet invoke Metabole as part of a live governed workflow.

The contract defines Allagma → Metabole, required headers, schema-profile route, SLOD flow, and `X-Allagma-Run-Id` echo. 

**Fix:** add an Allagma Metabole pipeline client and workflow step:

```text
POST /allagma/v0/runs
  purpose = "progressplay-slod-profile"
    → Allagma policy/topology check
    → Metabole POST /pipeline-runs/schema-profile
    → Metabole GET /events/evidence/slod-candidates
    → Kanon mapping approval / review queue
    → Allagma run completed with Metabole evidence anchors
```

Acceptance should verify:

* trace/correlation preserved;
* `X-Allagma-Run-Id` persisted in Metabole;
* Allagma event stream contains Metabole run id;
* Metabole evidence is linked into Allagma evidence export;
* idempotent retry does not duplicate Metabole pipeline run.

---

### C. Live Metabole peer routes in Kanon and Conexus

**Gap:** Metabole HTTP adapters target peer routes that may not yet exist in Kanon/Conexus dev servers.

Metabole’s audit lists:

```text
Kanon   POST /ontology/v0/metabole/mapping-candidates/validate
Conexus POST /conexus/v0/metabole/mapping-candidates/suggest
```

and says current implementation uses `mapping-candidates/*`, while DEEPEN proposed `slod-candidates/*`. 

**Fix:** pick one route contract and freeze it.

Recommended:

```text
Kanon:
POST /ontology/v0/metabole/mapping-candidates/validate
POST /ontology/v0/metabole/slod-candidates/validate

Conexus:
POST /conexus/v0/metabole/mapping-candidates/suggest
POST /conexus/v0/metabole/profile-summary/suggest
```

Then add:

* Kanon route inventory entries;
* Conexus route inventory entries;
* Metabole adapter contract tests against live sibling services;
* Allagma system route matrix updates.

---

### D. Evidence spine integration

**Gap:** Allagma/Kanon/Conexus have evidence and reconstructability loops; Metabole has lineage evidence, but it is not yet a first-class participant in the cross-service evidence spine.

Metabole exposes run evidence and lineage evidence.  The platform manifest has golden scenario G for Metabole transformation provenance. 

**Fix:** define Metabole evidence anchors:

```text
metabolePipelineRunId
schemaProfileId
mappingCandidateId
slodCandidateGroupId
sourceFingerprint
lineageEvidenceId
kanonValidationDecisionId
conexusAdvisoryModelCallId
allagmaRunId
```

Then add read paths:

```text
Allagma evidence export → includes Metabole evidence anchors
Kanon decision record → links mapping candidate / validation decision
Conexus model-call evidence → links advisory request
Metabole evidence → links upstream Allagma run and downstream Kanon/Conexus IDs
```

---

### E. Error envelope consistency

**Gap:** Cross-service error envelope is “closed” at platform level, but service-specific exceptions remain by design.

Platform manifest marks the error-envelope spine closed across all four repos.  But in practice:

* Conexus uses OpenAI-shaped errors plus admin/API errors.
* Kanon uses Ontogony `ApiError` plus typed validation DTOs. 
* Allagma uses cross-service error envelopes.
* Metabole uses local error envelope middleware. 

**Fix:** do not force identical wire shape everywhere. Instead produce a **cross-service error compatibility matrix** with explicit mappings:

```text
conexus.provider_unavailable       → allagma.conexus.model_unavailable
kanon.plan_validation_failed       → allagma.kanon.plan_rejected
metabole.mapping_validation_failed → allagma.metabole.mapping_rejected
metabole.source_unavailable        → allagma.metabole.source_unavailable
```

The goal is not one JSON shape; the goal is predictable orchestration behavior.

---

### F. Trace/correlation/idempotency

**Gap:** Platform says trace/correlation/idempotency spine is closed, but Metabole still uses local transitional mechanics and is not in the Allagma runtime lock.

The platform manifest marks trace/correlation/idempotency closed across all four repos.  Metabole also documents actor/trace propagation on runs/events/evidence/errors/outbound adapter JSON. 

But the final system proof is missing:

```text
Allagma run id
  → Metabole pipeline run id
    → Kanon validation decision id
    → Conexus advisory model call id
      → all linked by traceId/correlationId
```

**Fix:** add one golden trace artifact for Metabole scenario G.

---

### G. OpenAPI and route inventory discipline

**Gap:** Kanon and Allagma have mature route/OpenAPI discipline; Conexus currently has snapshot drift; Metabole has route inventory JSON but OpenAPI deferred.

Conexus has 3 snapshot-drift failures.  Metabole’s OpenAPI snapshot is deferred. 

**Fix:**

1. Conexus: refresh OpenAPI/route inventory snapshots and get baseline back to green.
2. Metabole: either add Swagger/OpenAPI or formally waive OpenAPI and standardize EndpointDataSource-generated route inventory as the accepted discipline.
3. Allagma system matrices: add Metabole route family and route checks.
4. Platform manifest: distinguish “OpenAPI present” from “route inventory present.”

---

### H. Replay and reconstructability

**Gap:** Cross-service replay exists in Allagma/Kanon/Conexus, but Metabole only has a replay bundle builder and no HTTP replay execution route.

Metabole audit says replay bundle builder exists, but `POST .../replay` is not implemented.  Kanon replay execution is also not implemented, only export/prepare. 

**Fix:** define two separate semantics:

```text
Replay-read:
  reconstruct previous run/evidence without side effects.

Replay-execute:
  re-run pipeline / model / validation actions under explicit operator approval.
```

For now, implement **replay-read only** across all four services. Do not enable replay-execute until side-effect controls are mature.

---

### I. Production/IAM/security

**Gap:** All four repos remain alpha/security-transitional.

Examples:

* Conexus: no formal security review and admin IAM is alpha. 
* Allagma: enterprise IAM not implemented; shared token only. 
* Kanon: service token and trusted headers exist, but policy-based auth is partial. 
* Metabole: enterprise IAM/OIDC not implemented. 

**Fix:** do not try to solve full IAM in each repo separately. Define one platform-level auth evolution plan:

```text
Phase 0: shared service tokens / project keys
Phase 1: service identity + operator actor propagation
Phase 2: OIDC/JWT validation
Phase 3: policy-based route authorization
Phase 4: tenant/workspace scoping
Phase 5: audit-grade production authorization
```

---

## 4. Documentation/status drift to clean immediately

These are small but dangerous because they confuse agents.

1. **Metabole integration matrix is stale.**
   It says Kanon/Conexus are fake adapter only and Postgres is not included, while README/status say HTTP adapters and Postgres are opt-in implemented. 

2. **Metabole audit has stale subclaim about no route inventory JSON.**
   The audit says no generated route JSON existed, but `docs/generated/METABOLE_ROUTE_INVENTORY.json` now exists.  

3. **Allagma compatibility matrix route counts may be stale.**
   Allagma matrix mentions Kanon 94 routes in some places, while Kanon current state says 104 routes, 86 client, 18 server-only.  

4. **Conexus current state admits stale earlier docs.**
   It notes an earlier doc pass incorrectly cited 810 passed / 0 failed; current truth is 822 passed / 3 failed snapshot drift. 

5. **Metabole “fifth backend runtime service” wording is odd.**
   Metabole system matrix calls it the fifth backend runtime service alongside Kanon, Conexus, Allagma, and Ontogony.Platform.  Since your current operative backend repos are four product runtimes plus platform mechanics, call it either “fourth product backend runtime” or “fifth including platform.” Don’t leave ambiguous wording.

---

## 5. Recommended next package

Use one cohesion package, not four isolated ones:

```text
ONTOGONY-BACKEND-INTEGRATION-COHESION-001
```

### Slice 001A — Truth sync and doc cleanup

* Refresh Allagma matrices against current Kanon 104-route truth.
* Fix Conexus snapshot drift.
* Fix Metabole integration matrix.
* Update platform manifest deferrals after Metabole 001J closeout.
* Add explicit “this repo status is authoritative” banners.

### Slice 001B — Runtime lock expansion

* Add `metabole-dotnet` to `ontogony-runtime.lock.json`.
* Add port `5084`.
* Add `/metabole/v0`.
* Add Metabole package/client status.
* Add post-lock delta support for Metabole.

### Slice 001C — Allagma → Metabole orchestration

* Add `Metabole.Client` or Allagma-owned HTTP adapter.
* Implement governed SLOD profiling workflow.
* Propagate headers and idempotency.
* Store Metabole run/evidence anchors in Allagma events.

### Slice 001D — Live Kanon/Conexus peer routes for Metabole

* Add/verify Kanon Metabole validation routes.
* Add/verify Conexus Metabole advisory routes.
* Contract tests in Kanon, Conexus, and Metabole.
* Remove ambiguity between `mapping-candidates` and `slod-candidates`.

### Slice 001E — Golden scenario G proof

Scenario:

```text
Allagma governed run
  → Metabole ProgressPlay SLOD profiling
  → Conexus advisory summary
  → Kanon mapping validation / review decision
  → Metabole lineage evidence
  → Allagma evidence export
  → platform evidence index
```

Acceptance:

* one local script runs all four services;
* evidence JSON saved;
* trace/correlation chain verified;
* idempotent retry verified;
* error path verified;
* no semantic truth accepted without Kanon/operator governance.

### Slice 001F — Evidence spine/read-model integration

* Add Metabole evidence anchors.
* Add cross-service evidence export.
* Add operator-read payload shape for pipeline/mapping lineage.
* Link Allagma run → Metabole run → Kanon decision → Conexus model call.

### Slice 001G — Release readiness truth gate

* Each repo reports: build, test, route inventory, OpenAPI/snapshot status, known deferrals.
* No “0 failed” claims where snapshot drift exists.
* Production non-claims repeated once, centrally.

---

## 6. Priority backlog

| Priority | Work                                                          | Owner repo                          |
| -------: | ------------------------------------------------------------- | ----------------------------------- |
|       P0 | Fix Conexus OpenAPI/route snapshot drift                      | `conexus-dotnet`                    |
|       P0 | Fix Metabole stale integration docs                           | `metabole-dotnet`                   |
|       P0 | Update Allagma matrices to current Kanon 104-route truth      | `allagma-dotnet`                    |
|       P1 | Add Metabole to Allagma runtime lock                          | `allagma-dotnet`                    |
|       P1 | Add Metabole to system route/env/auth/test matrices           | `allagma-dotnet`                    |
|       P1 | Implement Allagma → Metabole orchestration client             | `allagma-dotnet`                    |
|       P1 | Prove Golden G with live four-service local stack             | all four                            |
|       P2 | Add/verify Metabole-specific Kanon routes                     | `kanon-dotnet`                      |
|       P2 | Add/verify Metabole-specific Conexus routes                   | `conexus-dotnet`                    |
|       P2 | Normalize Metabole evidence into Evidence Spine               | `metabole-dotnet`, `allagma-dotnet` |
|       P3 | Decide Metabole OpenAPI vs formal route-inventory-only waiver | `metabole-dotnet`                   |
|       P3 | Add replay-read surface for Metabole                          | `metabole-dotnet`                   |
|       P3 | Enterprise IAM plan across services                           | platform + all four                 |

---

## Final assessment

The system has crossed an important threshold: the architectural boundaries are no longer vague. Each repo now has a defensible role.

But the sharpest unresolved issue is this:

> **Allagma/Kanon/Conexus are behaving like one alpha runtime; Metabole is still attached by contract, local proof, and platform manifest—not yet by runtime lock and live orchestration.**

So the next move should not be “more features.” It should be:

```text
Make the four backend repos behave as one traceable, idempotent, evidence-producing, semantically governed runtime.
```

The best next package name:

```text
ONTOGONY-BACKEND-INTEGRATION-COHESION-001
```

Goal:

```text
Absorb Metabole into the runtime cohesion baseline and prove Golden Scenario G end-to-end without weakening the existing Conexus/Kanon/Allagma boundaries.
```
