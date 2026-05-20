# Conexus deepening — scorecard

**Date:** 2026-05-20 (documentation closeout)  
**Scale:** 1–5 (5 = strong for Docker-local operator scope)

| Category | Score | Notes |
| --- | ---: | --- |
| Request lifecycle completeness | 4 | List + detail + execution-run lookup; cursor pagination; in-memory/postgres modes surfaced |
| Route-decision explainability | 4 | Enriched admin detail, explorer UI, links from list/detail/Allagma |
| Provider attempt / fallback clarity | 4 | Journal-backed attempts table; fallback chain; readiness on route detail |
| Token / cost clarity | 4 | Governance summary + breakdowns; missing-token drill-down; no fabricated usage |
| Cross-service evidence linkage | 4 | Evidence-links API + spine panel; correlation panel; explicit unavailable reasons |
| Redaction / retention boundary | 4 | Hash-only admin DTOs; export bundles redacted; in-memory/restart warnings |
| UI usability | 4 | Six-tab workbench; Recent default; cross-tab links; persistence banners |
| Test coverage | 4 | 12 API model-call tests, 8 application mapping tests, 61 frontend conexus tests |
| Docker-local verification | 2 | Seed/bootstrap documented; **browser walkthrough not executed** in closeout pass |
| **Overall (docs closeout)** | **4** | Implementation credible; operator browser proof pending |

## Strengths

- Operators can start at **Recent requests** without a model-call id.
- Usage window drills into filtered request rows with honest token coverage copy.
- Route decisions explain alias override, fallback chain, and provider readiness.
- Cross-service links do not invent synthetic route decision ids.
- Tabbed layout keeps diagnostics and provider posture available without dominating the page.

## Gaps

- No unified evidence spine graph (005 is per-model-call resolver + correlation).
- In-memory Docker-default mode loses history on gateway restart.
- No raw prompt/completion browsing in UI (by design).
- Playwright E2E uses mocked APIs; full stack browser QA not recorded on this date.
- Repo-wide `npm run typecheck` not clean (unrelated pre-existing debt).
