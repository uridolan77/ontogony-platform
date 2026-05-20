# Cross-service evidence spine — scorecard

**Date:** 2026-05-20  
**Scale:** 1–5 (5 = strong for Docker-local operator scope)

| Category | Score | Notes |
| --- | ---: | --- |
| ID taxonomy clarity | 5 | Platform operator doc + parser kinds + ambiguity flags |
| Unified resolver coverage | 4 | Run, eval, model call, decision, trace, correlation, baseline, route, human gate; dataset/scenario parsed only |
| Missing-link honesty | 5 | `unresolved_expected_link` edges + stable reason codes + warnings |
| Source attempt visibility | 5 | Every HTTP lookup recorded with status including `forbidden` |
| Graph workbench UX | 4 | Paste lookup, nodes/edges/attempts/missing/export; not a force-directed canvas |
| Export + redaction | 5 | Schema v1, platform fixture test, operator toggles for raw previews |
| Automated test confidence | 5 | 27 vitest + 6 Playwright + 1 schema test on closeout run |
| Docker-live browser proof | 3 | Script + provenance gate; live stack walkthrough not required for PASS |
| Documentation traceability | 5 | Per-slice evidence + sequence index + closeout set |
| **Overall (package closeout)** | **4.5** | v1 operator goal met; known resolver roots and live QA gaps documented |

## Strengths

- One workbench replaces mental stitching across Allagma, Conexus, and Kanon detail pages.
- Partial graphs are first-class: missing edges explain *why*, not silent omission.
- Reuses existing GET contracts—no semantic authority drift into platform.
- Export bundle is schema-governed and redacted by default.
- Trace correlation (`resolveTraceCorrelation`) remains for legacy surfaces; spine subsumes and extends it.

## Gaps

- `datasetId` / `scenarioId` classified in parser but not wired in `resolveEvidenceSpine` switch.
- `includeExports` / `includeEvents` / `includeRelated` accepted on input type but not expanded in v1 resolver.
- Human gate ID resolution scans Allagma run lists—no direct GET-by-gate-id.
- Playwright uses mocked APIs; live cross-service graph depends on seeded Docker data quality.
- No server-side graph cache or aggregator—30 API call cap can truncate deep chains.
