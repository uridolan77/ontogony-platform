# 05 — Extraction Roadmap From Existing Repos

## PR 1 — Observability extraction

Move/replace:

- Athanor `TraceIdMiddleware`
- Agentor `RequestTracingMiddleware`
- Agentor `AgentorCorrelationContext`
- Agentor `AgentorDiagnostics`
- Agentor correlation HTTP handler

Target:

```text
Ontogony.Observability
```

Acceptance:

- Athanor, Agentor, and a sample service all emit/echo `X-Ontogony-Trace-Id`.
- Legacy trace headers are accepted during migration.
- Outgoing HTTP calls receive trace/actor/project headers.

## PR 2 — Error contract extraction

Move/replace:

- Athanor error middleware shape
- Agentor exception middleware shape

Target:

```text
Ontogony.Errors
```

Acceptance:

- Same `ApiError` shape in Athanor and Agentor.
- Service-specific exception mappings remain in service repos.

## PR 3 — Hashing/idempotency extraction

Move/replace/inspire:

- Athanor SHA-256 content hash
- Agentor canonical JSON fingerprinting
- KGB chunk-hash lesson

Target:

```text
Ontogony.Hashing
Ontogony.Idempotency
```

Acceptance:

- Equivalent JSON objects produce same hash.
- Agentor run fingerprints can use shared canonical JSON.
- Athanor event payload hashes can use shared payload hasher.

## PR 4 — HTTP integration infrastructure

Move/inspire:

- Agentor named integration clients
- fake/http/disabled mode
- resilience handler
- correlation handler

Target:

```text
Ontogony.Http
```

Acceptance:

- Agentor integration clients are simpler.
- Future Athanor/Conexus adapters can reuse the same pattern.

## PR 5 — Messaging/outbox abstractions

Move/inspire:

- Agentor outbox and durable queue interfaces
- Athanor projection outbox worker discipline

Target:

```text
Ontogony.Messaging
```

Acceptance:

- Initial in-memory publisher for tests.
- Postgres outbox implementation can be added without changing service code.
