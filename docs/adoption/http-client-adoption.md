# HTTP Client Adoption

This guide shows how to adopt Ontogony.Http integration clients safely.

## Scope

Adopt shared mechanics:

- resilient transport policies
- idempotency-aware unsafe retries
- correlation header propagation

## Wiring Pattern

```csharp
builder.Services.AddOntogonyIntegrationHttpClient(
    "Athanor",
    sp => new HttpIntegrationOptions
    {
        BaseUrl = "https://athanor.internal",
        TimeoutSeconds = 30,
        DefaultHeaders =
        {
            ["User-Agent"] = "agentor-api"
        }
    });
```

## Safety Defaults

- GET/HEAD/OPTIONS: retry by default
- POST/PUT/PATCH/DELETE: retry only when Idempotency-Key is present (default)
- multipart/streaming/oversized: no retry by shared handler

## Verification Checklist

1. Unsafe methods without Idempotency-Key do not retry.
2. Unsafe methods with Idempotency-Key do retry on retryable status codes.
3. Circuit opens only on retryable failures (default configuration).
4. Correlation headers flow on outbound calls.

## Do Not Do This

- Do not disable idempotency safety globally without strong justification.
- Do not retry unsafe methods with unknown body replayability.
