# Service-to-Service Integration Adoption

Mechanical conventions for outbound HTTP calls between Ontogony services.

## Header conventions

Use `OntogonyIntegrationHeaders` from `Ontogony.Http`:

| Header | Purpose |
|--------|---------|
| `X-Ontogony-Correlation-Id` | Cross-service correlation (mirrors trace id when propagated from context) |
| `X-Ontogony-Actor-Id` | Actor identifier |
| `X-Ontogony-Actor-Type` | Opaque actor classifier |
| `X-Ontogony-Actor-Roles` | Comma-separated roles |
| `X-Ontogony-Tenant-Id` | Tenant scope |
| `X-Ontogony-Workspace-Id` | Workspace scope |
| `X-Ontogony-Idempotency-Key` | Idempotency key for unsafe-method retries |

Legacy `Idempotency-Key` and `X-Ontogony-Roles` are still recognized for retry and inbound interop.

## Registration

Named client:

```csharp
builder.Services.AddOntogonyIntegrationHttpClient(
    "downstream",
    sp => new HttpIntegrationOptions
    {
        BaseUrl = builder.Configuration["Integrations:Downstream:BaseUrl"]!,
    });
```

Typed client:

```csharp
builder.Services.AddOntogonyIntegrationHttpClient<IMyDownstreamApi, MyDownstreamApi>(
    "downstream",
    sp => new HttpIntegrationOptions { BaseUrl = "https://downstream.internal" });
```

When using ASP.NET actor context, also register:

```csharp
builder.Services.AddOntogonyOutboundActorPropagation();
```

## Idempotency

Set `OntogonyIntegrationContext` before an outbound unsafe call when a key is known:

```csharp
using (OntogonyIntegrationContext.Push(new IntegrationOutboundState(IdempotencyKey: key)))
{
    await client.PostAsync(...);
}
```

Or supply `X-Ontogony-Idempotency-Key` explicitly on the request.

## Error mapping

Use `IntegrationHttpError.ThrowIfUnsuccessfulAsync` for mechanical HTTP failures (redacted body, trace id suffix). Map to product errors in the calling service.

## Do not

- Add `KanonClient`, `AgentorClient`, or `ConexusClient` to Ontogony.Platform.
- Import another product service's implementation assembly.
