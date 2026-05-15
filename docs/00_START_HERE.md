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

## Package semantic contracts

Per-package intent (guarantees, non-goals, test-only APIs): [`docs/packages/`](./packages/).

## The correct mental model

```text
Ontogony.Platform = mechanical substrate
Kanon             = semantic / ontology authority
Allagma           = governed execution authority
Conexus           = LLM gateway / provider authority
Athanor           = canonical knowledge / provenance authority (legacy naming in some docs)
Agentor           = historical agent runtime name (see Allagma)
```

Product repos may emit events into the same mechanical vocabulary. They should not share domain brains.

**Consumer blueprints:** [`docs/consumer-blueprints/README.md`](./consumer-blueprints/README.md) — Conexus.NET is the active alpha consumer; Allagma.NET is documented as a blueprint with compile-only smoke in [`examples/AllagmaDotNetSkeleton/`](../examples/AllagmaDotNetSkeleton/).

## Immediate extraction targets

- Replace `Athanor.Api.Middleware.TraceIdMiddleware` and `Agentor.Api.Middleware.RequestTracingMiddleware` with `Ontogony.Observability`.
- Replace duplicated exception middleware shapes with `Ontogony.Errors`.
- Use `Ontogony.Hosting` to compose startup defaults mechanically (observability, errors, middleware order, health endpoints) without introducing service semantics.
- Use `Ontogony.Http` for integration clients in Agentor, Conexus edges that call other services, and future Athanor adapters.
- Use `Ontogony.Hashing` for payload/event/fingerprint hashes.
- Use `Ontogony.Contracts` for future AG-UI/MCP/A2A event recorder payloads.

## Adoption guides

- `docs/adoption/athanor-platform-adoption.md` for Athanor phased adoption (Primitives/Hashing/Contracts first, then Observability/Errors/Http) and legacy trace-header notes.
- `docs/adoption/agentor-platform-adoption.md` for the full Agentor mechanics checklist (Observability, Http, optional Errors) and trace-header compatibility.
- `docs/adoption/conexus-platform-adoption.md` for Conexus correlation headers, polyglot vs .NET adoption, and links to envelope emission guidance.
- `docs/adoption/athanor-observability-adoption.md` for Athanor tracing + errors wiring while keeping Athanor mappings local.
- `docs/adoption/error-middleware-adoption.md` for replacing local Athanor/Agentor exception middleware with Ontogony.Errors while keeping service-specific mappings in service repos.
- `docs/adoption/observability-error-ordering.md` for middleware ordering guidance when combining Ontogony.Observability with Ontogony.Errors.
- `docs/adoption/hosting-service-defaults-adoption.md` for replacing duplicated startup mechanics with Ontogony.Hosting while keeping per-service policy and endpoint mapping local.

## Planning packages

- `docs/planning/ontogony-platform-next-prs/README.md` for the PR26-PR35 infrastructure roadmap package.
