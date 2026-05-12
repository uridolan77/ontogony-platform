# Examples & Quickstarts

This guide includes code examples for common Ontogony integration patterns.

---

## Complete Service Example

Full working example of a service adopting Ontogony.

### Project Structure

```
OrderService/
├── Program.cs
├── OrderService.csproj
├── Services/
│   └── OrderService.cs
├── Controllers/
│   └── OrdersController.cs
├── Events/
│   └── OrderCreatedEvent.cs
└── Tests/
    └── OrderServiceConformanceTests.cs
```

### Program.cs

```csharp
using Ontogony.Configuration;
using Ontogony.Contracts.Events;
using Ontogony.Errors;
using Ontogony.Http;
using Ontogony.Messaging;
using Ontogony.Observability;
using Ontogony.Persistence.Postgres;
using Ontogony.Security;

var builder = WebApplication.CreateBuilder(args);

// Configure Ontogony observability
builder.Services.Configure<OntogonyObservabilityOptions>(opts =>
{
    opts.ServiceName = "order-service";
    opts.TraceHeaderName = "X-Ontogony-Trace-Id";
    // AcceptedIncomingTraceHeaders already includes legacy Athanor/Agentor headers
});

// Configure error handling
builder.Services.AddOntogonyErrors(opts =>
{
    opts.Map<ArgumentException>(HttpStatusCode.BadRequest, "invalid_argument");
    opts.Map<InvalidOperationException>(HttpStatusCode.Conflict, "conflict");
});

// Configure outbox persistence (no EF Core OrderDbContext here — that's product-specific)
builder.Services.AddOntogonyPostgresOutbox(opts =>
{
    opts.ConnectionString = builder.Configuration.GetConnectionString("Outbox")!;
    opts.EnsureSchemaOnStartup = true;
});

// Configure resilient HTTP client for payment service
builder.Services.AddOntogonyIntegrationHttpClient(
    "payment-service",
    sp => new HttpIntegrationOptions
    {
        BaseUrl = builder.Configuration["PaymentService:Url"],
        TimeoutSeconds = 10
    });
builder.Services.Configure<TransportResilienceOptions>(opts =>
{
    opts.MaxRetries = 3;
    opts.CircuitFailureThreshold = 5;
});

// Configure security — sign outbound, validate inbound
builder.Services.AddOntogonyServiceIdentityActorContext(opts =>
{
    opts.RequireHmacSignature = true;
    opts.ServiceSecrets["agentor"] = builder.Configuration["Ontogony:AgentorSecret"]!;
});
builder.Services.AddHttpClient("downstream")
    .AddHttpMessageHandler(sp => new OntogonyServiceIdentitySigningHandler(
        serviceId: "order-service",
        secret: builder.Configuration["Ontogony:SharedSecret"]!));

// Register application services
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentServiceClient, PaymentServiceClient>();

var app = builder.Build();

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    db.Database.Migrate();
}

// Middleware order matters!
app.UseOntogonyRequestTracing();          // Must be early
app.UseOntogonyExceptionHandling();       // Must be early
app.UseRouting();
app.MapControllers();

app.Run();
```

### OrderService.cs

```csharp
public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(CreateOrderCommand cmd, CancellationToken ct);
    Task<OrderDto> GetOrderAsync(string orderId, CancellationToken ct);
}

public class OrderService : IOrderService
{
    private readonly OrderDbContext _db;
    private readonly IEventPublisher _publisher;
    private readonly IPaymentServiceClient _payment;
    private readonly IClock _clock;

    public OrderService(
        OrderDbContext db,
        IEventPublisher publisher,
        IPaymentServiceClient payment,
        IClock clock)
    {
        _db = db;
        _publisher = publisher;
        _payment = payment;
        _clock = clock;
    }

    public async Task<OrderDto> CreateOrderAsync(CreateOrderCommand cmd, CancellationToken ct)
    {
        // Create order entity
        var order = new Order
        {
            Id = Guid.NewGuid().ToString("n"),
            CustomerId = cmd.CustomerId,
            Items = cmd.Items,
            Total = cmd.Items.Sum(i => i.Price * i.Quantity),
            CreatedAt = _clock.UtcNow
        };

        // Process payment (with resilience)
        var payment = await _payment.CaptureAsync(
            new PaymentRequest
            {
                OrderId = order.Id,
                Amount = order.Total,
                CustomerId = order.CustomerId
            },
            ct
        );

        if (!payment.Success)
            throw new InvalidOperationException($"Payment failed: {payment.Reason}");

        // Save to database
        _db.Orders.Add(order);
        await _db.SaveChangesAsync(ct);

        // Publish canonical event
        await _publisher.PublishAsync(
            new OntogonyEnvelope<OrderCreatedEvent>
            {
                EventId = Guid.NewGuid().ToString("n"),
                EventType = "ontogony.order.created",
                Source = "ontogony://order-service/domain",
                OccurredAt = _clock.UtcNow,
                TraceId = OntogonyCorrelationContext.TraceId,
                Protocol = ProtocolNames.Internal,
                Payload = new OrderCreatedEvent(order.Id, order.CustomerId, order.Total)
            },
            ct
        );

        return new OrderDto(order.Id, order.CustomerId, order.Total);
    }

    public async Task<OrderDto> GetOrderAsync(string orderId, CancellationToken ct)
    {
        var order = await _db.Orders
            .FirstOrDefaultAsync(o => o.Id == orderId, ct)
            ?? throw new InvalidOperationException($"Order not found: {orderId}");

        return new OrderDto(order.Id, order.CustomerId, order.Total);
    }
}
```

