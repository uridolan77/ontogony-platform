# Infrastructure backlog for Conexus support

These are candidate platform improvements discovered from the Conexus.NET gateway plan. They should be implemented only when the Conexus PR genuinely needs them.

| Platform PR | Title | Priority | Why it may be needed |
| --- | --- | --- | --- |
| PLAT-CX-PR-0001 | Consumer package/reference validation | High | Prevent Conexus from drifting from platform baseline |
| PLAT-CX-PR-0002 | Generic secret value resolver abstraction | High | Conexus needs provider keys without logging raw secrets |
| PLAT-CX-PR-0003 | Redaction test fixture and captured logger helpers | High | Conexus needs no-secret/no-prompt log tests |
| PLAT-CX-PR-0004 | Quota reservation/refund extension | Medium | Token-based final usage may differ from preflight estimate |
| PLAT-CX-PR-0005 | Idempotency result metadata extension | Medium | Gateway may need deterministic duplicate response references |
| PLAT-CX-PR-0006 | AI telemetry sink abstraction | Medium | Multiple services may record LLM telemetry similarly |
| PLAT-CX-PR-0007 | Execution journal convenience writer | Low/Medium | Avoid repeated append boilerplate without adding workflow semantics |
| PLAT-CX-PR-0008 | Artifact classification/redaction helpers | Medium | Raw provider payload capture needs safe artifact metadata |
| PLAT-CX-PR-0009 | ASP.NET consumer test host fixtures | Medium | Conexus API tests can reuse platform service-default wiring |
| PLAT-CX-PR-0010 | Durable quotas/idempotency persistence adapters | Later | Needed for multi-node production |
| PLAT-CX-PR-0011 | Package publishing / internal NuGet workflow | Later | Move from sibling project references after alpha |
| PLAT-CX-PR-0012 | Standard AI metrics names | Later | Useful once gateway metrics stabilize |

## Important

Do not add provider routing, model aliases, pricing, fallback, OpenAI compatibility, or provider choice semantics to the platform.
