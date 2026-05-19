# 07 — Test and Evidence State

**Audit:** PFH-001 (2026-05-19).

## Existing categories (preserve)

| Category | Location (representative) | Status |
| --- | --- | --- |
| Backend eval API | `AllagmaEvaluationApiTests.cs` incl. `GlobalEvaluationQuery` | PASS (EVAL-PRODUCT-001) |
| OpenAPI snapshot | `AllagmaOpenApiSnapshotTests.cs` | PASS |
| Baseline comparison | `BaselineComparisonTests.cs` | PASS |
| Postgres persistence | `PostgresEvaluationPersistenceTests.cs` | PASS (smoke category) |
| Dataset/harness | `EvaluationScenarioDatasetTests.cs`, `EvalHarnessRunnerTests.cs`, `EvalCiSuiteTests.cs` | PASS |
| Quality scoring | `EvalQualityScoringTests.cs` | PASS |
| FE adapters | `allagmaEvaluationAdapters.test.ts`, `allagmaEvaluationDashboardAdapters.test.ts` | PASS |
| Playwright eval | `e2e/allagma-eval-dashboards.spec.ts`, `allagma-eval-topology-evidence.spec.ts`, `fixture-live-boundary.spec.ts` | PASS |
| Playwright replay | `e2e/allagma-replay-evidence.spec.ts`, `allagma-run-detail-replay-navigation.spec.ts` | PASS |
| Docker-local FE | `e2e/docker-local-operator-walkthrough.spec.ts` | PASS |
| Kanon API surface | `kanon-dotnet/tests/Kanon.Tests/ApiSurfaceSnapshotTests.cs` | PASS |
| Conexus evidence | `ModelCallRouteEvidenceIntegrationTests.cs`, `RouteDecisionAdminIntegrationTests.cs` | PASS |
| Trace correlation | `ontogony-platform/docker/local-working-system/scripts/inspect-trace-correlation-evidence.ps1` | PASS (`TRACE_CONTRACT_001`) |
| CI cost gates | path-filtered aggregates (six repos) | PASS (`CI_COST_001`) |
| UI packaging | `ontogony-ui` `check:pack`, `check:exports`, `check:smoke-dist` | PASS (`UI_PACKAGING_STATUS_001`) |

## Platform evidence (PFH-relevant)

| File | Topic |
| --- | --- |
| `docs/evidence/PFH_000_PACKAGE_SETUP_EVIDENCE.md` | Package registration |
| `docs/evidence/EVAL_PRODUCT_001_QUERY_LIST_CONTRACT_EVIDENCE.md` | Global eval list contract (platform index) |
| `docs/evidence/EVAL_PRODUCT_002_BASELINE_COMPARISON_WORKBENCH_EVIDENCE.md` | Baseline comparison list/history workbench |
| `docs/evidence/EVAL_PRODUCT_003_SCENARIO_DATASET_SURFACES_EVIDENCE.md` | Scenario dataset index/detail + dashboard metadata consumption |
| `docs/evidence/EVAL_PRODUCT_004_QUALITY_SCORING_AND_JUDGE_CALIBRATION_EVIDENCE.md` | Quality/judge metadata contract + FE calibration surfaces |
| `docs/evidence/EVAL_POLISH_001_EVALUATION_SURFACE_CORRECTNESS_EVIDENCE.md` | Cross-repo polish closeout (parity + error semantics + wording) |
| `docs/evidence/FE_PRODUCT_002_RUN_DETAIL_EVIDENCE_DEPTH_EVIDENCE.md` | Run/eval detail evidence journey (FE-PRODUCT-002) |
| `ontogony-frontend/docs/evidence/FE_PRODUCT_002_RUN_DETAIL_EVIDENCE_DEPTH_EVIDENCE.md` | Frontend implementation evidence |
| `docs/evidence/FE_PRODUCT_003_REPLAY_EVIDENCE_WORKBENCH_EVIDENCE.md` | Replay workbench journey + limitations (FE-PRODUCT-003) |
| `ontogony-frontend/docs/evidence/FE_PRODUCT_003_REPLAY_EVIDENCE_WORKBENCH_EVIDENCE.md` | Frontend implementation evidence |
| `allagma-dotnet/docs/evidence/EVAL_POLISH_001A_BASELINE_QUERY_PARITY_EVIDENCE.md` | Backend parity and baseline cursor tests |
| `ontogony-frontend/docs/evidence/FE_EVAL_POLISH_001_ERROR_SEMANTICS_AND_WORDING_EVIDENCE.md` | Frontend detail error semantics, tri-state copy, fixture/live correction |
| `allagma-dotnet/docs/evidence/EVAL_PRODUCT_001_QUERY_LIST_CONTRACT_EVIDENCE.md` | Backend implementation |
| `ontogony-frontend/docs/evidence/EVAL_PRODUCT_001_FRONTEND_QUERY_LIST_CONTRACT_EVIDENCE.md` | Frontend consumption |
| `docs/evidence/ALIGN_EVAL_001_EVAL_ALIGNMENT_REFRESH_EVIDENCE.md` | Eval alignment snapshot |
| `docs/evidence/TRACE_CONTRACT_001_EVIDENCE.md` | Trace header contract |
| `docs/evidence/FE_HARDEN_001_EVIDENCE.md` | Docker-local FE walkthrough |
| `docs/evidence/FE_AUDIT_FIXTURES_001_EVIDENCE.md` | Fixture/live audit |
| `docs/evidence/FE_TEST_REPLAY_001_EVIDENCE.md` | Replay tests |
| `docs/evidence/FE_HYGIENE_CONFIG_001_EVIDENCE.md` | VITE/config catalog |
| `docs/evidence/POST_DOCKER_HARDENING_CLOSEOUT_001_EVIDENCE.md` | Hardening closeout |
| `docs/evidence/FE_PRODUCT_CLOSEOUT_001_EVIDENCE.md` | Product hardening v1 closeout |

