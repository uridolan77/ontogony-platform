# Adoption Path — Integrating Ontogony into Your Service

This guide walks you through adopting Ontogony platform mechanics step-by-step.

---

## Overview

Adopting Ontogony means your service will:

✅ Propagate trace IDs across requests (distributed tracing)  
✅ Return canonical error JSON with secure shapes  
✅ Publish events as `OntogonyEnvelope` with payload hashing  
✅ Call other services with resilient HTTP (retry, circuit-breaker)  
✅ Sign service-to-service requests with HMAC  
✅ Verify all mechanics with conformance tests  

---

## Phase 1: Foundation (1-2 hours)

### 1. Install Base Packages

```bash
cd your-service
dotnet add package Ontogony.Observability
dotnet add package Ontogony.Errors
dotnet add package Ontogony.Contracts
```

### 2. Wire Observability Middleware

In `Program.cs`:

```csharp
// Services
services.Configure<OntogonyObservabilityOptions>(options =>
{
    options.ServiceName = "my-service";
    options.TraceHeaderName = "X-Ontogony-Trace-Id";  // Canonical name
    options.AllowAliasingLegacyTraceHeader = true;     // Support migration
});

// Middleware (early in pipeline)
var app = builder.Build();

app.UseRequestTracingMiddleware();
app.UseOntogonyExceptionHandling();

// ... rest of middleware and endpoints
app.Run();
```

### 3. Test Trace Propagation

```csharp
var client = new HttpClient();
client.DefaultRequestHeaders.Add("X-Ontogony-Trace-Id", "trace-abc123");

var response = await client.GetAsync("https://localhost:5001/api/test");

// Verify trace ID is echoed in response header
var traceIdInResponse = response.Headers.GetValues("X-Ontogony-Trace-Id");
Assert.Contains("trace-abc123", traceIdInResponse);
```

---

## Phase 2: HTTP Resilience (1-2 hours)

### 1. Install Package

```bash
dotnet add package Ontogony.Http
```

### 2. Register Resilient HTTP Client

For **each external service** you call:

```csharp
services.AddIntegrationHttpClient<IMyServiceClient>(client =>
{
    client.BaseAddress = new Uri(configuration["ExternalService:BaseUrl"]);
})
.ConfigureHttpClientDefaults(opts =>
{
    opts.Timeout = TimeSpan.FromSeconds(10);
    opts.MaxAttempts = 3;
    opts.OpenCircuitAfterConsecutiveFailures = 5;
    opts.CircuitBreakerResetTimeout = TimeSpan.FromSeconds(30);
});
```

### 3. Implement Client

```csharp
public class MyServiceClient : IMyServiceClient
{
    private readonly HttpClient _http;

    public MyServiceClient(HttpClient client)
    {
        _http = client;
    }

    public async Task<MyResponse> GetAsync(string id)
    {
        var response = await _http.GetAsync($"/api/items/{id}");
        return await response.Content.ReadAsAsync<MyResponse>();
    }
}
```

### 4. Test Resilience

```csharp
[Fact]
public async Task CallsRetryOnTransient()
{
    var stub = new StubHttpMessageHandler()
        .ReplyWith(HttpStatusCode.ServiceUnavailable, 2)  // Fail twice
        .ReplyWith(HttpStatusCode.OK, "{}");               // Then succeed

    var client = new HttpClient(stub);
    // Wire resilience
    var handler = new ResilientIntegrationDelegatingHandler(
        stub, 
        options: new() { MaxAttempts = 3 }, 
        clock: clock
    );

    // Verify retries work
    var result = await handler.SendAsync(new HttpRequestMessage(...));
    Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    Assert.Equal(3, stub.RequestCount); // 2 failures + 1 success
}
```

---

## Phase 3: Error Handling (1 hour)

### 1. Map Domain Exceptions

```csharp
services.Configure<OntogonyExceptionMappingOptions>(opts =>
{
    opts.MapException<ValidationException>(
        statusCode: StatusCodes.Status400BadRequest,
        code: "validation_error"
    );
    
    opts.MapException<ItemNotFoundException>(
        statusCode: StatusCodes.Status404NotFound,
        code: "not_found"
    );
    
    opts.MapException<OperationFailedException>(
        statusCode: StatusCodes.Status500InternalServerError,
        code: "operation_failed"
    );
});
```

### 2. Verify Error Shape

Unmapped exceptions now return:

```json
{
  "type": "about:blank",
  "title": "Internal Server Error",
  "status": 500,
  "traceId": "trace-abc123",
  "detail": null
}
```

No internal exception message. No stack trace. Safe to send to clients.

### 3. Test Error Handling

