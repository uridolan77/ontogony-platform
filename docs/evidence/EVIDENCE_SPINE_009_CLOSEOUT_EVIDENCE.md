# EVIDENCE-SPINE-009 — Closeout

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** — sequence 000–008 complete; automated gates green; Docker-live QA **PARTIAL** — see [009A](./EVIDENCE_SPINE_009A_DOCKER_LIVE_QA_EVIDENCE.md)  
**Package:** `docs/_incoming/Ontogony-Cross-Service-Evidence-Spine-Package-v1/`

## Goal

Close the cross-service evidence spine package with scorecard, explicit limitations, prioritized next options, and verification that operators can paste supported known IDs into `/system/evidence-spine` and receive an honest partial-or-complete graph plus redacted export.

## Deliverables

| Artifact | Path |
| --- | --- |
| Closeout summary | [`docs/releases/CROSS_SERVICE_EVIDENCE_SPINE_CLOSEOUT.md`](../releases/CROSS_SERVICE_EVIDENCE_SPINE_CLOSEOUT.md) |
| Scorecard | [`docs/releases/CROSS_SERVICE_EVIDENCE_SPINE_SCORECARD.md`](../releases/CROSS_SERVICE_EVIDENCE_SPINE_SCORECARD.md) |
| Known limitations | [`docs/releases/CROSS_SERVICE_EVIDENCE_SPINE_KNOWN_LIMITATIONS.md`](../releases/CROSS_SERVICE_EVIDENCE_SPINE_KNOWN_LIMITATIONS.md) |
| Next options | [`docs/releases/CROSS_SERVICE_EVIDENCE_SPINE_NEXT_OPTIONS.md`](../releases/CROSS_SERVICE_EVIDENCE_SPINE_NEXT_OPTIONS.md) |
| Sequence status | [EVIDENCE_SPINE_SEQUENCE_STATUS.md](./EVIDENCE_SPINE_SEQUENCE_STATUS.md) |

## Sequence closeout table

| Item | Status | Evidence |
| --- | --- | --- |
| EVIDENCE-SPINE-000 audit | Done | [000 evidence](./EVIDENCE_SPINE_000_CURRENT_STATE_AUDIT_EVIDENCE.md), [review](../reviews/EVIDENCE_SPINE_000_CURRENT_STATE_AUDIT.md) |
| EVIDENCE-SPINE-001 contract/taxonomy | Done | [001 evidence](./EVIDENCE_SPINE_001_RESOLVER_CONTRACT_EVIDENCE.md), [taxonomy](../operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md) |
| EVIDENCE-SPINE-002 unified resolver | Done | [002 evidence](./EVIDENCE_SPINE_002_FRONTEND_UNIFIED_RESOLVER_EVIDENCE.md), `ontogony-frontend/src/evidence-spine/resolveEvidenceSpine.ts` |
| EVIDENCE-SPINE-003 Allagma normalization | Done | `ontogony-frontend/docs/evidence/EVIDENCE_SPINE_003_ALLAGMA_EVIDENCE_NORMALIZATION_EVIDENCE.md` |
| EVIDENCE-SPINE-004 Conexus linking | Done | `ontogony-frontend/docs/evidence/EVIDENCE_SPINE_004_CONEXUS_REQUEST_ROUTE_LINKING_EVIDENCE.md` |
| EVIDENCE-SPINE-005 Kanon linking | Done | `ontogony-frontend/docs/evidence/EVIDENCE_SPINE_005_KANON_DECISION_PROVENANCE_LINKING_EVIDENCE.md` |
| EVIDENCE-SPINE-006 graph UI | Done | [006 platform index](./EVIDENCE_SPINE_006_GRAPH_WORKBENCH_UI_EVIDENCE.md), frontend detail |
| EVIDENCE-SPINE-007 export bundle | Done | [007 evidence](./EVIDENCE_SPINE_007_EXPORT_BUNDLE_EVIDENCE.md) |
| EVIDENCE-SPINE-008 e2e/browser | Done | [008 evidence](./EVIDENCE_SPINE_008_E2E_BROWSER_VERIFICATION_EVIDENCE.md) |
| EVIDENCE-SPINE-009 closeout | Done | This file |

