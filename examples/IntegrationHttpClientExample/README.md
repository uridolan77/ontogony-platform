# IntegrationHttpClientExample

This is a documentation-only example for Ontogony.Http adoption.

Status: Documentation-only (not compiled by Ontogony.Platform.sln).

## Goal

Show how to register a named integration client with idempotency-safe retries and correlation propagation.

## Sample

```csharp
builder.Services.AddOntogonyIntegrationHttpClient(
    "Conexus",
    sp => new HttpIntegrationOptions
    {
        BaseUrl = "https://conexus.internal",
        TimeoutSeconds = 30
    });

var client = httpClientFactory.CreateClient("Conexus");
await client.PostAsync("/v1/request", content);
```

## Do Not Do This

- Do not assume unsafe retries are safe without Idempotency-Key.
- Do not send public internet traffic through trusted-internal auth assumptions.