```csharp
[Fact]
public async Task UnmappedExceptionProduces500()
{
    await ErrorShapeConformanceAssertions.AssertUnmappedExceptionProduces500Async();
}

[Fact]
public async Task MappedExceptionProducesExpectedStatus()
{
    await ErrorShapeConformanceAssertions.AssertMappedExceptionProducesExpectedShapeAsync<ValidationException>(
        exception: new ValidationException("Invalid input"),
        expectedStatus: StatusCodes.Status400BadRequest,
        expectedCode: "validation_error"
    );
}
```

---

## Phase 4: Events & Messaging (1-2 hours)

### 1. Install Package

```bash
dotnet add package Ontogony.Messaging
```

### 2. Publish Canonical Events

```csharp
public class OrderService
{
    private readonly IEventPublisher<OrderCreatedEvent> _publisher;

    public async Task CreateOrderAsync(CreateOrderCommand cmd)
    {
        // Domain logic
        var order = new Order(cmd.CustomerId, cmd.Items);

        // Publish as canonical envelope
        await _publisher.PublishAsync(new OntogonyEnvelope<OrderCreatedEvent>
        {
            EventId = Guid.NewGuid().ToString("n"),
            EventType = "ontogony.order.created",
            Source = "ontogony://order-service/domain",
            OccurredAt = DateTimeOffset.UtcNow,
            TraceId = OntogonyCorrelationContext.TraceId,
            Protocol = ProtocolNames.Internal,
            Payload = new OrderCreatedEvent(order.Id, order.Total)
        });
    }
}
```

### 3. Consume & Verify Events

```csharp
public class NotificationHandler : IEventSubscriber<OrderCreatedEvent>
{
    public async Task HandleAsync(OntogonyEnvelope<OrderCreatedEvent> envelope)
    {
        // envelope.TraceId is already set and propagated
        // envelope.Payload is validated
        
        var traceId = envelope.TraceId;
        var customerId = envelope.Payload.CustomerId;

        await _notificationService.SendOrderConfirmation(customerId, traceId);
    }
}
```

---

## Phase 5: Security & Signing (1-2 hours)

### 1. Install Package

```bash
dotnet add package Ontogony.Security
```

### 2. Configure Service Identity

```csharp
services.Configure<OntogonySecurityOptions>(opts =>
{
    opts.ServiceIdentity = "ontogony://order-service";
    opts.SharedSecret = configuration["Ontogony:SharedSecret"] 
        ?? throw new InvalidOperationException("Missing shared secret");
    opts.ClockSkew = TimeSpan.FromSeconds(30);
});
```

### 3. Sign Outbound Requests

```csharp
public class SecureServiceClient
{
    private readonly HttpClient _http;
    private readonly OntogonySecurityOptions _security;
    private readonly IClock _clock;

    public async Task<T> PostAsync<T>(string path, object payload)
    {
        var bodyJson = JsonSerializer.Serialize(payload);
        var bodyHash = _hasher.ComputeHash(bodyJson);

        // Create HMAC signature
        var signature = ServiceIdentityHmacSignatureHelper.SignRequest(
            secret: _security.SharedSecret,
            method: "POST",
            path: path,
            timestamp: _clock.UtcNow,
            nonce: Guid.NewGuid().ToString("n"),
            bodyHash: bodyHash
        );

        var request = new HttpRequestMessage(HttpMethod.Post, path)
        {
            Content = new StringContent(bodyJson, Encoding.UTF8, "application/json")
        };

        request.Headers.Add("X-Ontogony-Signature", signature.Signature);
        request.Headers.Add("X-Ontogony-Timestamp", signature.Timestamp.ToUniversalTime().ToString("O"));
        request.Headers.Add("X-Ontogony-Nonce", signature.Nonce);
        request.Headers.Add("X-Ontogony-Body-Hash", signature.BodyHash);

        var response = await _http.SendAsync(request);
        // ...
    }
}
```

### 4. Validate Incoming Signatures

```csharp
app.UseOntogonySignatureValidation(); // Middleware

// Or manually in handlers
public class SecureEventHandler
{
    public async Task HandleSignedRequest(HttpContext context)
    {
        var isValid = await _validator.ValidateRequestSignatureAsync(context);
        if (!isValid)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var actor = context.User.FindFirst("sub")?.Value;
        // Process authenticated request
    }
}
```

---

## Phase 6: Conformance Testing (1-2 hours)

### 1. Install Testing Package

```bash
dotnet add package Ontogony.Testing --scope=test
```

### 2. Create Conformance Test Suite

