# PFH-001 — Current-state audit

## Repos

```text
uridolan77/ontogony-platform
uridolan77/allagma-dotnet
uridolan77/kanon-dotnet
uridolan77/conexus-dotnet
uridolan77/ontogony-frontend
uridolan77/ontogony-ui
```

## Goal

Produce a precise, evidence-backed current-state audit **before** any product implementation PRs. Inventory eval routes, frontend eval surfaces, contract/OpenAPI/client wiring, tests, and operator docs; refresh package gap matrices with concrete findings; confirm the first implementation item remains `EVAL-PRODUCT-001`.

## Boundary

**Audit/docs only.** Read and search across all six repos; update package matrices and add evidence. No runtime source changes, no workflow changes, no secrets, no broad refactors.

**Not production readiness.** Do not conflate product gaps with production-readiness gaps (identity, TLS, cloud deploy, real provider mode). Record production items separately under “out of scope / separate program” when encountered.

## Inspection targets

### Platform (`ontogony-platform`)

| Path | What to extract |
| --- | --- |
| `docs/product-hardening/eval-alignment-frontend-depth/*` | Package sequence, gap matrices, known limitations — baseline before edits |
| `docs/alignment/eval-full-sanity-alignment/*` | Contract snapshot, frontend consumption matrix, OpenAPI provenance |
| `docs/alignment/backend-frontend-phase-v2-sandbox-evidence-alignment/*` | Snapshot provenance, compatibility CI patterns |
| `docs/planning/eval-durability-to-first-sanity-current/*` | Completed eval program state; what is done vs still open |
| `docs/releases/POST_DOCKER_LOCAL_HARDENING_CLOSEOUT.md` | Closed prerequisites |
| `docs/releases/POST_DOCKER_LOCAL_HARDENING_NEXT_OPTIONS.md` | Active program pointer; optional tracks |
| `docs/operators/TRACE_CORRELATION_CONTRACT.md` | Trace/correlation contract for cross-service ID mapping |
| `docs/evidence/*EVAL*` | Prior eval alignment / durability evidence |
| `docs/evidence/*TRACE*` | Trace contract evidence |
| `docs/evidence/*FE_*` | Frontend hardening, fixtures, replay, hygiene evidence |

### Allagma (`allagma-dotnet`)

| Area | Inspect |
| --- | --- |
| Eval HTTP surface | Eval routes/controllers (list, query, run, baseline, comparison, export if any) |
| Contracts | Eval DTOs, request/response shapes, pagination/filter semantics |
| Persistence | Eval repositories, migrations, baseline/scenario storage |
| Product features | Baseline comparison code, scenario/dataset docs or code |
| OpenAPI | `docs/api/*openapi*`, snapshot tests, provenance JSON |
| Tests | Eval API tests, OpenAPI snapshot tests, persistence tests, baseline comparison tests |
| Releases/evidence | `docs/releases/FIRST_FULL_SANITY_CLOSEOUT.md`, `docs/evidence/*EVAL*` |

Record per route: exists / partial / missing; sampling limits; id fields exposed.

### Frontend (`ontogony-frontend`)

| Area | Inspect |
| --- | --- |
| Routes/pages | Eval dashboard, run detail, replay evidence pages |
| Data layer | Eval hooks, adapters, generated clients (`openapi/`, `src/**/api/`) |
| Modes | Fixture/live/replay/config catalogs and boundary behavior |
| Tests | Unit/adapter tests, Playwright specs for eval/dashboard/replay/run detail |
| Operator docs | `docs/operators/FRONTEND_FIXTURE_LIVE_BOUNDARY.md`, `docs/operators/FRONTEND_CONFIG_OPERATOR_CONTRACT.md` |
| Evidence | `docs/evidence/FE_HARDEN_001*`, `FE_AUDIT_FIXTURES_001*`, `FE_TEST_REPLAY_001*`, `FE_HYGIENE_CONFIG_001*` |

Record per surface: route path, adapter/hook, fixture vs live, known limitation states.

### Kanon (`kanon-dotnet`)

| Area | Inspect |
| --- | --- |
| Decision records | Routes/docs for decision lookup by trace or id |
| Topology | Topology decision evidence docs, `topologyAuthorizationDecisionId` usage |
| Headers/trace | Trace/idempotency header docs relevant to cross-service correlation |
| Tests/docs | Tests and operator docs tied to topology authorization decisions |