### OrdersController.cs

```csharp
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _service;

    public OrdersController(IOrderService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateOrderCommand cmd,
        CancellationToken ct)
    {
        var order = await _service.CreateOrderAsync(cmd, ct);
        return Created($"/api/orders/{order.Id}", order);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrder(
        string orderId,
        CancellationToken ct)
    {
        var order = await _service.GetOrderAsync(orderId, ct);
        return Ok(order);
    }

    [HttpGet("health")]
    public IActionResult Health() => Ok(new { status = "healthy" });
}
```

### Events/OrderCreatedEvent.cs

```csharp
public record OrderCreatedEvent(string OrderId, string CustomerId, decimal Total);
```

### Tests/OrderServiceConformanceTests.cs

```csharp
public class OrderServiceConformanceTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public OrderServiceConformanceTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Service_EchoesTraceHeaders()
    {
        var client = _factory.CreateClient();
        var traceId = "trace-abc123";

        client.DefaultRequestHeaders.Add("X-Ontogony-Trace-Id", traceId);
        var response = await client.GetAsync("/api/orders/health");

        Assert.True(response.Headers.TryGetValues("X-Ontogony-Trace-Id", out var values));
        Assert.Contains(traceId, values);
    }

    [Fact]
    public async Task Service_Returns_CanonicalErrorShape()
    {
        var client = _factory.CreateClient();

        // Request with missing required field
        var response = await client.PostAsJsonAsync("/api/orders", new { });

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

        var content = await response.Content.ReadAsAsync<ErrorResponse>();
        Assert.NotNull(content.TraceId);
        Assert.NotNull(content.Title);
        Assert.Null(content.Detail); // No leaks
    }

    [Fact]
    public async Task PaymentClient_RetriesTransients()
    {
        var client = _factory.CreateClient();

        // This is an integration test; depends on test data setup
        // In reality, you'd mock the payment service to simulate failures
    }
}
```

---

## Trace Propagation Example

### Client → Service A → Service B

```csharp
// Client sends request to Service A
var client = new HttpClient();
var traceId = Guid.NewGuid().ToString("n");
client.DefaultRequestHeaders.Add("X-Ontogony-Trace-Id", traceId);

var response = await client.GetAsync("https://service-a/api/process");

// Service A receives request, echoes trace ID in response
// Service A calls Service B with same trace ID propagated

// Service B receives request with trace ID already set
// All events published by both services carry same trace ID
```

### Flow

```
Client
  ↓ (X-Ontogony-Trace-Id: trace-abc123)
Service A
  ├─ Middleware extracts trace ID
  ├─ OntogonyCorrelationContext.TraceId = "trace-abc123"
  ├─ Response header echoes trace ID
  ├─ Calls Service B (IntegrationHttpClient propagates trace ID)
  │   ↓ (X-Ontogony-Trace-Id: trace-abc123)
  │ Service B
  │   ├─ Middleware extracts trace ID
  │   ├─ OntogonyCorrelationContext.TraceId = "trace-abc123"
  │   ├─ Publishes event (event.TraceId = "trace-abc123")
  │   └─ Responds with trace ID
  └─ Publishes event (event.TraceId = "trace-abc123")
```

---

## Event Publishing Example

```csharp
// Domain event
public record UserRegisteredEvent(string UserId, string Email);

// Publish as canonical envelope
await eventPublisher.PublishAsync(
    new OntogonyEnvelope<UserRegisteredEvent>
    {
        EventId = Guid.NewGuid().ToString("n"),
        EventType = "ontogony.user.registered",
        Source = "ontogony://user-service/domain",
        OccurredAt = clock.UtcNow,
        TraceId = OntogonyCorrelationContext.TraceId,
                Protocol = ProtocolNames.GenericJson,
        Payload = new UserRegisteredEvent(userId, email)
    }
);

// Consume in another service
public class SendWelcomeEmailHandler : IEventHandler<UserRegisteredEvent>
{
    public async Task HandleAsync(OntogonyEnvelope<UserRegisteredEvent> envelope, CancellationToken cancellationToken = default)
    {
        var user = envelope.Payload;
        var traceId = envelope.TraceId;  // Same trace ID!

        await _emailService.SendWelcomeAsync(user.Email, traceId);
    }
}
```

