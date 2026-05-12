# PR26 — Ontogony.Hosting Service Defaults

## Goal

Create a new `Ontogony.Hosting` package that makes it easy to wire a safe, consistent ASP.NET host without turning the platform into a domain framework.

## Why now

After PR25, the core mechanics exist and are hardened enough. The next problem is repeated startup wiring across services: observability, errors, security preload, JSON, health, readiness, config validation, and middleware order.

## Scope

Add package:

```text
src/Ontogony.Hosting/Ontogony.Hosting.csproj
```

Add APIs:

```csharp
services.AddOntogonyServiceDefaults(configuration, options => { ... });
app.UseOntogonyServiceDefaults();
app.MapOntogonyHealthEndpoints();
```

Suggested options:

```csharp
public sealed class OntogonyServiceDefaultsOptions
{
    public string ServiceName { get; set; }
    public string ServiceVersion { get; set; }
    public bool AddObservability { get; set; } = true;
    public bool AddErrors { get; set; } = true;
    public bool UseRequestTracing { get; set; } = true;
    public bool UseExceptionHandling { get; set; } = true;
    public bool UseServiceIdentityBodyHashPreload { get; set; } = false;
    public bool MapHealthEndpoints { get; set; } = true;
    public string HealthPath { get; set; } = "/health";
    public string ReadinessPath { get; set; } = "/ready";
}
```

## Must not do

- Do not add Athanor/Agentor/Conexus-specific startup logic.
- Do not add product endpoints.
- Do not impose a single authentication policy.
- Do not hide underlying package APIs.

## Files

- `src/Ontogony.Hosting/`
- `tests/Ontogony.Infrastructure.Tests/OntogonyHostingDefaultsTests.cs` or new `tests/Ontogony.Hosting.Tests/`
- `docs/packages/Ontogony.Hosting.md`
- `docs/hosting/service-defaults.md`
- `examples/MinimalApiWithOntogonyHosting/`
- `CHANGELOG.md`
- `Ontogony.Platform.sln`

## Tests

- Service defaults register observability/errors by default.
- Middleware order produces trace ID in error payload.
- HMAC preload middleware is added only when opted in.
- Health/readiness endpoints work.
- Options can disable individual components.
- JSON options remain host-overridable.

## Acceptance

- `dotnet restore Ontogony.Platform.sln`
- `dotnet build Ontogony.Platform.sln --no-restore`
- `dotnet test Ontogony.Platform.sln --no-build`
- `PACKAGE_VERSION=0.2.0-local ./scripts/pack-all.ps1`
- Docs explain what defaults do and how to opt out.
