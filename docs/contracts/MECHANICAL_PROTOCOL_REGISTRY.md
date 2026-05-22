# Mechanical Protocol Registry

> **Platform contract authority.** This registry is the canonical reference for every mechanical
> protocol identifier used across the six-repo Ontogony system.  Product repos derive meaning from
> these identifiers; platform owns the shape and propagation rules.

All entries here are frozen by PLAT-9-002. Breaking changes require a migration note in the PR and a
version bump to the affected schema.

---

## Protocol families

| Family | Contract doc | Schema | Owning source |
| --- | --- | --- | --- |
| Trace context | [TRACE_CONTEXT_CONTRACT.md](TRACE_CONTEXT_CONTRACT.md) | `schemas/contracts/trace-context.schema.json` | `Ontogony.Contracts.Events.OntogonyEventHeaders` |
| Header propagation | [HEADER_PROPAGATION_CONTRACT.md](HEADER_PROPAGATION_CONTRACT.md) | `schemas/contracts/header-propagation.schema.json` | `Ontogony.Http.OntogonyPropagationHeaderContract` |
| Cross-service error | [CROSS_SERVICE_ERROR_CONTRACT.md](CROSS_SERVICE_ERROR_CONTRACT.md) | `schemas/contracts/cross-service-error.schema.json` | `Ontogony.Errors.CrossServiceErrorEnvelope` |
| Idempotency | [IDEMPOTENCY_CONTRACT.md](IDEMPOTENCY_CONTRACT.md) | `schemas/contracts/idempotency.schema.json` | `Ontogony.Idempotency` |
| Evidence identifier | [EVIDENCE_IDENTIFIER_CONTRACT.md](EVIDENCE_IDENTIFIER_CONTRACT.md) | `schemas/contracts/evidence-identifier.schema.json` | `Ontogony.Contracts.References` |

---

## Cross-cutting identifiers (all protocols)

| Identifier | HTTP header | Span attribute | Envelope field |
| --- | --- | --- | --- |
| Trace id | `X-Ontogony-Trace-Id` | `ontogony.trace_id` | `traceId` |
| W3C traceparent | `traceparent` | — | — |
| Correlation id | `X-Ontogony-Correlation-Id` | `ontogony.operation_id` | — |
| Actor id | `X-Ontogony-Actor-Id` | `ontogony.actor_id` | `actorId` |
| Idempotency key | `X-Ontogony-Idempotency-Key` | — | — |
| Allagma run id | `X-Allagma-Run-Id` | — | — |

---

## Consumer compliance requirements

| Repo | Must prove |
| --- | --- |
| `conexus-dotnet` | Inbound header extraction; outbound propagation; error envelope shape |
| `kanon-dotnet` | Inbound header extraction; outbound propagation; error envelope shape |
| `allagma-dotnet` | Full outbound propagation including `X-Allagma-Run-Id`; error envelope shape |
| `ontogony-frontend` | Renders all cross-cutting identifiers; links back to evidence using run/trace/correlation ids |
| `ontogony-ui` | Neutral rendering contracts accept trace/correlation/evidence fields without product semantics |

Compliance is proved by the consumer conformance suite (PLAT-9-003).

---

## Forbidden concepts in platform source

The following concepts must not appear in any file under `src/` in this repo:

```text
provider routing policy
ontology truth
canonical fact resolution
agent plan semantics
business approval rules
domain pack promotion logic
operator page behavior
frontend route behavior
```

See `scripts/check-no-product-semantics.ps1` (PLAT-9-006).
