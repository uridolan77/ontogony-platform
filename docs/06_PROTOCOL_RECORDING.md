# 06 — Protocol Recording Strategy

Ontogony Platform is the mechanical base for Athanor's future protocol recorder.

Record boundaries, not only protocols:

| Boundary | Protocol/source | Why record it |
| --- | --- | --- |
| agent ↔ user/UI | AG-UI | what user saw, approved, interrupted |
| agent ↔ tool/data | MCP | what tools/resources were invoked |
| agent ↔ agent | A2A | delegation, hidden agent influence |
| service ↔ service | OpenTelemetry/HTTP | execution trace and latency/errors |
| model gateway | Conexus/OpenAI/Anthropic | model, provider, tokens, cost, errors |
| app ↔ browser | Playwright/CDP/WebDriver BiDi | computer-use proof |
| enterprise events | CloudEvents/AsyncAPI | emitted business facts |
| data lineage | OpenLineage | data pipeline provenance |

## Initial adapter order

```text
1. AG-UI recorder
2. MCP recorder
3. Conexus LLM event recorder
4. Agentor run/step/tool event recorder
5. A2A recorder
6. OpenTelemetry ingestion
7. CloudEvents ingestion
```

## Why this platform matters

The recorder only works if all services agree on:

- trace ID
- event envelope
- actor/project/tenant context
- payload hashes
- error shape
- HTTP propagation

That agreement is what this repo provides.
