# Migration Checklist

## Athanor

- [ ] Replace `TraceIdMiddleware` with `UseOntogonyRequestTracing`.
- [ ] Keep accepting `X-Athanor-Trace-Id` on **inbound** requests during migration.
- [ ] Read **`X-Ontogony-Trace-Id`** on responses (legacy response echo is off unless `EchoLegacyHeaders = true`).
- [ ] Replace `ErrorHandlingMiddleware` with `UseOntogonyExceptionHandling` plus Athanor-specific mappings.
- [ ] Move `IClock`, `IIdGenerator`, and `IContentHashService` to shared primitives only if no domain coupling remains.
- [ ] Do not move canonization services.

## Agentor

- [ ] Replace `RequestTracingMiddleware` and `AgentorCorrelationContext`.
- [ ] Replace `CorrelationHeadersDelegatingHandler` with shared handler.
- [ ] Evaluate moving HTTP integration registration pattern into `Ontogony.Http`.
- [ ] Keep `ToolPayload`, plan execution, and policy semantics inside Agentor.
- [ ] Reuse canonical JSON hasher for idempotency fingerprint only after test parity.

## Conexus

- [ ] Do not port Conexus to .NET just to use this repo.
- [ ] Add Python event emitter for `llm.request.created`, `llm.response.completed`, `llm.provider.failed`.
- [ ] Map `X-Conexus-Request-Id` to `traceId` when emitting events.
- [ ] Do not emit prompts/responses by default.
- [ ] Add `X-Ontogony-Trace-Id` passthrough when receiving calls from Agentor.
