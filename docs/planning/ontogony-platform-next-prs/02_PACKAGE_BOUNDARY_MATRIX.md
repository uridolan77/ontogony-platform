# Package Boundary Matrix

| Package | Owns | Must not own |
|---|---|---|
| Ontogony.Primitives | Clock, IDs, simple generic abstractions | Domain identity meaning, business clocks |
| Ontogony.Configuration | Options validation, startup guards | Product config semantics |
| Ontogony.Contracts | Headers, envelopes, DTO-level wire contracts | Agent run meaning, canonization, business payload validation |
| Ontogony.Observability | Correlation, tracing, metrics names | Business KPIs, product dashboards as domain truth |
| Ontogony.Errors | Generic exception middleware and mapping hooks | Product exception taxonomy beyond local mappings |
| Ontogony.Http | Outbound correlation/resilience/redaction mechanics | Provider routing, integration business behavior |
| Ontogony.Hashing | Canonical JSON mechanics, SHA helpers | Athanor canonical hash policy unless explicitly migrated with vectors |
| Ontogony.Idempotency | Mechanical key construction | Business duplicate policy |
| Ontogony.Security | Current actor projection, service identity, HMAC mechanics | Product permission model, role hierarchy beyond generic roles |
| Ontogony.Messaging | Publisher abstractions, in-process reference publisher | Kafka/NATS semantics unless in provider package; domain event meaning |
| Ontogony.Persistence | Outbox contracts, provider-neutral persistence mechanics | Product repositories, domain transactions |
| Ontogony.Testing | Fixtures, conformance helpers | Service-specific test assertions |
| Ontogony.Hosting | Safe default host wiring | Product endpoints, product auth policy |
| Ontogony.ProtocolIngress | Protocol-to-envelope normalization | Interpretation of protocol events as domain decisions |

Boundary mantra:

```text
The platform may normalize, validate, correlate, hash, persist, and transport.
The products decide what the normalized facts mean.
```
