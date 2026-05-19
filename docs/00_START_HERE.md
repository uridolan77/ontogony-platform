# 00 — Start Here

**Documentation map (all programs, closeouts, evidence):** [`docs/README.md`](./README.md)

This repo exists because Allagma, Kanon, and Conexus need a shared infrastructure spine.

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
Agentor           = historical name replaced by Allagma
```

Product repos may emit events into the same mechanical vocabulary. They should not share domain brains.

**Consumer blueprints:** [`docs/consumer-blueprints/README.md`](./consumer-blueprints/README.md) — Conexus.NET and Allagma.NET are active alpha consumers; compile-only smokes live in [`examples/ConexusDotNetSkeleton/`](../examples/ConexusDotNetSkeleton/) and [`examples/AllagmaDotNetSkeleton/`](../examples/AllagmaDotNetSkeleton/).

## Immediate extraction targets

- Replace duplicated service-local trace middleware with `Ontogony.Observability`.
- Replace duplicated exception middleware shapes with `Ontogony.Errors`.
- Use `Ontogony.Hosting` to compose startup defaults mechanically (observability, errors, middleware order, health endpoints) without introducing service semantics.
- Use `Ontogony.Http` for resilient integration clients in Allagma, Kanon, Conexus, and related services.
- Use `Ontogony.Hashing` for payload/event/fingerprint hashes.
- Use `Ontogony.Contracts` for future AG-UI/MCP/A2A event recorder payloads.

## Adoption guides

- `docs/adoption/conexus-platform-adoption.md` for Conexus adoption notes (including polyglot boundaries).
- `docs/consumer-blueprints/allagma-dotnet-platform-readiness.md` and `docs/consumer-blueprints/conexus-dotnet-platform-readiness.md` for active consumer baselines.
- `docs/adoption/conexus-platform-adoption.md` for Conexus correlation headers, polyglot vs .NET adoption, and links to envelope emission guidance.
- `docs/adoption/error-middleware-adoption.md` for replacing local exception middleware with Ontogony.Errors while keeping service-specific mappings in service repos.
- `docs/adoption/observability-error-ordering.md` for middleware ordering guidance when combining Ontogony.Observability with Ontogony.Errors.
- `docs/adoption/hosting-service-defaults-adoption.md` for replacing duplicated startup mechanics with Ontogony.Hosting while keeping per-service policy and endpoint mapping local.

## Planning packages

- `docs/planning/ontogony-platform-next-prs/README.md` for the PR26-PR35 infrastructure roadmap package.
