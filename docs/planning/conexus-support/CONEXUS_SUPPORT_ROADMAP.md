# Conexus support roadmap

## Immediate

1. Keep the platform baseline stable while Conexus validates the starter.
2. Add only missing mechanics that block Conexus PRs.
3. Preserve the package graph and no-product-semantics rule.

## Likely first platform PRs

1. `PLAT-CX-PR-0001` — consumer package/reference validation.
2. `PLAT-CX-PR-0002` — generic secret value resolver abstraction.
3. `PLAT-CX-PR-0003` — redaction/captured logging test fixtures.

## Later platform PRs

- Quota reservation/refund if token accounting requires it.
- Idempotency result metadata if duplicate responses need stable references.
- Durable persistence adapters when Conexus leaves single-node in-memory operation.
- Package publishing/internal feed when Conexus stops using sibling project references.

## Explicit non-goals

- No Conexus provider routing in platform.
- No model preference logic in platform.
- No price catalog in platform.
- No content policy in platform.
- No Agentor orchestration or Microsoft Agent Framework runtime in platform.
