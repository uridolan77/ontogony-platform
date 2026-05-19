# 07 — Test and Evidence State

## Existing categories to preserve

| Category | Examples |
|---|---|
| Backend API tests | Allagma eval API, persistence, OpenAPI snapshot tests |
| Frontend adapter tests | Eval adapters, fixture/live/replay/config checks |
| Playwright | eval dashboards, fixture/live boundary, replay evidence, run detail correlation |
| Docker-local scripts | Conexus persistence, Kanon topology, trace correlation, guided flow |
| CI gates | path-filtered aggregate checks after CI-COST-001 |
| UI packaging | `@ontogony/ui` build, export, pack, smoke checks |

## Product-hardening test additions

| PR | Minimum test/evidence expectation |
|---|---|
| `EVAL-PRODUCT-001` | API/DTO tests, OpenAPI check, frontend adapter tests. |
| `ALIGN-PRODUCT-001` | Matrix evidence, generated client verification. |
| `FE-PRODUCT-001` | Adapter/unit tests and small stable Playwright checks. |
| `EVAL-PRODUCT-002` | Comparison API/unit tests and frontend drilldown tests. |
| `EVAL-PRODUCT-003` | Dataset matrix tests and UI fixture/live tests. |
| `EVAL-PRODUCT-004` | Scoring DTO/adapter tests and limitation wording tests. |
| `FE-PRODUCT-002` | Run detail integration tests with mocked API states. |
| `FE-PRODUCT-003` | Replay workbench fixture/live/degraded tests. |
| `EVAL-PRODUCT-005` | Export bundle schema validator. |