```csharp
public class OntogonyConformanceTests
{
    [Fact]
    public async Task Service_EchoesTraceHeaders()
    {
        // Arrange
        var traceId = "trace-abc123";
        var host = new WebApplicationFactory<Program>();
        var client = host.CreateClient();

        // Act
        client.DefaultRequestHeaders.Add("X-Ontogony-Trace-Id", traceId);
        var response = await client.GetAsync("/health");

        // Assert
        Assert.True(response.Headers.TryGetValues("X-Ontogony-Trace-Id", out var values));
        Assert.Contains(traceId, values);
    }

    [Fact]
    public async Task Service_Returns_CanonicalErrorShape()
    {
        await ErrorShapeConformanceAssertions.AssertUnmappedExceptionProduces500Async();
    }

    [Fact]
    public void PublishedEvents_AreCanonical()
    {
        // Arrange
        var envelope = new OntogonyEnvelope<MyEvent>
        {
            EventId = Guid.NewGuid().ToString("n"),
            EventType = "ontogony.myservice.operation.completed",
            Source = "ontogony://myservice/domain",
            OccurredAt = DateTimeOffset.UtcNow,
            TraceId = "trace-123",
            Protocol = ProtocolNames.Internal,
            Payload = new MyEvent()
        };

        // Assert
        EnvelopeConformanceAssertions.AssertFullConformance(envelope);
    }

    [Fact]
    public async Task HttpClient_RetriesTransientFailures()
    {
        var stub = new StubHttpMessageHandler()
            .ReplyWith(HttpStatusCode.ServiceUnavailable)
            .ReplyWith(HttpStatusCode.OK, "{\"result\": \"success\"}");

        await HttpResilienceConformanceHarness.AssertRetriesOnTransientFailureAsync(
            client: new HttpClient(stub),
            stub: stub,
            expectedTotalAttempts: 2
        );
    }
}
```

### 3. Run Tests

```bash
dotnet test

# Expected: All conformance tests pass
# If any fail, review the assertion message and fix your configuration
```

---

## Phase 7: Verification Checklist

- [ ] **Observability**
  - [ ] Middleware registered in correct order
  - [ ] `OntogonyCorrelationContext.TraceId` is populated in handlers
  - [ ] Trace ID echoed in response headers
  - [ ] Trace ID propagated to outbound HTTP calls

- [ ] **Error Handling**
  - [ ] Unmapped exceptions return 500 with canonical shape
  - [ ] Mapped exceptions return correct status and code
  - [ ] No internal exception details leak
  - [ ] Error JSON includes trace ID

- [ ] **HTTP Resilience**
  - [ ] Transient failures (5xx, timeouts) are retried
  - [ ] Retries use exponential backoff with jitter
  - [ ] Circuit breaker opens after threshold
  - [ ] Metrics recorded for all calls

- [ ] **Events**
  - [ ] Events published as `OntogonyEnvelope`
  - [ ] Event type follows `ontogony.*` convention
  - [ ] Source follows `ontogony://service/domain` scheme
  - [ ] Payload hash computed and validated

- [ ] **Security**
  - [ ] Service identity configured
  - [ ] Shared secret stored securely
  - [ ] Outbound requests signed with HMAC
  - [ ] Incoming signatures validated

- [ ] **Testing**
  - [ ] Conformance tests all pass
  - [ ] Can prove adoption to Ontogony reviewers
  - [ ] Integration tests cover happy and error paths

---

## Common Issues

### "Trace ID not propagated to outbound HTTP"

**Symptom:** Calling another service, but trace ID is not included.

**Fix:** Ensure `IntegrationHttpClient` is registered and used (not bare `HttpClient`).

```csharp
// ✅ Good
services.AddIntegrationHttpClient<IMyServiceClient>(...);

// ❌ Bad
services.AddScoped(sp => new HttpClient());
```

### "Conformance tests fail with 'middleware not registered'"

**Symptom:** Conformance test throws `InvalidOperationException("RequestTracingMiddleware not found")`.

**Fix:** Ensure middleware is registered **before** accessing correlation context.

```csharp
// ✅ Correct order
app.UseRequestTracingMiddleware();  // Must be early
app.UseRouting();
app.MapEndpoints();

// ❌ Wrong order
app.UseRouting();
app.UseRequestTracingMiddleware();  // Too late
```

### "Signature validation always fails"

**Symptom:** Incoming HMAC signatures never validate.

**Fix:** Ensure shared secret matches and timestamp is not skewed.

```csharp
// Check configuration
var opts = serviceProvider.GetRequiredService<IOptions<OntogonySecurityOptions>>();
var secret = opts.Value.SharedSecret;  // Must match sender's secret

// Check clock
var clock = serviceProvider.GetRequiredService<IClock>();
var now = clock.UtcNow;  // Must be reasonably close to request timestamp
```

---

## Next Steps

1. ✅ **Complete all phases 1-7** in your service
2. ✅ **Run conformance tests** and ensure all pass
3. ✅ **Submit PR** with adoption evidence (code review will verify)
4. ✅ **Link to this adoption path** in your PR description
5. ✅ **Share learnings** in platform issues if you hit snags

---

## Support

- **Integration questions:** Create issue in `ontogony-platform` with label `adoption-support`
- **Conformance test failures:** See [Conformance Testing](./conformance-testing.md)
- **Design review:** Request in PR for platform maintainers to review

---

**Last Updated:** May 2026  
**Version:** 0.2.0
