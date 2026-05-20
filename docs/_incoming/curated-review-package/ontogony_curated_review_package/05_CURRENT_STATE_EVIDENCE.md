# Current-state evidence notes used for curation

This package keeps implementation-relevant findings that still match current repository evidence.

## Allagma

- The API maps authenticated `/allagma/v0` routes for run list/start/get/events/capabilities/audit/resume/evaluation/baseline surfaces.
- Human-gate resume is exposed at `/allagma/v0/runs/{runId}/resume`.
- Conexus model calls are routed through `IAllagmaModelPurposeCatalog`; the request uses `route.ConexusModelAlias` and `route.SystemPrompt`.
- Application DI registers `SimulatedAgentToolSideEffectExecutor`, `LocalSandboxFileMarkerExecutor`, sandbox dry-run/execute services, model-purpose catalog, governed tool registry, and side-effect ledger.
- `IAgentToolSideEffectExecutor` remains explicitly described as an optional hook for future sandboxed tool execution; therefore real external tool execution should remain gated by design.

## Conexus

- `OpenAiChatCompletionRequest` includes model, messages, temperature, top_p, max_tokens, stream, metadata, and user; it does not include tools/tool_choice/function-call fields.
- `ConexusChatCompletionsClient` exists and sends bearer project API key, idempotency key for non-streaming calls, and additional integration headers for streaming.
- `GovernanceEndpoints` is not a pure placeholder: it exposes `/v1/governance/usage` for project-scoped usage summaries.
- `Program.cs` maps admin, routing admin, route decision, model-call, diagnostics, DbViewer, retention, chat completions, governance, and model-call evidence endpoint groups behind middleware.

## Kanon

- Default `appsettings.json` keeps `Kanon:Persistence:Mode` as `InMemory`.
- Default auth is `DevelopmentTrustedHeaders`; service-token mode exists but is not the default.
- Conexus assistance is present but disabled by default, with empty base URL/API key.
- `Program.cs` uses a large minimal-API setup with many mapped exception types and broad endpoint registration.

## Frontend

- `src/app/routes.tsx` mounts all routes under `OntogonyShell` without an explicit protected route wrapper.
- Root config duplication exists: both `vite.config.ts` and `vite.config.js` are present.
- `systemFixtures.ts` marks system nodes as fallback/demo when live health has not loaded.
- `allagmaExecutionFixtures.ts` marks run fixtures as demo-only and says release routes use live APIs.
- `kanonSemanticFixtures.ts` contains mock ontology/source-binding/plan/decision/provenance data.