## Allagma evidence (`docs/evidence/*EVAL*` — 20 files)

Index: `allagma-dotnet/docs/evidence/EVAL_SEQUENCE_STATUS_INDEX.md`. Key: `EVAL_DUR_001`, `EVAL_DATA_001`, `EVAL_QUALITY_001`, `EVAL_RUN_001`–`003`, `SYS_FULL_SANITY_001`.

Closeout: `allagma-dotnet/docs/releases/FIRST_FULL_SANITY_CLOSEOUT.md` (2026-05-18 PASS).

## Frontend evidence

| File | Topic |
| --- | --- |
| `ontogony-frontend/docs/evidence/FE_EVAL_002_DASHBOARDS_EVIDENCE.md` | Eval dashboards |
| `ontogony-frontend/docs/evidence/FE_POLISH_001_*` | Query UX, banners |
| `ontogony-frontend/docs/evidence/FE_AUDIT_FIXTURES_001_*` | Fixture/live |
| `ontogony-frontend/docs/evidence/FE_TEST_REPLAY_001_*` | Replay |
| `ontogony-frontend/docs/evidence/FE_HYGIENE_CONFIG_001_*` | Config |
| `ontogony-frontend/docs/evidence/FE_HARDEN_001_*` | Hardening E2E |

## Gaps for upcoming PRs

| PR | Test/evidence gap |
| --- | --- |
| `EVAL-PRODUCT-001` | New API/DTO tests if route added; OpenAPI + `openapi:check`; adapter tests for new list model |
| `ALIGN-PRODUCT-001` | **Done:** `ALIGN_PRODUCT_001_CONTRACT_MATRIX_REFRESH_EVIDENCE.md`; matrix `04`; consumption matrix refresh |
| `FE-PRODUCT-001` | **Done:** `FE_PRODUCT_001_EVAL_DASHBOARD_V2_EVIDENCE.md`; filter e2e in `allagma-eval-dashboards.spec.ts` |
| `EVAL-PRODUCT-002` | **Done:** backend API list tests + frontend/e2e workbench coverage (`EVAL_PRODUCT_002_BASELINE_COMPARISON_WORKBENCH_EVIDENCE.md`) |
| `EVAL-PRODUCT-003` | **Done:** dataset API route tests + FE route/adapter tests + OpenAPI sync/check evidence |
| `EVAL-PRODUCT-004` | **Done:** backend quality metadata contract tests + FE metadata adapter/detail wording tests |
| `EVAL-POLISH-001` | **Done:** baseline recommendation parity + baseline cursor tests + frontend detail error semantics and tri-state quality wording |
| `FE-PRODUCT-002` | **Done:** run detail + eval detail journey links; `buildRunEvidenceJourneyLinks` unit tests; `allagma-run-detail-evidence-journey` e2e; `FE_PRODUCT_002_RUN_DETAIL_EVIDENCE_DEPTH_EVIDENCE.md` |
| `FE-PRODUCT-003` | **Done:** `FE_PRODUCT_003_REPLAY_EVIDENCE_WORKBENCH_EVIDENCE.md`; replay journey + limitation e2e in `allagma-replay-evidence.spec.ts` |
| `EVAL-PRODUCT-005` | **Done:** schema validator + `GET /evaluations/{id}/evidence` + FE export panel (`EVAL_PRODUCT_005_EVAL_EVIDENCE_EXPORT_BUNDLE_EVIDENCE.md`) |
| `PFH-001` | `docs/evidence/PFH_001_CURRENT_STATE_AUDIT_EVIDENCE.md` (this audit) |
| `FE-PRODUCT-CLOSEOUT-001` | **Done:** closeout quartet + `FE_PRODUCT_CLOSEOUT_001_EVIDENCE.md` |

## Product-hardening test additions (unchanged expectations)

See original matrix in package; all rows still apply post-audit.
