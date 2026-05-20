# Curated backlog

| ID | Priority | Repo | Theme | Title | Status |
|---|---:|---|---|---|---|
| SYSTEM-COH-001 | P0 | allagma-dotnet primary; all repos referenced | Cross-repo cohesion | Create canonical system compatibility, environment, auth, route, and test matrices |
| SYSTEM-E2E-001 | P0 | allagma-dotnet primary; exercises kanon-dotnet and conexus-dotnet | Cross-repo cohesion | Add full local-stack E2E suite for the governed runtime loop |
| SYSTEM-ERR-001 | P0 | ontogony-platform first; consumed by allagma-dotnet, kanon-dotnet, conexus-dotnet, ontogony-frontend | Cross-service contracts | Standardize cross-service error envelope and typed client failures |
| SYSTEM-CTX-001 | P0 | allagma-dotnet, kanon-dotnet, conexus-dotnet, ontogony-frontend | Traceability | Standardize end-to-end trace, correlation, actor, and idempotency propagation |
| SYSTEM-DASH-001 | P1 | ontogony-platform or allagma-dotnet docs; dashboards may span all repos | Operations | Create starter dashboard and SLO pack for the three-node runtime |
| ALLAGMA-TOOL-TRUST-001 | P0 | allagma-dotnet | Safety / execution boundaries | Keep real tool execution blocked until the real-tool trust model is complete |
| ALLAGMA-SANDBOX-OBS-001 | P1 | allagma-dotnet | Operator clarity | Make sandbox/simulated execution status explicit in capabilities, events, and docs |
| ALLAGMA-EVIDENCE-001 | P1 | allagma-dotnet | Evidence / replay | Tighten run audit and evaluation evidence export for frontend and system E2E use |
| ALLAGMA-STREAM-001 | P2 | allagma-dotnet | Runtime UX | Plan Conexus streaming integration into Allagma progress events |
| CONEXUS-TOOLS-001 | P0 | conexus-dotnet | Gateway feature completeness | Add OpenAI-compatible tools/tool_choice/function-call pass-through |
| CONEXUS-GOV-DRILLDOWN-001 | P1 | conexus-dotnet + ontogony-frontend | Operator observability | Add row-level model-call/usage drill-down contracts for the operator console |
| CONEXUS-IDEMP-001 | P1 | conexus-dotnet | Durability | Make implicit idempotency multi-node durable |
| CONEXUS-STREAM-COST-001 | P1 | conexus-dotnet | Telemetry integrity | Close streaming usage/cost observability gaps |
| CONEXUS-ADMIN-SAFETY-001 | P1 | conexus-dotnet | Admin safety | Add explicit tests and docs for sensitive admin/diagnostic endpoint exposure |
| CONEXUS-RETENTION-001 | P2 | conexus-dotnet | Data lifecycle | Strengthen retention policy evidence and maintenance run visibility |
| KANON-POSTGRES-LOCAL-001 | P0 | kanon-dotnet | Durable local path | Make PostgreSQL the proven local acceptance path |
| KANON-EVIDENCE-001 | P0 | kanon-dotnet | Feature proof | Add workflow evidence packs for major Kanon surfaces |
| KANON-AUTH-LOCAL-001 | P1 | kanon-dotnet | Security posture | Harden and document local service-token auth path while preserving dev headers |
| KANON-API-MODULAR-001 | P1 | kanon-dotnet | Maintainability | Split large API Program.cs into feature endpoint modules |
| KANON-CONEXUS-ASSIST-001 | P1 | kanon-dotnet + allagma-dotnet E2E | Assistance seam | Prove Conexus assistance disabled/mock/local paths, not real-provider rollout |
| KANON-DOMAINPACK-GOV-001 | P2 | kanon-dotnet | Governance | Clarify domain-pack lifecycle promotion states and blocking rules |
| FE-CLEANUP-001 | P0 | ontogony-frontend | Build correctness | Resolve duplicate root config artifacts | **closed** |
| FE-AUTH-001 | P0 | ontogony-frontend | Operator access | Add route-level operator auth guard | **closed** |
| FE-ERRBOUND-001 | P0 | ontogony-frontend | UX resilience | Add route-level error boundaries and API failure states | **closed** |
| FE-FIXTURE-MATRIX-001 | P1 | ontogony-frontend | Live vs demo clarity | Create page-by-page live/fallback/fixture matrix and remove misleading demo paths | **closed** |
| FE-STUBS-001 | P1 | ontogony-frontend | Operator trust | Replace thin/stub pages with honest operational states or live wiring | **closed** |
| FE-API-ADAPTER-001 | P1 | ontogony-frontend | Contract safety | Add API/adapter contract tests for Conexus, Kanon, and Allagma clients | **closed** |
| FE-DOCKER-001 | P1 | ontogony-frontend or ontogony-platform | Local environment | Complete Docker-local frontend composition and ontogony-ui build strategy |
| FE-TEST-001 | P2 | ontogony-frontend | Regression coverage | Add smoke tests for the highest-value operator pages |