---

## Error Handling Example

```csharp
// Custom exception
public class OrderAlreadyShippedException : Exception
{
    public OrderAlreadyShippedException(string orderId)
        : base($"Order {orderId} is already shipped") { }
}

// Configure mapping
services.AddOntogonyErrors(opts =>
{
    opts.Map<OrderAlreadyShippedException>(HttpStatusCode.Conflict, "order_already_shipped");
});

// In handler
throw new OrderAlreadyShippedException(orderId);

// Response
{
  "type": "https://api.example.com/errors/order_already_shipped",
  "title": "Conflict",
  "status": 409,
  "traceId": "trace-abc123"
  // No "detail" with internal message!
}
```

---

## HMAC Signing Example

### Send Signed Request

```csharp
public class SecurePaymentClient : IPaymentServiceClient
{
    private readonly HttpClient _http;
    // Signing is handled transparently by OntogonyServiceIdentitySigningHandler (registered at startup):
    //   services.AddHttpClient("payment-service")
    //       .AddHttpMessageHandler(sp => new OntogonyServiceIdentitySigningHandler(serviceId, secret));
    // Inject IHttpClientFactory and call factory.CreateClient("payment-service") instead.

    public async Task<PaymentResponse> CaptureAsync(PaymentRequest request, CancellationToken ct)
    {
        var bodyJson = JsonSerializer.Serialize(request);
        var response = await _http.PostAsync(
            "/api/payments/capture",
            new StringContent(bodyJson, Encoding.UTF8, "application/json"),
            ct);
        return await response.Content.ReadFromJsonAsync<PaymentResponse>(cancellationToken: ct)
            ?? throw new InvalidOperationException("Empty payment response");
    }
}
```

### Validate Signed Request

```csharp
// Signature validation happens via AddOntogonyServiceIdentityActorContext (registered at startup).
// Accessing ICurrentActorAccessor.Current in an endpoint returns null if validation failed.
public class SecurePaymentController : ControllerBase
{
    [HttpPost("capture")]
    public IActionResult Capture(
        [FromBody] PaymentRequest request,
        [FromServices] ICurrentActorAccessor actor)
    {
        if (actor.Current is null)
            return Unauthorized();

        // actor.Current.ServiceId is the verified caller identity
        return Ok();
    }
}
```

---

## Testing with Conformance Kits

```csharp
public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    [Fact]
    public async Task Trace_Headers_Are_Echoed()
    {
        var client = _factory.CreateClient();
        const string traceId = "trace-test-123";

        client.DefaultRequestHeaders.Add("X-Ontogony-Trace-Id", traceId);
        var response = await client.GetAsync("/api/health");

        Assert.True(response.Headers.TryGetValues("X-Ontogony-Trace-Id", out var values));
        Assert.Contains(traceId, values);
    }

    [Fact]
    public async Task Error_Responses_Are_Canonical()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/orders", new { });

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

        var error = await response.Content.ReadAsAsync<ErrorResponse>();
        Assert.NotNull(error.TraceId);
        Assert.NotNull(error.Title);
        Assert.Null(error.Detail);  // No internal message leaks
    }

    [Fact]
    public void Published_Events_Are_Canonical()
    {
        var envelope = new OntogonyEnvelope<TestEvent>
        {
            EventId = Guid.NewGuid().ToString("n"),
            EventType = "ontogony.test.event",
            Source = "ontogony://test-service/domain",
            OccurredAt = DateTimeOffset.UtcNow,
            TraceId = "trace-123",
            Protocol = ProtocolNames.GenericJson,
            Payload = new TestEvent("data")
        };

        EnvelopeConformanceAssertions.AssertFullConformance(envelope);
    }
}
```

---

## Deterministic Testing with Fake Clock

```csharp
[Fact]
public async Task OrderService_Respects_DeliveryDeadline()
{
    var clock = new FakeClock(DateTimeOffset.UtcNow);
    var service = new OrderService(db, publisher, clock);

    var order = await service.CreateOrderAsync(cmd);
    Assert.Equal(clock.UtcNow, order.CreatedAt);

    // Advance time to delivery window
    clock.Advance(TimeSpan.FromHours(24));
    Assert.True(order.CanDeliver);

    // Advance past deadline
    clock.Advance(TimeSpan.FromHours(25));
    Assert.False(order.CanDeliver);
}
```

---

## More Examples

See `examples/` directory:

- [Minimal API with Ontogony Hosting](../../examples/MinimalApiWithOntogonyHosting/)
- [Event Envelope Example](../../examples/EventEnvelopeExample/)
- [Error Middleware Example](../../examples/ErrorMiddlewareExample/)
- [Integration HTTP Client Example](../../examples/IntegrationHttpClientExample/)

---

**Last Updated:** May 2026  
**Version:** 0.2.0
