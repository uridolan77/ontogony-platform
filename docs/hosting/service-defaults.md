# Service Defaults (Ontogony.Hosting)

`Ontogony.Hosting` composes common ASP.NET startup mechanics without introducing domain meaning.

## Install / reference

Reference `Ontogony.Hosting` and keep service-specific logic in your service project.

## Usage

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOntogonyServiceDefaults(builder.Configuration, options =>
{
    options.ServiceName = "agentor-api";
    options.ServiceVersion = "1.2.3";
    options.UseServiceIdentityBodyHashPreload = true;
});

var app = builder.Build();

app.UseOntogonyServiceDefaults();
app.MapOntogonyHealthEndpoints();

app.MapGet("/", () => Results.Ok("ok"));

app.Run();
```

## Defaults

- `AddObservability = true`
- `AddErrors = true`
- `UseRequestTracing = true`
- `UseExceptionHandling = true`
- `UseServiceIdentityBodyHashPreload = false`
- `MapHealthEndpoints = true`
- `HealthPath = "/health"`
- `ReadinessPath = "/ready"`

## Opt-out and overrides

All components are optional and can be disabled per host:

```csharp
builder.Services.AddOntogonyServiceDefaults(builder.Configuration, options =>
{
    options.AddErrors = false;
    options.UseExceptionHandling = false;
    options.MapHealthEndpoints = false;
});
```

JSON defaults remain host-overridable:

```csharp
builder.Services.AddOntogonyServiceDefaults(builder.Configuration);
builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.WriteIndented = true);
```

## Boundary rule

This package only wires reusable mechanics. It intentionally excludes product endpoints, product health checks, and service-specific authorization policies.
