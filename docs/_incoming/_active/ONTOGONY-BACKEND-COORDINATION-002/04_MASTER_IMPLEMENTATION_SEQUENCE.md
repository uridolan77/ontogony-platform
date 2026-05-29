# Master implementation sequence — ONTOGONY-BACKEND-COORDINATION-002

Execute slices **in order**. Phases 7–8 may parallelize after phase 6.

---

## Phase 1 — BACKEND-REPO-DOCS-ORDER-002

**Goal:** Every backend repo has a consistent docs spine: README → docs/README → status/evidence/deferrals; broken links fixed or deferrals filed.

| Step | Action | Repos |
| ---: | --- | --- |
| 1.1 | Add `_archived/` intake tier to validators OR document why `_consumed` only | Platform (validator), all repos |
| 1.2 | Unify `docs/README.md` sections: Architecture, Contracts, API, Runbooks, Status, Reviews, Evidence, Incoming, Deferrals | All six |
| 1.3 | Add "Current status" banner to long-lived planning docs | All six |
| 1.4 | Fix broken internal links found by `validate-docs-links.ps1` | Per repo |
| 1.5 | Refresh `BACKEND_REPO_DOCS_INDEX.md` | Allagma |
| 1.6 | Record closeout evidence | Each repo |

**Gate:** Link validator passes OR failures listed in `DEFERRALS.md` with ticket IDs.

**Prompt:** [`prompts/P01_BACKEND_REPO_DOCS_ORDER_002.md`](./prompts/P01_BACKEND_REPO_DOCS_ORDER_002.md)

---

## Phase 2 — SYSTEM-COMPATIBILITY-MATRIX-001

**Goal:** `SYSTEM_COMPATIBILITY_MATRIX.md`, runtime lock, route inventories, and package pins are internally consistent and validated in CI.

| Step | Action | Owner |
| ---: | --- | --- |
| 2.1 | Audit matrix vs actual route inventories (Allagma, Kanon, Conexus, Metabole, Aisthesis) | Allagma |
| 2.2 | Refresh `ontogony-runtime.lock.json` locked commits if main moved | Allagma |
| 2.3 | Add/update `*_COMPATIBILITY_MANIFEST.json` snapshots | Each product repo |
| 2.4 | Extend `validate-runtime-lock.ps1` / architecture conformance for Metabole + Aisthesis rows | Allagma |
| 2.5 | Fix Conexus/Kanon/Allagma build failures blocking conformance | Product repos |
| 2.6 | Evidence JSON for matrix refresh | Allagma |

**Gate:** `./scripts/validate-runtime-lock.ps1 -ReleaseMode` PASS; cross-repo conformance PASS or classified deferrals.

**Prompt:** [`prompts/P02_SYSTEM_COMPATIBILITY_MATRIX_001.md`](./prompts/P02_SYSTEM_COMPATIBILITY_MATRIX_001.md)

---

## Phase 3 — SHARED-ERROR-CONTRACT-001

**Goal:** All services emit/consume `CrossServiceErrorEnvelope` for middleware-mapped failures; integration docs aligned.

| Step | Action | Owner |
| ---: | --- | --- |
| 3.1 | Finalize `CROSS_SERVICE_ERROR_ENVELOPE_V1.md` contract | Platform |
| 3.2 | JSON schema + OpenAPI component if applicable | Platform |
| 3.3 | Middleware adoption audit per service | Each API host |
| 3.4 | Update `SYSTEM_ERROR_COMPATIBILITY_MATRIX.md` | Allagma |
| 3.5 | Tests: `CrossServiceErrorEnvelope` shape per service | Each repo |
| 3.6 | Document endpoint-local typed DTO exceptions (Kanon validate/compile) | Kanon |

**Gate:** System compat gate error checks PASS; representative 400/404/403/500 samples match envelope in each service.

**Prompt:** [`prompts/P03_SHARED_ERROR_CONTRACT_001.md`](./prompts/P03_SHARED_ERROR_CONTRACT_001.md)

---

## Phase 4 — CROSS-REPO-IDENTITY-CORRELATION-001

**Goal:** `X-Ontogony-Trace-Id`, correlation, actor, idempotency headers propagate across Allagma→Kanon→Conexus and producers→Aisthesis.

| Step | Action | Owner |
| ---: | --- | --- |
| 4.1 | Finalize `CROSS_SERVICE_CONTEXT_PROPAGATION_V1.md` | Platform |
| 4.2 | Client middleware: Allagma HTTP clients to Kanon/Conexus/Aisthesis | Allagma |
| 4.3 | Verify Kanon.Client, Conexus.Client, Metabole.Client propagation | Product repos |
| 4.4 | Update `SYSTEM_TRACE_CONTEXT_MATRIX.md` | Allagma |
| 4.5 | Smoke: correlation chain test in governed E2E lib | Allagma |
| 4.6 | Tests pinning header constants vs docs | Platform + Allagma |

