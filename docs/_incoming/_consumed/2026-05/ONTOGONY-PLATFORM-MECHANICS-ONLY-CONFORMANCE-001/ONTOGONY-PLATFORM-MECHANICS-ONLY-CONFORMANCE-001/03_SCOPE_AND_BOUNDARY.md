# Scope and boundary

## In scope

### Mechanics-only governance

- Proposal classification.
- Ownership routing.
- Product-semantic leak scanning.
- Package dependency guardrails.
- Neutral DTO / schema placement rules.

### Consumer conformance

Harnesses product repos can run for:

- header propagation;
- error envelope shape;
- idempotency behavior;
- outbox/artifact reference mechanics;
- observability meter naming;
- no-product-semantics;
- actor context propagation;
- mechanical schema compatibility.

### Cross-service mechanical schemas

Versioned schemas for:

- error envelope;
- correlation/header context;
- evidence reference;
- idempotency state;
- replay contract;
- actor context;
- observability meter descriptor;
- consumer conformance report.

## Out of scope

| Concern | Owner |
|---|---|
| Semantic truth, ontology acceptance, canonical facts | Kanon |
| Model/provider routing, fallback, cost policy | Conexus |
| Run lifecycle, agent execution, tool execution, human gates | Allagma |
| Source profiling, SLOD mapping candidates, medallion/lakehouse semantics | Metabole |
| Reconstructability scoring, evidence graph interpretation, memory graph | Aisthesis |
| Operator UI | frontend/UI repos |
| Runtime baseline lock authority | Allagma/system evidence |

## Reuse test

Every new Platform addition must pass:

```text
Can this be reused by Conexus, Kanon, Allagma, Metabole, and Aisthesis without importing product meaning?
```

## Routing examples

| Proposed item | Platform? | Correct owner |
|---|---:|---|
| `X-Trace-Id` propagation schema | Yes | Platform |
| Problem-details error envelope | Yes | Platform |
| Kanon semantic decision payload | No | Kanon |
| Conexus model alias routing policy | No | Conexus |
| Allagma human gate lifecycle | No | Allagma |
| Metabole SLOD candidate confidence algorithm | No | Metabole |
| Aisthesis reconstructability grade algorithm | No | Aisthesis |
| Evidence reference neutral shape | Yes | Platform |
| Aisthesis evidence graph edge semantics | No | Aisthesis |
