# Ontogony.Hosting

Shared ASP.NET service-default wiring for Ontogony platform services.

## What this is

- Compose observability + errors defaults.
- Apply stable middleware ordering.
- Optionally include service-identity body-hash preload middleware.
- Map health/readiness endpoints with configurable paths.

## What this is not

- No Athanor/Agentor/Conexus domain behavior.
- No product endpoint registration.
- No auth policy decisions.