### Conexus (`conexus-dotnet`)

| Area | Inspect |
| --- | --- |
| Model/route evidence | Route decision, model-call evidence surfaces |
| Journal/admin | Execution journal, admin evidence routes |
| Bootstrap | Provider/bootstrap docs and readiness behavior |
| Correlation | Trace/correlation metadata docs |

### Ontogony UI (`ontogony-ui`)

| Area | Inspect |
| --- | --- |
| Packaging | Packaging status docs, export/pack smoke evidence |
| Consumption | Components used by frontend eval/run/replay surfaces (if any shared UI) |

## Deliverables

Update **in place** under `docs/product-hardening/eval-alignment-frontend-depth/`:

| File | Required content |
| --- | --- |
| `01_CURRENT_STATE.md` | Actual repo-state findings: closed prerequisites, bottleneck summary, six-repo HEAD pins (optional but recommended), what changed since prior alignment packages |
| `03_EVAL_PRODUCT_GAP_MATRIX.md` | Concrete eval product gaps (query/list, comparison, datasets, scoring, export) with priority |
| `04_BACKEND_FRONTEND_ALIGNMENT_GAP_MATRIX.md` | Route → OpenAPI snapshot → generated client → hook → adapter → UI page → test mapping; mark **aligned / partial / missing / deferred** |
| `05_FRONTEND_OPERATOR_DEPTH_GAP_MATRIX.md` | Dashboard, run detail, replay workbench depth gaps with operator impact |
| `06_CONTRACT_AND_OPENAPI_STATE.md` | Per-service snapshot path, provenance status, client generation status, drift-check commands |
| `07_TEST_AND_EVIDENCE_STATE.md` | Actual tests and evidence files per category; gaps for upcoming PRs |
| `08_KNOWN_LIMITATIONS.md` | Update only if audit finds new or corrected limitations |

Add:

| File | Required content |
| --- | --- |
| `docs/evidence/PFH_001_CURRENT_STATE_AUDIT_EVIDENCE.md` | Commands run, repos inspected, key findings, limitations, explicit **not production readiness** statement |

## Audit outputs (required sections in evidence)

1. **Six-repo inspection log** — repo, branch/HEAD (if recorded), areas searched, files touched (docs only).
2. **Eval route inventory** — Allagma eval HTTP routes with method, path, DTO summary, test coverage pointer.
3. **Frontend eval surface inventory** — routes/pages, hooks/adapters, fixture/live/replay mode per surface.
4. **Product gap priority list** — ordered gaps tied to `EVAL-PRODUCT-*` / `ALIGN-PRODUCT-*` / `FE-PRODUCT-*` specs.
5. **Production-readiness separation** — explicit list of items deferred to `PROD-READINESS-*` / `ENV-REAL-PROVIDER-001` / cloud program.
6. **First implementation PR confirmation** — must remain **`EVAL-PRODUCT-001`** per `02_PRODUCT_HARDENING_SEQUENCE.md` (backend semantics first).

## Acceptance

```text
all six repos inspected
eval routes and frontend eval surfaces inventoried
product gaps separated from production-readiness gaps
first implementation PR confirmed as EVAL-PRODUCT-001
package matrices 01, 03–08 updated with concrete findings (not placeholder bullets)
docs/evidence/PFH_001_CURRENT_STATE_AUDIT_EVIDENCE.md added
no runtime source changes
no workflow changes
no secrets committed
not production readiness
```

## Validation

```text
Repo searches and file reads documented in evidence
Gap matrices cite real paths (routes, files, tests)
OpenAPI/client status names actual snapshot files and check commands (e.g. npm run openapi:check)
git diff limited to docs/ under ontogony-platform (and docs-only in other repos if any)
```

## Evidence

`docs/evidence/PFH_001_CURRENT_STATE_AUDIT_EVIDENCE.md` — commands, results, repo HEADs (if captured), limitations, **not production readiness** statement.

## Non-goals

- production readiness
- real provider mode
- cloud deployment
- unscoped runtime changes
- implementing `EVAL-PRODUCT-001` or any product feature in this PR

## After this PR

Next implementation item: **`EVAL-PRODUCT-001`** — Eval query/list contract and dashboard data model (`pr-specs/EVAL-PRODUCT-001_EVAL_QUERY_LIST_CONTRACT.md`).
