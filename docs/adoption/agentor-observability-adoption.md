# Agentor Observability Adoption

This guide describes adopting Ontogony.Observability and Ontogony.Http in Agentor while preserving Agentor-specific semantics.

## Scope

Adopt shared mechanics:

- request tracing middleware
- correlation header propagation for outbound calls
- span tags and request metrics

Keep Agentor-owned behavior:

- run/plan/tool semantic event meaning
- Agentor-specific trace interpretations

## Wiring Pattern

```csharp
builder.Services.AddOntogonyObservability(options =>
{
    options.ServiceName = "Agentor.Api";
    options.ServiceVersion = "0.1.0";
});

builder.Services.AddOntogonyIntegrationHttpClient(
    "Athanor",
    sp => new HttpIntegrationOptions
    {
        BaseUrl = builder.Configuration["Integrations:Athanor:BaseUrl"]!,
        TimeoutSeconds = 30
    });

var app = builder.Build();
app.UseOntogonyRequestTracing();
```

## Compatibility Notes

- Keep accepting and optionally emitting X-Agentor-Trace-Id for backward compatibility where required.
- Prefer X-Ontogony-Trace-Id and W3C traceparent/tracestate as canonical correlation surfaces.

## Verification Checklist

1. Incoming requests without trace headers receive a generated trace id.
2. Incoming X-Agentor-Trace-Id is accepted when Ontogony header is missing.
3. Outbound named HttpClient calls propagate trace and correlation headers.
4. Error payloads and logs contain the same trace id.

## Do Not Do This

- Do not move Agentor domain event semantics into Ontogony.Platform.
- Do not replace Agentor-specific event naming rules inside shared packages.
- Do not trust header actor mode on public routes.