## Acceptance checklist mapping

| Criterion | Result |
| --- | --- |
| Canonical ID taxonomy exists | **PASS** — [EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md](../operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md) |
| Unified graph model exists | **PASS** — `evidenceGraphTypes.ts` |
| Resolver accepts run ID | **PASS** — unit + e2e |
| Resolver accepts eval ID | **PASS** — unit + e2e |
| Resolver accepts model-call/request ID | **PASS** — unit + e2e |
| Resolver accepts Kanon decision ID | **PASS** — unit + e2e |
| Resolver accepts trace ID | **PASS** — via `resolveTraceCorrelation` integration |
| Missing links explained | **PASS** — missing edges panel + e2e partial scenario |
| Source attempts visible | **PASS** — `attemptedSources` on every resolution |
| Graph UI usable | **PASS** — `/system/evidence-spine` workbench |
| Export bundle exists and validates | **PASS** — schema test + export panel e2e |
| Browser/e2e proof exists | **PASS** — mocked Playwright (6 cases) |
| Paste any **supported** known ID | **PASS** with documented exceptions (dataset/scenario, gate orphans) |

## Validation run (2026-05-20)

### Frontend (`ontogony-frontend`)

```text
npm test -- src/evidence-spine → 27 passed (5 files)
npm run test:e2e -- e2e/evidence-spine-workbench.spec.ts → 6 passed
```

Coverage includes: parser ambiguity, graph helpers, `resolveEvidenceSpine` multi-root cases, workbench submit/render, export bundle builder, and e2e for run/eval/model-call/decision lookup plus partial missing-edge and export schema metadata.

### Platform (`ontogony-platform`)

```text
dotnet test … --filter EvidenceSpineExportBundleSchemaTests → 1 passed
```

Schema: `docs/schemas/ontogony-cross-service-evidence-spine-bundle-v1.schema.json`  
Fixture: `docs/schemas/fixtures/valid/cross-service-evidence-spine-bundle-minimal.json`

### Docker-local (009A)

```powershell
.\docker\local-working-system\scripts\run-evidence-spine-docker-local-verification.ps1 -Build
.\docker\local-working-system\scripts\seed-and-verify-local-working-system.ps1
```

**EXECUTED 2026-05-20** — [EVIDENCE_SPINE_009A_DOCKER_LIVE_QA_EVIDENCE.md](./EVIDENCE_SPINE_009A_DOCKER_LIVE_QA_EVIDENCE.md)

| Gate | Result |
| --- | --- |
| Frontend provenance rebuild | **PASS** (`cf09a04`) |
| Workbench route HTTP 200 | **PASS** |
| Allagma/Kanon/route-decision live GET | **PASS** |
| Conexus model-call admin GET | **FAIL** (404) |
| Manual browser paste/export | Not recorded |

## Unsupported / deferred (honesty)

Documented in [limitations](../releases/CROSS_SERVICE_EVIDENCE_SPINE_KNOWN_LIMITATIONS.md):

- `datasetId` / `scenarioId` not resolved in v1 switch
- `includeExports` flags not expanded
- No backend aggregator API
- Human gate indirect lookup only
- Mocked e2e ≠ live multi-service correlation quality

## Sign-off criteria

- [x] EVIDENCE-SPINE-000–008 evidence indexed
- [x] Closeout / scorecard / limitations / next-options docs
- [x] Automated frontend + platform schema tests pass on closeout date
- [x] Acceptance checklist mapped with explicit exceptions
- [x] Docker-live QA executed (009A) — **PARTIAL** (model-call admin 404)
- [ ] Manual browser paste/export walkthrough recorded (009B)
