# Acceptance matrix

| ID | Status | Acceptance criteria |
|---|---|---|
| SYSTEM-COH-001 | A new developer can start the local stack from the matrix; every cross-service client call in Allagma/Kanon/Conexus maps to one documented route/auth/env row; no obsolete naming appears. |
| SYSTEM-E2E-001 | A single command produces artifacts/system-e2e/<timestamp>/summary.json with route responses, event timeline, decision IDs, model call IDs, trace IDs, and pass/fail status. |
| SYSTEM-ERR-001 | Kanon.Client and Conexus.Client expose deterministic typed failures; Allagma translates downstream failures into the shared envelope; frontend adapters can render a single error model. |
| SYSTEM-CTX-001 | Trace/correlation IDs appear in Allagma events, Kanon decision records, and Conexus telemetry/model-call records for the same local-stack run. |
| SYSTEM-DASH-001 | A local Grafana/importable dashboard or documented panel pack can be loaded against the local stack. |
| ALLAGMA-TOOL-TRUST-001 | Design covers deny-by-default registry, per-tool permissions, secret scope, outbound/filesystem allowlists, human gate policy, durable side-effect ledger, no-reexecution replay rule, timeout/cancellation, and evidence export. |
| ALLAGMA-SANDBOX-OBS-001 | Every run that touches tool execution records mode=symbolic|dry_run|local_sandbox|real_external, with real_external unavailable until the trust model passes. |
| ALLAGMA-EVIDENCE-001 | Frontend can render an audit/evidence panel using live APIs; system E2E summary links every run to Kanon decision/provenance and Conexus model-call IDs. |
| ALLAGMA-STREAM-001 | A non-default local smoke can stream through Conexus and produce redacted Allagma progress events. |
| CONEXUS-TOOLS-001 | A request containing tools and tool_choice round-trips through Conexus to an OpenAI-compatible provider or fake provider without contract loss. |
| CONEXUS-GOV-DRILLDOWN-001 | Frontend Conexus observability page can display recent calls, route, fallback, token/cost, artifact/evidence availability, and trace/run links from live APIs. |
| CONEXUS-IDEMP-001 | In Postgres mode, duplicate implicit requests are coordinated durably across process restart or multiple service instances. |
| CONEXUS-STREAM-COST-001 | Streaming model calls have either cost values or explicit cost_unknown reason visible in admin/API telemetry. |
| CONEXUS-ADMIN-SAFETY-001 | Unauthenticated and project-key-only callers cannot reach admin/diagnostic endpoints; production config validation documents expected controls. |
| CONEXUS-RETENTION-001 | Admin can query retention config and last cleanup summary; local test proves expired records are handled as configured. |
| KANON-POSTGRES-LOCAL-001 | A local script runs migrations and verifies the major workflows persist across process restart. |
| KANON-EVIDENCE-001 | Each evidence pack has commands, expected responses, storage/persistence notes, and failure cases. |
| KANON-AUTH-LOCAL-001 | Local stack can run Kanon in ServiceToken mode and Allagma/Kanon client tests still pass. |
| KANON-API-MODULAR-001 | No route behavior changes; tests/OpenAPI route inventory remain stable. |
| KANON-CONEXUS-ASSIST-001 | System E2E verifies one assistance route produces a redacted draft_only decision record and provenance entry. |
| KANON-DOMAINPACK-GOV-001 | Domain-pack lifecycle docs and tests cover promote, active lookup, deprecate, blocked promotion, hash mismatch, and signature failure. |
| FE-CLEANUP-001 | **closed** | Only intended Vite/Vitest/Tailwind configs are present; CI proves Vite and Vitest resolve the canonical files. |
| FE-AUTH-001 | **closed** | Without configured local token/session, operator routes render an auth-required state; authenticated local settings allow access. |
| FE-ERRBOUND-001 | **closed** | Each domain has a page-level error state for failed backend calls; tests cover at least one failure per domain. |
| FE-FIXTURE-MATRIX-001 | **closed** | Every route in route-workflow-catalog has dataSource=live|live_with_fallback|fixture_only|not_implemented and the UI labels non-live content clearly. |
| FE-STUBS-001 | planned | No released route renders a near-empty page without clear status and next action. |
| FE-API-ADAPTER-001 | **closed** | Generated/handwritten client DTO changes fail tests if UI contracts break. |
| FE-DOCKER-001 | One local command serves the frontend and reaches Conexus/Kanon/Allagma health plus at least one live page per domain. |
| FE-TEST-001 | Vitest covers render/loading/error/success state for these pages; optional Playwright smoke covers main navigation. |