**Gate:** Correlation chain scenario PASS in system cohesion acceptance.

**Prompt:** [`prompts/P04_CROSS_REPO_IDENTITY_CORRELATION_001.md`](./prompts/P04_CROSS_REPO_IDENTITY_CORRELATION_001.md)

---

## Phase 5 — ALLAGMA-CONEXUS-MODEL-ALIAS-001

**Goal:** Allagma requests Conexus **only** via configured model aliases/purposes; shared manifest documents alias ↔ purpose mapping.

| Step | Action | Owner |
| ---: | --- | --- |
| 5.1 | Audit code/tests for hard-coded `gpt-*` provider model strings | Allagma |
| 5.2 | Publish `conexus-model-alias-manifest.snapshot.json` refresh | Allagma + Conexus |
| 5.3 | Conexus admin/docs: alias registry contract for consumers | Conexus |
| 5.4 | `appsettings` examples use aliases only | Allagma |
| 5.5 | Tests: purpose resolver never emits provider model IDs | Allagma |
| 5.6 | Document streaming vs non-streaming alias behavior | Conexus |

**Gate:** Grep gate: no `gpt-4` in Allagma `src/` (tests may use `test-alias` only); manifest validates in CI.

**Prompt:** [`prompts/P05_ALLAGMA_CONEXUS_MODEL_ALIAS_001.md`](./prompts/P05_ALLAGMA_CONEXUS_MODEL_ALIAS_001.md)

---

## Phase 6 — BACKEND-SYSTEM-E2E-001

**Goal:** Documented five-service governed smoke produces evidence JSON with PASS status.

| Step | Action | Owner |
| ---: | --- | --- |
| 6.1 | Refresh `SYSTEM_E2E_SCENARIOS.md` + test matrix | Allagma |
| 6.2 | `run-local-stack.ps1` + `smoke-first-system.ps1` green on clean dev root | Allagma |
| 6.3 | Include Metabole + Aisthesis in stack profile | Allagma |
| 6.4 | Evidence artifact schema validation | Platform + Allagma |
| 6.5 | Package-mode E2E path documented | Allagma |
| 6.6 | Closeout with trace ID reference | Allagma |

**Gate:** At least one governed five-service scenario `PASS` with committed evidence under `docs/evidence/`.

**Prompt:** [`prompts/P06_BACKEND_SYSTEM_E2E_001.md`](./prompts/P06_BACKEND_SYSTEM_E2E_001.md)

---

## Phase 7 — AISTHESIS-RECONSTRUCTABILITY-SPINE-001

**Goal:** Live (not fixture-only) five-service reconstructability certification PASS.

| Step | Action | Owner |
| ---: | --- | --- |
| 7.1 | Merge/absorb `AISTHESIS-LIVE-FIVE-SERVICE-PASS-009` if still active | Aisthesis |
| 7.2 | Align SDK policy (`global.json`) with workspace standard | Aisthesis |
| 7.3 | Create `docs/system/` boundary matrices (optional) | Aisthesis |
| 7.4 | Live certification script PASS | Aisthesis + Allagma stack |
| 7.5 | Required-edge matrix v2 satisfied with native producers | All producers |
| 7.6 | Evidence: `reconstructabilityGrade = complete` | Aisthesis |

**Gate:** Live certification summary JSON `status: PASS` — see slice spec.

**Prompt:** [`prompts/P07_AISTHESIS_RECONSTRUCTABILITY_SPINE_001.md`](./prompts/P07_AISTHESIS_RECONSTRUCTABILITY_SPINE_001.md)

---

## Phase 8 — METABOLE-DATA-SPINE-HARDENING-001

**Goal:** SLOD/data-spine certification matrix PASS; Kanon handoff documented and tested.

| Step | Action | Owner |
| ---: | --- | --- |
| 8.1 | Merge/absorb `METABOLE-LIVE-CERTIFY-PROMOTE-001` / cohesion packages if overlapping | Metabole |
| 8.2 | Certification matrix for schema-profile → SLOD → Kanon peer routes | Metabole |
| 8.3 | Postgres across stores smoke in five-service context | Metabole |
| 8.4 | Undeniability + status-truth gates green | Metabole |
| 8.5 | Document ELT/medallion deferrals explicitly | Metabole |
| 8.6 | Evidence closeout | Metabole |

**Gate:** `run-metabole-five-service-certification.ps1` PASS or equivalent documented command.

**Prompt:** [`prompts/P08_METABOLE_DATA_SPINE_HARDENING_001.md`](./prompts/P08_METABOLE_DATA_SPINE_HARDENING_001.md)

---

## Commit strategy

- **One PR per slice per repo** where possible (small, reviewable).
- Platform contract PRs merge **before** product adoption PRs for slices 3–4.
- Runtime lock bump is a **dedicated commit** with evidence in Allagma.
- Do not mix slice 6 E2E evidence with unrelated feature work.
