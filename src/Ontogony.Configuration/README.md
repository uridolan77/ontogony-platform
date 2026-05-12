# Ontogony.Configuration

Reusable **options validation** and startup guard helpers for ASP.NET and worker hosts.

## What this is

- `AddValidatedOptions<TOptions>` — bind a configuration section with data annotations and validate on start.
- `EnvironmentGuard` — fail-fast environment checks for production posture.
- `RequiredConnectionStringValidator` / `RequiredConnectionStringOptions` — reusable connection string validation.

## What this is not

- Not service-specific settings schemas or secrets management (hosts own those).
