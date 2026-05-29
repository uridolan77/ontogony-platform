# Risk register — ONTOGONY-BACKEND-COORDINATION-002

| ID | Risk | Likelihood | Impact | Mitigation | Owner slice |
| --- | --- | --- | --- | --- | --- |
| R-001 | Parallel slice work causes merge conflicts in `docs/system/` | Medium | Medium | Strict phase order; one PR per slice | Parent |
| R-002 | Conexus/Kanon/Allagma build failures block conformance | High | High | Fix builds in slice 2 before matrix refresh | 2 |
| R-003 | Error envelope breaks Kanon typed validation DTOs | Medium | Medium | Document intentional exceptions; dual-shape tests | 3 |
| R-004 | Header propagation breaks provider adapters | Low | High | Incremental middleware; provider smoke after slice 4 | 4 |
| R-005 | Alias manifest drift between Conexus admin and Allagma config | Medium | Medium | Snapshot test + lock row | 5 |
| R-006 | E2E flake without Postgres/services | Medium | Medium | Document required services; fixture fallback classified | 6 |
| R-007 | Aisthesis live PASS conflated with fixture ingestion | High | High | Acceptance matrix requires `mode: Live` | 7 |
| R-008 | Metabole Kanon boundary confusion during hardening | Medium | Medium | Boundary doc + peer route tests | 8 |
| R-009 | Active intake packages overlap (009, live-certify) | Medium | Low | Merge per scope doc conflict resolution | 7, 8 |
| R-010 | Runtime lock bump without evidence | Low | High | Dedicated commit + validate-runtime-lock ReleaseMode | 2 |
| R-011 | Real tools accidentally enabled during E2E | Low | Critical | `validate-real-tools-block.ps1` in slice 6 gate | 6 |
| R-012 | SDK pin blocks Aisthesis CI | Medium | Medium | Align global.json in slice 7 | 7 |
