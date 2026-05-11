# 00 — Start Here

This repo exists because Athanor, Agentor, and Conexus now need a shared infrastructure spine.

Do not begin by moving everything into shared code. Begin with the duplicated or cross-cutting mechanics that must be consistent for the protocol-recorder strategy:

1. Trace/correlation vocabulary.
2. Event envelope shape.
3. Error shape.
4. HTTP integration pattern.
5. Hashing/fingerprinting/idempotency.
6. Configuration/startup safety.
7. Basic messaging/outbox abstractions.

## The correct mental model

```text
Ontogony.Platform = mechanical substrate
Athanor           = canonical knowledge / provenance authority
Agentor           = agent runtime / execution authority
Conexus           = LLM gateway / provider authority
```

Athanor, Agentor, and Conexus may all emit events into the same event vocabulary. They should not share domain brains.

## Immediate extraction targets

- Replace `Athanor.Api.Middleware.TraceIdMiddleware` and `Agentor.Api.Middleware.RequestTracingMiddleware` with `Ontogony.Observability`.
- Replace duplicated exception middleware shapes with `Ontogony.Errors`.
- Use `Ontogony.Http` for integration clients in Agentor and future Athanor adapters.
- Use `Ontogony.Hashing` for payload/event/fingerprint hashes.
- Use `Ontogony.Contracts` for future AG-UI/MCP/A2A event recorder payloads.

## Adoption guides

- `docs/adoption/error-middleware-adoption.md` for replacing local Athanor/Agentor exception middleware with Ontogony.Errors while keeping service-specific mappings in service repos.
- `docs/adoption/observability-error-ordering.md` for middleware ordering guidance when combining Ontogony.Observability with Ontogony.Errors.
