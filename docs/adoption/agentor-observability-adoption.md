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
// If you also use Ontogony.Errors, call UseOntogonyExceptionHandling after tracing — see observability-error-ordering.md.
// Exception middleware that reads trace response headers must run after UseOntogonyRequestTracing.
```

## CI and sibling project references

Local `ProjectReference` paths to this repo are a dev convenience only. For CI and release, prefer multi-checkout or an internal NuGet feed; see [local-repo-layout-and-ci.md](./local-repo-layout-and-ci.md) and [private-nuget-feed.md](./private-nuget-feed.md).

## Compatibility Notes

- `OntogonyCorrelationContext.FromHeaders` accepts `X-Agentor-Trace-Id` (`OntogonyEventHeaders.LegacyAgentorTraceId`) when `X-Ontogony-Trace-Id` is absent, so older clients keep correlating.
- Prefer emitting `X-Ontogony-Trace-Id` and W3C `traceparent` / `tracestate` as canonical correlation surfaces on new integrations.
- **Response legacy aliases** (`X-Agentor-Trace-Id`, etc.) are off by default (`EchoLegacyHeaders = false`). Set `EchoLegacyHeaders = true` only while external callers still require those response headers.

## Verification Checklist

1. Incoming requests without trace headers receive a generated trace id.
2. Incoming X-Agentor-Trace-Id is accepted when Ontogony header is missing.
3. Outbound named HttpClient calls propagate trace and correlation headers.
4. Error payloads and logs contain the same trace id.

## Do Not Do This

- Do not move Agentor domain event semantics into Ontogony.Platform.
- Do not replace Agentor-specific event naming rules inside shared packages.
- Do not trust header actor mode on public routes.
