# Hosting Service Defaults Adoption

Use `Ontogony.Hosting` to remove duplicated startup mechanics while keeping each service repository responsible for policy, endpoints, and domain behavior.

## What to centralize

Move repeated mechanical startup wiring to `AddOntogonyServiceDefaults(...)`, `UseOntogonyServiceDefaults()`, and `MapOntogonyHealthEndpoints()`:

- request tracing + correlation middleware wiring
- exception middleware wiring
- optional service-identity body-hash preload middleware wiring
- health/readiness endpoint mapping defaults

## What must stay local per service

Keep these concerns in Athanor, Agentor, Conexus, or other service repos:

- authentication/authorization policy selection
- service-specific endpoint mapping
- service-specific readiness checks (DB, broker, provider dependencies)
- exception mapping semantics and business error codes
- product/domain behavior

## Typical migration pattern

1. Replace duplicated startup mechanics with `Ontogony.Hosting` extension calls.
2. Keep existing service auth policy and endpoint mapping in place.
3. Keep service-specific health checks registered locally, then map through local endpoints as needed.
4. Verify middleware ordering and trace-id continuity on handled/unhandled failures.

## Future hardening (separate bounded PR)

For stricter startup guarantees later, add an `IValidateOptions<OntogonyServiceDefaultsOptions>` validator in a dedicated PR to enforce path/value sanity (for example non-empty health/readiness paths with leading `/`).

Do not fold that validator into unrelated behavior changes.
